using CLIMB_BE.Services;
using CLIMB_BE.Controllers;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from the .env file
Env.Load();

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
     options.OperationFilter<FileUploadOperationFilter>();
});


// Register BlobStorageService and pass environment variables
builder.Services.AddSingleton<BlobStorageService>(provider =>
{
    var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
    var problemsContainerName = Environment.GetEnvironmentVariable("AZURE_PROBLEMS_CONTAINER_NAME");
    var resumesContainerName = Environment.GetEnvironmentVariable("AZURE_RESUMES_CONTAINER_NAME");

    // Ensure the environment variables are set, otherwise throw an exception
    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(problemsContainerName) || string.IsNullOrEmpty(resumesContainerName))
    {
        throw new ArgumentNullException("One or more Azure Blob Storage environment variables are not set.");
    }

    return new BlobStorageService(connectionString, problemsContainerName, resumesContainerName);
});

// Register your other services
builder.Services.AddSingleton<IChatServices, ChatServices>(); 
builder.Services.AddSingleton<IOCRServices, OCRServices>(); 

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
