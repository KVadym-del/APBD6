using APBD6.Models;

namespace APBD6.Data
{
    public static class InMemoryDataStore
    {
        public static readonly List<Animal> Animals = new()
    {
        new Animal(1, "Buddy", "Dog", 25.5, "Brown"),
        new Animal(2, "Whiskers", "Cat", 5.2, "Gray"),
        new Animal(3, "Rocky", "Dog", 30.1, "Black"),
        new Animal(4, "Luna", "Cat", 4.8, "White")
    };

        public static readonly List<Visit> Visits = new()
    {
        new Visit(1, DateTime.Now.AddDays(-10), 1, "Annual Checkup", 50.00m),
        new Visit(2, DateTime.Now.AddDays(-5), 2, "Vaccination", 75.50m),
        new Visit(3, DateTime.Now.AddDays(-2), 1, "Skin Rash", 60.00m)
    };

        private static int _nextAnimalId = Animals.Max(a => a.Id) + 1;
        private static int _nextVisitId = Visits.Any() ? Visits.Max(v => v.Id) + 1 : 1;

        public static int GetNextAnimalId() => Interlocked.Increment(ref _nextAnimalId);
        public static int GetNextVisitId() => Interlocked.Increment(ref _nextVisitId);
    }
}
