var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.kitapsin_Server>("kitapsin-server");

builder.Build().Run();
