using dotenv.net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganizerAPI.Test
{
    internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            DotEnv.Load();
            //base.ConfigureWebHost(builder);
            builder.ConfigureAppConfiguration(config => { });
            builder.ConfigureTestServices(Services => { });
        }
    }
}
