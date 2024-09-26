using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Features.RequestFromAapleSarkar;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using System.Text;


namespace Cidco.Middleware.Application
{
    public class CPFInfoRESTService : ICPFInfoRESTService
    {
        private readonly IConfiguration _configuration;
        public CPFInfoRESTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<ErrorLogResponse> fnProcCreateDoc()
        {
            // CPFInfoRESTService cPFInfoRESTService = new CPFInfoRESTService(_configuration);
            DataAccess dataAccess = new DataAccess(_configuration);
            DataTable dtDoc = new DataTable();
            string suserid, strackid, sserviceid;
            string sresult;
            DateTime fromdt = DateTime.Now;
            fromdt = fromdt.AddDays(-4);
            // Response.Write(fromdt);
            try
            {
                //dtDoc = objDBAccess.GetDataTable("Select top(10) * from aapleUserTb where ApplicationID is null order by entrydate desc");
                dtDoc = dataAccess.GetDataTable("Select * from aapleUserTb where ApplicationID is null and entrydate >= '" + fromdt.ToString("yyyy/MM/dd") + "'  order by entrydate desc");
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    suserid = dtDoc.Rows[i]["userid"].ToString();
                    strackid = dtDoc.Rows[i]["trackid"].ToString();
                    sserviceid = dtDoc.Rows[i]["serviceid"].ToString();
                    DataTable dt1 = new DataTable();
                    dt1 = SAPService.PostSAPDocumentNOinAapleSarkar(suserid, sserviceid, strackid);
                    if (dt1.Rows.Count > 0)
                    {
                        string sappno, sSerday;
                        sappno = dt1.Rows[0]["APP_NO"].ToString();
                        sSerday = dt1.Rows[0]["SERVICE_DAYS"].ToString();
                        if (sappno.Trim() != "")
                        {
                            if (sSerday.Trim() == "")
                            {
                                sSerday = "80";
                            }
                            if (Convert.ToInt64(sSerday) == 0)
                            {
                                sSerday = "80";
                            }
                            sresult = SAPService.createDoc(suserid.Trim(), sserviceid.Trim(), strackid.Trim(), sappno.Trim(), sSerday.Trim());
                        }
                    }
                }
                List<ErrorLogResponse> errorLog = new List<ErrorLogResponse>();

                foreach (DataRow row in dtDoc.Rows)
                {
                    ErrorLogResponse errorLogResponse = new ErrorLogResponse
                    {
                        Tid = Convert.ToInt32(row["Tid"]),
                        Trackid = row["Trackid"].ToString(),
                        Userid = row["Userid"].ToString(),
                        ServiceId = row["ServiceId"].ToString(),
                        ApplicationId = row["ApplicationId"].ToString(),
                        ServiceDate = row["ServiceDate"].ToString(),
                        RejectReason = row["RejectReason"].ToString()
                    };

                    errorLog.Add(errorLogResponse);
                }
                return errorLog;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }


        }

        public class DataAccess
        {
            #region Variable Declaration

            SqlConnection _connection;
            SqlCommand _command;
            SqlDataAdapter _Adapter;
            DataTable _DT;
            int _count;

            string strConnectionString = "";
            bool handleErrors = false;
            string strLastError = "";
            SqlCommand cmd = new SqlCommand();

            #endregion
            private readonly string _connString;
            public DataAccess(IConfiguration configuration)
            {
                _connString = getDecryptedConnectstring(configuration.GetConnectionString("EODBConnectString"));
            }

            public int ExecuteQuery(string _query)
            {
                try
                {

                    _connection = new SqlConnection(_connString);
                    _connection.Open();
                    _command = new SqlCommand(_query, _connection);
                    _count = _command.ExecuteNonQuery();
                    _connection.Close();
                    return _count;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    _connection.Close();
                }

            }


