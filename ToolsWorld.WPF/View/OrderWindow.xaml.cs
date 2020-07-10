using System;
using System.Collections.Generic;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using System.Linq;

namespace ToolsWorld.WPF.View
{
    public partial class OrderWindow : Window
    {
        private static readonly MyDbContext myDbContext = new MyDbContext();
        protected static DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("d"));

        public OrderWindow()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)ProductsDataGrid.SelectedItem;
            bool unique = true;

            var lastOrder = myDbContext.Orders.ToList().Last(); // последняя запись в таблице заказов

            // проверяем, не пытается ли пользователь добавить один и тот же товар дважды
            foreach (OrderProduct orderProduct in myDbContext.OrderProducts)
            {
                if ((orderProduct.OrderId == lastOrder.OrderId) && (orderProduct.Product.ProductName == product.ProductName))
                {
                    unique = false;
                }
            }

            if (unique == true)
            {
                try
                {
                    if (SupplierNamesComboBox.Text != "")
                    {
                        int orderId = lastOrder.OrderId;
                        int productId = product.ProductId;
                        int amount = Convert.ToInt32(AmountTextBox.Text);
                        double price = product.Price * 0.3; // закупочная цена = розничная цена - 30%

                        lastOrder.SupplierId = Supplier.GetSupplierBySupplierName(SupplierNamesComboBox.Text).SupplierId;   // определяем поставщика
                        OrderProduct.Insert(orderId, productId, amount, price); // добавляем товар в заказ
                        myDbContext.SaveChanges();

                        MessageBoxResult result = MessageBox.Show($"Товар успешно добавлен. Хотите еще добавить товар?", "Сообщение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        { }
                        else
                            Close();
                    }
                    else
                    {
                        MessageBox.Show("Выберите поставщика из списка.", "Ошибка");
                    }
                }
                catch
                {
                    MessageBox.Show("Проверьте корректность введенных данных!", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Данный товар уже имеется в текущем заказе!", "Ошибка");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Добавить.
        /// </summary>
        private void Insert()
        {
            try
            {
                Random rnd = new Random();
                List<int> supplierIds = new List<int>();

                // заполняем список поставщиков
                foreach (Supplier supplier in myDbContext.Suppliers)
                    supplierIds.Add(supplier.SupplierId);

                Worker worker = Worker.GetWorkerByLog(LogTextBlock.Text);
                DateTime orderDate = currentDate;
                Order.Insert(worker.WorkerId, supplierIds[rnd.Next(supplierIds.Count)], orderDate);
            }
            catch
            {
                Close();
                MessageBox.Show("В базе данных отсутствуют поставщики.", "Ошибка");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SupplierNamesComboBox.ItemsSource = Supplier.GetSupplierNames();    // список поставщиков
            ProductsDataGrid.ItemsSource = myDbContext.Products.ToList();   // список товаров
            Insert();
        }
    }
}
