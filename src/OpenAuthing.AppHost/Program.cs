using OpenAuthing.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

const string authingDbName = "OpenAuthing";
var authingDb = builder.AddMySql("mysql")
    .WithEnvironment("MYSQL_DATABASE", authingDbName)
    .WithBindMount("../../data/mysql", "/docker-entrypoint-initdb.d")
    .AddDatabase(authingDbName);

builder.AddProject<Projects.BeniceSoft_OpenAuthing_AdminApi>("openauthing-api")
    .WithExternalHttpEndpoints()
    .WithReference(authingDb);

builder.AddProject<Projects.BeniceSoft_OpenAuthing_SSO>("openauthing-sso")
    .WithExternalHttpEndpoints()
    .WithReference(authingDb);

builder.Build().Run();