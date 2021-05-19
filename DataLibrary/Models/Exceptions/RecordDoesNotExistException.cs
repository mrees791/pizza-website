using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Exceptions
{
    public class RecordDoesNotExistException : Exception
    {
        public RecordDoesNotExistException(string message) : base(message)
        {

        }
    }
}
