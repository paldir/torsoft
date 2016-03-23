using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace raks
{
    public class Program
    {
        private static void Main()
        {
            DataTable kontrahenci = new DataTable();

            using (OleDbConnection połączenie = new OleDbConnection($@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={Path.Combine(Environment.CurrentDirectory, "Dbf")};Extended Properties=dBASE IV;"))
            {
                połączenie.Open();

                using (OleDbCommand komenda = new OleDbCommand("SELECT * FROM KONTRA.DBF", połączenie))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(komenda))
                    adapter.Fill(kontrahenci);


            }
        }
    }
}