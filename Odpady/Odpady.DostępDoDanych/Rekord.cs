using System;
using System.Data;

namespace Odpady.DostępDoDanych
{
    public abstract class Rekord
    {
        private static Połączenie _połączenieDlaObcychObiektów;

        protected static Połączenie PołączenieDlaObcychObiektów
        {
            get
            {
                if (_połączenieDlaObcychObiektów.Stan != ConnectionState.Open)
                    _połączenieDlaObcychObiektów = new Połączenie();

                return _połączenieDlaObcychObiektów;
            }

            private set { _połączenieDlaObcychObiektów = value; }
        }

        private long _id;

        public long ID
        {
            get { return _id; }

            set
            {
                if (_id != 0)
                    throw new Exception("Zmiana ID nie jest dozwolona. - PZ");

                _id = value;
            }
        }

        protected static void UstawObcyObiekt<T>(ref T obiekt, long kluczObcy) where T : Rekord
        {
            if ((obiekt == null) || (kluczObcy != obiekt.ID))
                obiekt = PołączenieDlaObcychObiektów.Pobierz<T>(kluczObcy);
        }

        static Rekord()
        {
            PołączenieDlaObcychObiektów = new Połączenie();
        }
    }
}