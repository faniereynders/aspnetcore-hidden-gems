using AwesomeConventions;

namespace Api.People
{
    public class NamesApi
    {
        private readonly IPeopleRepository people;

        public NamesApi(IPeopleRepository people)
        {
            this.people = people;
        }
        
        public string[] Get() => people.All;
    }
}