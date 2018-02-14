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
    /// Логика взаимодействия для AddNewChiefWindow.xaml
    /// </summary>
    public partial class AddNewChiefWindow : Window
    {
        DB.Entity db = new DB.Entity();

        public AddNewChiefWindow()
        {
            InitializeComponent();
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
                if ((tbxName.Text.Length == 0) || (tbxAddress.Text.Length == 0))
                    throw new Exception();

                db.Chiefs.Add(new DB.Chiefs {
                    Name = tbxName.Text,
                    Address = tbxAddress.Text,
                    Experience = Int16.Parse(tbxExperience.Text)
                });
                db.SaveChanges();

                if (MessageBox.Show("Новый материально ответственный был успаешно добавлен! Желаете добавить ещё?", "Perfect", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    new AddNewChiefWindow().Show();
                    this.Close();
                }
                else
                    click_Back(null,null);
            }
            catch 
            {
                MessageBox.Show("Введите корректные данные!","Error",MessageBoxButton.OK,  MessageBoxImage.Error);
            }
        }
    }
}
