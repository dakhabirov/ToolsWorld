using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.WPF.View;

namespace ToolsWorld.WPF
{
    public partial class ProductsWindow : Window
    {
        private readonly MyDbContext myDbContext = new MyDbContext();

        public ProductsWindow()
        {
            InitializeComponent();

            List<Product> products = new List<Product>(myDbContext.Products);   // список всех товаров
            ProductsDataGrid.ItemsSource = products;
        }

        private void PlaceOrderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PlaceOrder placeOrder = new PlaceOrder();
            placeOrder.LogLabel.Content = LogLabel.Content;
            placeOrder.Show();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow();
            productWindow.EnterButton.Content = "Добавить";
            productWindow.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = (Product)ProductsDataGrid.SelectedItem;

                ProductWindow productWindow = new ProductWindow();

                productWindow.ProductNameTextBox.Text = product.ProductName;
                productWindow.ProductNameTextBox.IsEnabled = false;
                productWindow.AmountTextBox.Text = product.Amount.ToString();
                productWindow.UnitComboBox.Text = product.Unit;
                productWindow.PriceTextBox.Text = product.Price.ToString();
                productWindow.DepartamentNamesComboBox.Text = product.Departament.DepartamentName;
                productWindow.EnterButton.Content = "Сохранить";

                productWindow.Show();
            }
            catch
            { }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < ProductsDataGrid.SelectedItems.Count; i++)
                {
                    if (ProductsDataGrid.SelectedItems[i] is Product product)
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить товар из базы данных?", "Предупреждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            Product.Delete(product.ProductId);
                            MessageBox.Show($"Товар успешно удален из базы данных.", "Сообщение");
                            Refresh();
                        }
                    }
                }
            }
        }

        #region Поиск

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cv;
            string param = ParamComboBox.Text;

            switch (param)
            {
                case "Наименование":
                    try
                    {
                        cv = Product.FilterByProductName(ValueTextBox.Text);
                        ProductsDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Количество":
                    try
                    {
                        cv = Product.FilterByAmount(Convert.ToInt32(ValueTextBox.Text));
                        ProductsDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Единица измерения":
                    try
                    {
                        cv = Product.FilterByUnit(ValueTextBox.Text);
                        ProductsDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Розничная цена":
                    try
                    {
                        cv = Product.FilterByPrice(Convert.ToDouble(ValueTextBox.Text));
                        ProductsDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Отдел":
                    try
                    {
                        cv = Product.FilterByDepartamentName(ValueTextBox.Text);
                        ProductsDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
            }
        }

        #endregion

        #region Фильтр

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cv;
            string param = ParamComboBox.Text;

            switch (param)
            {
                case "Наименование":
                    try
                    {
                        cv = Product.FilterByProductName(ValueTextBox.Text);
                        ProductsDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Количество":
                    try
                    {
                        cv = Product.FilterByAmount(Convert.ToInt32(ValueTextBox.Text));
                        ProductsDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Единица измерения":
                    try
                    {
                        cv = Product.FilterByUnit(ValueTextBox.Text);
                        ProductsDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Розничная цена":
                    try
                    {
                        cv = Product.FilterByPrice(Convert.ToDouble(ValueTextBox.Text));
                        ProductsDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Отдел":
                    try
                    {
                        cv = Product.FilterByDepartamentName(ValueTextBox.Text);
                        ProductsDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
            }
        }

        #endregion

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            myDbContext.Dispose();
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        private void Refresh()
        {
            ProductsDataGrid.ItemsSource = Product.GetProducts();
            ProductsDataGrid.SelectedItems.Clear();
            ParamComboBox.SelectedItem = null;
            ValueTextBox.Text = null;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
