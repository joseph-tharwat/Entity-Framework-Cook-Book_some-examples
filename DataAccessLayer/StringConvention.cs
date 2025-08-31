using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public class StringConvention:IPropConvention
    {
        private readonly string MaxLengthProp = "MaxLength";
        private readonly int MaxLengthNum = 35;
        public static readonly StringConvention Instance = new StringConvention();
        public void Apply(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entityType.GetProperties())
                {
                    if (prop.FindAnnotation(MaxLengthProp) == null)
                    {
                        if (prop.ClrType == typeof(string))
                        {
                            prop.AddAnnotation(MaxLengthProp, MaxLengthNum);
                        }
                    }
                }
            }
        }
    }
}
