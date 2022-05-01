using System;
using System.ComponentModel;
using System.Windows;
using disUtils;

namespace Storehouse
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        public static Employeer CurrentUser;
        public static StorehouseEntities Db;
        public App()
        {
            CryptoInit();
            
        }
        private static void CryptoInit()
        {
            try
            {
                Crypt.ProgramKey = new Guid("FD0767FA-1234-5678-81C3-37AB83E0EDC8");
                Crypt.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка запуска приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void RunInBackground(
        DoWorkEventHandler doWork,
        RunWorkerCompletedEventHandler endWork,
        ProgressChangedEventHandler reportProgress = null)
        {
            if (doWork == null) throw new ArgumentNullException("Не указан метод для выполнения работы.");
            if (endWork == null) throw new ArgumentNullException("Не указан метод для завершения работы.");
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += doWork;
            worker.RunWorkerCompleted += endWork;
            if (reportProgress != null)
                worker.ProgressChanged += reportProgress;
            worker.RunWorkerAsync();
        }
    }
}
