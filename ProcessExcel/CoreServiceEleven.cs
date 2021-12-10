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
        readonly int contractId = MainConstants.ContractId;

        public CoreServiceEleven()
        {
            Sender = new Sender();
        }

        public override void ProcessService(List<User> users)
        {
            //foreach (var user in users)
            //    CheckIdValid(user);

            ProcessAll(users);

            SaveLog();
        }

       
        public override void CheckIdValid(User user)
        {
            bool byCpf = ExistsContactByCpf(user.registry_code);
            bool byEmail = ExistsContactByEmail(user.email);

            if (byCpf)
                existentCpf.Add(user.registry_code);

            if (byEmail)
                existentEmail.Add(user.email);

            user.Valid = !byCpf && !byEmail;
        }

        #region privates
        private void SaveLog()
        {
            StringBuilder sb = new StringBuilder();
            existentCpf.AddRange(existentEmail);
            existentCpf.ForEach(e => sb.AppendLine($"Conta já existente: {e}"));

            MyLog.SaveLog(sb);
        }

        private void ProcessAll(List<User> users)
        {
            try
            {
                foreach (var user in users?.Where(u => u.Valid))
                {
                    var created = CreateUser(user);
                    if (created != null)
                        createdList?.Add(created);
                }

                if (createdList?.Count > 0)
                {
                    RelateContract(createdList, contractId);
                    createdList?.ForEach(u => SendEmail(u.global_id));
                }
            }
            catch (Exception)
            {
            }
        }

        private CreatedUser CreateUser(User user)
        {
            CreatedUser createdUser = null;
            try
            {
                createdUser = Sender.SendAsync<CreatedUser>($"{MainConstants.urlBaseBackAdmin}/", user, HttpMethod.Post, MainConstants.token).Result;
            }
            catch (Exception ex)
            {
                SaveLog($"Não consegui criar com o user {user.ToString()}");
            }
            return createdUser;
        }

        private bool RelateContract(List<CreatedUser> users, int contractId)
        {
            ContractRequest contractRequest = new();
            users.ForEach(u => contractRequest.access.Add(new Access() { contact_id = u.id }));

            try
            {
                var relatedContract = Sender.SendAsync<object>($"{MainConstants.urlBackBase}/{contractId}", contractRequest, HttpMethod.Post, MainConstants.token).Result;
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        private bool SendEmail(int id)
        {
            try
            {
                var sent = Sender.SendAsync<object>($"{MainConstants.urlBaseBackAdmin}/{id}/reset-password", new { }, HttpMethod.Get, MainConstants.token).Result;
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void SaveLog(string erro)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(erro);
            MyLog.SaveLog(sb);
        }


        #endregion
    }
}
