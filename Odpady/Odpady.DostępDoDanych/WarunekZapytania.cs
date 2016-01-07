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
        object _wartość;

        public object Wartość
        {
            get { return _wartość; }

            set
            {
                if (value is string)
                {
                    string napisWartości = value.ToString();

                    if (!napisWartości.StartsWith("'"))
                        napisWartości = String.Concat("'", napisWartości);

                    if (!napisWartości.EndsWith("'"))
                        napisWartości = String.Concat(napisWartości, "'");

                    _wartość = napisWartości;
                }
                else
                    _wartość = value;
            }
        }

        public string NazwaPola { get; set; }
        public ZnakPorównania Znak { get; set; }

        public WarunekZapytania()
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

            switch (Znak)
            {
                case ZnakPorównania.RównaSię:
                    format = "{0} = {1}";

                    break;

                case ZnakPorównania.Zawiera:
                    format = "{0} CONTAINING {1}";

                    break;

                default:
                    format = null;

                    break;
            }

            budowniczyZapytania.AppendFormat(format, NazwaPola, Wartość);
        }
    }
}