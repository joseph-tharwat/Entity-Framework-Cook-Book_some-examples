namespace EntityFrameworkeCookBook.DataAccessLayer.Query
{
    public static class UserNameFilter
    {
        public static IQueryable<User> FilterByName(this IQueryable<User> query, string name)
        {
            return query.Where(u => u.name == name);
        }
    }
}
