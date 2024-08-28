using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.HTTP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if(builder.Environment.IsProduction())
{
    System.Console.WriteLine("--> Using SQL Server Db");
    builder.Services.AddDbContext<ApplicationDBContext>( opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"))
    );
}
else
{
   System.Console.WriteLine("--> Using InMemTemp Db");
   builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseInMemoryDatabase("InMemTemp"));
}

builder.Services.AddGrpc();
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient,HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient,MessageBusClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/Platforms.proto", async context => 
{
    await context.Response.WriteAsync(System.IO.File.ReadAllText("Protos/Platforms.proto"));
});

PrepDb.PrepPopulation(app,builder.Environment.IsProduction());

app.Run();


