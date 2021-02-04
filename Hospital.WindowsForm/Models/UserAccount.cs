using Hospital.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.WindowsForm.Models
{
    public class UserAccount
    {
        public static User user { get; set; } = null;
        public static DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public int  Marks { get; set; }
    }
}
