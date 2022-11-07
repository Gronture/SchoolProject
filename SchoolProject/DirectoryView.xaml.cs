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

namespace SchoolProject
{
    /// <summary>
    /// Логика взаимодействия для DirectoryView.xaml
    /// </summary>
    public partial class DirectoryView : Window
    {
        public DirectoryView()
        {
            InitializeComponent();
            UpdateView();
        }
        public void UpdateView()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                grid.ItemsSource = db.Справочник_Факультативов.ToList();
            }
        }

        public List<Справочник_Факультативов> GetDirectory()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var returnList = db.Справочник_Факультативов.ToList();
                return returnList;
            }
        }

        private void BackMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedElement = grid.SelectedItem as Справочник_Факультативов;
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var findElement = db.Справочник_Факультативов.Where(x => x.Код_Справочника == selectedElement.Код_Справочника).FirstOrDefault();
                db.Справочник_Факультативов.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddDirectory().Show();
            this.Close();
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as Справочник_Факультативов;
            new EditDirectory(selectedItem).Show();
            this.Close();
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var list = GetDirectory();
            grid.ItemsSource = list.Where(x => x.Название.ToLower().Contains(SortTextBox.Text)).ToList();
        }
    }
}
