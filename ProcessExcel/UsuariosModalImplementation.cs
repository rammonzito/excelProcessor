using ProcessExcel.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProcessExcel
{
    public class UsuariosModalImplementation : IData
    {
        readonly ExternalService externalService = new CoreServiceEleven();

        public User Prepare(DataRow data)
        {
            return PrepareUser(data);
        }

        public void Process(List<User> Users)
        {
            foreach (var user in Users)
                externalService.ProcessService(user);
        }

        #region privates

        private User PrepareUser(DataRow data)
        {
            var arrayName = data.ItemArray[1].ToString().Split(" ");
            string name = arrayName[0];
            List<string> keyname = new();
            keyname.Add(arrayName[0]);

            arrayName = arrayName.Except(keyname).ToArray();
            string last_name = string.Join(" ", arrayName);
            string registryCode = data.ItemArray[2].ToString();
            string email = data.ItemArray[3].ToString();

            User user = new(registryCode, email, name, last_name);
            return user;
        }

        #endregion
    }
}
