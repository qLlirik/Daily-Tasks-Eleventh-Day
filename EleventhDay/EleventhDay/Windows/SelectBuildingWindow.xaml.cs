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
using System.Windows.Shapes;

namespace EleventhDay.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectBuildingWindow.xaml
    /// </summary>
    public partial class SelectBuildingWindow : Window
    {
        public SelectBuildingWindow()
        {
            InitializeComponent();
            spBuilding.DataContext = HelpClasses.StaticClass.SelectBuilding;
        }

        private void click_Back(object sender, RoutedEventArgs e)
        {
            HelpClasses.StaticClass.SelectBuilding = null;
            new MainWindow().Show();
            this.Close();
        }

        private void click_Halls(object sender, RoutedEventArgs e)
        {
            new BuildingsHallsWindow().Show();
            this.Close();
        }

        private void click_AddNewHall(object sender, RoutedEventArgs e)
        {
            new AddNewHallWindow().Show();
            this.Close();
        }
    }
}
