using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DataAccess
{
    public interface IRecord
    {
        void Set(string[] record);
        IRecord Find(Czynsze_Entities dataBase, int id);
        string Validate(Enums.Action action, string[] record);
    }
}