using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DostępDoBazy
{
    public abstract class Rekord
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public abstract int id { get; set; }

        [System.ComponentModel.DataAnnotations.Key]
        public int __record { get; set; }

        public abstract void Ustaw(string[] rekord);
        public abstract string Waliduj(Enumeratory.Akcja akcja, string[] rekord);
        public abstract string[] WszystkiePola();
        public abstract string[] PolaDoTabeli();
    }
}