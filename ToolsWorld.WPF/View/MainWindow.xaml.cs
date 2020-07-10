using System;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.WPF.View;

namespace ToolsWorld.WPF
{
    /// <summary>
    /// Главное окно.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Инициализировать случайные записи.
        /// </summary>
        private async void InitializeDataMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            Initializer randomInitializer = new Initializer();
            await randomInitializer.FillDepartamentsAsync();
            await randomInitializer.FillWorkerAsync(10);
            await randomInitializer.FillSupplierAsync(10);
            await randomInitializer.FillProductAsync(20);
            randomInitializer.FillSale(10);
            randomInitializer.FillOrder(10);
            randomInitializer.FillSaleProducts();
            randomInitializer.FillOrderProducts();
            MessageBox.Show("Инициализация записей базы данных прошла успешно.");
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            SalesWindow salesWindow = new SalesWindow();
            salesWindow.LogTextBlock.Text = LogTextBlock.Text;
            salesWindow.Show();
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow productsWindow = new ProductsWindow();
            productsWindow.LogLabel.Content = LogTextBlock.Text;

            // разграничение доступа
            if (Convert.ToInt32(LvlTextBlock.Text) < 2)
            {
                productsWindow.InsertButton.IsEnabled = false;
                productsWindow.UpdateButton.IsEnabled = false;
                productsWindow.DeleteButton.IsEnabled = false;
            }

            productsWindow.Show();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow();
            ordersWindow.LogTextBlock.Text = LogTextBlock.Text;

            // разграничение доступа
            if (Convert.ToInt32(LvlTextBlock.Text) < 2)
            {
                ordersWindow.InsertButton.IsEnabled = false;
                ordersWindow.DeleteButton.IsEnabled = false;
            }

            ordersWindow.Show();
        }

        private void WorkersButton_Click(object sender, RoutedEventArgs e)
        {
            WorkersWindow workersWindow = new WorkersWindow();
            workersWindow.Show();
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            SuppliersWindow suppliersWindow = new SuppliersWindow();

            // разграничение доступа
            if (Convert.ToInt32(LvlTextBlock.Text) < 2)
            {
                suppliersWindow.InsertButton.IsEnabled = false;
                suppliersWindow.UpdateButton.IsEnabled = false;
                suppliersWindow.DeleteButton.IsEnabled = false;
            }

            suppliersWindow.Show();
        }

        private void DepartamentsButton_Click(object sender, RoutedEventArgs e)
        {
            DepartamentsWindow departamentsWindow = new DepartamentsWindow();
            departamentsWindow.Show();
        }

        private void ChangePasswordMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow();

            using (MyDbContext myDbContext = new MyDbContext())
            {
                Worker worker = Worker.GetWorkerByLog(LogTextBlock.Text); // идентифицируем сотрудника
                changePasswordWindow.LogTextBlock.Text = worker.Log;
            }

            changePasswordWindow.Show();
        }

        /// <summary>
        /// Разграничение доступа.
        /// </summary>
        public void AccessControl()
        {
            if (Convert.ToInt32(LvlTextBlock.Text) < 3)
            {
                WorkersButton.IsEnabled = false;
                DepartamentsButton.IsEnabled = false;
                InitializeDataMenuItemAsync.IsEnabled = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccessControl();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ExitAccountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
