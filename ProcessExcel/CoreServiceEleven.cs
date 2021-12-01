using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel
{
    public class CoreServiceEleven : ExternalServiceFacade
    {
        public override bool CreateContact(User user)
        {
            throw new NotImplementedException();
        }

        public override bool ExistsContactByCpf(long cpf)
        {
            return true;
            // throw new NotImplementedException();
        }

        public override bool ExistsContactByEmail(string email)
        {
            return true;
            // throw new NotImplementedException();
        }

        public override bool IsValid(User user)
        {
            return !(ExistsContactByCpf(user.registry_code) && ExistsContactByEmail(user.email));
        }

        public override bool ProcessService(User user)
        {
            if (IsValid(user))
                return CreateContact(user);
            else
                return false;
        }
    }
}
