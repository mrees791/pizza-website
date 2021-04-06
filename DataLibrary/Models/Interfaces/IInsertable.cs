using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Interfaces
{
    public interface IInsertable : IRecord
    {
        void AddInsertItems(List<IInsertable> itemsList);
        void Insert(IDbConnection connection, IDbTransaction transaction = null);
    }
}
