// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Health.Common.DependencyInjection;
using Microsoft.Health.Extensions.Fhir;
using Microsoft.Health.Extensions.Fhir.Service;
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
using Microsoft.Health.FitOnFhir.GoogleFit.Repositories;
using Microsoft.Health.FitOnFhir.GoogleFit.Services;
using Microsoft.Health.Logging.Telemetry;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddConfiguration<GoogleFitAuthorizationConfiguration>(context.Configuration);
        services.AddConfiguration<AuthenticationConfiguration>(context.Configuration);
        services.AddSingleton<IGoogleFitClient, GoogleFitClient>();
        services.AddSingleton<ISecretClientProvider, SecretClientProvider>();
        services.AddSingleton<IQueueClientProvider, QueueClientProvider>();
        services.AddSingleton<IBlobContainerClientProvider, BlobContainerClientProvider>();
        services.AddSingleton<IUsersKeyVaultRepository, UsersKeyVaultRepository>();
        services.AddSingleton<IGoogleFitUserTableRepository, GoogleFitUserTableRepository>();
        services.AddSingleton<IUsersService, UsersService>();
        services.AddSingleton<IGoogleFitTokensService, GoogleFitTokensService>();
        services.AddSingleton<IGoogleFitAuthService, GoogleFitAuthService>();
        services.AddSingleton<IRoutingService, RoutingService>();
        services.AddHttpClient<IOpenIdConfigurationProvider, OpenIdConfigurationProvider>();
        services.AddSingleton<ITokenValidationService, TokenValidationService>();
        services.AddSingleton<IJwtSecurityTokenHandlerProvider, JwtSecurityTokenHandlerProvider>();
        services.AddSingleton<IAuthStateService, AuthStateService>();
        services.AddSingleton<ITelemetryLogger, TelemetryLogger>();
        services.AddFhirClient(context.Configuration);
        services.AddSingleton<IFhirService, FhirService>();
        services.AddSingleton<ResourceManagementService>();
        services.AddSingleton<GoogleFitAuthorizationHandler>();
        services.AddSingleton<UnknownAuthorizationHandler>();
        services.AddSingleton<IQueueService, QueueService>();
        services.AddSingleton(typeof(Func<DateTimeOffset>), () => DateTimeOffset.UtcNow);
        services.AddSingleton(sp => sp.CreateOrderedHandlerChain<RoutingRequest, Task<IActionResult>>(typeof(GoogleFitAuthorizationHandler), typeof(UnknownAuthorizationHandler)));
        services.AddSingleton<IUsersTableRepository, UsersTableRepository>();
        services.AddConfiguration<AzureConfiguration>(context.Configuration);
        services.AddSingleton<ITableClientProvider, TableClientProvider>();
        services.AddSingleton<ITokenCredentialProvider, TokenCredentialProvider>();
    })
    .Build();

host.Run();
