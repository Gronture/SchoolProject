using SchoolProject.ClassConnection;
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
    /// Логика взаимодействия для ElectivesView.xaml
    /// </summary>
    public partial class ElectivesView : Window
    {
        public ElectivesView()
        {
            InitializeComponent();
            UpdateView();
        }
        public void UpdateView()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                grid.ItemsSource = GetElectives();
            }
        }
        public List<ElectivesUpd> GetElectives()
        {

            using (ElectivesEntities db = new ElectivesEntities())
            {
                var firstJoin = db.Факультативов_в_семестре.ToList().GroupJoin(
                    db.Преподаватель.ToList(),
                    cl => cl.Код_преподавателя,
                    ci => ci.Код_Преподавателя,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (elective, teacher) => new ElectivesUpd
                    {
                        Код_Факультатива = elective.cl.Код_Факультатива,
                        ФамилияПреподавателя = teacher.Фамилия,
                        Количество_часов = elective.cl.Количество_часов,
                        ЛР = elective.cl.ЛР,
                        Практика = elective.cl.Практика,
                        Номер_семестра = elective.cl.Номер_семестра,

                    }).ToList();


                var secondJoin = db.Факультативов_в_семестре.ToList().GroupJoin(
                    db.Справочник_Факультативов.ToList(),
                    cl => cl.Код_справочника,
                    ci => ci.Код_Справочника,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (elective, directory) => new ElectivesUpd
                    {
                        НазваниеФакультатива = directory.Название,
                        
                    }).ToList();


                for (int i = 0; i < firstJoin.Count; i++)
                    firstJoin[i].НазваниеФакультатива = secondJoin[i].НазваниеФакультатива;

                return firstJoin;
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
            var selectedElement = grid.SelectedItem as ElectivesUpd;
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var findElement = db.Факультативов_в_семестре.Where(x => x.Код_Факультатива == selectedElement.Код_Факультатива).FirstOrDefault();
                db.Факультативов_в_семестре.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddElective().Show();
            this.Close();
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as ElectivesUpd;
            new EditElective(selectedItem).Show();
            this.Close();
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var list = GetElectives();
            grid.ItemsSource = list.Where(x => x.НазваниеФакультатива.ToLower().Contains(SortTextBox.Text)).ToList();
        }
    }
}
