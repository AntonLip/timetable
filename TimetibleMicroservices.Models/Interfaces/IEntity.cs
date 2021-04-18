using System;
using System.Collections.Generic;
using System.Text;

namespace TimetibleMicroservices.Models.Interfaces
{

    public interface IEntity<TId>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TId Id { get; set; }
    }
}
