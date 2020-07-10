using System;
using System.ComponentModel;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.BL.ViewModel;
using ToolsWorld.WPF.View;
using System.Linq;

namespace ToolsWorld.WPF
{
    public partial class OrdersWindow : Window
    {
        private readonly MyDbContext myDbContext = new MyDbContext();
        protected static DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("d"));

        public OrdersWindow()
        {
            InitializeComponent();

            DataContext = new OrderViewModel();
            OrdersDataGrid.ItemsSource = (from o in myDbContext.Orders select o).ToList();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.LogTextBlock.Text = this.LogTextBlock.Text;
            orderWindow.Show();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < OrdersDataGrid.SelectedItems.Count; i++)
                {
                    if (OrdersDataGrid.SelectedItems[i] is Order order)
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить заказ из базы данных?", "Предупреждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            Order.Delete(order.OrderId);
                            MessageBox.Show($"Заказ успешно удалена из базы данных.", "Сообщение");
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
                case "Дата заказа":
                    try
                    {
                        cv = Order.FilterByOrderDate(Convert.ToDateTime(ValueTextBox.Text));
                        OrdersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                        MessageBox.Show("Дата указана неверно!", "Ошибка");
                    }
                    break;
                case "Оформил":
                    try
                    {
                        cv = Order.FilterByWorkerFullname(ValueTextBox.Text);
                        OrdersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Поставщик":
                    try
                    {
                        cv = Order.FilterBySupplierName(ValueTextBox.Text);
                        OrdersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Дата поставки":
                    try
                    {
                        cv = Order.FilterBySupplyDate(Convert.ToDateTime(ValueTextBox.Text));
                        OrdersDataGrid.SelectedItem = cv.CurrentItem;
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
                case "Дата заказа":
                    try
                    {
                        cv = Order.FilterByOrderDate(Convert.ToDateTime(ValueTextBox.Text));
                        OrdersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                        MessageBox.Show("Дата указана неверно!", "Ошибка");
                    }
                    break;
                case "Оформил":
                    try
                    {
                        cv = Order.FilterByWorkerFullname(ValueTextBox.Text);
                        OrdersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Поставщик":
                    try
                    {
                        cv = Order.FilterBySupplierName(ValueTextBox.Text);
                        OrdersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Дата поставки":
                    try
                    {
                        cv = Order.FilterBySupplyDate(Convert.ToDateTime(ValueTextBox.Text));
                        OrdersDataGrid.ItemsSource = cv;
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

        /// <summary>
        /// Обновить.
        /// </summary>
        private void Refresh()
        {
            OrdersDataGrid.ItemsSource = (from o in myDbContext.Orders select o).ToList();
            OrdersDataGrid.SelectedItems.Clear();
            ParamComboBox.SelectedItem = null;
            ValueTextBox.Text = null;
        }

        private void SetSupplyDateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < OrdersDataGrid.SelectedItems.Count; i++)
                {
                    if (OrdersDataGrid.SelectedItems[i] is Order order)
                    {
                        MessageBoxResult result = MessageBox.Show($"Отметить поставку заказа?", "Предупреждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            order.SupplyDate = currentDate;
                            MessageBox.Show($"Информация о поставке заказа успешно обновлена.", "Сообщение");
                        }
                    }
                }
            }
            myDbContext.SaveChanges();
            Refresh();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
