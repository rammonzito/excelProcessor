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
        public User(long registryCode, string email, string name, string last_name)
        {
            this.registry_code = registryCode;
            this.email = email.Trim();
            this.first_name = name;
            this.last_name = last_name;

            accounts_id = 25696;
            user_groups_id = "3";
            status = "active";
        }

        public long registry_code { get; set; }
        public string first_name { get; set; }
        public string status { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }

        public long accounts_id { get; set; }
        public string user_groups_id { get; set; }
    }
}
