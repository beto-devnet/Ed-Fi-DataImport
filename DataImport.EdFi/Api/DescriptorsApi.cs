// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using DataImport.EdFi.Models;
using RestSharp;

namespace DataImport.EdFi.Api
{
    public class DescriptorsApi
    {
        private readonly IRestClient _client;

        public DescriptorsApi(IRestClient client)
        {
            _client = client;
        }

        public List<Descriptor> GetAllDescriptors(string descriptorPath, int? offset = null, int? limit = null)
        {
            var request = new RestRequest($"/{descriptorPath}", Method.Get) { RequestFormat = DataFormat.Json };

            if (offset != null)
                request.AddParameter("offset", offset, ParameterType.HttpHeader);
            if (limit != null)
                request.AddParameter("limit", limit, ParameterType.HttpHeader);
            request.AddHeader("Accept", "application/json");

            var clientExecute = _client.ExecuteAsync<List<Descriptor>>(request);
            clientExecute.Wait();
            var response = clientExecute.Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new DescriptorNotFoundException(new Exception($"Descriptor '{descriptorPath}' could not be found."));

            return response.Data;
        }
    }
}
