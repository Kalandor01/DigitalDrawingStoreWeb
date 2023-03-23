using System;
using System.Collections.Generic;

namespace XperiCad.Common.Infrastructure.DataSource
{
    public interface IEntity
    {
        Guid Id { get; }
        IDictionary<string, object> Attributes { get; }
    }
}
