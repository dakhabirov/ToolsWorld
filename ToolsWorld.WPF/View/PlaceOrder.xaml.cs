using System;
using System.Collections.Generic;
using System.Windows;
using ToolsWorld.BL.Model;

namespace ToolsWorld.WPF.View
{
    public partial class PlaceOrder : Window
    {
        public PlaceOrder()
        {
            InitializeComponent();

            SupplierNamesComboBox.ItemsSource = Supplier.GetSupplierNames();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            Place();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Сформировать заказ.
        /// </summary>
        private void Place()
        {
            Worker worker = Worker.GetWorkerByLog(LogLabel.Content.ToString());
            Supplier supplier = Supplier.GetSupplierBySupplierName(SupplierNamesComboBox.Text);

            try
            {
                List<Product> products = Product.GetFewProducts(Convert.ToInt32(TextBox1.Text));
                Product.PlaceOrder(products, Convert.ToInt32(TextBox2.Text), worker.WorkerId, supplier.SupplierId);
                MessageBox.Show("Заказ успешно сформирован.", "Сообщение");
                Close();
            }
            catch
            {
                MessageBox.Show("Проверьте корректность введенных данных!", "Ошибка");
            }
        }
    }
}
