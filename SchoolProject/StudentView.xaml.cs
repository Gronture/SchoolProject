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

namespace SchoolProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class StudentView : Window
    {
        public StudentView()
        {
            InitializeComponent();
            UpdateView();
        }

        public List<Студенты> GetStudent()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var returnList = db.Студенты.ToList();
                return returnList;
            }
        }
        public void UpdateView()
        {
            using(ElectivesEntities db = new ElectivesEntities())
            {
                grid.ItemsSource = db.Студенты.ToList();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedElement = grid.SelectedItem as Студенты;
                using (ElectivesEntities db = new ElectivesEntities())
                {
                    var findElement = db.Студенты.Where(x => x.Код_студента == selectedElement.Код_студента).FirstOrDefault();
                    db.Студенты.Remove(findElement);
                    db.SaveChanges();
                }
            UpdateView();
        }

        private void BackMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddStudent().Show();
            this.Close();
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as Студенты;
            new EditStudent(selectedItem).Show();
            this.Close();
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var list = GetStudent();
            grid.ItemsSource = list.Where(x => x.Фамилия.ToLower().Contains(SortTextBox.Text)).ToList();
        }
    }
}
