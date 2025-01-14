using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMS.Application.Interfaces;
using PIMS.Application.Services;
using PIMS.Domain.Entities;
using PIMS.Infrastructure.Data;
using PIMS.Infrastructure.Providers.Implament;
using PIMS.Infrastructure.Providers.Interface;
using PIMS.Infrastructure.Repositories;
using PIMS.Application.UnitOfWork;
using PIMS.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPropertyRepository<Property>, PropertyRepository>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IContactRepository<Contact>, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ITimeProvider, SystemTimeProvider>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

//app.UseMiddleware<PIMS.Infrastructure.ExceptionMiddleware.ExceptionMiddleware>();

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
