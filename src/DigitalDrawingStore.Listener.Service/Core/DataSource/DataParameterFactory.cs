using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using XperiCad.Common.Infrastructure.DataSource;

namespace XperiCad.Common.Core.DataSource
{
    internal class DataParameterFactory : IDataParameterFactory
    {
        #region Fields
        private IList<IDictionary<string, object>> _parameters;
        #endregion

        #region ctor
        public DataParameterFactory()
        {
            _parameters = new List<IDictionary<string, object>>();
        }
        #endregion

        #region IDataParameterFactory members
        public IDataParameter CreateParameter(string parameterName, SqlDbType sqlDbType, object value, int size)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var parameter = new SqlParameter(parameterName, sqlDbType, size);
            parameter.Value = value;

            return parameter;
        }

        public IDataParameterFactory ConfigureParameter(string parameterName, SqlDbType sqlDbType, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException($"'{nameof(parameterName)}' cannot be null or whitespace.", nameof(parameterName));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return ConfigureParameter(parameterName, sqlDbType, value, null);
        }

        public IDataParameterFactory ConfigureParameter(string parameterName, SqlDbType sqlDbType, object value, int size)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException($"'{nameof(parameterName)}' cannot be null or whitespace.", nameof(parameterName));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return ConfigureParameter(parameterName, sqlDbType, value, new int?(size));
        }

        public IEnumerable<IDataParameter> GetConfiguredParameters()
        {
            var sqlParameters = new List<IDataParameter>();

            foreach (var parameter in _parameters)
            {
                var parameterName = parameter["parameterName"].ToString();
                var sqlDbType = (SqlDbType)parameter["sqlDbType"];
                var value = parameter["value"];
                var sqlParameter = default(SqlParameter);

                if (parameter.ContainsKey("size"))
                {
                    var size = (int)parameter["size"];
                    sqlParameter = new SqlParameter(parameterName, sqlDbType, size);
                }
                else
                {
                    sqlParameter = new SqlParameter(parameterName, sqlDbType);
                }
                sqlParameter.Value = value;

                sqlParameters.Add(sqlParameter);
            }

            _parameters.Clear();
            return sqlParameters;
        }
        #endregion

        #region Private members
        private IDataParameterFactory ConfigureParameter(string parameterName, SqlDbType sqlDbType, object value, int? size)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("parameterName", parameterName);
            dictionary.Add("sqlDbType", sqlDbType);
            if (size != null)
            {
                dictionary.Add("size", size);
            }
            dictionary.Add("value", value);

            _parameters.Add(dictionary);

            return this;
        }
        #endregion
    }
}
