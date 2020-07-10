using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.WPF.View;

namespace ToolsWorld.WPF
{
    public partial class SuppliersWindow : Window
    {
        private readonly MyDbContext myDbContext = new MyDbContext();

        public SuppliersWindow()
        {
            InitializeComponent();

            List<Supplier> suppliers = new List<Supplier>(myDbContext.Suppliers);   // список всех поставщиков
            SuppliersDataGrid.ItemsSource = suppliers;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            SupplierWindow supplierWindow = new SupplierWindow();
            supplierWindow.EnterButton.Content = "Добавить";
            supplierWindow.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Supplier supplier = (Supplier)SuppliersDataGrid.SelectedItem;

                SupplierWindow supplierWindow = new SupplierWindow();

                supplierWindow.SupplierNameTextBox.Text = supplier.SupplierName;
                supplierWindow.SupplierNameTextBox.IsEnabled = false;
                supplierWindow.TelTextBox.Text = supplier.Tel;
                supplierWindow.EnterButton.Content = "Сохранить";

                supplierWindow.Show();
            }
            catch
            { }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < SuppliersDataGrid.SelectedItems.Count; i++)
                {
                    if (SuppliersDataGrid.SelectedItems[i] is Supplier supplier)
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить {supplier.SupplierName} из базы данных?", "Предупреждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            Supplier.Delete(supplier.SupplierId);
                            MessageBox.Show($"Поставщик {supplier.SupplierName} успешно удален из базы данных.", "Сообщение");
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
                        cv = Supplier.FilterBySupplierName(ValueTextBox.Text);
                        SuppliersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Номер телефона":
                    try
                    {
                        cv = Supplier.FilterByTel(ValueTextBox.Text);
                        SuppliersDataGrid.SelectedItem = cv.CurrentItem;
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
                        cv = Supplier.FilterBySupplierName(ValueTextBox.Text);
                        SuppliersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Номер телефона":
                    try
                    {
                        cv = Supplier.FilterByTel(ValueTextBox.Text);
                        SuppliersDataGrid.ItemsSource = cv;
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
        /// Сброс.
        /// </summary>
        public void Refresh()
        {
            SuppliersDataGrid.ItemsSource = Supplier.GetSuppliers();
            SuppliersDataGrid.SelectedItems.Clear();
            ParamComboBox.SelectedItem = null;
            ValueTextBox.Text = null;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
