using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    /// <summary>
    /// Converts a PizzaWebsite model to its DataLibrary equivalent.
    /// This is used for creating view models for TEntity database records.
    /// </summary>
    // todo: Update documentation here
    public interface IEntityConverter<TEntity> where TEntity : class, new()
    {
        TEntity ToDbModel();
        void FromDbModel(TEntity dbModel);
    }
}