using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotuwDataAccess
{
    public abstract class Record
    {
        public abstract long Id { get; set; }

        public abstract void Add();
        public abstract void Remove();
        public abstract void Set(string[] fields);

        public static WotuwEntities DataBase = new WotuwEntities();

        public void Update()
        {
            DataBase.SaveChanges();
        }
        
        public static Nullable<T> ConvertIfPossible<T>(string from) where T : struct
        {
            if (String.IsNullOrEmpty(from))
                return null;
            else
                return (T)Convert.ChangeType(from, typeof(T));
        }

        public static bool IsConvertible<T>(string from)
        {
            if (!String.IsNullOrEmpty(from))
                try { Convert.ChangeType(from, typeof(T)); }
                catch (FormatException) { return false; }

            return true;
        }
    }
}