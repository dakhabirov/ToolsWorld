using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.BL.ViewModel;
using ToolsWorld.WPF.View;

namespace ToolsWorld.WPF
{
    public partial class SalesWindow : Window
    {
        private readonly MyDbContext myDbContext = new MyDbContext();
        protected static DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("d"));

        public string SaleId { get; set; }

        public SalesWindow()
        {
            InitializeComponent();

            DataContext = new SaleViewModel();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            SaleWindow saleWindow = new SaleWindow();
            saleWindow.Show();
            Insert();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SalesDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < SalesDataGrid.SelectedItems.Count; i++)
                {
                    if (SalesDataGrid.SelectedItems[i] is Sale sale)
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить продажу из базы данных?", "Предупреждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                Sale.Delete(sale.SaleId);
                                MessageBox.Show($"Продажа успешно удалена из базы данных.", "Сообщение");
                                Refresh();
                            }
                            catch
                            { }
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
                case "Дата продажи":
                    try
                    {
                        cv = Sale.FilterBySaleDate(Convert.ToDateTime(ValueTextBox.Text));
                        SalesDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                        MessageBox.Show("Дата указана неверно!", "Ошибка");
                    }
                    break;
                case "Реализовал":
                    try
                    {
                        cv = Sale.FilterByWorkerFullName(ValueTextBox.Text);
                        SalesDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Отдел":
                    try
                    {
                        cv = Sale.FilterByWorkerDepartamentName(ValueTextBox.Text);
                        SalesDataGrid.SelectedItem = cv.CurrentItem;
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
                case "Дата продажи":
                    try
                    {
                        cv = Sale.FilterBySaleDate(Convert.ToDateTime(ValueTextBox.Text));
                        SalesDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                        MessageBox.Show("Дата указана неверно!", "Ошибка");
                    }
                    break;
                case "Реализовал":
                    try
                    {
                        cv = Sale.FilterByWorkerFullName(ValueTextBox.Text);
                        SalesDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Отдел":
                    try
                    {
                        cv = Sale.FilterByWorkerDepartamentName(ValueTextBox.Text);
                        SalesDataGrid.ItemsSource = cv;
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

        private void GetDayResultMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow dayResultWindow = new ResultWindow();
            dayResultWindow.GetCurrentDayResultToTextBlock();
            dayResultWindow.GetCurrentDayResultToListView();
            dayResultWindow.Show();
        }

        private void GetLastMonthResultMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow dayResultWindow = new ResultWindow();
            dayResultWindow.GetLastMonthResultToTextBlock();
            dayResultWindow.GetLastMonthResultToListView();
            dayResultWindow.Show();
        }

        private void GetVolumeSalesOnWeekDays_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow resultWindow = new ResultWindow();
            resultWindow.GetYearResultByDaysOfWeekToListView();
            resultWindow.GetYearResultToTextBlock();
            resultWindow.Show();
        }

        private void GetVolumeSalesOnMonths_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow resultWindow = new ResultWindow();
            resultWindow.GetYearResultByMonthsToListView();
            resultWindow.GetYearResultToTextBlock();
            resultWindow.Show();
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
            SalesDataGrid.ItemsSource = Sale.GetSales();
            SalesDataGrid.SelectedItems.Clear();
            ParamComboBox.SelectedItem = null;
            ValueTextBox.Text = null;
        }

        /// <summary>
        /// Добавить.
        /// </summary>
        private void Insert()
        {
            int workerId = Worker.GetWorkerByLog(LogTextBlock.Text).WorkerId;
            DateTime saleDate = currentDate;
            Sale.Insert(workerId, saleDate);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
