using MD.PersianDateTime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Utilities
{
    public class ConvertDate
    {
        public string ToShamsi(DateTime date, string format)
        {
            PersianDateTime pd = new PersianDateTime(date);
            return pd.ToString(format);
        }
        public DateTime ToMiladi(string date)
        {
            PersianDateTime pd = PersianDateTime.Parse(date);
            return pd.ToDateTime();
        }
    }
}
