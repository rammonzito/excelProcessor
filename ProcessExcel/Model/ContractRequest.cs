using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel.Model
{
    public class ContractRequest
    {
        public bool preserve_items { get; set; } = true;
        public bool preserve_access { get; set; } = true;
        public bool preserve_discounts { get; set; } = true;
        public int origin { get; set; } = 2;
        public List<Access> access { get; set; } = new();

    }

    public class Access
    {
        public int origin { get; set; } = 2; 
        public int contact_id { get; set; }
        public int products_id { get; set; } = 74;
    }

}
