using AwesomeConventions;

namespace Api.People
{
    public class NamesApi
    {
        #region ctor
        private readonly IPeopleRepository people;

        public NamesApi(IPeopleRepository people)
        {
            this.people = people;
        }
        #endregion
        
        public string[] Get() => people.All;
    }
}