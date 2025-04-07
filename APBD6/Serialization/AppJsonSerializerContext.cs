using APBD6.Models;
using System.Text.Json.Serialization;

namespace APBD6.Serialization
{
    [JsonSerializable(typeof(Animal))]
    [JsonSerializable(typeof(List<Animal>))] // Needed for the GET /animals endpoint
    [JsonSerializable(typeof(AnimalCreateDto))]
    [JsonSerializable(typeof(AnimalUpdateDto))]
    [JsonSerializable(typeof(Visit))]
    [JsonSerializable(typeof(List<Visit>))]   // Needed for the GET /animals/{id}/visits endpoint
    [JsonSerializable(typeof(VisitCreateDto))]
    // Add any other types returned directly by endpoints, like ProblemDetails if customizing errors
    // [JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
    public partial class AppJsonSerializerContext : JsonSerializerContext
    {
        // The source generator will implement the necessary logic in a partial class definition.
    }
}
