using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMHosxpGateway.Models;
using HL7.Dotnetcore;


namespace FMHosxpGateway.Models
{

public class ORMMsgModel
    {
        public ORMMsgModel()
        {
            this.OBRS = new List<OBRMsgModel>();
        }
        public Segment Org_PID { set; get; }
        public Segment Org_PV1 { set; get; }
        public Segment Org_ORC { set; get; }
        public List<Segment> Org_OBR { set; get; }


        public string HN { set; get; }
        public string IDCard { set; get; }
        public string PatientPrename { set; get; }
        public string PatientFirstname { set; get; }
        public string PatientLastname { set; get; }
        public string PatientAddress { set; get; }
        public string PatientDOB { set; get; }
        public string PatientSex { set; get;}

        public string OrderControl { set; get; }
        public string AccessionNO { set; get; }
        public string OrderDateTime { set; get; }


        public List<OBRMsgModel> OBRS { set; get; }

        public void addOBRItem(OBRMsgModel item)
        {
            this.OBRS.Add(item);
        }
    }
}
