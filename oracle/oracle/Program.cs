using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace oracle
{
    public class Program
    {
        private static void Main()
        {
            string parametry = ConfigurationManager.AppSettings["Parametry"];
            OracleConnection połączenie = new OracleConnection(parametry);

            Console.WriteLine("Connection string: {0}", parametry);

            try
            {
                połączenie.Open();
            }
            catch (Exception e)
            {
                while (e != null)
                {
                    Console.WriteLine(e.Message);

                    e = e.InnerException;
                }
            }

            Console.WriteLine("Naciśnij klawisz,a by kontynuować.");
            Console.ReadKey();
        }
    }
}