using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using ttm3._0.Models;

namespace TVA.Helper
{
    public class myHelper
    {
        public static cleve ConvertToEVE(tbSetting setting)
        {
            cleve eve = new cleve();
            string[] lst = setting.Value.Split(';').ToArray();
            if (lst.Count() == 6)
            {
                eve.Id = 1;
                eve.Ip = lst[0];
                eve.Username = lst[1];
                eve.Password = lst[2];
                eve.RePassword = lst[2];
                eve.Path = lst[3];
                eve.UsernameEve = lst[4];
                eve.PasswordEve = lst[5];
                eve.RePasswordEve = lst[5];
            }
            return eve;
        }
        public static int ConvertToInt(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            string stNumber = new string(s.Where(Char.IsDigit).ToArray());
            int number;
            if (int.TryParse(stNumber, out number))
                return number;
            return 0;
        }
        public static double ConvertToDouble(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            string stNumber = new string(s.Where(p=>p!=',').ToArray());
            double number;
            if (double.TryParse(stNumber, out number))
                return number;
            return 0;
        }
        public static DateTime? ConvertToDatetime(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
                    "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy"};
            DateTime dt = new DateTime();
            if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;
            return null;
        }      
    }
}