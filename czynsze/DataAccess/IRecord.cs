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
        string Validate(Enums.Action action, string[] record);
        string[] AllFields();
    }
}