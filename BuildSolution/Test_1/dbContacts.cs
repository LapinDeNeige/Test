using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_1
{
    public class dbContacts
    {
        public dbContacts()
        {

        }
        public dbContacts(string name,string surname,string fathersname,string phone)
        {
            Name = name;
            Surname = surname;
            Fathersname = fathersname;
            Phone = phone;
        }

        public void updateContacts(string name, string surname, string fathersname, string phone)
        {
            Name = name;
            Surname = surname;
            Fathersname = fathersname;
            Phone = phone;

        }
        public string Name;
        public string Surname;
        public string Fathersname;
        public string Phone;
        public string Id;
    }
}
