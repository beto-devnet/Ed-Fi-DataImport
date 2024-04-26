// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DataImport.Models;
using MediatR;

namespace DataImport.Web.Features.DataMaps
{
    public class DataMapIndex
    {
        public class ViewModel
        {
            public string Name { get; set; }
            public string ResourceName { get; set; }
            public int Id { get; set; }
        }

        public class Query : IRequest<ViewModel[]>
        {
        }

        public class QueryHandler : IRequestHandler<Query, ViewModel[]>
        {
            private readonly DataImportDbContext _database;
            private readonly IMapper _mapper;

            public QueryHandler(DataImportDbContext database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public Task<ViewModel[]> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_database.DataMaps
                    .OrderBy(x => x.Name)
                    .ToList()
                    .Select(x => _mapper.Map<ViewModel>(x))
                    .ToArray());
            }
        }
    }
}
