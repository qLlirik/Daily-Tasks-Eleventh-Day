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
    /// Логика взаимодействия для AddNewBuildingWindow.xaml
    /// </summary>
    public partial class AddNewBuildingWindow : Window
    {
        DB.Entity db = HelpClasses.StaticClass.InicializateDateBase;

        public AddNewBuildingWindow()
        {
            InitializeComponent();
        }

        private void click_Back(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void click_SelectImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "All Images|*.png;*.bmp;*.jpg;*.jpeg";
            if (ofd.ShowDialog() == true)
                tbxPath.Text = ofd.FileName;
        }

        private byte[] ImageToByte(string uri)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(uri,UriKind.Relative))));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            encoder.Save(ms);
            return ms.ToArray();
        }

        private void click_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((tbxName.Text.Length == 0) || (tbxAddress.Text.Length == 0) || (tbxMaterial.Text.Length == 0))
                    throw new Exception();

                db.Buildings.Add(new DB.Buildings {
                    Name = tbxName.Text,
                    Land = double.Parse(tbxLand.Text),
                    Address = tbxAddress.Text,
                    Year = short.Parse(tbxYear.Text),
                    Material = tbxMaterial.Text,
                    Wear = byte.Parse(tbxWear.Text),
                    Flow = byte.Parse(tbxFlow.Text),
                    Picture = ImageToByte(tbxPath.Text),
                    Comment = tbxComment.Text.Length == 0 ? null : tbxComment.Text
                });
                db.SaveChanges();

                if (MessageBox.Show("Новое здание успешно добавлено! Желаете добавить ещё?", "Perfect", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    new AddNewBuildingWindow().Show();
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
