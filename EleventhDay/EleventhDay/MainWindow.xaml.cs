using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EleventhDay
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DB.Entity db = HelpClasses.StaticClass.InicializateDateBase;

        public MainWindow()
        {
            InitializeComponent();

            click_Search(null, null);

            cbxSearch.ItemsSource = db.Buildings.ToList();
            cbxSearch.DisplayMemberPath = "Address";
        }

        private void click_Search(object sender, RoutedEventArgs e)
        {
            spBuildings.Children.Clear();

            var qwery = db.Buildings.Where(w=>w.Kadastr != null);
            if (cbxSearch.Text.Length != 0)
                qwery = qwery.Where(w=>w.Address.Contains(cbxSearch.Text));

            foreach(var i in qwery)
            {
                UserControls.BuildingsUserControl uc = new UserControls.BuildingsUserControl {
                    DataContext = i,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,0,0,1),
                    Margin = new Thickness(10),
                    Cursor = Cursors.Hand
                };
                uc.cbxDelete.Click += CbxDelete_Click;
                uc.MouseLeftButtonDown += Uc_MouseLeftButtonDown;
                spBuildings.Children.Add(uc);
            }
        }

        private void CbxDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.Buildings.Remove((DB.Buildings)((Button)sender).DataContext);
                db.SaveChanges();
                MessageBox.Show("Удаление здания прошло успешно!", "Perfect", MessageBoxButton.OK, MessageBoxImage.Information);
                click_Search(null, null);
            }
            catch
            {
                MessageBox.Show("Удаление здания не может быть произведена из-за наличия у него помещений! Удалите сначала помещения!");
            }
        }

        private void Uc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HelpClasses.StaticClass.SelectBuilding = (DB.Buildings)((UserControls.BuildingsUserControl)sender).DataContext;
            new Windows.SelectBuildingWindow().Show();
            this.Close();
        }

        private void keydoown_cbxSearch(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                click_Search(null,null);
        }

        private void click_Add(object sender, RoutedEventArgs e)
        {
            new Windows.AddNewBuildingWindow().Show();
            this.Close();
        }
    }
}
