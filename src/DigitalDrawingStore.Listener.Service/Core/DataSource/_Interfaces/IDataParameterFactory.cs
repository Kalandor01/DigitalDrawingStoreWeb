using System.Collections.Generic;
using System.Data;

namespace XperiCad.Common.Infrastructure.DataSource
{
    public interface IDataParameterFactory
    {
        IDataParameter CreateParameter(string parameterName, SqlDbType sqlDbType, object value, int size);
        IDataParameterFactory ConfigureParameter(string parameterName, SqlDbType sqlDbType, object value);
        IDataParameterFactory ConfigureParameter(string parameterName, SqlDbType sqlDbType, object value, int size);
        IEnumerable<IDataParameter> GetConfiguredParameters();
    }
}
