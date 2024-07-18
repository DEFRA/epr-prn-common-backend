using EPR.PRN.Backend.API;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var startup = new Startup(builder.Environment, builder.Configuration);
startup.ConfigureServices(builder.Services);
startup.Configure(app, builder.Environment);

app.Run();