namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
        public ISet<Phone> phones { get; set; }= new HashSet<Phone>();
    }
}
