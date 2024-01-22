// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Health.Common.DependencyInjection;
using Microsoft.Health.FitOnFhir.Common.Config;
using Microsoft.Health.FitOnFhir.Common.Providers;
using Microsoft.Health.FitOnFhir.Common.Repositories;
using Microsoft.Health.FitOnFhir.Common.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IImportTriggerMessageService, ImportTriggerMessageService>();
        services.AddSingleton<IUsersTableRepository, UsersTableRepository>();
        services.AddConfiguration<AzureConfiguration>(context.Configuration);
        services.AddSingleton<ITableClientProvider, TableClientProvider>();
        services.AddSingleton<ITokenCredentialProvider, TokenCredentialProvider>();
    })
    .Build();

host.Run();
