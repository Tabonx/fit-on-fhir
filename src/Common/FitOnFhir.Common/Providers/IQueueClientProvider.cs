﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Azure.Storage.Queues;

namespace Microsoft.Health.FitOnFhir.Common.Providers
{
    public interface IQueueClientProvider
    {
        QueueClient GetQueueClient(string queueName);
    }
}
