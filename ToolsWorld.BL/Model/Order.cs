using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order : INotifyPropertyChanged
    {
        private static MyDbContext myDbContext = new MyDbContext();

        DateTime? supplyDate;

        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор сотрудника.
        /// </summary>
        public int WorkerId { get; set; }

        /// <summary>
        /// Идентификатор поставщика.
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// Дата заказа.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Дата поставки.
        /// </summary>
        public DateTime? SupplyDate
        {
            get { return supplyDate; }
            set
            {
                supplyDate = Convert.ToDateTime(value);
                OnPropertyChanged("SupplyDate");
            }
        }

        /// <summary>
        /// Сотрудник.
        /// </summary>
        public virtual Worker Worker { get; set; }

        /// <summary>
        /// Поставщик.
        /// </summary>
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// Коллекция товаров в заказе.
        /// </summary>
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Реализация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Получить представление коллекции.
        /// </summary>
        /// <returns>Представление коллекции.</returns>
        private static ICollectionView GetCollectionView()
        {
            var orders = GetOrders(); // вся коллекция, без фильтра
            ICollectionView ordersCollectionView;
            ordersCollectionView = CollectionViewSource.GetDefaultView(orders);   // представляем всю коллекцию
            return ordersCollectionView;
        }

        /// <summary>
        /// Получить коллекцию заказов.
        /// </summary>
        /// <returns>Коллекция заказов.</returns>
        public static ICollection<Order> GetOrders()
        {
            List<Order> orders = new List<Order>(myDbContext.Orders);
            return orders;
        }

        #region Добавление

        /// <summary>
        /// Добавить заказ.
        /// </summary>
        /// <param name="workerId">Идентификатор сотрудника.</param>
        /// <param name="supplierId">Идентификатор поставщика.</param>
        /// <param name="orderDate">Дата заказа.</param>
        public static void Insert(int workerId, int supplierId, DateTime orderDate)
        {
            Order order = new Order
            {
                WorkerId = workerId,
                SupplierId = supplierId,
                OrderDate = orderDate
            };

            myDbContext.Orders.Add(order);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удалить.
        /// </summary>
        /// <param name="orderId">Идентификатор заказа.</param>
        public static void Delete(int orderId)
        {
            Order order = myDbContext.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();

            myDbContext.Orders.Remove(order);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Фильтр

        /// <summary>
        /// Фильтровать по дате оформления.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterByOrderDate(DateTime value)
        {
            var filter = GetCollectionView();  // получаем всю коллекцию
            filter.Filter = o => ((Order)o).OrderDate == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по дате поставки.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterBySupplyDate(DateTime value)
        {
            var filter = GetCollectionView();  // получаем всю коллекцию
            filter.Filter = o => ((Order)o).SupplyDate == value;  // фильтруем коллекцию
            return filter;
        }
        /// <summary>
        /// Фильтровать по имени сотрудника.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterByWorkerFullname(string value)
        {
            var filter = GetCollectionView();  // получаем всю коллекцию
            filter.Filter = o => ((Order)o).Worker.Fullname == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по наименованию поставщика.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterBySupplierName(string value)
        {
            var filterSales = GetCollectionView();  // получаем всю коллекцию
            filterSales.Filter = o => ((Order)o).Supplier.SupplierName == value;  // фильтруем коллекцию
            return filterSales;
        }

        #endregion

        /// <summary>
        /// Проверка на изменение значений.
        /// </summary>
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
