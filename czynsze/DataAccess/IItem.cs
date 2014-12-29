using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DataAccess
{
    interface IItem
    {
        void Add(string[] record);
        void Edit(string[] record);
        void Remove(string[] record);
        void Set(string[] record);
        string Validate(string[] record);
    }
}