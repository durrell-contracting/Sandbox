using Reveal.DatabaseAccess.Configuration;
using Reveal.DatabaseAccess.Extensions;
using Reveal.ObjectStorage.Configuration;
using Reveal.ObjectStorage.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDatabaseAccess(builder.Configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>());
builder.Services.AddObjectStorage(builder.Configuration.GetSection("ObjectStorageConfig").Get<ObjectStorageConfig>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
