using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using XperiCad.Common.Infrastructure.DataSource;

namespace XperiCad.Common.Core.DataSource
{
    public class MsSqlDataSource : IDataSource
    {
        #region Fields
        private readonly string _connectionString;
        #endregion

        #region ctor
        public MsSqlDataSource(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
        }
        #endregion

        #region IDataSource members
        public int PerformCommand(string command)
        {
            return PerformCommand(command, new List<IDataParameter>());
        }

        public int PerformCommand(string command, IEnumerable<IDataParameter> parameters)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using (var connection = new SqlConnection(_connectionString))
            using (var sqlCommand = new SqlCommand(command, connection))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    var errorMessage = $"Could not open connection by the following connection string: {_connectionString}.";
                    throw new InvalidOperationException(errorMessage, e);
                }

                foreach (var parameter in parameters)
                {
                    sqlCommand.Parameters.Add(parameter);
                }

                try
                {
                    sqlCommand.Prepare();
                }
                catch (Exception e)
                {
                    var errorMessage = $"Could not prepare command: {command}.";
                    throw new InvalidOperationException(errorMessage, e);
                }

                try
                {
                    var queryResult = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                    return queryResult;
                }
                catch (Exception e)
                {
                    var errorMessage = $"Could not execute command: {command}.";
                    throw new InvalidOperationException(errorMessage, e);
                }
            }
        }

        public IEnumerable<IEntity> PerformQuery(string command, params string[] attributes)
        {
            return PerformQuery(command, new List<IDataParameter>(), attributes);
        }

        public IEnumerable<IEntity> PerformQuery(string command, IEnumerable<IDataParameter> parameters, params string[] attributes)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = new List<IEntity>();

            using (var connection = new SqlConnection(_connectionString))
            using (var sqlCommand = new SqlCommand(command, connection))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    var errorMessage = $"Could not open connection: {_connectionString}.";
                    throw new InvalidOperationException(errorMessage, e);
                }

                foreach (var parameter in parameters)
                {
                    sqlCommand.Parameters.Add(parameter);
                }

                try
                {
                    sqlCommand.Prepare();
                }
                catch (Exception e)
                {
                    var errorMessage = $"Could not prepare command: {command}.";
                    throw new InvalidOperationException(errorMessage, e);
                }

                try
                {
                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dictionary = new Dictionary<string, object>();

                            foreach (var attribute in attributes)
                            {
                                dictionary.Add(attribute, dataReader[attribute]);
                            }

                            result.Add(new Entity(new Guid(dataReader["Id"].ToString()), dictionary));
                        }
                    }
                }
                catch (Exception e)
                {
                    var errorMessage = $"Could not read from datasource by the following query: {command}.";
                    throw new InvalidOperationException(errorMessage, e);
                }
            }

            return result;
        }
        #endregion
    }
}
