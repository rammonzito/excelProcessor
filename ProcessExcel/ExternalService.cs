using ProcessExcel.Model;

namespace ProcessExcel
{
    public abstract class ExternalService
    {
        public abstract void ProcessService(User user);
        public abstract bool IsValid(User user);
    }
}
