using System;
using System.Collections.Generic;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using System.Linq;

namespace ToolsWorld.WPF.View
{
    public partial class SaleWindow : Window
    {
        private static readonly MyDbContext myDbContext = new MyDbContext();

        public SaleWindow()
        {
            InitializeComponent();

            List<Product> products = new List<Product>(myDbContext.Products);   // список всех продаж
            ProductsDataGrid.ItemsSource = products;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)ProductsDataGrid.SelectedItem;
            bool unique = true;

            var lastSale = myDbContext.Sales.ToList().Last(); // последняя запись в таблице продаж

            // проверяем, не пытается ли пользователь добавить один и тот же товар дважды
            foreach (SaleProduct saleProduct in myDbContext.SaleProducts)
            {
                if ((saleProduct.SaleId == lastSale.SaleId) && (saleProduct.Product.ProductName == product.ProductName))
                {
                    unique = false;
                }
            }

            if (unique == true)
            {
                try
                {
                    int saleId = lastSale.SaleId;
                    int productId = product.ProductId;
                    int amount = Convert.ToInt32(AmountTextBox.Text);
                    double sum = (Double)amount * product.Price;

                    SaleProduct.Insert(saleId, productId, amount, sum);
                    MessageBoxResult result = MessageBox.Show($"Товар успешно добавлен. Хотите еще добавить товар?", "Сообщение", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    { }
                    else
                        Close();
                }
                catch
                {
                    MessageBox.Show("Проверьте корректность введенных данных!", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Данный товар уже имеется в текущей продаже!", "Ошибка");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}