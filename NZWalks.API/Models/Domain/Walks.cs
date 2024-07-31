namespace NZWalks.API.Models.Domain
{
    public class Walks
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string WalkImageUrl { get; set; }
        public Guid DifficultyID { get; set; }
        public Guid RegionID { get; set; }

        //Nevigation Property
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }

    }
}
