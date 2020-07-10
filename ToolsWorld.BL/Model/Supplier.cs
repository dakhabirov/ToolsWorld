using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Data;
using System.Linq;

namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Поставщик.
    /// </summary>
    public class Supplier : INotifyPropertyChanged
    {
        static readonly MyDbContext myDbContext = new MyDbContext();

        string supplierName;
        string tel;

        /// <summary>
        /// Идентификатор поставщика.
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// Наименование поставщика.
        /// </summary>
        [MaxLength(100)]
        public string SupplierName
        {
            get { return supplierName; }
            set
            {
                supplierName = value;
                OnPropertyChanged("SupplierName");
            }
        }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [MaxLength(16)]
        public string Tel
        {
            get { return tel; }
            set
            {
                tel = value;
                OnPropertyChanged("Tel");
            }
        }

        /// <summary>
        /// Коллекция поставок.
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Реализация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Проверка на изменение значений.
        /// </summary>
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Добавление

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="supplierName">Наименование.</param>
        /// <param name="tel">Номер телефона.</param>
        public static void Insert(string supplierName, string tel)
        {
            Supplier supplier = new Supplier
            {
                SupplierName = supplierName,
                Tel = tel
            };

            myDbContext.Suppliers.Add(supplier);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// Изменить.
        /// </summary>
        /// <param name="supplierName">Наименование.</param>
        /// <param name="tel">Номер телефона.</param>
        public static void Update(string supplierName, string tel)
        {
            var query = from s in myDbContext.Suppliers
                        where s.SupplierName == supplierName
                        select s;

            foreach (Supplier supplier in query)
            {
                supplier.SupplierName = supplierName;
                supplier.Tel = tel;
            }

            myDbContext.SaveChanges();
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удалить.
        /// </summary>
        /// <param name="supplierId">Идентификатор поставщика.</param>
        public static void Delete(int supplierId)
        {
            Supplier supplier = myDbContext.Suppliers.Where(s => s.SupplierId == supplierId).FirstOrDefault();

            myDbContext.Suppliers.Remove(supplier);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Фильтр

        /// <summary>
        /// Фильтровать по наименованию.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterBySupplierName(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = sp => ((Supplier)sp).SupplierName == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по номеру телефона.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterByTel(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = sp => ((Supplier)sp).Tel == value;  // фильтруем коллекцию
            return filter;
        }

        #endregion

        /// <summary>
        /// Получить представление коллекции.
        /// </summary>
        /// <returns>Представление коллекции.</returns>
        private static ICollectionView GetCollectionView()
        {
            var suppliers = GetSuppliers(); // вся коллекция, без фильтра
            ICollectionView workersCollectionView;
            workersCollectionView = CollectionViewSource.GetDefaultView(suppliers);   // представляем всю коллекцию
            return workersCollectionView;
        }

        /// <summary>
        /// Получить коллекцию поставщиков.
        /// </summary>
        /// <returns>Коллекция поставщиков.</returns>
        public static ICollection<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>(myDbContext.Suppliers);
            return suppliers;
        }

        /// <summary>
        /// Получить наименования поставщиков.
        /// </summary>
        /// <returns>Список наименований поставщиков.</returns>
        public static List<string> GetSupplierNames()
        {
            List<string> SupplierNames = new List<string>();

            var query = from s in myDbContext.Suppliers
                        select s;

            foreach (Supplier supplier in query)
                SupplierNames.Add(supplier.SupplierName);

            return SupplierNames;
        }

        /// <summary>
        /// Получить поставщика по наименованию.
        /// </summary>
        /// <param name="SupplierName">Наименование поставщика.</param>
        /// <returns>Поставщик.</returns>
        public static Supplier GetSupplierBySupplierName(string supplierName)
        {
            Supplier supplier = new Supplier();

            var query = from s in myDbContext.Suppliers
                        where s.SupplierName == supplierName
                        select s;

            foreach (Supplier s in query)
                supplier = s;

            return supplier;
        }
    }
}
