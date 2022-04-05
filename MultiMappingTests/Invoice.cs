namespace MultiMappingTests
{
    public class Invoice
    {
        public int Id { get; }
        public string Title { get; }

        public Invoice(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}