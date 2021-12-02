using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProcessExcel
{
    public class UsuariosModalImplementation : IData
    {
        public bool Process(DataRow data)
        {
            var arrayName = data.ItemArray[1].ToString().Split(" ");
            string name = arrayName[0];
            List<string> bla = new();
            bla.Add(arrayName[0]);

            arrayName = arrayName.Except(bla).ToArray();
            string last_name = string.Join(" ", arrayName);
            long registryCode = long.Parse(data.ItemArray[2].ToString());
            string email = data.ItemArray[3].ToString();

            User user = new(registryCode, email, name, last_name);
            Console.WriteLine(registryCode);
            Console.WriteLine(email);

            ExternalServiceFacade externalService = new CoreServiceEleven();

            return externalService.ProcessService(user);
        }
    }
}
