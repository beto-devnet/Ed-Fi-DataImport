// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using DataImport.Models;
using Newtonsoft.Json.Linq;

namespace DataImport.Web.Services.Swagger
{
    public interface ISwaggerMetadataProcessor
    {
        bool CanHandle(JObject swaggerDocument);
        Task<List<SwaggerResource>> GetMetadata(JObject swaggerDocument, ApiSection apiSection);
        string GetTokenUrl(JObject swaggerDocument);
        string GetTokenUrl(string apiUrl, string apiVersion, string tenant, string context);
        string GetAuthUrl(JObject swaggerDocument);
    }
}
