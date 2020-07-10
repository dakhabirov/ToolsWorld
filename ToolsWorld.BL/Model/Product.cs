using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows.Data;

namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Товар.
    /// </summary>
    public class Product
    {
        private static readonly MyDbContext myDbContext = new MyDbContext();
        private static readonly Random rnd = new Random();
        private static readonly DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("d"));  // текущая дата

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Идентификатор отдела.
        /// </summary>
        public int DepartamentId { get; set; }

        /// <summary>
        /// Наименование товара.
        /// </summary>
        [MaxLength(100)]
        public string ProductName { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        [MaxLength(10)]
        public string Unit { get; set; }

        /// <summary>
        /// Розничная цена.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Коллекция проданных товаров.
        /// </summary>
        public virtual ICollection<SaleProduct> SaleProducts { get; set; }

        /// <summary>
        /// Коллекция поставленных товаров.
        /// </summary>
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Отдел.
        /// </summary>
        public virtual Departament Departament { get; set; }

        /// <summary>
        /// Получить коллекцию товаров.
        /// </summary>
        /// <returns>Список товаров.</returns>
        public static List<Product> GetProducts()
        {
            List<Product> products = new List<Product>(myDbContext.Products);
            return products;
        }

        /// <summary>
        /// Получить представление коллекции.
        /// </summary>
        /// <returns>Представление коллекции.</returns>
        private static ICollectionView GetCollectionView()
        {
            var products = GetProducts(); // вся коллекция, без фильтра
            ICollectionView workersCollectionView;
            workersCollectionView = CollectionViewSource.GetDefaultView(products);   // представляем всю коллекцию
            return workersCollectionView;
        }

        #region Добавление

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="amount"></param>
        /// <param name="unit"></param>
        /// <param name="price"></param>
        /// <param name="departamentId"></param>
        public static void Insert(string productName, int amount, string unit, double price, int departamentId)
        {
            Product product = new Product
            {
                ProductName = productName,
                Amount = amount,
                Unit = unit,
                Price = price,
                DepartamentId = departamentId,
            };

            myDbContext.Products.Add(product);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// Изменить.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="amount"></param>
        /// <param name="unit"></param>
        /// <param name="price"></param>
        /// <param name="departamentId"></param>
        public static void Update(string productName, int amount, string unit, double price, int departamentId)
        {
            var query = from p in myDbContext.Products
                        where p.ProductName == productName
                        select p;

            foreach (Product product in query)
            {
                product.ProductName = productName;
                product.Amount = amount;
                product.Unit = unit;
                product.Price = price;
                product.DepartamentId = departamentId;
            }

            myDbContext.SaveChanges();
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удалить.
        /// </summary>
        /// <param name="productId">Идентификатор товара.</param>
        public static void Delete(int productId)
        {
            Product product = myDbContext.Products.Where(p => p.ProductId == productId).FirstOrDefault();

            myDbContext.Products.Remove(product);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Фильтр

        /// <summary>
        /// Фильтровать по наименованию.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция товаров.</returns>
        public static ICollectionView FilterByProductName(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = p => ((Product)p).ProductName == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по количеству.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция товаров.</returns>
        public static ICollectionView FilterByAmount(int value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = p => ((Product)p).Amount == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по единице измерения.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция товаров.</returns>
        public static ICollectionView FilterByUnit(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = p => ((Product)p).Unit == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по розничной цене.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция товаров.</returns>
        public static ICollectionView FilterByPrice(double value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = p => ((Product)p).Price == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по наименованию отдела.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция товаров.</returns>
        public static ICollectionView FilterByDepartamentName(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = p => ((Product)p).Departament.DepartamentName == value;  // фильтруем коллекцию
            return filter;
        }

        #endregion

        /// <summary>
        /// Получить товар по его по наименованию.
        /// </summary>
        /// <param name="productName">Наименование.</param>
        /// <returns>Сотрудник.</returns>
        public static Product GetProductByProductName(string productName)
        {
            Product product = new Product();

            var query = from p in myDbContext.Products
                        where p.ProductName == productName
                        select p;

            foreach (Product p in query)
                product = p;

            return product;
        }

        /// <summary>
        /// Получить товары, запасы которых подходят к концу.
        /// </summary>
        /// <param name="amount">Количество.</param>
        /// <returns>Список товаров.</returns>
        public static List<Product> GetFewProducts(int amount)
        {
            List<Product> productsList = (from p in myDbContext.Products
                                          where p.Amount < amount
                                          select p).ToList();

            return productsList;
        }

        /// <summary>
        /// Оформить заказ на товары.
        /// </summary>
        /// <param name="products">Список товаров.</param>
        /// <param name="amount">Количество товара.</param>
        /// <param name="workerId">Сотрудник.</param>
        /// <param name="supplierId">Поставщик.</param>
        public static void PlaceOrder(List<Product> products, int amount, int workerId, int supplierId)
        {
            List<int> workerIds = new List<int>(); // список сотрудников
            List<int> supplierIds = new List<int>(); // список поставщиков

            // заполняем список всех сотрудников
            foreach (Worker worker in myDbContext.Workers)
                workerIds.Add(worker.WorkerId);

            // заполняем список всех поставщиков
            foreach (Supplier supplier in myDbContext.Suppliers)
                supplierIds.Add(supplier.SupplierId);

            Order.Insert(workerId, supplierId, currentDate); // добавляем заказ

            var lastOrder = myDbContext.Orders.ToList().Last(); // последняя запись в таблице заказов

            // добавляем товары в заказ
            foreach (Product product in products)
            {
                double price = product.Price - (product.Price * 0.3);    // закупочная цена = розничная цена - 30%
                OrderProduct.Insert(lastOrder.OrderId, product.ProductId, amount, price);
            }
        }
    }
}
