using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Interfaces
{
    public interface ICsvExportService
    {
        byte[] Export<T>(IEnumerable<T> data);
    }
}
