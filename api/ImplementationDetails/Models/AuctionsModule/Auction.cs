namespace Models.AuctionsModule
{
    public class Auction : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CurrentPrice { get; set; }
    }
}
