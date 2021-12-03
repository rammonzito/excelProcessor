using ProcessExcel.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel
{
    public interface IData
    {
        User Prepare(DataRow data);
        void Process(List<User> Users);
    }
}
