using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.Text;

namespace EleventhDay.Windows
{
    /// <summary>
    /// Логика взаимодействия для BuildingsHallsWindow.xaml
    /// </summary>
    public partial class BuildingsHallsWindow : Window
    {
        DB.Entity db = HelpClasses.StaticClass.InicializateDateBase;

        public BuildingsHallsWindow()
        {
            InitializeComponent();

            click_Sort(null,null);

            cbxSort.ItemsSource = new List<string> { "Площадь","Кол-о окон", "Число элементов в батареях", "Назначение", "Кафедра", "Материально ответсттвенный" };
            cbxSort.SelectedIndex = 0;
        }

        private void click_Back(object sender, RoutedEventArgs e)
        {
            new SelectBuildingWindow().Show();
            this.Close();
        }

        private void click_Sort(object sender, RoutedEventArgs e)
        {
            var qwery = db.Halls.Where(w => w.BuildingID == HelpClasses.StaticClass.SelectBuilding.Kadastr);
            switch(cbxSort.SelectedIndex)
            {
                case 0: { qwery = qwery.OrderBy(w=>w.Square); break; }
                case 1: { qwery = qwery.OrderBy(w=>w.Windows); break; }
                case 2: { qwery = qwery.OrderBy(w=>w.Heating); break; }
                case 3: { qwery = qwery.OrderBy(w=>w.Target); break; }
                case 4: { qwery = qwery.OrderBy(w=>w.Departments.Name); break; }
                case 5: { qwery = qwery.OrderBy(w=>w.Chiefs.Name); break; }
            }
            lv.ItemsSource = qwery.ToList();
        }

