Host
    .CreateDefaultBuilder()
    .ConfigureWebHost(webHost =>
        webHost
            .ConfigureServices(services => services.AddMvc(o => o.EnableEndpointRouting = false))
            .Configure(app => app.UseMvc())
            .UseTorenvalk(o =>
            {
                o.InboxPath = @".\process\inbox";
                o.OutboxPath = @".\process\outbox";
            }))
    .Build()
    .Run();
