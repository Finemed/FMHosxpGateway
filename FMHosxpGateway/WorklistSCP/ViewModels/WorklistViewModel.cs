using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data;
using MySql.Data.MySqlClient;

using WorklistSCP.Models;

namespace FMHosxpGateway.WorklistSCP.ViewModels
{
    public class WorklistViewModel
    {
        string mwlclient_tbname = "";
        
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public WorklistViewModel()
        {
            mwlclient_tbname = System.Configuration.ConfigurationManager.AppSettings["mwlclient_tbname"].ToString();
        }

        //for Add Patient
        public void InsertWorklistItem(WorklistItem item)
        {
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;
            

            try
            {
                m_con = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["mwlclient_constring"].ToString());
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;

                string query = $@"INSER INTO {mwlclient_tbname} ( AccessionNumber,
                                                                 PatientID,              
                                                                 Surname,
                                                                 Forename,
                                                                 Title,
                                                                 Sex,
                                                                 DateOfBirth,
                                                                 ReferringPhysician,
                                                                 PerformingPhysician,
                                                                 Modality,
                                                                 ExamDateAndTime,
                                                                 ExamRoom,
                                                                 ExamDescription,
                                                                 StudyUID,
                                                                 ProcedureID,
                                                                 ProcedureStepID,
                                                                 HospitalName,
                                                                 ScheduledAET,
                                                                 IsComplete) 
                                                                 VALUES (@AccessionNumber,
                                                                 @PatientID,
                                                                 @Surname,
                                                                 @Forename,
                                                                 @Title,
                                                                 @Sex,
                                                                 @DateOfBirth,
                                                                 @ReferringPhysician,
                                                                 @PerformingPhysician,
                                                                 @Modality,
                                                                 @ExamDateAndTime,
                                                                 @ExamRoom,
                                                                 @StudyUID,
                                                                 @ProcedureID,
                                                                 @ProcedureStepID,
                                                                 @HospitalName,
                                                                 @ScheduledAET,
                                                                 @IsComplete
                                                                 )";
                m_cmd.CommandText = query;
                m_cmd.Parameters.AddWithValue("@AccessionNumber",item.AccessionNumber);
                m_cmd.Parameters.AddWithValue("@PatientID", item.PatientID);
                m_cmd.Parameters.AddWithValue("@Surname", item.Surname);
                m_cmd.Parameters.AddWithValue("@Forename", item.Forename);
                m_cmd.Parameters.AddWithValue("@Title", item.Title);
                m_cmd.Parameters.AddWithValue("@Sex", item.Sex);
                m_cmd.Parameters.AddWithValue("@DateOfBirth", item.DateOfBirth);
                m_cmd.Parameters.AddWithValue("@ReferringPhysician", item.ReferringPhysician);
                m_cmd.Parameters.AddWithValue("@PerformingPhysician", item.PerformingPhysician);
                m_cmd.Parameters.AddWithValue("@Modality", item.Modality);
                m_cmd.Parameters.AddWithValue("@ExamDateAndTime", item.ExamDateAndTime);
                m_cmd.Parameters.AddWithValue("@ExamRoom", item.ExamRoom);
                m_cmd.Parameters.AddWithValue("@StudyUID", item.StudyUID);
                m_cmd.Parameters.AddWithValue("@ProcedureID", item.ProcedureID);
                m_cmd.Parameters.AddWithValue("@ProcedureStepID", item.ProcedureStepID);
                m_cmd.Parameters.AddWithValue("@HospitalName", item.HospitalName);
                m_cmd.Parameters.AddWithValue("@ScheduledAET", item.ScheduledAET);
                m_cmd.Parameters.AddWithValue("@IsComplete", item.IsComplete);

