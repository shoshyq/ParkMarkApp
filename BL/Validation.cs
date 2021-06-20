using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BL
{
    public class Validation
    {
        //phone
        public static bool PhoneNum(string ph)
        {
            string pattern = @"^\+?(972|0)(\-)?0?(([23489]{1}\d{7})|[5]{1}\d{8})$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(ph);
        }
        public static bool IsHebrew(string text)
        {
            string pattern = @"\b[א-ת-\s ]+$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(text);

        }        
        //email
        public static bool Email(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if(regex.IsMatch(email))
                return true;
            return false;
        }
        //numbers
        public static bool Numbers(string num)
        {
            Regex reg = new Regex(@"[0-9]+$");
            return reg.IsMatch(num);
        }
        //NaN
        public static bool NaN(string str)
        {
            Regex regex = new Regex("^[^0-9]+$");
            return regex.IsMatch(str);
        }
        //between 4-30 , digits and English letters, allow _ and -
        public static bool Username(string username)
        {
            Regex regex = new Regex(@"^(?=.{4,30}$)(?:[a-zA-Z\d]+(?:(?:\.|-|_)[a-zA-Z\d])*)+$");
            return regex.IsMatch(username);
        }
        //hebrew and english letters numbers and whitespaces
        public static bool Text(string str)
        {
            Regex regex = new Regex(@"^[0-9a-zA-Zא-ת\s]*$");
            return regex.IsMatch(str);
        }

    }
}
