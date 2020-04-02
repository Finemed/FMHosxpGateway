using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHosxpGateway.Models
{
    public class XrayRequestModel
    {
        public int xray_request_id { set; get; }
        public string xray_request_xn { set; get; }
        public string xray_request_msg_type { set; get; }
        public byte[] xray_request_data { set; get; }
        public DateTime xray_request_datetime { set; get; }
        public string xray_request_receive { set; get; }
        public DateTime xray_request_receive_datetime { set; get; }


        public string GetXrayRequestMsgString(System.Text.Encoding enc)
        {
            string result = "";
            if (this.xray_request_data != null)
            {
                try
                {
                    result = enc.GetString(this.xray_request_data);
                }
                catch
                {
                    result = "";
                }

            }
            return result;
        }
    }

}

