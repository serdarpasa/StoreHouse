using System;
using System.Linq;
using System.Windows;
using disUtils;

namespace Storehouse.View
{
    /// <summary>
    /// Логика взаимодействия для WinUserRegistration.xaml
    /// </summary>
    public partial class WinUserRegistration
    {
        public StorehouseEntities Db;
        public int Role = 1;
        public WinUserRegistration()
        {
            InitializeComponent();
        }

        private void ButtonRegistration_OnClick(object sender, RoutedEventArgs e)
        {
           
           
            var lastName = TbxLastName.Text;
            var firstName = TbxFirstName.Text;
            var middleName = TbxMiddleName.Text;

            if (string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Поле 'Фамилия' обязательно к заполнению");
                return;
            }

            
           
            var password = TbxPassword.Password;
            var passConfirmation = TbxPasswordConfirmation.Password;
            if (password != passConfirmation)
            {
                MessageBox.Show("Пароль не подтвержден!");
                return;
            }
            password = Crypt.EncryptStr(password);

            var user = new Employeer()
            {
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
                Password = password,
                Role = 1
            };
            Db.Employeers.AddObject(user);
            try
            {
                Db.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }

        
            Close();
        }
    }
}
