using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;

namespace ToolsWorld.WPF.View
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (MyDbContext myDbContext = new MyDbContext())
            {
                Worker worker = Worker.GetWorkerByLog(LogTextBlock.Text);

                if (worker.Pass == OldPasswordBox.Password)  // проверяем, правильно ли введен текущий пароль
                {
                    if (NewPasswordBox.Password == RepeatNewPasswordBox.Password)    // проверяем, совпадают ли новые пароли
                    {
                        Worker.ChangePassword(worker.WorkerId, NewPasswordBox.Password.ToString());  // присваиваем новый пароль
                        MessageBox.Show("Пароль успешно изменен.", "Сообщение");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Введенные новые пароли не совпадают.", "Ошибка");
                    }
                }
                else
                {
                    MessageBox.Show("Текущий пароль введен неверно!", "Ошибка");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
