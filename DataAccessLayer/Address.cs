namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public class Address
    {
        public int BlockNum { get; set; }
        public string stName { get; set; }
        
        public int userId { get; set; }
        public User user {get; set;}

    }
}