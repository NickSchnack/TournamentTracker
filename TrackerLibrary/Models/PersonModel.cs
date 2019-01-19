namespace TrackerLibrary.Models
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Fullname {
            get
            {
                return $"{Firstname} {Lastname}";
            }
        }

        public PersonModel()
        {

        }

        public PersonModel(string firstName, string lastName, string phone, string email)
        {
            this.Firstname = firstName;
            this.Lastname = lastName;
            this.EmailAddress = phone;
            this.PhoneNumber = email;
        }
    }
}

