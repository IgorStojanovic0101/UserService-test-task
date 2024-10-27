using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application.Abstraction
{
    public interface IErrorService
    {
        void AddErrorRecord(string message, Exception exception);
    }
}
