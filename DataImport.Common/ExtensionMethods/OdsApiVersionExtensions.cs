// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

namespace DataImport.Common.ExtensionMethods
{
    public static class OdsApiVersionExtensions
    {
        public static bool IsOdsV2(this string apiVersion)
        {
            return apiVersion != null && apiVersion.StartsWith("2.");
        }

        public static bool IsOdsV3(this string apiVersion)
        {
            return apiVersion != null && apiVersion.StartsWith("7.");
        }
    }
}
