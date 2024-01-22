// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Health.FitOnFhir.Common.Services;

namespace Microsoft.Health.FitOnFhir.ImportTimerTrigger
{
    public class ImportTimerTriggerFunction
    {
        private readonly IImportTriggerMessageService _messageService;

        private readonly ILogger _logger;

        public ImportTimerTriggerFunction(
            IImportTriggerMessageService messageService,
            ILoggerFactory loggerFactory)
        {
            _messageService = messageService;
            _logger = loggerFactory.CreateLogger<ImportTimerTriggerFunction>();
        }

        [Function("import-timer")]
        [QueueOutput("import-data")]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task<string[]> Run(
            [TimerTrigger("%SCHEDULE%")] TimerInfo myTimer,
            CancellationToken cancellationToken)
        {
            var queueService = new List<string>();
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            await _messageService.AddImportMessagesToCollector(queueService, cancellationToken);
            return queueService.ToArray();
        }
    }
}
