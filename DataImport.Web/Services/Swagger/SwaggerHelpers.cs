// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

namespace DataImport.Web.Services.Swagger
{
    public static class SwaggerHelpers
    {
        public static string GetSwagger20EntityNameFromReference(string swaggerReference)
        {
            if (string.IsNullOrEmpty(swaggerReference)) return null;

            var swaggerEntityPath = swaggerReference.Split('/');
            if (swaggerEntityPath.Length >= 4)
                return swaggerEntityPath[3];
            else if (swaggerEntityPath.Length >= 3)
                return swaggerEntityPath[2];
            else
                return swaggerReference;
        }
    }
}
