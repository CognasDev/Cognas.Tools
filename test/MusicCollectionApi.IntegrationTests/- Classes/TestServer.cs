using Cognas.ApiTools.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Samples.MusicCollection.Api.AllMusic.Albums;
using Samples.MusicCollection.Api.AllMusic.Artists;
using Samples.MusicCollection.Api.AllMusic.Genres;
using Samples.MusicCollection.Api.AllMusic.Keys;
using Samples.MusicCollection.Api.AllMusic.Labels;
using Samples.MusicCollection.Api.AllMusic.Tracks;
using Samples.MusicCollection.Api.AllMusic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Program = Samples.MusicCollection.Api.Program;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Shared;
using Samples.MusicCollection.Api.AllMusic.MixableTracks.Rules;
using Cognas.ApiTools.Extensions;

namespace MusicCollectionApi.IntegrationTests;

public class TestServer : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
        });
    }
}
