using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class PersonData
    {
       public List<Person> Person { get; set; }
        public List<Address> Address { get; set; }
        public List<CountryRegion> CountryRegion { get; set; }
        public List<EmailAddress> EmailAddress { get; set; }
        public List<PersonPhone> PersonPhone { get; set; }
        public PersonData() { }
        public PersonData(List<Person> person, List<Address> address, List<CountryRegion> countryRegion, List<EmailAddress> emailAddress, List<PersonPhone> personPhone)
        {
            Person = person;
            Address = address;
            CountryRegion = countryRegion;
            EmailAddress = emailAddress;
            PersonPhone = personPhone;
        }
    }
}
