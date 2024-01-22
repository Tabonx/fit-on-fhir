// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Health.Common.DependencyInjection;
using Microsoft.Health.FitOnFhir.Common;
using Microsoft.Health.FitOnFhir.Common.Config;
using Microsoft.Health.FitOnFhir.Common.ExtensionMethods;
using Microsoft.Health.FitOnFhir.Common.Handlers;
using Microsoft.Health.FitOnFhir.Common.Interfaces;
using Microsoft.Health.FitOnFhir.Common.Providers;
using Microsoft.Health.FitOnFhir.Common.Repositories;
using Microsoft.Health.FitOnFhir.Common.Requests;
using Microsoft.Health.FitOnFhir.Common.Services;
using Microsoft.Health.FitOnFhir.GoogleFit.Client;
using Microsoft.Health.FitOnFhir.GoogleFit.Client.Config;
using Microsoft.Health.FitOnFhir.GoogleFit.Client.Handlers;
using Microsoft.Health.FitOnFhir.GoogleFit.Client.Telemetry;
using Microsoft.Health.FitOnFhir.GoogleFit.Repositories;
using Microsoft.Health.FitOnFhir.GoogleFit.Services;
using Microsoft.Health.Logging.Telemetry;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddConfiguration<GoogleFitAuthorizationConfiguration>(context.Configuration);
        services.AddConfiguration<GoogleFitDataImporterConfiguration>(context.Configuration);
        services.AddSingleton<IGoogleFitClient, GoogleFitClient>();
        services.AddSingleton<ISecretClientProvider, SecretClientProvider>();
        services.AddSingleton<IUsersKeyVaultRepository, UsersKeyVaultRepository>();
        services.AddSingleton<IEventHubProducerClientProvider, EventHubProducerClientProvider>();
        services.AddSingleton<IGoogleFitAuthService, GoogleFitAuthService>();
        services.AddSingleton<IGoogleFitUserTableRepository, GoogleFitUserTableRepository>();
        services.AddSingleton<IUsersService, UsersService>();
        services.AddSingleton<IErrorHandler, ErrorHandler>();
        services.AddSingleton<IImporterService, ImporterService>();
        services.AddSingleton<GoogleFitDataImportHandler>();
        services.AddSingleton<UnknownDataImportHandler>();
        services.AddSingleton<IGoogleFitImportService, GoogleFitImportService>();
        services.AddSingleton<GoogleFitImportOptions>();
        services.AddSingleton<GoogleFitExceptionTelemetryProcessor>();
        services.AddSingleton<ITelemetryLogger, TelemetryLogger>();
        services.AddSingleton<IGoogleFitDataImporter, GoogleFitDataImporter>();
        services.AddSingleton<IGoogleFitTokensService, GoogleFitTokensService>();
        services.AddSingleton(typeof(Func<DateTimeOffset>), () => DateTimeOffset.UtcNow);
        services.AddSingleton(sp => sp.CreateOrderedHandlerChain<ImportRequest, Task<bool?>>(typeof(GoogleFitDataImportHandler), typeof(UnknownDataImportHandler)));
        services.AddSingleton<IUsersTableRepository, UsersTableRepository>();
        services.AddConfiguration<AzureConfiguration>(context.Configuration);
        services.AddSingleton<ITableClientProvider, TableClientProvider>();
        services.AddSingleton<ITokenCredentialProvider, TokenCredentialProvider>();
    })
    .Build();

host.Run();
