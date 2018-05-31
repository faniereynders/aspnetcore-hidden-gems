namespace AwesomeConventions
{
    public class PeopleRepository : IPeopleRepository
    {
        public string[] All => new string[] { "Fanie", "John", "Joe" };
    }
}
