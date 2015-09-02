using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DostępDoBazy
{
    public abstract class Rekord
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int __record { get; private set; }

        /*public abstract void Ustaw(string[] rekord);
        public abstract string Waliduj(Enumeratory.Akcja akcja, string[] rekord);*/

        public virtual IEnumerable<string> PolaDoTabeli()
        {
            return new string[] { __record.ToString() };
        }
    }
}