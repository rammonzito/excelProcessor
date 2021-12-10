using ProcessExcel.Model;
using System.Collections.Generic;

namespace ProcessExcel
{
    public abstract class ExternalService
    {
        public abstract void ProcessService(List<User> users);
        public abstract void CheckIdValid(User user);
    }
}
