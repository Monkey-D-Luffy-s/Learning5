namespace Learning5.Models.Account
{
    public class Holiday
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
