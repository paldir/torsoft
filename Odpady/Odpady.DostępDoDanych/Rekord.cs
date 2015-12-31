using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace Odpady.DostępDoDanych
{
    public abstract class Rekord
    {
        static Połączenie _połączenieDlaObcychObiektów;

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

        long _id;
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

        static Rekord()
        {
            PołączenieDlaObcychObiektów = new Połączenie();
        }
    }
}