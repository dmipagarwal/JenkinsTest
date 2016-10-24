using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXPOC_JWT.Common
{
    public class TokenViewModel
    {
        public string key_name { get; set; }
        public string key_value { get; set; }
        public DateTime creation_date { get; set; }
    }
}