            public DataTable GetDataTable(string _query)
            {

                try
                {
                    _DT = new DataTable();
                    _connection = new SqlConnection(_connString);
                    _command = new SqlCommand(_query, _connection);
                    _Adapter = new SqlDataAdapter(_command);
                    _Adapter.Fill(_DT);
                    return _DT;

                }
                catch (Exception ex)
                {

                    throw ex;

                }
                finally
                {
                    _DT.Dispose();
                }
            }

            public DataAccess()
            {
                strConnectionString = _connString;
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = strConnectionString;
                cmd.Connection = cnn;
            }

            public DataAccess(String sSelectDataBase)
            {
                if (sSelectDataBase == "")
                {
                    strConnectionString = _connString;
                }
                else
                {
                    strConnectionString = sSelectDataBase;
                }
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = strConnectionString;
                cmd.Connection = cnn;
            }

            public DataAccess(Boolean SetSP)
            {
                strConnectionString = _connString;
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = strConnectionString;
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
            }

            public DataAccess(Boolean SetSP, String sSelectDataBase)
            {
                if (sSelectDataBase == "")
                {
                    strConnectionString = _connString;
                }
                else
                {
                    strConnectionString = sSelectDataBase;
                }
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = strConnectionString;
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
            }

            public DataTable GetSPDataTable(string _query, ArrayList Param, string _conString)
            {
                SqlConnection _connection;
                SqlCommand _command;
                SqlDataAdapter _Adapter;

                if (_conString != null && _conString != "")
                {
                    _connection = new SqlConnection(_conString);
                }
                else
                {
                    _connection = new SqlConnection(_connString);
                }


                try
                {


                    _connection.Open();
                    _command = new SqlCommand(_query, _connection);
                    _command.CommandType = CommandType.StoredProcedure;


                    // int c=sqlParamColl.Count;
                    if (Param != null)
                    {
                        SqlCommandBuilder.DeriveParameters(_command);
                        SqlParameterCollection sqlParamColl = _command.Parameters;
                        int count = Param.Count;

                        for (int i = 0; i < count; i++)
                        {
                            sqlParamColl[i + 1].Value = Param[i];
                        }
                    }
                    DataTable dt = new DataTable();
                    _Adapter = new SqlDataAdapter(_command);
                    _Adapter.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _connection.Close();
                }


            }

            public int ExecuteProcedure(string sProcName, ArrayList Param, string constr = "")
            {
                dynamic sTempTest = "";
                SqlConnection sCon;
                if (constr != null && constr != "")
                {
                    sCon = new SqlConnection(getDecryptedConnectstring(constr));
                }
                else
                {
                    sCon = new SqlConnection(_connString);
                }
                SqlParameterCollection sParamcoll = default(SqlParameterCollection);
                try
                {
                    //sCon.ConnectionString = sconstr;
                    sCon.Open();
                    SqlCommand scom = new SqlCommand(sProcName, sCon);
                    scom.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(scom);
                    sParamcoll = scom.Parameters;
                    int count = Param.Count;
                    sTempTest = "Exec " + sProcName;
                    for (int i = 0; i <= count - 1; i++)
                    {
                        sParamcoll[i + 1].Value = Param[i];
                        sTempTest = sTempTest + " '" + Param[i].ToString().Trim() + "',";
                    }
                    scom.CommandTimeout = 380;
                    count = scom.ExecuteNonQuery();
                    sCon.Close();
                    return 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sCon.Close();
                }

            }

            public IDataReader ExecuteReader()
            {
                IDataReader reader = null;
                try
                {
                    this.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    this.Close();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }
                return reader;
            }

            public IDataReader ExecuteReader(string commandtext)
            {
                IDataReader reader = null;
                try
                {
                    cmd.CommandText = commandtext;
                    reader = this.ExecuteReader();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return reader;
            }

            public object ExecuteScalar()
            {
                object obj = null;
                try
                {
                    this.Open();
                    obj = cmd.ExecuteScalar();
                    this.Close();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return obj;
            }

            public static string getDecryptedConnectstring(string sConText)
            {
                //code to encrypt and decrypt connection string
                string sconnectstr;
                sconnectstr = sConText;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(sconnectstr);
                string sEncryptpass = builder.Password;
                string sDecryptpass = Decrypt(sEncryptpass);
                sconnectstr = sconnectstr.Replace(sEncryptpass, sDecryptpass);
                return sconnectstr;
            }

            public static string Encrypt(string clearText)
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }

            public static string Decrypt(string cipherText)
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }


