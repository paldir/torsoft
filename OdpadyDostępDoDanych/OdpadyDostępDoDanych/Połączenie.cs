using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace OdpadyDostępDoDanych
{
    public class Połączenie
    {
        public Połączenie()
        {
            string parametry =
            "User=SYSDBA;" +
            "Password=masterkey;" +
            @"Database=C:\Users\paldir\Documents\GitHub\torsoft\OdpadyDostępDoDanych\Testy\bin\Debug\odpady.fdb;" +
            "DataSource=localhost;" +
            "Port=3050;" +
            "Dialect=3;" +
            "Charset=NONE;" +
            "Role=;" +
            "Connection lifetime=15;" +
            "Pooling=true;" +
            "MinPoolSize=0;" +
            "MaxPoolSize=50;" +
            "Packet Size=8192;" +
            "ServerType=0";

            FbConnection połączenie = new FbConnection(parametry);

            połączenie.Open();

            FbDataAdapter adapter = new FbDataAdapter("SELECT * FROM address_list", połączenie);
            DataSet zbiór = new DataSet();

            adapter.Fill(zbiór);
        }
    }
}