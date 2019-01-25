using AwesomeConventions;

namespace Api.People
{
    public class NamesApi
    {
        // GET api/people/names
        public string[] Get() => people.All;
        
        
        
        
        #region ctor
        private readonly IPeopleRepository people;

        public NamesApi(IPeopleRepository people)
        {
            this.people = people;
        }
        #endregion
        
    }
}