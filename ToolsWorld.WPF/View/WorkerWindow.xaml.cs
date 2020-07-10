using System.Windows;
using System;
using ToolsWorld.BL.Model;

namespace ToolsWorld.WPF.View
{
    public partial class WorkerWindow : Window
    {
        public WorkerWindow()
        {
            InitializeComponent();

            DepartamentNamesComboBox.ItemsSource = Departament.GetDepartamentNames();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EnterButton.Content.ToString() == "Добавить")
                {
                    Insert();
                }
                else if (EnterButton.Content.ToString() == "Сохранить")
                {
                    Update();
                }
            }
            catch
            {
                MessageBox.Show("Проверь правильность введеных данных!", "Ошибка.");
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
            string log = LogTextBox.Text;
            string pass = PasswordBox.Password;
            string fullname = FullnameTextBox.Text;
            string tel = TelTextBox.Text;
            int departamentId = Departament.GetDepartamentId(DepartamentNamesComboBox.Text);
            int lvl = Convert.ToInt32(LvlsComboBox.Text);

            // проверяем на уникальность
            Worker worker = Worker.GetWorkerByLog(log);
            if (worker.Log == log)
            {
                MessageBox.Show($"Пользователь с логином {log} уже зарегистрирован!", "Ошибка");
            }
            else
            {
                Worker.Insert(log, pass, fullname, tel, departamentId, lvl);
                MessageBox.Show($"Пользователь {fullname} успешно добавлен в базу данных.", "Сообщение");
                Close();
            }
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        private void Update()
        {
            string log = LogTextBox.Text;
            string pass = PasswordBox.Password;
            string fullname = FullnameTextBox.Text;
            string tel = TelTextBox.Text;
            int departamentId = Departament.GetDepartamentId(DepartamentNamesComboBox.Text);
            int lvl = Convert.ToInt32(LvlsComboBox.Text);

            Worker.Update(log, pass, fullname, tel, departamentId, lvl);
            MessageBox.Show($"Данные сотрудника {fullname} успешно обновлены.", "Сообщение");
            Close();
        }
    }
}
