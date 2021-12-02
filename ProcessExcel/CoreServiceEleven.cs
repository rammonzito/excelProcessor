using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace ProcessExcel
{
    public class CoreServiceEleven : ExternalServiceFacade
    {
        public Sender Sender { get; set; }

        public List<long> existentCpf = new List<long>();
        public List<string> existentEmail = new List<string>();
        public CoreServiceEleven()
        {
            Sender = new Sender();
        }

        public override bool CreateContact(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            try
            {
                CreatedUser created = Sender.SendAsync<CreatedUser>($"{Constants.urlBase}", json, HttpMethod.Post, Constants.token).Result;
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public override bool ExistsContactByCpf(long cpf)
        {
            try
            {
                var bla = Sender.SendAsync<object>($"{Constants.urlBase}/?excludes_accounts_id=1&registry_code={cpf}", null, HttpMethod.Get, Constants.token).Result;
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public override bool ExistsContactByEmail(string email)
        {
            try
            {
                var user = Sender.SendAsync<object>($"{Constants.urlBase}/?excludes_accounts_id=1&email={email}", null, HttpMethod.Get, Constants.token).Result;
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public override bool IsValid(User user)
        {
            bool byCpf = ExistsContactByCpf(user.registry_code);
            bool byEmail = ExistsContactByEmail(user.email);

            if (byCpf)
                existentCpf.Add(user.registry_code);

            if (byEmail)
                existentEmail.Add(user.email);


            return !(byCpf && byEmail);
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
