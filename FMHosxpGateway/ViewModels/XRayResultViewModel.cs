using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FMHosxpGateway.Models;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace FMHosxpGateway.ViewModels
{
    public class XRayResultViewModel
    {
        private string m_connnectionstring = "";
        private string m_tbname = "";
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public XRayResultViewModel()
        {
            m_connnectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["xrayresult_constring"].ToString();
            m_tbname = System.Configuration.ConfigurationManager.AppSettings["xrayresult_tbname"];
        }

        public void InsertXrayResult(XrayRequestModel request,XrayResultModel result)
        {
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;
            try
            {
                m_con = new MySqlConnection(m_connnectionstring);
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;

                //get latsted xray_result_id
                int lastid = 0;
                m_cmd.CommandText = $@"SELECT MAX(XRAY_RESULT_ID) FROM {m_tbname} ";
                try
                {
                   lastid = Convert.ToInt32(m_cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    lastid = 0;
                }

                
                lastid = lastid + 1;

                string query = $@"INSERT INTO {m_tbname} (XRAY_RESULT_ID,
                                                          XRAY_RESULT_XN,
                                                          XRAY_RESULT_MSG_TYPE,
                                                          XRAY_RESULT_DATA,
                                                          XRAY_RESULT_DATETIME,
                                                          XRAY_RESULT_RECEIVE) 
                                                          VALUES (@xray_result_id,@xray_result_xn,@xray_result_msg_type,@xray_result_data,@xray_result_datetime,@xray_result_receive)";

                m_cmd.CommandText = query;
                m_cmd.Parameters.AddWithValue("@xray_result_id", lastid);
                m_cmd.Parameters.AddWithValue("@xray_result_xn", request.xray_request_xn);
                m_cmd.Parameters.AddWithValue("@xray_result_msg_type", result.xray_result_msg_type);
                m_cmd.Parameters.AddWithValue("@xray_result_data", result.GetXrayresultMsgByte(System.Text.Encoding.GetEncoding(874)));
                m_cmd.Parameters.AddWithValue("@xray_result_datetime", result.xray_result_datetime);
                m_cmd.Parameters.AddWithValue("@xray_result_receive", "N");

                m_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
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

        public void UpdateXrayResult(XrayRequestModel request, XrayResultModel result)
        {
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;
            try
            {
                m_con = new MySqlConnection(m_connnectionstring);
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;

                string query = $@"UPDATE {m_tbname}   
                                                    XRAY_RESULT_XN=@xray_result_xn,
                                                    XRAY_RESULT_MSG_TYPE=@xray_result_msg_type,
                                                    XRAY_RESULT_DATA=@xray_result_data,
                                                    XRAY_RESULT_DATETIME=@xray_result_datetime,
                                                    XRAY_RESULT_RECEIVE=@xray_result_receive)";

                m_cmd.CommandText = query;

                m_cmd.Parameters.AddWithValue("@xray_result_xn",result.xray_result_xn);
                m_cmd.Parameters.AddWithValue("@xray_result_msg_type", result.xray_result_msg_type);
                m_cmd.Parameters.AddWithValue("@xray_result_data", result.GetXrayresultMsgByte(System.Text.Encoding.GetEncoding(874)));
                m_cmd.Parameters.AddWithValue("@xray_result_datetime", result.xray_result_datetime);
                m_cmd.Parameters.AddWithValue("@xray_result_receive", "N");

                m_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
        //UpdateRequestStatusAndTime
    }
}
