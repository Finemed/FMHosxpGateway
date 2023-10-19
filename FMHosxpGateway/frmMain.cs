using FMHosxpGateway.Models;
using FMHosxpGateway.ViewModels;
using HL7.Dotnetcore;
using NLog;
using NLog.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace FMHosxpGateway
{
    public partial class frmMain : Form
    {
        private System.Timers.Timer interval = null;
        private XRayRequestViewModel reqvm = null;
        private XRayResultViewModel resvm = null;
        private TPACSMWLVieweModel tpvm = null;

        private string m_hospitalname = "";

        private NLog.Logger Logger;

        public frmMain()
        {
            InitializeComponent();

            Logger = NLog.LogManager.GetCurrentClassLogger();

            Logger.Info("Init");

            RichTextBoxTarget.ReInitializeAllTextboxes(this);

            interval = new System.Timers.Timer();
            interval.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["interval"]) * 1000;
            interval.AutoReset = true;
            interval.Elapsed += Interval_Elapsed;

            reqvm = new XRayRequestViewModel();
            resvm = new XRayResultViewModel();
            tpvm = new TPACSMWLVieweModel();
            m_hospitalname = ConfigurationManager.AppSettings["m_hospitalname"];
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void Interval_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Logger.Info("Interval arrive");

            GetMessageFromHosxp();
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (cmdStart.Text == "Start")
            {
                cmdStart.Text = "Stop";
                interval.Start();
                Logger.Info("Start Interval");
            }
            else
            {
                cmdStart.Text = "Start";
                interval.Stop();
                Logger.Info("Stop Interval");
            }
        }

        //Function GetRequest Messge from HOSXP

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
                    Logger.Info($"request accession: {reqmsg.xray_request_xn} type: {reqmsg.xray_request_msg_type}");

                    string msg = reqmsg.GetXrayRequestMsgString(Encoding.GetEncoding(874));

                    //If Request MSG is ORM^O01
                    if (reqmsg.xray_request_msg_type == "ORM^O01")
                    {
                        Logger.Info("Get Request Message ORM^O01 try to parsing");

                        //test string

                        ORMMsgModel requestmsg = ParseXrayRequestORM01Msg(msg);

                        if (requestmsg != null && requestmsg.OrderControl == "NW")
                        {
                            //parse msg OK
                            //put message to worklist server

                            //update status in Hosxp RequestDatabase
                            reqvm.UpdateXrayRequest(reqmsg);
                            //add Hosxp ResultDatabase
                            //create result msg
                            HL7.Dotnetcore.Message result = CreateXrayResultMsg(requestmsg);

                            if (requestmsg.OrderControl == "NW")
                            {
                                if (requestmsg.OBRS != null && requestmsg.OBRS.Count > 0)
                                {
                                    tpvm.InsertORMToWorklistDB(requestmsg, requestmsg.OBRS[0]);
                                }
                            }
                        }
                        else if (requestmsg != null && requestmsg.OrderControl == "CA")
                        {
                            reqvm.UpdateXrayRequest(reqmsg);
                            tpvm.UpdateWorklistStatus(requestmsg.AccessionNO, "Y");
                            Logger.Info("Cancle Message for Accession No. " + requestmsg.AccessionNO);
                        }
                        else
                        {
                            Logger.Error("Missing format for Request Message:");
                        }
                    }
                    //If Request MSG is ADT^A08 for Create or Update Patient OPD/IPD
                    else if (reqmsg.xray_request_msg_type == "ADT^A08")
                    {
                        reqvm.UpdateXrayRequest(reqmsg);
                    }
                    //If Request MSG is ADT^A40 for Patient Merge
                    else if (reqmsg.xray_request_msg_type == "ADT^A40")
                    {
                    }
                }
            }
            else
            {
                Logger.Info("Not have update xray request data");
            }
        }

        private HL7.Dotnetcore.Message CreateXrayResultMsg(ORMMsgModel req)
        {
            HL7.Dotnetcore.Message resultmsg = new HL7.Dotnetcore.Message();
            resultmsg.AddSegmentMSH("FINEMED", m_hospitalname, "HOSXP", "", "", "ORM^O01", req.AccessionNO, "P", "2.3");
            resultmsg.SetValue("MSH.7", DateTime.Now.ToString("yyyymmddhhMMss"));

            //add Segment PID
            resultmsg.AddNewSegment(req.Org_PID);
            //add Segment PV1
            resultmsg.AddNewSegment(req.Org_PV1);
            //add Segment ORC
            Segment ORC = new Segment("ORC", new HL7Encoding());
            ORC.AddNewField("SC", 1);
            ORC.AddEmptyField();
            ORC.AddNewField(req.AccessionNO, 3);
            ORC.AddNewField("CM", 5);
            resultmsg.AddNewSegment(ORC);

            if (req.Org_OBR != null)
            {
                foreach (Segment obr in req.Org_OBR)
                {
                    resultmsg.AddNewSegment(obr);
                }
            }

            return resultmsg;
        }

        private ORMMsgModel ParseXrayRequestORM01Msg(string data)
        {
            ORMMsgModel result = null;

            HL7.Dotnetcore.Message message = new HL7.Dotnetcore.Message(data);

            bool isParsed = false;
            try
            {
                isParsed = message.ParseMessage();

                if (isParsed)
                {
                    result = new ORMMsgModel();

                    Segment ORC = message.Segments("ORC")[0];
                    result.Org_ORC = ORC;
                    result.OrderControl = ORC.Fields(1).Value;
                    result.AccessionNO = ORC.Fields(2).Value;
                    
                    //if NW Case
                    if (result.OrderControl == "NW")
                    {
                        Segment PID = message.Segments("PID")[0];
                        result.Org_PID = PID;

                        result.HN = PID.Fields(2).Value;
                        result.IDCard = PID.Fields(4).Value;
                        result.PatientPrename = PID.Fields(5).Components(5).Value;
                        result.PatientFirstname = PID.Fields(5).Components(2).Value;
                        result.PatientLastname = PID.Fields(5).Components(1).Value;
                        result.PatientAddress = PID.Fields(11).Value;
                        result.PatientDOB = PID.Fields(7).Value;
                        result.PatientSex = PID.Fields(8).Value;

                        Segment PV1 = message.Segments("PV1")[0];
                        result.Org_PV1 = PV1;

                        result.Org_OBR = message.Segments("OBR");

                        foreach (Segment obr in message.Segments("OBR"))
                        {
                            OBRMsgModel obritem = new OBRMsgModel();
                            obritem.SetID = obr.Fields(1).Value;
                            obritem.AccessionNo = obr.Fields(2).Value;
                            obritem.XrayItemID = obr.Fields(4).Components(1).Value;
                            obritem.XrayItemName = obr.Fields(4).Components(2).Value;
                            obritem.XrayRequestDateTime = obr.Fields(6).Value;
                            result.addOBRItem(obritem);
                        }

                        result.OrderDateTime = ORC.Fields(9).Value;

                    }
                    else if (result.OrderControl == "CA")
                    {

                        Logger.Info($"ORM^01 Cancle order : {result.AccessionNO} ");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception:" + ex.ToString());
                result = null;
            }
            return result;
        }

    }
}