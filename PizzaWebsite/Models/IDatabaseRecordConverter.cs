using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    /// <summary>
    /// Converts a PizzaWebsite model to its DataLibrary equivalent.
    /// </summary>
    public interface IDatabaseRecordConverter<T> where T : class, new()
    {
        T ToDbModel();
    }
}