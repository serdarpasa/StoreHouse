using System.Windows;
using System.Windows.Input;
using disUtils;
using System.Linq;

namespace Storehouse.View
{
    
    public partial class WinPasswordChange
    {
        public WinPasswordChange()
        {
            InitializeComponent();
        }

        private StorehouseEntities db = new StorehouseEntities();
  
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {        
            if (App.CurrentUser == null) _Exit(sender,e);
            tbx_Employees.Text = App.CurrentUser.LastName+" "+App.CurrentUser.FirstName+" "+App.CurrentUser.MiddleName;
            tbx_Pass.Focus();
     
        }

        private void _Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void _Save(object sender, RoutedEventArgs e)
        {
            if (tbx_Pass.Password != Crypt.DecryptStr(App.CurrentUser.Password))
            {
                MessageBox.Show("Неверный старый пароль. Пароль вводится с учетом регистра");
                return;
            }
            if (tbx_NewPass.Password != tbx_NewPassRepeat.Password)
            {
                MessageBox.Show("Новый пароль и подтверждение не совпадают");
                return;
            }

        
            try
            {
                var x = db.Employeers.FirstOrDefault(_=>_.Id == App.CurrentUser.Id);
                x.Password = Crypt.EncryptStr(tbx_NewPass.Password);
                db.SaveChanges();
            }
            catch { };

            
            _Exit(sender,e);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    {
                        _Exit(sender,e);
                        break;
                    }
                case Key.Enter:
                    {
                        _Save(sender,e);
                        break;
                    }
            }
        }

       

      
    }
}
