using System.Data.Entity;

namespace ToolsWorld.BL.Model
{
    class DbContextInitializer : DropCreateDatabaseIfModelChanges<MyDbContext>
    {
        /// <summary>
        /// Инициализация базы данных.
        /// </summary>
        /// <param name="dbContext">База данных.</param>
        protected override void Seed(MyDbContext dbContext)
        {
            Worker adm = new Worker
            {
                Log = "admin",
                Fullname = "Администратор",
                Pass = "12345",
                Lvl = 777
            };

            dbContext.Workers.Add(adm);
            dbContext.SaveChanges();
            dbContext.Dispose();
        }
    }
}
