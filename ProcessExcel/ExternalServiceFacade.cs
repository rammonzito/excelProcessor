using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel
{
    public abstract class ExternalServiceFacade
    {
        public abstract bool ProcessService(User user);
        public abstract bool ExistsContactByCpf(long cpf);
        public abstract bool ExistsContactByEmail(string email);
        public abstract bool IsValid(User user);
        public abstract bool CreateContact(User user);
    }

    public class User
    {
        private long registryCode;

        public User(long registryCode, string email)
        {
            this.registryCode = registryCode;
            this.email = email;
        }

        public long registry_code { get; set; }
        public string email { get; set; }
    }
}
