using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Przychodnia.Class.XML
{
    class ObslugaXml
    {
        public static List<DwaStringi> WczytajKasyChorych()
        {
            XDocument xml = XDocument.Load("Xml/KasyChorych.xml");

            List<DwaStringi> listaKasChorych =
            (
                from dataRow in xml.Root.Elements("dataRow")
                select new DwaStringi
                (
                    dataRow.Element("nazwa").Value,
                    dataRow.Element("wartosc").Value
                )
            ).ToList<DwaStringi>();
            return listaKasChorych;
        }

        public static List<DwaStringi> WczytajPlatnicy()
        {
            XDocument xml = XDocument.Load("Xml/Platnicy.xml");

            List<DwaStringi> listaPlatnicy =
            (
                from dataRow in xml.Root.Elements("dataRow")
                select new DwaStringi
                (
                    dataRow.Element("nazwa").Value,
                    dataRow.Element("wartosc").Value
                )
            ).ToList<DwaStringi>();
            return listaPlatnicy;
        }

        public static List<DwaStringi> WczytajRealizujaceOsoby()
        {
            XDocument xml = XDocument.Load("Xml/RealizujaceOsoby.xml");

            List<DwaStringi> listaRealizujaceOsoby =
            (
                from dataRow in xml.Root.Elements("dataRow")
                select new DwaStringi
                (
                    dataRow.Element("nazwa").Value,
                    dataRow.Element("wartosc").Value
                )
            ).ToList<DwaStringi>();
            return listaRealizujaceOsoby;
        }

        public static List<DwaStringi> WczytajRodzajSwiadczenia()
        {
            XDocument xml = XDocument.Load("Xml/RodzajSwiadczenia.xml");

            List<DwaStringi> listaRodzajSwiadczenia =
            (
                from dataRow in xml.Root.Elements("dataRow")
                select new DwaStringi
                (
                    dataRow.Element("nazwa").Value,
                    dataRow.Element("wartosc").Value
                )
            ).ToList<DwaStringi>();
            return listaRodzajSwiadczenia;
        }

        public static List<DwaStringi> WczytajPoradnie()
        {
            XDocument xml = XDocument.Load("Xml/Poradnie.xml");

            List<DwaStringi> listaPoradn =
            (
                from dataRow in xml.Root.Elements("dataRow")
                select new DwaStringi
                (
                    dataRow.Element("nazwa").Value,
                    dataRow.Element("wartosc").Value
                )
            ).ToList<DwaStringi>();
            return listaPoradn;
        }
    }
}
