// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using DataImport.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DataImport.Web.Features.Share
{
    public class FileExport : ExportBase
    {
        public class Query : IRequest<Command>
        {
        }

        public class Command : CommandBase, IRequest<SharingModel>
        {
        }

        public class Validator : ValidatorBase<Command>
        {
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly DataImportDbContext _database;

            public QueryHandler(DataImportDbContext database)
            {
                _database = database;
            }

            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Command
                {
                    Bootstraps = BootstrapSelections(_database),
                    DataMaps = DataMapSelections(_database)
                });
            }
        }

        public class CommandHandler : IRequestHandler<Command, SharingModel>
        {
            private readonly DataImportDbContext _database;

            public CommandHandler(DataImportDbContext database)
            {
                _database = database;
            }


            public Task<SharingModel> Handle(Command request, CancellationToken cancellationToken)
            {
                SharingLookup[] lookupExports;
                SharingPreprocessor[] preprocessorExports;
                var bootstrapExports = BootstrapExports(_database, request);
                var dataMapExports = DataMapExports(_database, request, out lookupExports, out preprocessorExports);

                var template = new SharingTemplate
                {
                    Bootstraps = bootstrapExports,
                    Maps = dataMapExports,
                    Lookups = lookupExports,
                    Preprocessors = preprocessorExports
                };

                var sharingModel = new SharingModel
                {
                    Title = request.Title,
                    Description = request.Description,
                    ApiVersion = request.ApiVersion,
                    Template = template
                };
                return Task.FromResult(sharingModel);
            }
        }
    }
}
