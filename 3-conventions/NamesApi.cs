using AwesomeConventions;
using Microsoft.AspNetCore.Mvc;

namespace Api.People
{
    public class NamesApi
    {
        // GET api/people/names
        public string[] Get([FromServices]IPeopleRepository people) => people.All;
        
    }
}