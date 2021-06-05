using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {
        public int Code { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public Nullable<int> PaymentDetails1 { get; set; }
        public Nullable<int> PaymentDetails2 { get; set; }
    }
}
