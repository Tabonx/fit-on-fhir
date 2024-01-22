// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using EnsureThat;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Health.FitOnFhir.Common.Services;

namespace Microsoft.Health.FitOnFhir.Import
{
    public class ImportFunction
    {
        private readonly IImporterService _importerService;

        private readonly ILogger _logger;

        public ImportFunction(IImporterService importerService, ILoggerFactory loggerFactory)
        {
            _importerService = EnsureArg.IsNotNull(importerService);
            _logger = loggerFactory.CreateLogger<ImportFunction>();
        }

        [Function("import-data")]
        public async Task Run(
            [QueueTrigger("import-data")] string message,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("import-data has message: {0}", message);

            await _importerService.Import(message, cancellationToken);
        }
    }
}
