using System.Collections.Generic;

namespace Statystyka.Generowanie
{
    public class DaneDoWydruku
    {
        public string Tytuł { get; set; }
        public IEnumerable<WierszZestawienia> WierszeZestawienia { get; set; }
    }
}