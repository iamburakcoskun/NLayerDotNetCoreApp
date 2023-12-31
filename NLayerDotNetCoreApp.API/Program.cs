using Microsoft.EntityFrameworkCore;
using NLayerDotNetCoreApp.API.Filters;
using NLayerDotNetCoreApp.API.Middlewares;
using NLayerDotNetCoreApp.Business.Mapping;
using NLayerDotNetCoreApp.Business.Services.Abstract;
using NLayerDotNetCoreApp.Business.Services.Concrete;
using NLayerDotNetCoreApp.Core.Repositories;
using NLayerDotNetCoreApp.Core.UnitOfWorks;
using NLayerDotNetCoreApp.Data.EntityFramework;
using NLayerDotNetCoreApp.Data.Repositories;
using NLayerDotNetCoreApp.Data.UnitOfWorks;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Dev_ConString"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//builder.Services.AddScoped<IGenericRepository, GenericRepository();

var app = builder.Build();

app.UseCustomException();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
