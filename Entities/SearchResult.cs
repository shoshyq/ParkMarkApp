using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SearchResult
    {
        public int Code { get; set; }
        public Nullable<int> Usercode { get; set; }
        public Nullable<int> SearchCode { get; set; }
        public Nullable<int> ResultPSCode { get; set; }
    }
}
