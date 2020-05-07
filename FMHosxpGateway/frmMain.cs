using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FMHosxpGateway.ViewModels;
using FMHosxpGateway.Models;
using HL7.Dotnetcore;
using MySqlX.XDevAPI.Common;
using System.Configuration;

namespace FMHosxpGateway
{
    public partial class frmMain : Form
    {
        private System.Timers.Timer interval = null;
        XRayRequestViewModel reqvm = null;
        XRayResultViewModel resvm = null;
        public frmMain()
        {
            InitializeComponent();
            interval = new System.Timers.Timer();
            interval.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["interval"])*1000;
            interval.AutoReset = true;
            interval.Elapsed += Interval_Elapsed;

            reqvm = new XRayRequestViewModel();
            resvm = new XRayResultViewModel();

        }

        private void Interval_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lstLog.Items.Add("Interval arrive: " + DateTime.Now.ToString("HH:mm:ss"));
            GetMessageFromHosxp();
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (cmdStart.Text == "Start")
            {
                cmdStart.Text = "Stop";
                interval.Start();
                lstLog.Items.Add("Start Interval");
            }
            else
            {
                cmdStart.Text = "Start";
                interval.Stop();
                lstLog.Items.Add("Stop Interval");
            }
        }

        private RequestMsgModel ParseXrayRequestMsg(string data)
        {
            RequestMsgModel result = null;

            HL7.Dotnetcore.Message message = new HL7.Dotnetcore.Message(data);
           
            bool isParsed = false;
            try
            {
                isParsed = message.ParseMessage();

                if (isParsed)
                {
                    result = new RequestMsgModel();

                    Segment PID = message.Segments("PID")[0];
                    result.Org_PID = PID;

                    result.HN = PID.Fields(2).Value;
                    result.IDCard = PID.Fields(4).Value;
                    result.PatientPrename = PID.Fields(5).Components(5).Value;
                    result.PatientFirstname = PID.Fields(5).Components(2).Value;
                    result.PatientLastname = PID.Fields(5).Components(1).Value;
                    result.PatientAddress = PID.Fields(11).Value;
                    result.PatientDOB = PID.Fields(7).Value;


                    Segment ORC = message.Segments("ORC")[0];
                    result.Org_ORC = ORC;
                    result.OrderControl = ORC.Fields(1).Value;
                    result.AccessionNO = ORC.Fields(2).Value;
                    result.OrderDateTime = ORC.Fields(9).Value;

                    result.Org_OBR = message.Segments("OBR");

                    foreach (Segment obr in message.Segments("OBR"))
                    {
                        OBRItem obritem = new OBRItem();
                        obritem.SetID = obr.Fields(1).Value;
                        obritem.AccessionNo = obr.Fields(2).Value;
                        obritem.XrayItemID = obr.Fields(4).Components(1).Value;
                        obritem.XrayItemName = obr.Fields(4).Components(2).Value;
                        obritem.XrayRequestDateTime = obr.Fields(6).Value;
                        result.addOBRItem(obritem);
                    }
                        
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show(ex.ToString());
                result = null;
            }
            return result;
        }

        private void GetMessageFromHosxp()
        {
            //get xray reuqest from hosxp database
            List<XrayRequestModel> reqs = reqvm.GetXrayRequest();
        
            //has xray request in hosxp database
            if (reqs != null && reqs.Count > 0)
            {

                //parse xray request message
                foreach (XrayRequestModel reqmsg in reqs)
                {
                    lstLog.Items.Add("request accession:" + reqmsg.xray_request_xn);
                    string msg = reqmsg.GetXrayRequestMsgString(Encoding.GetEncoding(874));
                    RequestMsgModel reqeustmsg = ParseXrayRequestMsg(msg);

                    if (reqeustmsg != null)
                    {
                        //parse msg OK
                        //put message to worklist server

                        //update status in Hosxp RequestDatabase
                        reqvm.UpdateXrayRequest(reqmsg);
                        //add Hosxp ResultDatabase
                        //create result msg
                        HL7.Dotnetcore.Message result = CreateXrayResultMsg(reqeustmsg);

                        XrayResultModel resmodel = new XrayResultModel();
                        resmodel.xray_result_msg_type = "ORU^R01";
                        resmodel.xray_result_xn = reqeustmsg.AccessionNO;
                        resmodel.xray_result_datetime = DateTime.Now;
                        resmodel.xray_result_receive = "N";
                        resmodel.xray_result_data = result.SerializeMessage(false); //create result msg;
                        resvm.InsertXrayResult(reqmsg, resmodel);
                    }
                    else
                    { 
                        //parse msg Fail

                    }



                }
            }
            else
            {
                //not have update xray data in hosxpdatabase
                lstLog.Items.Add("Not have update xray request data");
            }
        }

        private HL7.Dotnetcore.Message CreateXrayResultMsg(RequestMsgModel req)
        {
            
            HL7.Dotnetcore.Message resultmsg = new HL7.Dotnetcore.Message();
            resultmsg.AddSegmentMSH("SONICDICOM", "", "HOSXP", "", "", "ORU^R01",req.AccessionNO, "P", "2.3");
            resultmsg.SetValue("MSH.7", DateTime.Now.ToString("yyyymmddhhMMss"));
            //resultmsg.SetValue("MSH.18", "UNICODE UTF-8");
           
            Segment PID = req.Org_PID;
            resultmsg.AddNewSegment(PID);
            foreach (Segment obr in req.Org_OBR)
            {
                resultmsg.AddNewSegment(obr);
            }
            return resultmsg;
        }
    }
}
