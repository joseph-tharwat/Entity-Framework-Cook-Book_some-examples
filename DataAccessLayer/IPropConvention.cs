using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public interface IPropConvention
    {
        public void Apply(ModelBuilder modelBuilder);
    }
}
