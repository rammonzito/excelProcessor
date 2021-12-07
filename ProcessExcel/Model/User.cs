using ProcessExcel.Contants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel.Model
{
    public class User
    {
        public User(string registryCode, string email, string name, string last_name)
        {
            this.registry_code = registryCode;
            this.email = email.Trim();
            this.first_name = name;
            this.last_name = last_name;

            accounts_id = MainConstants.accountsId;
            user_groups_id = MainConstants.userGroupsId;
            status = "active";
        }

        public string registry_code { get; set; }
        public string first_name { get; set; }
        public string status { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }

        public long accounts_id { get; set; }
        public string user_groups_id { get; set; }
    }
}
