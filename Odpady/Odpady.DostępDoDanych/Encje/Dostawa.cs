using System;

namespace Odpady.DostępDoDanych
{
    public class Dostawa : Rekord
    {
        public long? FK_KONTRAHENT { get; set; }
        public DateTime DATA { get; set; }

        public Kontrahent KONTRAHENT
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Kontrahent>(FK_KONTRAHENT.Value); }
        }
    }
}