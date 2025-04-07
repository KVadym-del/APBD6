namespace APBD6.Models
{
    public record Visit(
        int Id,
        DateTime DateOfVisit,
        int AnimalId, 
        string Description,
        decimal Price
    );
}