                m_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
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
                    m_con.Close();
                    m_con.Dispose();
                    m_con = null;
                }
            }


        }
        //for Update Patient Informatio from HL7
        public void UpdateWorklistItem(WorklistItem item)
        {
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;

            try
            {
                m_con = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["mwlclient_constring"].ToString());
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;

                string query = $@"UPDATE {mwlclient_tbname}  SET 
                                                    AccessionNumber=@AccessionNumber,
                                                    PatientID=@PatientID,
                                                    Surname=@Surname,
                                                    Forename=@Forename,
                                                    Title=@Title,
                                                    Sex=@Sex,
                                                    DateOfBirth=@DateOfBirth,
                                                    ReferringPhysician=@ReferringPhysician,
                                                    PerformingPhysician=@PerformingPhysician,
                                                    Modality=@Modality,
                                                    ExamDateAndTime=@ExamDateAndTime,
                                                    ExamRoom=@ExamRoom,
                                                    StudyUID=@StudyUID,
                                                    ProcedureID=@ProcedureID,
                                                    ProcedureStepID=@ProcedureStepID,
                                                    HospitalName=@HospitalName,
                                                    ScheduledAET=@ScheduledAET,
                                                    IsComplete=@IsComplete,
                                                   )";
                m_cmd.CommandText = query;
                m_cmd.Parameters.AddWithValue("@AccessionNumber", item.AccessionNumber);
                m_cmd.Parameters.AddWithValue("@PatientID", item.PatientID);
                m_cmd.Parameters.AddWithValue("@Surname", item.Surname);
                m_cmd.Parameters.AddWithValue("@Forename", item.Forename);
                m_cmd.Parameters.AddWithValue("@Title", item.Title);
                m_cmd.Parameters.AddWithValue("@Sex", item.Sex);
                m_cmd.Parameters.AddWithValue("@DateOfBirth", item.DateOfBirth);
                m_cmd.Parameters.AddWithValue("@ReferringPhysician", item.ReferringPhysician);
                m_cmd.Parameters.AddWithValue("@PerformingPhysician", item.PerformingPhysician);
                m_cmd.Parameters.AddWithValue("@Modality", item.Modality);
                m_cmd.Parameters.AddWithValue("@ExamDateAndTime", item.ExamDateAndTime);
                m_cmd.Parameters.AddWithValue("@ExamRoom", item.ExamRoom);
                m_cmd.Parameters.AddWithValue("@StudyUID", item.StudyUID);
                m_cmd.Parameters.AddWithValue("@ProcedureID", item.ProcedureID);
                m_cmd.Parameters.AddWithValue("@ProcedureStepID", item.ProcedureStepID);
                m_cmd.Parameters.AddWithValue("@HospitalName", item.HospitalName);
                m_cmd.Parameters.AddWithValue("@ScheduledAET", item.ScheduledAET);
                m_cmd.Parameters.AddWithValue("@IsComplete", item.IsComplete);

                m_cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                Logger.Error(ex.ToString());
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
                    m_con.Close();
                    m_con.Dispose();
                    m_con = null;
                }
            }

        }


        //for Cancle Patient from HL7
        public void DeleteWorklistItme(string strAccessionNumber)
        {
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;

            try
            {
                m_con = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["mwlclient_constring"].ToString());
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;

                string query = $@"DELETE FROM {mwlclient_tbname} 
                                 WHERE AccessionNumber=@AccessionNumber";

                m_cmd.CommandText = query;
                m_cmd.Parameters.AddWithValue("@AccessionNumber", strAccessionNumber);
                m_cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
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
                    m_con.Close();
                    m_con.Dispose();
                    m_con = null;
                }
            }

        }

        public void UpdateWorklistItemStatus(string strAccessionNumber, string strIsComplete)
        {
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;

            try
            {
                m_con = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["mwlclient_constring"].ToString());
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;

                string query = $@"UPDATE {mwlclient_tbname} SET 
                                 IsComplete=@IsComplete 
                                 WHERE AccessionNumber=@AccessionNumber";

                m_cmd.CommandText = query;
                m_cmd.Parameters.AddWithValue("@IsComplete",strIsComplete);
                m_cmd.Parameters.AddWithValue("@AccessionNumber", strAccessionNumber);
                m_cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
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
                    m_con.Close();
                    m_con.Dispose();
                    m_con = null;
                }
            }
        }
        
    }
}
