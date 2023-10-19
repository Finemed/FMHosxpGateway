using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using FMHosxpGateway.Models;
//using MySql;
//using MySql.Data.MySqlClient;
using MySqlConnector;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using Dicom;

namespace FMHosxpGateway.ViewModels
{
    public class TPACSMWLVieweModel
    {
        private string m_connnectionstring = "";
        private string m_tbname = "";
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private string m_hospitalname = "";


        public TPACSMWLVieweModel()
        {
            m_connnectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["mwlclient_constring"].ToString();
            m_tbname = System.Configuration.ConfigurationManager.AppSettings["mwlclient_tbname"];
            m_hospitalname = System.Configuration.ConfigurationManager.AppSettings["hospitalname"];
        }

        public  void InsertORMToWorklistDB(ORMMsgModel ormmsg,OBRMsgModel obrmsg)
        {
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;

            try
            {
                m_con = new MySqlConnection(m_connnectionstring);
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;
               
                    m_cmd.CommandText = $@"INSERT IGNORE INTO {m_tbname} " + @" (AccessionNumber,PatientID,Sex,Title,Surname,Forename,DateOfBirth,ReferringPhysician,PerformingPhysician,Modality,ExamDateAndTime,ExamRoom,ExamDescription,StudyUID,ProcedureID,ProcedureStepID,HospitalName,ScheduledAET) 
                                      Values (@AccessionNumber,@PatientID,@Sex,@Title,@Surname,@Forename,@DateOfBirth,@ReferringPhysician,@PerformingPhysician,@Modality,@ExamDateAndTime,@ExamRoom,@ExamDescription,@StudyUID,@ProcedureID,@ProcedureStepID,@HospitalName,@ScheduledAET)";

                    m_cmd.Parameters.AddWithValue("@AccessionNumber",obrmsg.AccessionNo);
                    m_cmd.Parameters.AddWithValue("@PatientID", ormmsg.HN);
                    m_cmd.Parameters.AddWithValue("@Sex", ormmsg.PatientSex);
                    m_cmd.Parameters.AddWithValue("@Title", ormmsg.PatientPrename);
                    m_cmd.Parameters.AddWithValue("@Forename", ormmsg.PatientFirstname);
                    m_cmd.Parameters.AddWithValue("@Surname", ormmsg.PatientLastname);
                    m_cmd.Parameters.AddWithValue("@DateOfBirth", ormmsg.PatientDOB);
                    m_cmd.Parameters.AddWithValue("@ReferringPhysician","");
                    m_cmd.Parameters.AddWithValue("@PerformingPhysician", "");
             

                    DateTime ExamDateTime = DateTime.Now;
                    try
                    {
                        ExamDateTime = DateTime.ParseExact(obrmsg.XrayRequestDateTime, "yyyyMMddHHmmss",new System.Globalization.CultureInfo("en-US"));
                    }
                    catch (Exception excp)
                    {
                        Logger.Error($"Try parse Examdate faile { excp.ToString()}");
                    }
                 
                
                    m_cmd.Parameters.AddWithValue("@ExamDateAndTime", ExamDateTime);
                    m_cmd.Parameters.AddWithValue("@ExamRoom", null);
             
                    m_cmd.Parameters.AddWithValue("@ExamDescription", obrmsg.XrayItemName);

                    string modality = getmodalityfromxraydescription(obrmsg.XrayItemName);
                    m_cmd.Parameters.AddWithValue("@Modality", modality);
               
                    m_cmd.Parameters.AddWithValue("@StudyUID", Dicom.DicomUID.Generate().UID);
                    m_cmd.Parameters.AddWithValue("@ProcedureID", obrmsg.XrayItemID);
                    m_cmd.Parameters.AddWithValue("@ProcedureStepID", obrmsg.XrayItemID);
                    m_cmd.Parameters.AddWithValue("@HospitalName", m_hospitalname);
                    m_cmd.Parameters.AddWithValue("@ScheduledAET", null);
                    m_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

            }
            finally
            {
                if (m_cmd != null)
                {
                    m_cmd.Dispose();
                    m_cmd = null;
                }
                if (m_con != null)
                {
                    m_con.Dispose();
                    m_con.Close();
                    m_con = null;
                }
            }
        }

        public void UpdateWorklistStatus(string accessionno,string status)
        {

            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;

            try
            {
                m_con = new MySqlConnection(m_connnectionstring);
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;
                m_cmd.CommandText = $@"Update {m_tbname} set IsComplete = '{status}' where AccessionNumber = @accessionno";
                m_cmd.Parameters.AddWithValue("@accessionno", accessionno);
                int effectrow = m_cmd.ExecuteNonQuery();
                Logger.Info($@"Update IsComplete ={status} where AccessionNumber={accessionno} effect row= {effectrow}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                if (m_cmd != null)
                {
                    m_cmd.Dispose();
                    m_cmd = null;
                }
                if (m_con != null)
                {
                    m_con.Dispose();
                    m_con.Close();
                    m_con = null;
                }
            }

        }

        //add fuction use with regularexpression for mapping xraydescription to modality
        private string getmodalityfromxraydescription(string xray_description)
        {
            string result = "";
            if (Regex.IsMatch(xray_description, "US.*"))
            {
                result = "US";
            }
            else
            {
                result = "CR";
            }
            return result;
        }
        public static DicomUID GenerateDerivedFromUUID()  // DicomUIDGenerator
        {
            var guid = Guid.NewGuid().ToByteArray();
            var bigint = new System.Numerics.BigInteger(guid);
            if (bigint < 0) bigint = -bigint;
            var uid = "2.25." + bigint;

            return new DicomUID(uid, "Local UID", DicomUidType.Unknown);
        }
    }
}
