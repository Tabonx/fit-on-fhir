// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Azure.Functions.Worker;
using Microsoft.Health.FitOnFhir.Common;

namespace Microsoft.Health.FitOnFhir.ImportTimerTrigger
{
    public class DispatchedMessages
    {
        [QueueOutput(Constants.ImportDataQueueName, Connection = "AzureWebJobsStorage")]
        public IEnumerable<string> Messages { get; set; }
    }
}
