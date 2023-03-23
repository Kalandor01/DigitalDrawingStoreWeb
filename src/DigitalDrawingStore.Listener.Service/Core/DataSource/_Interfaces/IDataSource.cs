using System.Collections.Generic;
using System.Data;

namespace XperiCad.Common.Infrastructure.DataSource
{
    public interface IDataSource
    {
        int PerformCommand(string command);
        int PerformCommand(string command, IEnumerable<IDataParameter> parameters);
        IEnumerable<IEntity> PerformQuery(string command, params string[] attributes);
        IEnumerable<IEntity> PerformQuery(string command, IEnumerable<IDataParameter> parameters, params string[] attributes);
    }
}
