using System;
using System.Collections.Generic;
using XperiCad.Common.Infrastructure.DataSource;

namespace XperiCad.Common.Core.DataSource
{
    internal class Entity : IEntity
    {
        #region Properties
        public Guid Id { get; }
        public IDictionary<string, object> Attributes { get; }
        #endregion

        #region ctor
        public Entity(
            Guid id,
            IDictionary<string, object> attributes)
        {
            Id = id;
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return Id.ToString();
        }
        #endregion
    }
}
