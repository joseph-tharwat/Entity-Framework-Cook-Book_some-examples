using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public class User : IAuditable
    {
        public int id { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
        public ISet<Phone> phones { get; set; }= new HashSet<Phone>();

        [Timestamp]
        public Byte[] rowVersion { get; set; }
    }
}
