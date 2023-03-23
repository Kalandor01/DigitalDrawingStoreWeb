using System;
using XperiCad.Common.Infrastructure.Application.DataSource;

namespace XperiCad.Common.Core.Application.DataSource
{
    internal class ApplicationConfigurationService : IApplicationConfigurationService
    {
        #region Properties
        public IApplicationConfigurationQuery Query { get; }
        public IApplicationConfigurationCommand Command { get; }
        #endregion

        #region ctor
        public ApplicationConfigurationService(IApplicationConfigurationQuery query, IApplicationConfigurationCommand command)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }
        #endregion
    }
}
