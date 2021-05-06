using DataLibrary.Models;
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
    public interface IRecordConverter<TRecord> where TRecord : Record, new()
    {
        TRecord ToRecord();
        void FromRecord(TRecord record);
    }
}