using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHosxpGateway.Models
{
    public class XrayResultModel
    {
        public int xray_result_id { set; get; }
        public string xray_result_xn { set; get; }
        public string xray_result_msg_type { set; get; }
        public string xray_result_data { set; get; }
        public DateTime xray_result_datetime { set; get; }
        public string xray_result_receive { set; get; }
        public DateTime xray_result_receive_datetime { set; get; }

        public byte[] GetXrayresultMsgByte(System.Text.Encoding enc)
        {
            byte[] result = null;
            if (this.xray_result_data != null)
            {
                try
                {
                    result = enc.GetBytes(this.xray_result_data);
                }
                catch
                {
                    result = null;
                }

            }
            return result;
        }
    }
}
