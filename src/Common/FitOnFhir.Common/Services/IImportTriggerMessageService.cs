// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

namespace Microsoft.Health.FitOnFhir.Common.Services
{
    public interface IImportTriggerMessageService
    {
#pragma warning disable CA1002 // Do not expose generic lists
        Task AddImportMessagesToCollector(List<string> collector, CancellationToken cancellationToken);
#pragma warning restore CA1002 // Do not expose generic lists
    }
}
