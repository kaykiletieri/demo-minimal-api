using DemoMinimalAPI.Data;
using DemoMinimalAPI.Extensions;
using DemoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddEntityFramework();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapGet("/suppliers", async (
    ApplicationDbContext context) =>
    await context.Suppliers.AsNoTracking().ToListAsync())
    .WithName("GetSuppliers")
    .WithTags("Suppliers");

app.MapGet("/suppliers/{id}", async (
    ApplicationDbContext context, Guid id) =>
    await context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id && s.Active == true)
        is Supplier supplier 
            ? Results.Ok(supplier) 
            : Results.NotFound())
    .Produces<Supplier>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetSupplierById")
    .WithTags("Suppliers");

app.MapPost("/suppliers", async (
    ApplicationDbContext context, Supplier supplier) =>
    {
        if (!MiniValidator.TryValidate(supplier, out var errors))
            return Results.ValidationProblem(errors);

        context.Suppliers.Add(supplier);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? Results.Created($"/suppliers/{supplier.Id}", supplier)
            : Results.BadRequest("An error occurred while saving the record");
    })
    .ProducesValidationProblem()
    .Produces<Supplier>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("CreateSupplier")
    .WithTags("Suppliers");

app.MapPut("/suppliers/{id}", async (
    ApplicationDbContext context, Guid id, Supplier supplier) =>
    {
        var existingSupplier = await context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id && s.Active == true);
        if (existingSupplier is null) return Results.NotFound();

        if (!MiniValidator.TryValidate(supplier, out var errors))
            return Results.ValidationProblem(errors);

        context.Update(supplier);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.Ok(supplier)
            : Results.BadRequest("An error occurred while saving the record");
    })
    .ProducesValidationProblem()
    .Produces<Supplier>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("UpdateSupplier")
    .WithTags("Suppliers");

app.MapDelete("/suppliers/{id}", async (
    ApplicationDbContext context, Guid id) =>
    {
        var supplier = await context.Suppliers.FirstOrDefaultAsync(s => s.Id == id && s.Active == true);
        if (supplier is null) return Results.NotFound();

        context.Suppliers.Remove(supplier);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("An error occurred while deleting the record");
    })
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("DeleteSupplier")
    .WithTags("Suppliers");

app.Run();