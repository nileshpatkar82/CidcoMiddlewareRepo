using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using System.Text;

namespace Cidco.Middleware.Application
{
    public class SAPService
    {
        public static string PostDOCDet(string data)
        {
            string DataFromPHP = null;
            try
            {
                string urlServer = "http://202.191.151.15:50000/RESTAdapter/creatapp";

                string url = urlServer;
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);
                WebReq.Timeout = 1900000;
                WebReq.Method = "POST";
                WebReq.ContentType = "application/json ; charset=UTF-8"; // "application/x-www-form-urlencoded";
                WebReq.Accept = "application/json";
                WebReq.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("Pisuper:Welcome@123"));
                WebReq.PreAuthenticate = true;
                WebReq.ContentLength = buffer.Length;
                Stream PostData = WebReq.GetRequestStream();
                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                Stream Answer = WebResp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                DataFromPHP = _Answer.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                WriteToFile(ex.ToString());
            }
            return DataFromPHP;
        }

        public static void WriteToFile(string stext)
        {
            string root = "D:\\AapleTemp";/*ConfigurationManager.AppSettings["TempPath"].ToString(); //"D:\\aapleSchedule.txt";*/

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            string path = root + "\\" + "aapleError.txt";// ConfigurationManager.AppSettings["Errorfile"].ToString();

            if (!File.Exists(path))
            {
                var myFile = File.Create(path);
                myFile.Close();
            }

            stext = stext + " " + DateTime.Now.ToString();
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine(string.Format(stext, DateTime.Now.ToString()));
                writer.Close();
            }

        }
        public static string createDoc(string tusrid, string tsevid, string ttrackid, string tappno, string tserviceday)
        {
            string t_result = null;
            try
            {
                string url = "https://www.cidcoindia.com/TESTAapleCIDCORestService/aapleSarkarWCIDCOAPP.svc/CreateAPPLICATION?TrackID=" + ttrackid.Trim() + "&UsrID=" + tusrid.Trim() + "&ServiceID=" + tsevid.Trim() + "&AppNo=" + tappno.Trim() + "&ServiceDay=" + tserviceday.Trim();
                byte[] buffer = Encoding.ASCII.GetBytes(url);
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);
                WebReq.Timeout = 1900000;
                WebReq.Method = "GET";
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                Stream Answer = WebResp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                t_result = _Answer.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return t_result;

        }


        //-- function to Get Document no for aaple sarkar TrackID
        public static DataTable PostSAPDocumentNOinAapleSarkar(string custid, string serviceid, string trackid)
        {
            modulePost _modul = new modulePost();
            string entity;
            string data;
            string respon;
            string json;
            try
            {
                string seno = trackid;

                string susernm = "Ganesha";
                string spasswd = "Ganesh#4";
                entity = "&username=" + susernm + "&password=" + spasswd;
                //data = "{" + "" + "MT_CIDCO_PROMORTGAGE_REQ_OB" + "" + ": { " + "" + "ALT_INPUT" + "" + ": {" + "" + "item" + "" + ": { " + "" + "EXTRA_2" + "" + ": " + "" + "VENDET" + "" + "," + "" + "EXTRA_3" + "" + ": " + "" + EXTRA_3 + "" + " } } }}";
                //&app=" + app + "&module=" + module + "&action=" + action + "" + entity;

                //data = "{\"DATA\": {\"item\": {\"F1\": \"051\",\"F2\": \"" + seno + "\" } } }";
                data = "{\"CUST_ID\": \"" + custid + "\", \"DATE\":\"" + "\",\"SERVICE_ID\": \"" + serviceid + "\",\"TRACK_ID\":\"" + trackid + "\"}";
                //data = "{\"DATA\": {\"item\": {\"F1\": \"051\",\"F2\": \"" + seno + "\",\"F46\": \"" + sempnm + "\",\"F36\": \"" + sDept + "\",\"F37\": \"" + sempdesg + "\",\"F16\": \"" + empmobile + "\",\"F3\": \"" + hodempno + "\",\"F47\": \"" + hodempnm + "\",\"F38\": \"" + hodempdesg + "\",\"F4\": \"" + sAppno + "\",\"F5\": \"" + sAppType + "\",\"F48\": \"" + sPurpose + "\" } } }";

                respon = PostDOCDet(data);

                json = respon.TrimStart('[');
                json = json.TrimEnd(']');
                JObject o = JObject.Parse(json);
                //dvResult.InnerText = o.ToString();

                DataTable dt = new DataTable();
                dt.Columns.Add("APP_NO");
                dt.Columns.Add("CUST_ID");
                dt.Columns.Add("SERVICE_ID");
                dt.Columns.Add("TRACK_ID");
                dt.Columns.Add("MSG_CODE");
                dt.Columns.Add("MSG_DESC");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("SERVICE_DAYS");
                dt.Columns.Add("EXTRA_1");
                dt.Columns.Add("EXTRA_2");
                dt.Columns.Add("EXTRA_3");
                dt.Columns.Add("EXTRA_4");
                dt.Columns.Add("EXTRA_5");
                dt.Columns.Add("EXTRA_6");
                dt.Columns.Add("EXTRA_7");
                dt.Columns.Add("EXTRA_8");
                dt.Columns.Add("EXTRA_9");
                dt.Columns.Add("EXTRA_10");
                dt.Columns.Add("EXTRA_11");
                dt.Columns.Add("EXTRA_12");
                dt.Columns.Add("EXTRA_13");
                dt.Columns.Add("EXTRA_14");
                dt.Columns.Add("EXTRA_15");
                dt.TableName = "ACAPPLResp";

                //"{\"RES_DATA\":{\"item\":{\"APP_NO\":8000224640,\"CUST_ID\":\"06C4F97C-91EE-4C95-A6AB-6EAE31959C03\",\"SERVICE_ID\":7073,\"TRACK_ID\":23100400110000081,\"MSG_CODE\":\"01\",\"MSG_DESC\":\"Successfully Created\",\"STATUS\":111,\"SERVICE_DAYS\":\"0000000000\",\"EXTRA_1\":\"\",\"EXTRA_2\":\"\",\"EXTRA_3\":\"\",\"EXTRA_4\":\"\",\"EXTRA_5\":\"\",\"EXTRA_6\":\"\",\"EXTRA_7\":\"\",\"EXTRA_8\":\"\",\"EXTRA_9\":\"\",\"EXTRA_10\":\"\",\"EXTRA_11\":\"\",\"EXTRA_12\":\"\",\"EXTRA_13\":\"\",\"EXTRA_14\":\"\",\"EXTRA_15\":\"\"}}}"
                string ls = o.ToString();
                string lst1 = "{\r\n  \"RES_DATA\": {\r\n    \"item\":";
                string lst2 = "\r\n    }\r\n  }";
                ls = ls.Replace(lst1, "");
                ls = ls.Replace(lst2, "");
                //ls = "{\r\n      \"ZMSG_TYPE\": \"01\",\r\n      \"F1\": \"\",\r\n      \"F2\": \"\",\r\n      \"F3\": \"Employee Created\"\r\n    }";


               // var serializer = //new System.Web.Script.Serialization.JavaScriptSerializer();
                SapCreateDocCls obj = JsonConvert.DeserializeObject< SapCreateDocCls>(ls);
                dt.Rows.Add(obj.APP_NO, obj.CUST_ID, obj.SERVICE_ID, obj.TRACK_ID, obj.MSG_CODE, obj.MSG_DESC, obj.STATUS, obj.SERVICE_DAYS, obj.EXTRA_1, obj.EXTRA_2, obj.EXTRA_3, obj.EXTRA_4, obj.EXTRA_5, obj.EXTRA_6, obj.EXTRA_7, obj.EXTRA_8, obj.EXTRA_9, obj.EXTRA_10, obj.EXTRA_11, obj.EXTRA_12, obj.EXTRA_13, obj.EXTRA_14, obj.EXTRA_15);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Database Error: " + ex.Message);
            }
        }
        //--end of function

        public class createAPPLDetCls
        {
            public string ZMSG_TYPE { get; set; }
            public string F3 { get; set; }
        }

        public class SapCreateDocCls
        {
            public string APP_NO { get; set; }
            public string CUST_ID { get; set; }
            public string SERVICE_ID { get; set; }
            public string TRACK_ID { get; set; }
            public string MSG_CODE { get; set; }
            public string MSG_DESC { get; set; }
            public string STATUS { get; set; }
            public string SERVICE_DAYS { get; set; }
            public string EXTRA_1 { get; set; }
            public string EXTRA_2 { get; set; }
            public string EXTRA_3 { get; set; }
            public string EXTRA_4 { get; set; }
            public string EXTRA_5 { get; set; }
            public string EXTRA_6 { get; set; }
            public string EXTRA_7 { get; set; }
            public string EXTRA_8 { get; set; }
            public string EXTRA_9 { get; set; }
            public string EXTRA_10 { get; set; }
            public string EXTRA_11 { get; set; }
            public string EXTRA_12 { get; set; }
            public string EXTRA_13 { get; set; }
            public string EXTRA_14 { get; set; }
            public string EXTRA_15 { get; set; }
        }
    }
}
