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
    // todo: Update documentation here
    // A is DataLibrary class
    public interface IDatabaseRecordConverter<A> where A : class
    {
        A ToDbModel();
        void FromDbModel(A dbModel);
    }
}