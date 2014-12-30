using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DataAccess
{
    interface IItem
    {
        void Add();
        void Edit();
        void Remove();
        void Set(string[] record);
        string Validate(string[] record);
    }
}