// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using EnsureThat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Health.Common.DependencyInjection;
using Microsoft.Health.FitOnFhir.Common.Config;
using Microsoft.Health.FitOnFhir.Common.Providers;
using Microsoft.Health.FitOnFhir.Common.Repositories;

namespace Microsoft.Health.FitOnFhir.Common
{
    public abstract class StartupBase
    {
        public void Configure(IHostBuilder hostBuilder)
        {
            EnsureArg.IsNotNull(hostBuilder, nameof(hostBuilder));

            hostBuilder.ConfigureServices((context, services) =>
            {
                IConfiguration config = context.Configuration;

                services.AddLogging();
                services.AddConfiguration<AzureConfiguration>(config);
                services.AddSingleton<ITokenCredentialProvider, TokenCredentialProvider>();
                services.AddSingleton<ITableClientProvider, TableClientProvider>();
                services.AddSingleton<IUsersTableRepository, UsersTableRepository>();

                ConfigureServices(services, config);
            });
        }

        public abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}