            public object ExecuteScalar(string commandtext)
            {
                object obj = null;
                try
                {
                    cmd.CommandText = commandtext;
                    obj = this.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return obj;
            }

            public int ExecuteNonQuery()
            {
                int i = 0;
                try
                {
                    this.Open();
                    i = cmd.ExecuteNonQuery();
                    this.Close();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return i;
            }

            public Int64 InsertRowWithIdentity(String commandtext)
            {
                Int64 Identity = 0;
                object obj = null;
                try
                {
                    cmd.CommandText = commandtext + " SELECT @@IDENTITY AS [NewID]";
                    obj = this.ExecuteScalar();
                    Identity = Convert.ToInt64(obj);
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }
                return Identity;
            }

            public int ExecuteNonQuery(string commandtext)
            {
                int i = 0;
                try
                {
                    cmd.CommandText = commandtext;
                    i = this.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return i;
            }

            public DataSet ExecuteDataSet(String[] tableNames)
            {
                SqlDataAdapter da = null;
                DataSet ds = null;
                try
                {
                    da = new SqlDataAdapter();
                    da.SelectCommand = (SqlCommand)cmd;
                    ds = new DataSet();
                    if (tableNames.Length > 0)
                    {
                        foreach (String table in tableNames)
                            da.Fill(ds, table);
                    }
                    else
                    {
                        da.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return ds;
            }

            public DataSet ExecuteDataSet(string commandtext, string[] tableNames)
            {
                DataSet ds = null;
                try
                {
                    cmd.CommandText = commandtext;
                    ds = this.ExecuteDataSet(tableNames);
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return ds;
            }

            public DataTable ExecuteDataTable()
            {
                SqlDataAdapter da = null;
                DataTable dt = null;
                try
                {
                    this.Open();
                    da = new SqlDataAdapter();
                    da.SelectCommand = (SqlCommand)cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    this.Close();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else
                        throw;
                }
                catch
                {
                    throw;
                }

                return dt;
            }

            public DataTable ExecuteDataTable(string commandtext)
            {
                DataTable dt = null;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = commandtext;
                    dt = this.ExecuteDataTable();
                }
                catch (Exception ex)
                {
                    if (handleErrors)
                        strLastError = ex.Message;
                    else

                        throw;
                }
                catch
                {
                    throw;
                }

                return dt;
            }

            public string CommandText
            {
                get
                {
                    return cmd.CommandText;
                }
                set
                {
                    cmd.CommandText = value;
                    cmd.Parameters.Clear();
                }
            }

            public IDataParameterCollection Parameters
            {
                get
                {
                    return cmd.Parameters;
                }
            }

            public void AddParameter(string paramname, object paramvalue)
            {
                SqlParameter param = new SqlParameter(paramname, paramvalue);
                cmd.Parameters.Add(param);
            }

            public void AddParameter(IDataParameter param)
            {
                cmd.Parameters.Add(param);
            }


            public string ConnectionString
            {
                get
                {
                    return strConnectionString;
                }
                set
                {
                    strConnectionString = value;
                }
            }

            private void Open()
            {
                cmd.Connection.Open();
            }

            private void Close()
            {
                cmd.Connection.Close();
            }

            public bool HandleExceptions
            {
                get
                {
                    return handleErrors;
                }
                set
                {
                    handleErrors = value;
                }
            }

            public string LastError
            {
                get
                {
                    return strLastError;
                }
            }

            public void Dispose()
            {
                cmd.Dispose();
            }
        }
    }
}
