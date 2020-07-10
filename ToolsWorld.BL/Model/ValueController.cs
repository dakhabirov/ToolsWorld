namespace ToolsWorld.BL.Model
{
    public class ValueController
    {
        readonly MyDbContext dbContext = new MyDbContext();

        #region Проверить уникальность записей в базе данных.

        #region Проверить данные сотрудника.

        /// <summary>
        /// Проверить данные сотрудника на уникальность.
        /// </summary>
        /// <param name="worker">Сотрудник.</param>
        public bool CheckDuplicateWorker(Worker worker)
        {
            var workers = dbContext.Workers;    // список всех сотрудников из базы данных
            var checker = false;

            try
            {
                foreach (Worker w in workers)
                {
                    if (w.Log == worker.Log)
                    {
                        checker = true;
                    }
                }
            }
            catch
            { }

            return checker;
        }

        #endregion

        #region Проверить данные поставщика.

        /// <summary>
        /// Проверить данные поставщика на уникальность.
        /// </summary>
        /// <param name="supplier">Поставщик.</param>
        public bool CheckDuplicateSupplier(Supplier supplier)
        {
            var suppliers = dbContext.Suppliers;  // список всех поставщиков из базы данных
            var checker = false;

            try
            {
                foreach (Supplier s in suppliers)
                {
                    if (s.SupplierName == supplier.SupplierName)
                    {
                        checker = true;
                    }
                }
            }
            catch
            { }

            return checker;
        }

        #endregion

        #region Проверить данные отдела.

        /// <summary>
        /// Проверить данные отдела на уникальность.
        /// </summary>
        /// <param name="departament">Отдел.</param>
        public bool CheckDuplicateDepartament(Departament departament)
        {
            var departaments = dbContext.Departaments;  // список всех отделов из базы данных
            var checker = false;

            try
            {
                foreach (Departament d in departaments)
                {
                    if (d.DepartamentName == departament.DepartamentName)
                    {
                        checker = true;
                    }
                }
            }
            catch
            { }

            return checker;
        }

        #endregion

        #region Проверить данные товара.

        /// <summary>
        /// Проверить данные товара на уникальность.
        /// </summary>
        /// <param name="product">Товар.</param>
        public bool CheckDuplicateProduct(Product product)
        {
            var products = dbContext.Products;  // список всех товаров из базы данных
            var checker = false;

            try
            {
                foreach (Product p in products)
                {
                    if (p.ProductName == product.ProductName)
                    {
                        checker = true;
                    }
                }
            }
            catch
            { }

            return checker;
        }

        #endregion

        #endregion
    }
}
