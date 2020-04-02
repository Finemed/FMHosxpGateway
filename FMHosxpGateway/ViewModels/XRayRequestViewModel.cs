using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FMHosxpGateway.Models;
using NHapi.Base.Parser;
using NHapiTools.Base.Net;
using MySql.Data;
using MySql.Data.MySqlClient;



namespace FMHosxpGateway.ViewModels
{
   public class XRayRequestViewModel
    {
        public List<XrayRequestModel> GetXrayRequest()
        {
            List<XrayRequestModel> result = null;
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;

            try
            {
                m_con = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["HOSXPWorklistDBConstring"].ToString());
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;
              

                string query = @"SELECT XRAY_REQUEST_ID,
                                        XRAY_REQUEST_XN,
                                        XRAY_REQUEST_MSG_TYPE,
                                        XRAY_REQUEST_DATA,
                                        XRAY_REQUEST_DATETIME,
                                        XRAY_REQUEST_RECEIVE 
                                        From XRAY_REQUEST 
                                        Where XRAY_REQUEST_RECEIVE = 'N' Order by XRAY_REQUEST_DATETIME asc";


              m_cmd.CommandText = query;

                MySqlDataReader m_rd = m_cmd.ExecuteReader();

                if (m_rd.HasRows)
                {
                    result = new List<XrayRequestModel>();

                    while (m_rd.Read())
                    {
                        XrayRequestModel requestdata = new XrayRequestModel();
                        requestdata.xray_request_id = Convert.ToInt32(m_rd["XRAY_REQUEST_ID"]);
                        requestdata.xray_request_xn = Convert.ToString(m_rd["XRAY_REQUEST_XN"]);
                        requestdata.xray_request_msg_type = m_rd["XRAY_REQUEST_MSG_TYPE"].ToString();
                        requestdata.xray_request_data = (byte[])m_rd["XRAY_REQUEST_DATA"];
                        requestdata.xray_request_datetime = Convert.ToDateTime(m_rd["XRAY_REQUEST_DATETIME"]);
                        requestdata.xray_request_receive = Convert.ToString(m_rd["XRAY_REQUEST_RECEIVE"]);
                        result.Add(requestdata);
                    }
                }
                else
                {
                    //not have row
                    result = null;
                }
               
                // Console.WriteLine(string.Format("\nDATA READER VALUES FirstName: {0} LastName: {1}", user.FirstName, user.LastName));
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

            return result;
        }

        public void UpdateXrayRequest(XrayRequestModel xrayrquest)
        {
         
            MySqlConnection m_con = null;
            MySqlCommand m_cmd = null;

            try
            {
                string query = @"UPDATE xray_request set xray_request_receive = @xray_request_receive, xray_request_receive_datetime = @xray_request_receive_datetime where xray_request_xn = @xray_request_xn";


                m_con = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["HOSXPWorklistDBConstring"].ToString());
                m_con.Open();
                m_cmd = new MySqlCommand();
                m_cmd.Connection = m_con;

                m_cmd.CommandText = query;

                m_cmd.Parameters.AddWithValue("@xray_request_receive","Y");
                m_cmd.Parameters.AddWithValue("@xray_request_receive_datetime", DateTime.Now);
                m_cmd.Parameters.AddWithValue("@xray_request_xn", xrayrquest.xray_request_xn);

                m_cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string SendRequestToServer(string requestmsg)
        {
            string ppIP = System.Configuration.ConfigurationManager.AppSettings["PacsPlusServerIP"];
            int ppPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PacsPlusServerPort"]);

            var connection = new SimpleMLLPClient(ppIP, ppPort, Encoding.GetEncoding(874));

            var msgresponse = connection.SendHL7Message(requestmsg);
      

            string response = msgresponse.ToString();

        
            if (connection != null)
            {
                connection.Disconnect();
                connection.Dispose();
                connection = null;
            }

            return response;
        }

    }
}
