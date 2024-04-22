using System;
using System.Threading.Tasks;
using DataImport.Web.Features.ApiServers;
using NUnit.Framework;
using Shouldly;
using static DataImport.Web.Tests.Testing;

namespace DataImport.Web.Tests.Features.ApiServers
{
    public class RefreshDataModelTests
    {
        [TestCase(1)]
        public async Task ShouldSuccessfullyRefreshDataModel(int ApiServerId)
        {
            var refreshDataModelResponse = await Send(new RefreshDataModel.Command
            {
                ApiServerId = ApiServerId
            });

            refreshDataModelResponse.Message.ShouldBe("Data model was updated");
            refreshDataModelResponse.ApiServerId.ShouldBe(ApiServerId);
        }

        [TestCase(0)]
        public void ShouldThrowAnExceptionWhenApiServerNotExist(int ApiServerId)
        {
            var command = new RefreshDataModel.Command { ApiServerId = ApiServerId };
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await Send(command));

            exception.Message.ShouldBe("Api server not found");
        }
    }
}
