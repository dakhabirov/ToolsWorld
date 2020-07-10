using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;

namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Продажа.
    /// </summary>
    public class Sale
    {
        private static readonly MyDbContext myDbContext = new MyDbContext();

        protected static int lastMonth = DateTime.Now.Month - 1;
        protected static DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("d"));

        /// <summary>
        /// Идентификатор продажи.
        /// </summary>
        public int SaleId { get; set; }

        /// <summary>
        /// Идентификатор сотрудника.
        /// </summary>
        public int WorkerId { get; set; }

        /// <summary>
        /// Дата продажи.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Коллекция проданных товаров.
        /// </summary>
        public virtual ICollection<SaleProduct> SaleProducts { get; set; }

        /// <summary>
        /// Сотрудник.
        /// </summary>
        public virtual Worker Worker { get; set; }

        /// <summary>
        /// Получить коллекцию продаж.
        /// </summary>
        /// <returns>Коллекция продаж.</returns>
        public static ICollection<Sale> GetSales()
        {
            List<Sale> sales = new List<Sale>(myDbContext.Sales);
            return sales;
        }

        /// <summary>
        /// Получить представление коллекции.
        /// </summary>
        /// <returns>Представление коллекции.</returns>
        private static ICollectionView GetCollectionView()
        {
            var sales = GetSales(); // вся коллекция, без фильтра
            ICollectionView salesCollectionView;
            salesCollectionView = CollectionViewSource.GetDefaultView(sales);   // представляем всю коллекцию
            return salesCollectionView;
        }

        #region Добавление

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="workerId">Сотрудник.</param>
        /// <param name="saleDate">Дата.</param>
        public static void Insert(int workerId, DateTime saleDate)
        {
            Sale sale = new Sale
            {
                WorkerId = workerId,
                SaleDate = saleDate
            };

            myDbContext.Sales.Add(sale);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удалить.
        /// </summary>
        /// <param name="saleId">Идентификатор продажи.</param>
        public static void Delete(int saleId)
        {
            Sale sale = myDbContext.Sales.Where(s => s.SaleId == saleId).FirstOrDefault();
            myDbContext.Sales.Remove(sale);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Фильтр

        /// <summary>
        /// Фильтровать по дате продажи.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Представление коллекции продаж.</returns>
        public static ICollectionView FilterBySaleDate(DateTime value)
        {
            var filter = GetCollectionView();  // получаем всю коллекцию
            filter.Filter = s => ((Sale)s).SaleDate == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по имени сотрудника.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterByWorkerFullName(string value)
        {
            var filter = GetCollectionView();  // получаем всю коллекцию
            filter.Filter = s => ((Sale)s).Worker.Fullname == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по отделу.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterByWorkerDepartamentName(string value)
        {
            var filterSales = GetCollectionView();  // получаем всю коллекцию
            filterSales.Filter = s => ((Sale)s).Worker.Departament.DepartamentName == value;  // фильтруем коллекцию
            return filterSales;
        }

        #endregion

        /// <summary>
        /// Получить сумму проданных товаров за определенные сутки.
        /// </summary>
        /// <param name="dateTime">Дата.</param>
        /// <returns>Сумма.</returns>
        public static double GetDayResult(DateTime dateTime)
        {
            double sum = 0;

            var query = from s in myDbContext.Sales
                        where s.SaleDate == dateTime
                        select s.SaleProducts;   // получаем множество коллекций проданных товаров

            foreach (var saleProducts in query) // для каждой коллекции из запроса
                foreach (SaleProduct saleProduct in saleProducts)   // для каждого товара из коллекции
                {
                    sum += saleProduct.Sum;
                }

            return sum;
        }

        /// <summary>
        /// Получить сумму проданных товаров за прошлый месяц.
        /// </summary>
        /// <returns>Сумма.</returns>
        public static double GetLastMonthResult()
        {
            double sum = 0;

            var query = from s in myDbContext.Sales
                        where s.SaleDate.Month == lastMonth
                        select s.SaleProducts;   // получаем множество коллекций проданных товаров

            foreach (var saleProducts in query) // для каждой коллекции из запроса
                foreach (SaleProduct saleProduct in saleProducts)   // для каждого товара из коллекции
                {
                    sum += saleProduct.Sum;
                }

            return sum;
        }

        /// <summary>
        /// Получить сумму проданных товаров за определенный день недели.
        /// </summary>
        /// <param name="dayOfWeek">Неделя.</param>
        /// <returns>Сумма.</returns>
        public static double GetDayOfWeekResult(int year, DayOfWeek dayOfWeek)
        {
            double sum = 0;

            var query = from s in myDbContext.Sales.ToList()  // приводим к List, т.к. иначе LINQ не поддерживает DayOfWeek
                        where s.SaleDate.DayOfWeek == dayOfWeek
                        where s.SaleDate.Year == year
                        select s.SaleProducts;   // получаем множество коллекций проданных товаров

            foreach (var saleProducts in query) // для каждой коллекции из запроса
                foreach (SaleProduct saleProduct in saleProducts)   // для каждого товара из коллекции
                {
                    sum += saleProduct.Sum;
                }

            return sum;
        }

        /// <summary>
        /// Получить сумму проданных товаров за определенный день недели.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <returns>Сумма.</returns>
        public static Dictionary<string, double> GetYearResultByDaysOfWeek(int year)
        {
            Dictionary<DayOfWeek, string> dictionaryDaysOfWeek = new Dictionary<DayOfWeek, string>
            {
                [DayOfWeek.Monday] = "Понедельник",
                [DayOfWeek.Tuesday] = "Вторник",
                [DayOfWeek.Wednesday] = "Среда",
                [DayOfWeek.Thursday] = "Четверг",
                [DayOfWeek.Friday] = "Пятница",
                [DayOfWeek.Saturday] = "Суббота",
                [DayOfWeek.Sunday] = "Воскресенье"
            };

            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            // заполняем словарь
            foreach (KeyValuePair<DayOfWeek, string> keyValuePair in dictionaryDaysOfWeek)
            {
                double sum = GetDayOfWeekResult(year, keyValuePair.Key);
                try
                {
                    dictionary.Add(keyValuePair.Value, sum);
                }
                catch
                {
                    dictionary[keyValuePair.Value] = sum;
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Получить сумму проданных товаров за определенный год.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <returns>Сумма.</returns>
        public static double GetYearResult(int year, int month)
        {
            double sum = 0;

            var query = from s in myDbContext.Sales
                        where s.SaleDate.Year == year
                        where s.SaleDate.Month == month
                        select s.SaleProducts;   // получаем множество коллекций проданных товаров

            foreach (var saleProducts in query) // для каждой коллекции из запроса
                foreach (SaleProduct saleProduct in saleProducts)   // для каждого товара из коллекции
                {
                    sum += saleProduct.Sum;
                }

            return sum;
        }

        /// <summary>
        /// Получить сумму проданных товаров за определенный месяц.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <returns>Сумма.</returns>
        public static Dictionary<string, double> GetYearResultByMonths(int year)
        {
            Dictionary<int, string> dictionaryMonths = new Dictionary<int, string>
            {
                [1] = "Январь",
                [2] = "Февраль",
                [3] = "Март",
                [4] = "Апрель",
                [5] = "Май",
                [6] = "Июнь",
                [7] = "Июль",
                [8] = "Август",
                [9] = "Сентябрь",
                [10] = "Октябрь",
                [11] = "Ноябрь",
                [12] = "Декабрь"
            };

            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            for (int i = 0; i < dictionaryMonths.Count; i++)
            {
                // заполняем словарь
                foreach (KeyValuePair<int, string> keyValuePair in dictionaryMonths)
                {
                    double sum = 0;
                    double s = GetYearResult(currentDate.Year, keyValuePair.Key);
                    try
                    {
                        dictionary.Add(keyValuePair.Value, s);
                    }
                    catch
                    {
                        sum += s;
                        dictionary[keyValuePair.Value] = sum;
                    }
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Получить сотрудников, которые реализовали продажу в определенный день.
        /// </summary>
        /// <param name="dateTime">Дата.</param>
        /// <returns>Список сотрудников.</returns>
        public static List<Worker> GetWorkersWhoRealizedSaleOnDay(DateTime dateTime)
        {
            List<Worker> workersList = new List<Worker>();

            List<Worker> query = (from s in myDbContext.Sales
                                  where s.SaleDate == dateTime
                                  select s.Worker).ToList();   // получаем список сотрудников

            foreach (Worker worker in query)
            {
                if (workersList.Contains(worker))
                { }
                else
                    workersList.Add(worker);    // добавляем в список каждого сотрудника только один раз
            }

            return workersList;
        }

        /// <summary>
        /// Получить сотрудников, которые реализовали продажу в прошлом месяце.
        /// </summary>
        /// <returns>Список сотрудников.</returns>
        public static List<Worker> GetWorkersWhoRealizedSaleOnLastMonth()
        {
            List<Worker> workersList = new List<Worker>();

            List<Worker> query = query = (from s in myDbContext.Sales
                                          where s.SaleDate.Month == lastMonth
                                          select s.Worker).ToList();   // получаем cписок сотрудников

            foreach (Worker worker in query)
            {
                if (workersList.Contains(worker))
                { }
                else
                    workersList.Add(worker);    // добавляем в список каждого сотрудника только один раз
            }

            return workersList;
        }

        /// <summary>
        /// Получить сумму проданных товаров сотрудника за текущие сутки.
        /// </summary>
        /// <param name="worker">Сотрудник.</param>
        /// <returns>Сумма.</returns>
        public static double GetWorkerDayResult(Worker worker)
        {
            double sum = 0;

            var query = from s in myDbContext.Sales
                        where s.Worker.WorkerId == worker.WorkerId
                        where s.SaleDate == currentDate
                        select s.SaleProducts;   // получаем множество коллекций проданных товаров

            foreach (var saleProducts in query) // для каждой коллекции из запроса
                foreach (SaleProduct saleProduct in saleProducts)   // для каждого товара из коллекции
                {
                    sum += saleProduct.Sum;
                }

            return sum;
        }

        /// <summary>
        /// Получить сумму проданных товаров сотрудника за прошлый месяц.
        /// </summary>
        /// <param name="worker">Сотрудник.</param>
        /// <returns>Сумма.</returns>
        public static double GetWorkerLastMonthResult(Worker worker)
        {
            double sum = 0;

            var query = from s in myDbContext.Sales
                        where s.Worker.WorkerId == worker.WorkerId
                        where s.SaleDate.Month == lastMonth
                        select s.SaleProducts;   // получаем множество коллекций проданных товаров

            foreach (var saleProducts in query) // для каждой коллекции из запроса
                foreach (SaleProduct saleProduct in saleProducts)   // для каждого товара из коллекции
                {
                    sum += saleProduct.Sum;
                }

            return sum;
        }
    }
}