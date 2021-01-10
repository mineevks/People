
namespace People.MSSql
{

    public static class DbInitializer
    {

        public static void Initialize(PeopleDbContext context)
        {
            context.Database.EnsureCreated();

            /*
             * Add Intitial records
             *
             */
            context.SaveChanges();

        }


    }
}
