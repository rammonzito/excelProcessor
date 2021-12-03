using Newtonsoft.Json;
using ProcessExcel.Contants;
using ProcessExcel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace ProcessExcel
{
    public class CoreServiceEleven : ExternalService
    {
        public Sender Sender { get; set; }

        public List<string> existentCpf = new();
        public List<string> existentEmail = new();
        public List<CreatedUser> createdList = new();
        readonly int contractId = 0;

        public CoreServiceEleven()
        {
            Sender = new Sender();
        }

        public override void ProcessService(User user)
        {
            if (IsValid(user))
                ProcessAll(user);

            SaveLog();
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

        #region privates
        private void SaveLog()
        {
            StringBuilder sb = new StringBuilder();
            existentCpf.AddRange(existentEmail);
            existentCpf.ForEach(e => sb.Append($";{e}"));

            File.AppendAllText($"{Assembly.GetExecutingAssembly().Location}-NotCreated.txt", sb.ToString());
            sb.Clear();
        }

        private bool ProcessAll(User user)
        {
            try
            {
                createdList.Add(CreateUser(user));

                if (createdList?.Count > 0) {
                    RelateContract(createdList, contractId);
                    SendEmail(createdList.Select(u => u.id).ToList());
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private CreatedUser CreateUser(User user)
        {
            CreatedUser createdUser = new();
            try
            {
                createdUser = Sender.SendAsync<CreatedUser>($"{MainConstants.urlBaseBackAdmin}/", user, HttpMethod.Post, MainConstants.token).Result;
            }
            catch (Exception)
            {
            }
            return createdUser;
        }

        private bool RelateContract(List<CreatedUser> users, int contractId)
        {
            ContractRequest contractRequest = new();
            users.ForEach(u => contractRequest.access.Add(new Access() { contact_id = u.id }));

            try
            {
                var relatedContract = Sender.SendAsync<object>($"{MainConstants.urlBaseBackAdmin}/{contractId}", contractRequest, HttpMethod.Post, MainConstants.token).Result;
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        private bool SendEmail(List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var sent = Sender.SendAsync<object>($"{MainConstants.urlBaseBackAdmin}/{id}/reset-password", new { }, HttpMethod.Get, MainConstants.token).Result;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool ExistsContactByCpf(string cpf)
        {
            try
            {
                var result = Sender.SendAsync<object>($"{MainConstants.urlBaseBackAdmin}/?excludes_accounts_id=1&registry_code={cpf}", null, HttpMethod.Get, MainConstants.token).Result;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool ExistsContactByEmail(string email)
        {
            try
            {
                var user = Sender.SendAsync<object>($"{MainConstants.urlBaseBackAdmin}/?excludes_accounts_id=1&email={email}", null, HttpMethod.Get, MainConstants.token).Result;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        #endregion
    }
}