        private void click_DeleteHall(object sender, RoutedEventArgs e)
        {
            db.Halls.Remove((DB.Halls)((System.Windows.Controls.Button)sender).DataContext);
            db.SaveChanges();
            click_Sort(null,null);
            System.Windows.MessageBox.Show("Удаление помещения прошло успешно!","Perfect",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void click_ImpExs(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            cbxFormat.ItemsSource = new List<string> { "CSV", "XML", "XLS", "XLSX"};
            cbxFormat.SelectedIndex = 0;
            tbxImpExs.Text = btn.Content.ToString() + " в: ";
            btnImpExs.Content = btn.Content;
            btnImpExs.Tag = btn.Tag;
            pp.IsOpen = true;
        }

        private void click_PopupClose(object sender, RoutedEventArgs e)
        {
            pp.IsOpen = false;
        }

        private void click_Move(object sender, RoutedEventArgs e)
        {
            pp.IsOpen = false;

            List<DB.Halls> Halls = (List<DB.Halls>)lv.ItemsSource;

            if (((System.Windows.Controls.Button)sender).Tag + "" == "0")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                string Path = "";
                fbd.Description = "Выберите папку для записи файла.";
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    Path = fbd.SelectedPath;
                else
                {
                    pp.IsOpen = true;
                    return;
                }

                switch (cbxFormat.SelectedIndex)
                {
                    case 0:
                        {
                            List<string> strList = new List<string>();
                            foreach(var i in Halls)
                            {
                                strList.Add(i.ID + "" + ';' + i.Square + "" + ';' + i.Windows + ';' + i.Heating + ';' + i.Target + ';' + i.DepartmentID + ';' + i.BuildingID + ';' + i.ChiefID);
                            }
                            File.WriteAllLines(Path + @"\Exsport.csv", strList);
                            break;
                        }
                    case 1:
                        {
                            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(Path + @"\Exsport.xml");
                            writer.WriteStartDocument();
                            writer.WriteStartElement("Halls");
                            foreach(var i in Halls)
                            {
                                writer.WriteStartElement("Hall");
                                writer.WriteElementString("ID",i.ID + "");
                                writer.WriteElementString("Square",i.Square + "");
                                writer.WriteElementString("Windows",i.Windows + "");
                                writer.WriteElementString("Heating",i.Heating + "");
                                writer.WriteElementString("Target",i.Target);
                                writer.WriteElementString("Department",i.DepartmentID + "");
                                writer.WriteElementString("Building",i.BuildingID + "");
                                writer.WriteElementString("Chief",i.ChiefID + "");
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                            writer.Close();
                            break;
                        }
                    case 2:
                        {
                            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Extended properties=Excel 8.0; User ID=;Password=;Data Source = " + Path + @"\Exsport.xls";
                            OleDbConnection conn = new OleDbConnection(connectionString);

                            conn.Open();
                            OleDbCommand cmd = new OleDbCommand();
                            cmd.Connection = conn;

                            cmd.CommandText = "CREATE TABLE [Halls] (ID VARCHAR, Square VARCHAR, Windows VARCHAR, Heating VARCHAR, Target VARCHAR, DepartmentID VARCHAR, BuildingID VARCHAR, ChiefID VARCHAR);";
                            cmd.ExecuteNonQuery();

                            foreach(var i in Halls)
                            {
                                cmd.CommandText = "INSERT INTO [Halls] (ID,Square,Windows,Heating,Target,DepartmentID,BuildingID,ChiefID) values ('" + i.ID + "', '" + i.Square + "', '" + i.Windows + "', '" + i.Heating + "', '" + i.Target + "', '" + i.DepartmentID + "', '" + i.BuildingID + "', '" + i.ChiefID + "';";
                                cmd.ExecuteNonQuery(); 
                            }
                            conn.Close();
                            break;
                        }
                    case 3:
                        {
                            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Extended properties=Excel 8.0; User ID=;Password=;Data Source = " + Path + @"\Exsport.xlsx";
                            OleDbConnection conn = new OleDbConnection(connectionString);

                            conn.Open();
                            OleDbCommand cmd = new OleDbCommand();
                            cmd.Connection = conn;

                            cmd.CommandText = "CREATE TABLE [Halls] (ID VARCHAR, Square VARCHAR, Windows VARCHAR, Heating VARCHAR, Target VARCHAR, DepartmentID VARCHAR, BuildingID VARCHAR, ChiefID VARCHAR);";
                            cmd.ExecuteNonQuery();

                            foreach (var i in Halls)
                            {
                                cmd.CommandText = "INSERT INTO [Halls] (ID,Square,Windows,Heating,Target,DepartmentID,BuildingID,ChiefID) values ('" + i.ID + "', '" + i.Square + "', '" + i.Windows + "', '" + i.Heating + "', '" + i.Target + "', '" + i.DepartmentID + "', '" + i.BuildingID + "', '" + i.ChiefID + "';";
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();
                            break;
                        }
                }

                System.Windows.MessageBox.Show("Экспорт совершён!","Perfect",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                
                switch(cbxFormat.SelectedIndex)
                {
                    case 0:
                        {
                            if (System.Windows.MessageBox.Show("Для удачной записи файл должен быть следующего формата: |Square|Windows|Heating|Target|DepartmentID|BuildingID|ChiefID|. Продолжить? ","Caution",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.No)
                            {
                                pp.IsOpen = true;
                                return;
                            }
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Filter = "CSV|*.csv;";
                            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            {
                                pp.IsOpen = true;
                                return;
                            }

                            var csv = File.ReadAllLines(ofd.FileName);

                            int GoodCount = 0;
                            int BadCount = 0;
                            try
                            {
                                foreach(var i in csv)
                                {
                                    var str = i.Split(';');
                                    db.Halls.Add(new DB.Halls {
                                        Square = double.Parse(str[0]),
                                        Windows = int.Parse(str[1]),
                                        Heating = int.Parse(str[2]),
                                        Target = str[3],
                                        DepartmentID = int.Parse(str[4]),
                                        BuildingID = int.Parse(str[5]),
                                        ChiefID = int.Parse(str[6])
                                    });
                                }
                                db.SaveChanges();
                                GoodCount++;
                            }
                            catch 
                            {
                                BadCount++;
                            }

                            string Message = "";
                            if (BadCount == 0)
                                Message = "Импорт прошёл успешно!";
                            else if (GoodCount != 0)
                                Message = "Импорт прошёл с некоторыми ошибками!";
                            else
                                Message = "Импорт не прошёл из-за появившихся ошибок!";
                            System.Windows.MessageBox.Show(Message, "Message",MessageBoxButton.OK,MessageBoxImage.Information);
                            break;
                        }
                    case 1:
                        {

                            break;
                        }
                }
                click_Sort(null,null);
            }

        }
        private string GetConnectionString()
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = "C:\\MyExcel.xlsx";

            // XLS - Excel 2003 and Older
            //props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            //props["Extended Properties"] = "Excel 8.0";
            //props["Data Source"] = "C:\\MyExcel.xls";

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }
    }
}
