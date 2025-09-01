namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public interface IAuditable
    {
    }

    public static class Auditable
    {
        public static string CreatedBy { get; set; } = "CreatedBy";
        public static string CreatedOn { get; set; } = "CreateOn";
        public static string UpdatedBy { get; set; } = "UpdatedBy";
        public static string UpdatedOn { get; set; } = "UpdatedOn";
    }
}
