using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public class Phone
    {
        public int Id { get; set; }
        public string phone { get; set; }
        public User user { get; set; }

    }
}
