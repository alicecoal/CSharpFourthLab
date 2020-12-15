using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace ServiceLayer
{
    public class ServiceLayer
    {
        public DataAccessLayer.DataAccessLayer DAL;

        public ServiceLayer(DataAccessLayer.Options.ConnectionOptions options)
        {
            DAL = new DataAccessLayer.DataAccessLayer(options);
        }

        public PersonData GetPersonInfo()
        {
            List<Person> persons = DAL.GetListItems<Person>("GetPerson");
            List<Address> addresses = new List<Address>();
                //= DAL.GetListItems<Address>("GetAddress");
            List<CountryRegion> countryRegions = DAL.GetListItems<CountryRegion>("GetCountryRegions");
            List<EmailAddress1> emailAddresses = DAL.GetListItems<EmailAddress1>("GetEmailAddresses");
            List<PersonPhone> personPhones = DAL.GetListItems<PersonPhone>("GetPersonPhones");
            return new PersonData(persons, addresses, countryRegions, emailAddresses, personPhones);
        }
    }
}
