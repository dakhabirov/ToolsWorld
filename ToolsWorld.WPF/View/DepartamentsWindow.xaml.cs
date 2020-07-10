using System;
using System.ComponentModel;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.BL.ViewModel;
using ToolsWorld.WPF.View;

namespace ToolsWorld.WPF
{
    public partial class DepartamentsWindow : Window
    {
        private readonly MyDbContext myDbContext = new MyDbContext();

        public DepartamentsWindow()
        {
            InitializeComponent();

            DataContext = new DepartamentViewModel();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            DepartamentWindow departamentWindow = new DepartamentWindow();
            departamentWindow.EnterButton.Content = "Добавить";
            departamentWindow.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Departament departament = (Departament)DepartamentsDataGrid.SelectedItem;

                DepartamentWindow departamentWindow = new DepartamentWindow();
                departamentWindow.DepartamentNameTextBox.Text = departament.DepartamentName;
                departamentWindow.DepartamentNameTextBlock.Text = departament.DepartamentName;
                departamentWindow.EnterButton.Content = "Сохранить";
                departamentWindow.Show();
            }
            catch
            { }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartamentsDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < DepartamentsDataGrid.SelectedItems.Count; i++)
                {
                    if (DepartamentsDataGrid.SelectedItems[i] is Departament departament)
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить {departament.DepartamentName} из базы данных?", "Предупреждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                Departament.Delete(departament.DepartamentId);
                                MessageBox.Show($"{departament.DepartamentName} успешно удален из базы данных.", "Сообщение");
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
                case "Наименование":
                    try
                    {
                        cv = Departament.FilterByDepartamentName(ValueTextBox.Text);
                        DepartamentsDataGrid.SelectedItem = cv.CurrentItem;
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
                        cv = Departament.FilterByDepartamentName(ValueTextBox.Text);
                        DepartamentsDataGrid.ItemsSource = cv;
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
            DepartamentsDataGrid.ItemsSource = Departament.GetDepartaments();
            DepartamentsDataGrid.SelectedItems.Clear();
            ParamComboBox.SelectedItem = null;
            ValueTextBox.Text = null;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
