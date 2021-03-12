using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform
{
    public class Account
    {
            private string APIKEY = "";
            private string APISECRET = "";
            private string DATABASESERVER = "";
            private string DATABASENAME = "";
            private string DATABASEUSERNAME = "";
            private string DATABASEPASSWROD = "";

            public Account()
            {
                Tools tool = new Tools();
                string[] arr = tool.Read_File_ToArray(@"C:\Bitmex\creds.txt");

                if(arr.Length != 0)
                {
                    APIKEY = arr[0];
                    APISECRET = arr[1];
                }
            }

            #region GetSets

            public string ApiKey
            {
                get { return APIKEY; }
                set { APIKEY = value; }
            }
            public string ApiSecret
            {
                get { return APISECRET; }
                set { APISECRET = value; }
            }
            public string DatabaseServer
            {
                get { return DATABASESERVER; }
                set { DATABASESERVER = value; }
            }
            public string DatabaseName
            {
                get { return DATABASENAME; }
                set { DATABASENAME = value; }
            }
            public string DatabaseUsername
            {
                get { return DATABASEUSERNAME; }
                set { DATABASEUSERNAME = value; }
            }
            public string DatabasePassword
            {
                get { return DATABASEPASSWROD; }
                set { DATABASEPASSWROD = value; }
            }

            #endregion GetSets
    }
}
