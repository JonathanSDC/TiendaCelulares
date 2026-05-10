var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Tienda_Celulares_ApiService>("apiservice");

builder.AddProject<Projects.Tienda_Celulares_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
