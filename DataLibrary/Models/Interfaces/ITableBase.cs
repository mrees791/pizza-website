using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Interfaces
{
    public interface ITableBase : IInsertable, IUpdatable, IRecord
    {
    }
}