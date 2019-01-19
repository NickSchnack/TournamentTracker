using System.Collections.Generic;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PersonModel> Members { get; set; } = new List<PersonModel>();
    }
}

