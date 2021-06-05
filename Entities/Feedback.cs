using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Feedback
    {
        public int Code { get; set; }
        public Nullable<int> DescriberUserCode { get; set; }
        public Nullable<int> DescriptedUserCode { get; set; }
        public string Feedback1 { get; set; }
        public Nullable<int> Rating { get; set; }

    }
}
