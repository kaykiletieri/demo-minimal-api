using DemoMinimalAPI.Data;
using DemoMinimalAPI.Extensions;
using DemoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

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
    .WithName("GetSupplierById")
    .WithTags("Suppliers");

app.MapPost("/suppliers", async (
    ApplicationDbContext context, Supplier supplier) =>
    {
        context.Suppliers.Add(supplier);
        await context.SaveChangesAsync();
        return Results.Created($"/suppliers/{supplier.Id}", supplier);
    })
    .Produces<Supplier>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("CreateSupplier")
    .WithTags("Suppliers");
/*
app.MapPut("/suppliers/{id}", async (
    ApplicationDbContext context, Guid id, Supplier supplier) =>
    {
        if (id != supplier.Id)
        {
            return Results.BadRequest("Id mismatch");
        }

        context.Entry(supplier).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Results.NoContent();
    })
    .WithName("UpdateSupplier")
    .WithTags("Suppliers");
*/
app.MapDelete("/suppliers/{id}", async (
    ApplicationDbContext context, Guid id) =>
    {
        var supplier = await context.Suppliers.FirstOrDefaultAsync(s => s.Id == id && s.Active == true);
        if (supplier is null)
        {
            return Results.NotFound();
        }

        context.Suppliers.Remove(supplier);
        await context.SaveChangesAsync();
        return Results.NoContent();
    })
    .WithName("DeleteSupplier")
    .WithTags("Suppliers");

app.Run();