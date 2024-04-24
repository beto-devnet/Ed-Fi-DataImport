using DataImport.Models;
using DataImport.Web.Helpers;
using DataImport.Web.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataImport.Web.Features.ApiServers
{
    public class RefreshDataModel
    {
        public class Command : IRequest<Response>
        {
            public int ApiServerId { get; set; }
        }

        public class Response : ToastResponse
        {
            public int ApiServerId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly DataImportDbContext _database;
            private readonly IConfigurationService _configurationService;

            public CommandHandler(DataImportDbContext database, IConfigurationService configurationService)
            {
                _database = database;
                _configurationService = configurationService;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var apiServer = await _database.ApiServers
                    .Include(x => x.ApiVersion)
                    .SingleOrDefaultAsync(x => x.Id == request.ApiServerId, cancellationToken);

                if (apiServer is null)
                {
                    throw new System.ArgumentException("Api server not found");
                }

                await _configurationService.FillSwaggerMetadata(apiServer);


                var dataMaps = await (from dm in _database.DataMaps
                                      join res in _database.Resources on dm.ApiVersionId equals res.ApiVersionId
                                      select dm
                                     ).ToListAsync(cancellationToken);

                var resourcesPath = dataMaps.Select(map => map.ResourcePath).Distinct().ToList();

                var metadataFromRresources = await _database
                    .Resources
                    .Where(resource => resource.ApiVersionId == apiServer.ApiVersionId && resourcesPath.Contains(resource.Path))
                    .Select(resource => new { resource.Path, resource.Metadata })
                    .ToListAsync(cancellationToken);

                dataMaps.ForEach(map =>
                {
                    map.Metadata = metadataFromRresources.Where(x => x.Path == map.ResourcePath).Select(x => x.Metadata).First();
                });

                _database.DataMaps.UpdateRange(dataMaps);
                await _database.SaveChangesAsync(cancellationToken);

                return new Response
                {
                    ApiServerId = apiServer.Id,
                    Message = "Data model was updated"
                };

            }
        }
    }
}
