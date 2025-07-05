using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.DataSeed
{
    public interface IDataSeed
    {
        Task DataSeedAsync();
        Task IdentityDataSeedAsync();
    }
}
