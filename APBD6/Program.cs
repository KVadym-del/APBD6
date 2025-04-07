using APBD6.Data;
using APBD6.Models;
using APBD6.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/animals", () => Results.Ok(InMemoryDataStore.Animals))
    .WithName("GetAnimals") 
    .WithTags("Animals"); 

app.MapGet("/animals/{id:int}", (int id) =>
{
    var animal = InMemoryDataStore.Animals.FirstOrDefault(a => a.Id == id);
    return animal is not null ? Results.Ok(animal) : Results.NotFound($"Animal with ID {id} not found.");
})
    .WithName("GetAnimalById")
    .WithTags("Animals");

app.MapPost("/animals", (AnimalCreateDto createDto) =>
{
    if (string.IsNullOrWhiteSpace(createDto.Name) || string.IsNullOrWhiteSpace(createDto.Category))
    {
        return Results.BadRequest("Name and Category are required.");
    }

    var newAnimal = new Animal(
        InMemoryDataStore.GetNextAnimalId(),
        createDto.Name,
        createDto.Category,
        createDto.Weight,
        createDto.FurColor
    );

    InMemoryDataStore.Animals.Add(newAnimal);
    return Results.CreatedAtRoute("GetAnimalById", new { id = newAnimal.Id }, newAnimal);
})
    .WithName("CreateAnimal")
    .WithTags("Animals");

app.MapPut("/animals/{id:int}", (int id, AnimalUpdateDto updateDto) =>
{
    var existingAnimal = InMemoryDataStore.Animals.FirstOrDefault(a => a.Id == id);
    if (existingAnimal is null)
    {
        return Results.NotFound($"Animal with ID {id} not found.");
    }

    var index = InMemoryDataStore.Animals.FindIndex(a => a.Id == id);
    if (index == -1)
    {
        return Results.Problem("Could not find animal index after initial check.");
    }

    var updatedAnimal = existingAnimal with 
    {
        Name = string.IsNullOrWhiteSpace(updateDto.Name) ? existingAnimal.Name : updateDto.Name,
        Category = string.IsNullOrWhiteSpace(updateDto.Category) ? existingAnimal.Category : updateDto.Category,
        Weight = updateDto.Weight ?? existingAnimal.Weight, // Use provided value or keep old one
        FurColor = string.IsNullOrWhiteSpace(updateDto.FurColor) ? existingAnimal.FurColor : updateDto.FurColor
    };

    InMemoryDataStore.Animals[index] = updatedAnimal;

    return Results.Ok(updatedAnimal); 
})
    .WithName("UpdateAnimal")
    .WithTags("Animals");


app.MapDelete("/animals/{id:int}", (int id) =>
{
    var animalToRemove = InMemoryDataStore.Animals.FirstOrDefault(a => a.Id == id);
    if (animalToRemove is null)
    {
        return Results.NotFound($"Animal with ID {id} not found.");
    }

    InMemoryDataStore.Animals.Remove(animalToRemove);

    InMemoryDataStore.Visits.RemoveAll(v => v.AnimalId == id);

    return Results.NoContent();
})
    .WithName("DeleteAnimal")
    .WithTags("Animals");


app.MapGet("/animals/{animalId:int}/visits", (int animalId) =>
{
    if (!InMemoryDataStore.Animals.Any(a => a.Id == animalId))
    {
        return Results.NotFound($"Animal with ID {animalId} not found.");
    }

    var visits = InMemoryDataStore.Visits.Where(v => v.AnimalId == animalId).ToList();
    return Results.Ok(visits);
})
    .WithName("GetVisitsForAnimal")
    .WithTags("Visits");

app.MapPost("/animals/{animalId:int}/visits", (int animalId, VisitCreateDto createDto) =>
{
    var animal = InMemoryDataStore.Animals.FirstOrDefault(a => a.Id == animalId);
    if (animal is null)
    {
        return Results.NotFound($"Cannot add visit: Animal with ID {animalId} not found.");
    }

    if (string.IsNullOrWhiteSpace(createDto.Description))
    {
        return Results.BadRequest("Visit description is required.");
    }

    var newVisit = new Visit(
        InMemoryDataStore.GetNextVisitId(),
        createDto.DateOfVisit ?? DateTime.UtcNow, 
        animalId, 
        createDto.Description,
        createDto.Price
    );

    InMemoryDataStore.Visits.Add(newVisit);

    return Results.Created($"/animals/{animalId}/visits", newVisit); 
})
    .WithName("CreateVisitForAnimal")
    .WithTags("Visits");

app.Run();

public record AnimalCreateDto(string Name, string Category, double Weight, string FurColor);

public record AnimalUpdateDto(string? Name, string? Category, double? Weight, string? FurColor);

public record VisitCreateDto(DateTime? DateOfVisit, string Description, decimal Price);


