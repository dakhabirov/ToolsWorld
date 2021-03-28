using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;

namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Инициализатор случайных значений для базы данных.
    /// </summary>
    public class Initializer
    {
        readonly MyDbContext myDbContext = new MyDbContext();
        readonly Random rnd = new Random();
        readonly Generator generator = new Generator();
        readonly ValueController valueController = new ValueController();

        #region Заполнение таблиц.

        #region Заполнить таблицу сотрудников.

        /// <summary>
        /// Заполнить таблицу сотрудников.
        /// </summary>
        /// <param name="count">Количество записей.</param>
        public async System.Threading.Tasks.Task FillWorkerAsync(int count)
        {
            var workers = myDbContext.Workers;    // список всех сотрудников из базы данных

            while (count > 0)
            {
                Worker worker = await RandomWorkerAsync();  // получаем случайного сотрудника
                string log = worker.Log;    // логин этого сотрудника

                // проверяем логин на уникальность
                if (valueController.CheckDuplicateWorker(worker) == true)
                {
                    //MessageBox.Show($"Пользователь с логином {log} уже зарегистрирован!");
                }
                else
                {
                    workers.Add(worker);
                    myDbContext.SaveChanges();
                }
                count--;
            }
        }

        #endregion

        #region Заполнить таблицу поставщиков.

        /// <summary>
        /// Заполнить таблицу поставщиков.
        /// </summary>
        /// <param name="count">Количество записей.</param>
        public async System.Threading.Tasks.Task FillSupplierAsync(int count)
        {
            var suppliers = myDbContext.Suppliers;

            while (count > 0)
            {
                Supplier supplier = await RandomSupplierAsync();  // получаем случайного поставщика
                string supplierName = supplier.SupplierName;    // наименование этого поставщика

                // проверяем наименование на уникальность
                if (valueController.CheckDuplicateSupplier(supplier) == true)
                {
                    //MessageBox.Show($"Поставщик с наименование {supplierName} уже зарегистрирован!");
                }
                else
                {
                    suppliers.Add(supplier);
                    myDbContext.SaveChanges();
                }
                count--;
            }
        }

        #endregion

        #region Заполнить таблицу отделов.

        /// <summary>
        /// Сгенерировать отдел.
        /// </summary>
        /// <returns>Отдел.</returns>
        public async Task FillDepartamentsAsync()
        {
            var departaments = myDbContext.Departaments;  // список всех отделов из базы данных
            Departament departament = new Departament();

            string pathDepartamentNames = @"..\..\..\Resourses\Departaments\DepartamentNames.txt";
            string readedDepName;

            try
            {
                using (StreamReader sr = new StreamReader(pathDepartamentNames, System.Text.Encoding.Default))
                {
                    while ((readedDepName = await sr.ReadLineAsync()) != null)
                    {
                        departament.DepartamentName = readedDepName;

                        // проверяем наименование отдела на уникальность
                        if (valueController.CheckDuplicateDepartament(departament) == true)
                        {
                            //MessageBox.Show($"Отдел с наименование {readedDepName} уже зарегистрирован!");
                        }
                        else
                        {
                            departaments.Add(departament);
                            myDbContext.SaveChanges();
                        }
                    }
                    readedDepName = null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        #region Заполнить таблицу продаж.

        /// <summary>
        /// Заполнить таблицу продаж.
        /// </summary>
        /// <param name="count">Количество записей.</param>
        public void FillSale(int count)
        {
            int i = count;
            while (i > 0)
            {
                myDbContext.Sales.Add(RandomSale());
                i--;
            }
            myDbContext.SaveChanges();
        }

        #endregion

        #region Заполнить таблицу заказов.

        /// <summary>
        /// Заполнить таблицу заказов.
        /// </summary>
        /// <param name="count">Количество записей.</param>
        public void FillOrder(int count)
        {
            int i = count;
            while (i > 0)
            {
                myDbContext.Orders.Add(RandomOrder());
                i--;
            }
            myDbContext.SaveChanges();
        }

        #endregion

        #region Заполнить таблицу товаров.

        /// <summary>
        /// Заполнить таблицу товаров.
        /// </summary>
        /// <param name="count">Количество записей.</param>
        public async System.Threading.Tasks.Task FillProductAsync(int count)
        {
            var products = myDbContext.Products;    // список всех товаров из базы данных

            while (count > 0)
            {
                Product product = await RandomProductAsync();  // получаем случайный товар
                string productName = product.ProductName;    // наименование этого товара

                // проверяем наименование на уникальность
                if (valueController.CheckDuplicateProduct(product) == true)
                {
                    //MessageBox.Show($"Товар с наименованием {productName} уже зарегистрирован!");
                }
                else
                {
                    products.Add(product);
                    myDbContext.SaveChanges();
                }
                count--;
            }
        }

        #endregion

        #region Заполнить таблицу проданных товаров.

        /// <summary>
        /// Заполнить таблицу проданных товаров.
        /// </summary>
        public void FillSaleProducts()
        {
            int i;

            // заполняем список всех продаж
            foreach (Sale sale in myDbContext.Sales)
            {
                i = rnd.Next(1, 3);    // количество товаров в продаже от 1 до 3
                while (i > 0)
                {
                    myDbContext.SaleProducts.Add(RandomSaleProduct(sale.SaleId));
                    i--;
                }
            }

            myDbContext.SaveChanges();
        }

        #endregion

        #region Заполнить таблицу заказанных товаров.

        /// <summary>
        /// Заполнить таблицу заказанных товаров.
        /// </summary>
        public void FillOrderProducts()
        {
            int i;

            // заполняем список всех заказов
            foreach (Order order in myDbContext.Orders)
            {
                i = rnd.Next(5, 15);    // количество товаров в заказе от 5 до 25
                while (i > 0)
                {
                    myDbContext.OrderProducts.Add(RandomOrderProduct(order.OrderId));
                    i--;
                }
            }

            myDbContext.SaveChanges();
        }

        #endregion

        #endregion

        #region Генерация записей.

        #region Сгенерировать сотрудника.

        /// <summary>
        /// Сгенерировать сотрудника.
        /// </summary>
        /// <returns>Сотрудник.</returns>
        protected async System.Threading.Tasks.Task<Worker> RandomWorkerAsync()
        {
            Worker worker = new Worker();

            string pathFirstnames = @"..\..\..\Resourses\Workers\Firstnames.txt";
            string pathSurtnames = @"..\..\..\Resourses\Workers\Surnames.txt";
            string pathPatronymics = @"..\..\..\Resourses\Workers\Patronymics.txt";
            string pathTelNumberCodes = @"..\..\..\Resourses\Workers\TelNumberCodes.txt";

            string line;
            List<string> list = new List<string>();

            List<string> firstnames = new List<string>();
            List<string> surnames = new List<string>();
            List<string> patronymics = new List<string>();

            List<int> departamentIds = new List<int>(); // список отделов

            try
            {
                // заполняем список отделов
                foreach (Departament departament in myDbContext.Departaments)
                    departamentIds.Add(departament.DepartamentId);

                using (StreamReader sr = new StreamReader(pathFirstnames, System.Text.Encoding.Default))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        firstnames.Add(line);
                    }
                    line = null;
                }

                using (StreamReader sr = new StreamReader(pathSurtnames, System.Text.Encoding.Default))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        surnames.Add(line);
                    }
                    line = null;
                }

                using (StreamReader sr = new StreamReader(pathPatronymics, System.Text.Encoding.Default))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        patronymics.Add(line);
                    }
                    line = null;
                }

                using (StreamReader sr = new StreamReader(pathTelNumberCodes, System.Text.Encoding.Default))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        list.Add(line);
                    }
                    worker.Tel = string.Format("+7({0}){1}", list[rnd.Next(list.Count)], generator.GenerateRandomNumeric(7));   // назначаем случайный номер телефона
                    line = null;
                    list.Clear();
                }

                string rndSurname = surnames[rnd.Next(surnames.Count)]; // определяем случайную фамилию
                string rndFirstname = firstnames[rnd.Next(firstnames.Count)];   // определяем случайное имя
                string rndPatronymic = patronymics[rnd.Next(patronymics.Count)];    // определяем случайное отчество

                // теперь назначаем сотруднику полное имя
                worker.Fullname = string.Format("{0} {1} {2}", rndSurname, rndFirstname, rndPatronymic);

                worker.Log = Transliterator.Front($"{rndSurname}{rndFirstname[0]}{rndPatronymic[0]}");  // назначаем логин в формате ФамилияИО
                worker.Pass = generator.GenerateRandomString(12); // назначаем случайный пароль
                worker.DepartamentId = departamentIds[rnd.Next(departamentIds.Count)];  // назначаем случайный отдел
                worker.Lvl = rnd.Next(1, 4);  // назначаем случайный уровень доступа
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return worker;
        }

        #endregion.

        #region Сгенерировать поставщика.

        /// <summary>
        /// Сгенерировать поставщика.
        /// </summary>
        /// <returns>Поставщик.</returns>
        protected async System.Threading.Tasks.Task<Supplier> RandomSupplierAsync()
        {
            Supplier supplier = new Supplier();

            string pathSupplierNames = @"..\..\..\Resourses\Suppliers\SupplierNames.txt";
            string pathTelNumberCodes = @"..\..\..\Resourses\Workers\TelNumberCodes.txt";

            string line;
            List<string> list = new List<string>();
            List<int> orderIds = new List<int>(); // список заказов

            try
            {
                // заполняем список заказов
                foreach (Order order in myDbContext.Orders)
                    orderIds.Add(order.OrderId);

                using (StreamReader sr = new StreamReader(pathSupplierNames, System.Text.Encoding.Default))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        list.Add(line);
                    }
                    supplier.SupplierName = list[rnd.Next(list.Count)]; // назначаем случайное наименование поставщика
                    line = null;
                    list.Clear();
                }

                using (StreamReader sr = new StreamReader(pathTelNumberCodes, System.Text.Encoding.Default))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        list.Add(line);
                    }
                    supplier.Tel = String.Format("+7({0}){1}", list[rnd.Next(list.Count)], generator.GenerateRandomNumeric(7)); // назначаем случайный номер телефона
                    line = null;
                    list.Clear();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return supplier;
        }

        #endregion

        #region Сгенерировать продажу.

        /// <summary>
        /// Сгенерировать продажу.
        /// </summary>
        /// <returns>Продажа.</returns>
        protected Sale RandomSale()
        {
            Sale sale = new Sale();

            //int rndMinute = Convert.ToInt32(generator.GenerateRandomNumeric(2));   // случайное количество минут
            DateTime rndDateTime = Convert.ToDateTime(generator.GenerateRandomDate(DateTime.Now.AddYears(-1).ToString("d"), DateTime.Now.ToString("d"))); // случайная дата в пределах одного года

            List<int> workerIds = new List<int>(); // список сотрудников
            List<int> productIds = new List<int>(); // список товаров

            try
            {
                // заполняем список всех сотрудников
                foreach (Worker worker in myDbContext.Workers)
                    workerIds.Add(worker.WorkerId);

                // заполняем список всех товаров
                foreach (Product product in myDbContext.Products)
                    productIds.Add(product.ProductId);

                sale.WorkerId = workerIds[rnd.Next(workerIds.Count)];    // назначаем случайного сотрудника
                sale.SaleDate = rndDateTime;    // назначаем дату продажи

                // проверяем, не является ли выбранный сотрудник админом
                // если да, то переназначаем сотрудника
                while (sale.WorkerId == 1)
                {
                    sale.WorkerId = workerIds[rnd.Next(workerIds.Count)];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return sale;
        }

        #endregion

        #region Сгенерировать заказ.

        /// <summary>
        /// Сгенерировать заказ.
        /// </summary>
        /// <returns>Заказ.</returns>
        protected Order RandomOrder()
        {
            Order order = new Order();
            DateTime rndDateTime = Convert.ToDateTime(generator.GenerateRandomDate(DateTime.Now.AddYears(-1).ToString("d"), DateTime.Now.ToString("d"))); // случайная дата  в пределах одного года

            List<int> workerIds = new List<int>(); // список сотрудников
            List<int> supplierIds = new List<int>(); // список поставщиков

            try
            {
                // заполняем список сотрудников
                foreach (Worker worker in myDbContext.Workers)
                    workerIds.Add(worker.WorkerId);

                // заполняем список поставщиков
                foreach (Supplier supplier in myDbContext.Suppliers)
                    supplierIds.Add(supplier.SupplierId);

                order.WorkerId = workerIds[rnd.Next(workerIds.Count)];  // назначаем случайного сотрудника
                order.SupplierId = supplierIds[rnd.Next(supplierIds.Count)];    // назначаем случайного поставщика
                order.OrderDate = rndDateTime;  // назначаем дату заказа
                order.SupplyDate = Convert.ToDateTime(generator.GenerateRandomDate(rndDateTime.ToString("d"), rndDateTime.AddMonths(+1).ToString("d"))); // назначаем дату поставки в пределах одного месяца после заказа
                
                // проверяем, не является ли выбранный сотрудник админом
                // если да, то переназначаем сотрудника
                while (order.WorkerId == 1)
                {
                    order.WorkerId = workerIds[rnd.Next(workerIds.Count)];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return order;
        }

        #endregion

        #region Сгенерировать товар.

        /// <summary>
        /// Сгенерировать товар.
        /// </summary>
        /// <returns>Товар.</returns>
        protected async System.Threading.Tasks.Task<Product> RandomProductAsync()
        {
            Product product = new Product();
            OrderProduct orderProduct = new OrderProduct();

            string pathProducts = @"..\..\..\Resourses\Products\Products.txt";  // путь к текстовому файлу, который содержит данные товара

            string line;
            List<string> productNames = new List<string>(); // список наименований товаров
            List<string> units = new List<string>();    // список соответствующих единиц измерения
            List<double> prices = new List<double>();   // список соответствующих розничных цен
            List<string> departamentNames = new List<string>(); // список соответствующих наименований отделов

            try
            {
                using (StreamReader sr = new StreamReader(pathProducts, System.Text.Encoding.Default))
                {
                    // читаем тектовый файл с данными товаров
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        // разделяем с помощью символа '/' наименование отдела, наименование товара, единицу измерения и цену
                        // и заносим их в соответствующие списки
                        var prod = line.Split('/');
                        departamentNames.Add(prod[0]);
                        productNames.Add(prod[1]);
                        units.Add(prod[2]);
                        prices.Add(Convert.ToDouble(prod[3]));
                    }

                    int rndProd = rnd.Next(productNames.Count); // выбираем случайный товар
                    string depName = departamentNames[rndProd]; // наименование этого товара

                    // определяем id отдела по его имени
                    var depId = (from d in myDbContext.Departaments
                                 where d.DepartamentName == depName
                                 select d.DepartamentId).First();

                    product.DepartamentId = depId; // назначаем отдел
                    product.ProductName = productNames[rndProd];    // назначаем наименование
                    product.Amount = rnd.Next(1, 100); // назначаем случайное количество товара
                    product.Unit = units[rndProd];  // назначаем единицу измерения
                    product.Price = prices[rndProd]/100;    // назначаем розничную цену

                    line = null;
                    productNames.Clear();
                    units.Clear();
                    prices.Clear();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return product;
        }

        #endregion

        #region Сгенерировать таблицу проданных товаров.

        /// <summary>
        /// Сгенерировать список проданных товаров.
        /// </summary>
        /// <returns></returns>
        protected SaleProduct RandomSaleProduct(int o)
        {
            SaleProduct saleProduct = new SaleProduct();
            List<int> saleIds = new List<int>(); // список продаж
            List<int> productIds = new List<int>(); // список товаров

            try
            {
                // заполняем список всех продаж
                foreach (Sale sale in myDbContext.Sales)
                    saleIds.Add(sale.SaleId);

                // заполняем список всех товаров
                foreach (Product product in myDbContext.Products)
                    productIds.Add(product.ProductId);

                saleProduct.SaleId = o;    // определяем товар в список продажи
                saleProduct.ProductId = productIds[rnd.Next(productIds.Count)];    // назначаем случайный товар
                saleProduct.Amount = rnd.Next(1, 3); // назначаем количество проданного товара от 1 до 3

                // определяем розничную цену выбранного товара
                var prodPrice = (from p in myDbContext.Products
                                 where p.ProductId == saleProduct.ProductId
                                 select p.Price).First();

                saleProduct.Sum = prodPrice * saleProduct.Amount;   // сумма = цена * количество
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return saleProduct;
        }

        #endregion

        #region Сгенерировать таблицу заказанных товаров.

        /// <summary>
        /// Сгенерировать список заказанных товаров.
        /// </summary>
        /// <param name="o">Номер заказа.</param>
        /// <returns>Заказанные товары.</returns>
        public OrderProduct RandomOrderProduct(int o)
        {
            OrderProduct orderProduct = new OrderProduct();
            List<int> orderIds = new List<int>();   // список заказов
            List<int> productIds = new List<int>(); // список товаров

            try
            {
                // заполняем список всех заказов
                foreach (Order order in myDbContext.Orders)
                    orderIds.Add(order.OrderId);

                // заполняем список всех товаров
                foreach (Product product in myDbContext.Products)
                    productIds.Add(product.ProductId);

                orderProduct.OrderId = o;    // определяем товар в список заказа
                orderProduct.ProductId = productIds[rnd.Next(productIds.Count)];    // назначаем случайный товар
                orderProduct.Amount = rnd.Next(5, 15);    // назначаем количество заказанного товара в диапазоне от 5 до 15

                // определяем розничную цену выбранного товара
                var prodPrice = (from p in myDbContext.Products
                                 where p.ProductId == orderProduct.ProductId
                                 select p.Price).First();

                orderProduct.Price = prodPrice - (prodPrice * 0.3); // закупочная цена = розничная цена минус 30%
                orderProduct.Sum = orderProduct.Price * orderProduct.Amount;    // сумма = цена * количество
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return orderProduct;
        }

        #endregion

        #endregion
    }
}