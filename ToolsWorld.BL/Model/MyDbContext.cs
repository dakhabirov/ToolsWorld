using System.Data.Entity;
using ToolsWorld.BL.Model;

namespace ToolsWorld.BL
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("Data Source=localhost;Initial Catalog=ToolsWorld;Integrated Security=SSPI;MultipleActiveResultSets=true")    // Строка подключения к базе данных
        {
            // Инициализирует базу данных и указывает EF, что если модель изменилась, то воссоздать новую
            Database.SetInitializer(new DbContextInitializer());
        }

        /// <summary>
        /// Сотрудники.
        /// </summary>
        public DbSet<Worker> Workers { get; set; }

        /// <summary>
        /// Поставщики.
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// Товары.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Отделы.
        /// </summary>
        public DbSet<Departament> Departaments { get; set; }
        
        /// <summary>
        /// Продажи.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }

        /// <summary>
        /// Заказы.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Проданные товары.
        /// </summary>
        public DbSet<SaleProduct> SaleProducts { get; set; }

        /// <summary>
        /// Заказанные товары.
        /// </summary>
        public DbSet<OrderProduct> OrderProducts { get; set; }
    }
}
