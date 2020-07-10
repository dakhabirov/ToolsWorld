using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.WPF.View;

namespace ToolsWorld.WPF
{
    public partial class WorkersWindow : Window
    {
        private readonly MyDbContext myDbContext = new MyDbContext();

        public WorkersWindow()
        {
            InitializeComponent();

            List<Worker> workers = new List<Worker>(myDbContext.Workers);   // список всех товаров
            WorkersDataGrid.ItemsSource = workers;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            WorkerWindow workerWindow = new WorkerWindow();
            workerWindow.EnterButton.Content = "Добавить";
            workerWindow.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Worker worker = (Worker)WorkersDataGrid.SelectedItem;

                WorkerWindow workerWindow = new WorkerWindow();

                workerWindow.LogTextBox.Text = worker.Log;
                workerWindow.LogTextBox.IsEnabled = false;
                workerWindow.PasswordBox.Visibility = Visibility.Hidden;
                workerWindow.PasswordLabel.Visibility = Visibility.Hidden;
                try { workerWindow.FullnameTextBox.Text = worker.Fullname; } catch { }
                try { workerWindow.TelTextBox.Text = worker.Tel; } catch { }
                try { workerWindow.DepartamentNamesComboBox.Text = worker.Departament.DepartamentName; } catch { }
                try { workerWindow.LvlsComboBox.Text = worker.Lvl.ToString(); } catch { }
                workerWindow.EnterButton.Content = "Сохранить";

                workerWindow.Show();
            }
            catch
            { }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkersDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < WorkersDataGrid.SelectedItems.Count; i++)
                {
                    if (WorkersDataGrid.SelectedItems[i] is Worker worker)
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить пользователя с логином '{worker.Log}'?", "Предупреждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            if (worker.Log != "admin")
                            {
                                Worker.Delete(worker.WorkerId);
                                MessageBox.Show($"{worker.Fullname} успешно удален из базы данных.", "Сообщение");
                                Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Невозможно удалить администратора.", "Ошибка");
                            }
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
                case "Логин":
                    try
                    {
                        cv = Worker.FilterByLog(ValueTextBox.Text);
                        WorkersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Полное имя":
                    try
                    {
                        cv = Worker.FilterByFullname(ValueTextBox.Text);
                        WorkersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Отдел":
                    try
                    {
                        cv = Worker.FilterByDepartamentName(ValueTextBox.Text);
                        WorkersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Номер телефона":
                    try
                    {
                        cv = Worker.FilterByTel(ValueTextBox.Text);
                        WorkersDataGrid.SelectedItem = cv.CurrentItem;
                    }
                    catch
                    {
                    }
                    break;
                case "Уровень доступа":
                    try
                    {
                        cv = Worker.FilterByLvl(Convert.ToInt32(ValueTextBox.Text));
                        WorkersDataGrid.SelectedItem = cv.CurrentItem;
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
                case "Логин":
                    try
                    {
                        cv = Worker.FilterByLog(ValueTextBox.Text);
                        WorkersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Полное имя":
                    try
                    {
                        cv = Worker.FilterByFullname(ValueTextBox.Text);
                        WorkersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Отдел":
                    try
                    {
                        cv = Worker.FilterByDepartamentName(ValueTextBox.Text);
                        WorkersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Номер телефона":
                    try
                    {
                        cv = Worker.FilterByTel(ValueTextBox.Text);
                        WorkersDataGrid.ItemsSource = cv;
                    }
                    catch
                    {
                    }
                    break;
                case "Уровень доступа":
                    try
                    {
                        cv = Worker.FilterByLvl(Convert.ToInt32(ValueTextBox.Text));
                        WorkersDataGrid.ItemsSource = cv;
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
        public void Refresh()
        {
            WorkersDataGrid.ItemsSource = Worker.GetWorkers();
            WorkersDataGrid.SelectedItems.Clear();
            ParamComboBox.SelectedItem = null;
            ValueTextBox.Text = null;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}