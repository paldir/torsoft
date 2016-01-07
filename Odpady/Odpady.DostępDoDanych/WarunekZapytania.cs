using System;
using System.Text;

namespace Odpady.DostępDoDanych
{
    public enum ZnakPorównania
    {
        RównaSię,
        Zawiera
    }

    public class WarunekZapytania
    {
        public string NazwaPola { get; set; }
        public ZnakPorównania Znak { get; set; }
        public object Wartość { get; set; }

        public WarunekZapytania(string nazwaPola, object wartość) : this(nazwaPola, ZnakPorównania.RównaSię, wartość)
        {
        }

        public WarunekZapytania(string nazwaPola, ZnakPorównania znak, object wartość)
        {
            NazwaPola = nazwaPola;
            Znak = znak;
            Wartość = wartość;
        }

        public void GenerujWarunek(StringBuilder budowniczyZapytania)
        {
            string format;
            object wartośćSql;

            switch (Znak)
            {
                case ZnakPorównania.RównaSię:
                    format = "{0} = {1}";

                    break;

                case ZnakPorównania.Zawiera:
                    format = "{0} CONTAINING LOWER({1})";

                    break;

                default:
                    format = null;

                    break;
            }

            if (Wartość == null)
                wartośćSql = "null";
            else if (Wartość is string)
            {
                string napisWartości = Wartość.ToString();

                if (!napisWartości.StartsWith("'"))
                    napisWartości = String.Concat("'", napisWartości);

                if (!napisWartości.EndsWith("'") || napisWartości.Length == 1)
                    napisWartości = String.Concat(napisWartości, "'");

                wartośćSql = napisWartości;
            }
            else
                wartośćSql = Wartość;

            budowniczyZapytania.AppendFormat(format, NazwaPola, wartośćSql);
        }
    }
}