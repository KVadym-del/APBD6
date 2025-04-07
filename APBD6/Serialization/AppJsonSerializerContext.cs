using APBD6.Models;
using System.Text.Json.Serialization;

namespace APBD6.Serialization
{
    [JsonSerializable(typeof(Animal))]
    [JsonSerializable(typeof(List<Animal>))] 
    [JsonSerializable(typeof(AnimalCreateDto))]
    [JsonSerializable(typeof(AnimalUpdateDto))]
    [JsonSerializable(typeof(Visit))]
    [JsonSerializable(typeof(List<Visit>))]   
    [JsonSerializable(typeof(VisitCreateDto))]
    public partial class AppJsonSerializerContext : JsonSerializerContext
    {
        
    }
}
