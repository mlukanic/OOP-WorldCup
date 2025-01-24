using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Interfaces
{
    public interface IErrorReporter
    {
        event Action<string> OnError;
    }
}
