using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using disUtils;
using Storehouse.View;

namespace Storehouse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private StorehouseEntities _db;
       
        public MainWindow()
        {
            // Устанавливаем правильные региональные параметры для WPF
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)
                )
            );

            System.Windows.Documents.TextElement.LanguageProperty.OverrideMetadata(
                typeof(System.Windows.Documents.TextElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)
                )
            );
            InitializeComponent();
            try
            {
                _db = new StorehouseEntities();
                
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к базе данных!\nПроверьте параметры подключения к базе данных!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Opacity = 0;

            var pic = new View.SplashScreen()
            {
                Owner = this,
                ShowInTaskbar = false
            };
            pic.ShowDialog();
            var wind = new DoubleAnimation(0, 0, new Duration(TimeSpan.FromSeconds(1)))
            {
                From = 0.55,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            BeginAnimation(OpacityProperty, wind);

            if (!_db.Employeers.Any(_ => !_.IsLocked && _.Role!=2))
            {
                MessageBox.Show("В системе нет зарегистрированных неблокированных пользователей!\n" +
                                "Вам нужно пройти регистрацию.\n"+
                                "Будет создана учетная запись администратора./"
                               );
                var nw = new WinUserRegistration()
                {
                    Db = _db
                };
                if (nw.ShowDialog() != true)
                {
                    Application.Current.Shutdown(-1);
                }
                _db = new StorehouseEntities();
                if (!_db.Employeers.Any(_ => !_.IsLocked && _.Role != 2))
                {
                    MessageBox.Show("Ничего не изменилось!!!", "Упс!..", MessageBoxButton.OK, MessageBoxImage.Stop);

                    Application.Current.Shutdown(-1);
                }
                
            }
            var users = _db.Employeers.Where(_ => !_.IsLocked && _.Role != 2).ToList();
            CbxUsers.ItemsSource = users.OrderBy(_ => _.LastName).ToList();

            try
            {
                var lastUserId = Properties.Settings.Default.LastUser;
                CbxUsers.SelectedItem = users.FirstOrDefault(_ => _.Id == lastUserId);
            }
            catch (Exception)
            {
                //
            }
            

        }
        private void btn_Input_Click(object sender, RoutedEventArgs e)
        {
            var user = CbxUsers.SelectedItem as Employeer;
            if (user == null)
            {
                MessageBox.Show("Не указан пользователь!");
                CbxUsers.Focus();
                return;
            }
            
            var password = TbxPassword.Password;
            
            if (Crypt.DecryptStr(user.Password) ==  password)
            {
               

                App.CurrentUser = user;
                Properties.Settings.Default.LastUser = user.Id;
                try
                {
                    Properties.Settings.Default.Save();
                }
                catch
                {
                    // ignored
                }

                var nw = new WinHome();
                nw.Show();



                Close();
            }
            else
                MessageBox.Show("Неверный пароль");
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        btn_Input_Click(sender, e);
                        break;
                    }
                case Key.Escape:
                    {
                        Close();
                        break;
                    }
            }
        }
    }
}
