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
    /// Логика взаимодействия для AddNewHallWindow.xaml
    /// </summary>
    public partial class AddNewHallWindow : Window
    {
        DB.Entity db = HelpClasses.StaticClass.InicializateDateBase;

        public AddNewHallWindow()
        {
            InitializeComponent();

            cbxChiefs.ItemsSource = db.Chiefs.ToList();
            cbxChiefs.DisplayMemberPath = "Name";
            cbxChiefs.SelectedIndex = 0;

            cbxDepartments.ItemsSource = db.Departments.ToList();
            cbxDepartments.DisplayMemberPath = "Name";
            cbxDepartments.SelectedIndex = 0;
        }

        private void click_Back(object sender, RoutedEventArgs e)
        {
            new SelectBuildingWindow().Show();
            this.Close();
        }

        private void click_Add(object sender, RoutedEventArgs e)
        {
            db.Halls.Add(new DB.Halls {
                Square = double.Parse(tbxSquare.Text),
                Windows = int.Parse(tbxWindows.Text),
                Heating = int.Parse(tbxHeating.Text),
                Target = tbxTarget.Text,
                Departments = (DB.Departments)cbxDepartments.SelectedItem,
                Chiefs = (DB.Chiefs)cbxChiefs.SelectedItem,
                Buildings = HelpClasses.StaticClass.SelectBuilding
            });
            db.SaveChanges();

            if (MessageBox.Show("Добавление нового помещения прошло успшено! Желаете добавить ещё?", "Perfect", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                new AddNewHallWindow().Show();
                this.Close();
            }
            else
                click_Back(null,null);
        }

        private void click_AddNewChief(object sender, RoutedEventArgs e)
        {
            new AddNewChiefWindow().Show();
            this.Close();
        }

        private void click_AddNewDepartment(object sender, RoutedEventArgs e)
        {
            new AddNewDepartmentWindow().Show();
            this.Close();
        }
    }
}
