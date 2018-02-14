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
    /// Логика взаимодействия для AddNewDepartmentWindow.xaml
    /// </summary>
    public partial class AddNewDepartmentWindow : Window
    {
        DB.Entity db = HelpClasses.StaticClass.InicializateDateBase;

        public AddNewDepartmentWindow()
        {
            InitializeComponent();

            cbxCheif.ItemsSource = db.Chiefs.ToList();
            cbxCheif.DisplayMemberPath = "Name";
            cbxCheif.SelectedIndex = 0;

            cbxUnit.ItemsSource = db.Units.ToList();
            cbxUnit.DisplayMemberPath = "Name";
            cbxUnit.SelectedIndex = 0;

            dpDateStart.SelectedDate = DateTime.Now;
        }

        private void click_Back(object sender, RoutedEventArgs e)
        {
            new AddNewHallWindow().Show();
            this.Close();
        }

        private void click_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((tbxName.Text.Length == 0) || (tbxBoss.Text.Length == 0) || (tbxPhonr.Text.Length == 0) || (tbxOfficeDean.Text.Length == 0))
                    throw new Exception();

                db.Departments.Add(new DB.Departments {
                    Name = tbxName.Text,
                    Boss = tbxBoss.Text,
                    Phone = tbxPhonr.Text,
                    OfficeDean = tbxOfficeDean.Text,
                    Chiefs = (DB.Chiefs)cbxCheif.SelectedItem,
                    Units = (DB.Units)cbxUnit.SelectedItem,
                    DateStart = dpDateStart.SelectedDate.Value,
                    Cost = decimal.Parse(tbxCost.Text),
                    CostYear = short.Parse(tbxCostYear.Text),
                    CostAfter = decimal.Parse(tbxCostAfter.Text),
                    Period = Convert.ToInt16(DateTime.Now.Year - dpDateStart.SelectedDate.Value.Year)
                });
                db.SaveChanges();

                if (MessageBox.Show("Новая кафедра удачно добавлена! Желаете добавить ещё?", "Perfect", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    new AddNewDepartmentWindow().Show();
                    this.Close();
                }
                else
                    click_Back(null,null);
            }
            catch 
            {
                MessageBox.Show("Введите корректные данные!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
