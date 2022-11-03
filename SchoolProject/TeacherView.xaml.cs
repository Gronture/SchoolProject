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
    /// Логика взаимодействия для TeacherView.xaml
    /// </summary>
    public partial class TeacherView : Window
    {
        public TeacherView()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            using(ElectivesEntities db = new ElectivesEntities())
            {
                grid.ItemsSource = GetTeacher();
            }
        }
        public List<TeacherUpd> GetTeacher()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var result = db.Преподаватель.ToList().GroupJoin(
                    db.Должность.ToList(),
                    cl => cl.ID_Должности,
                    ci => ci.ID_Должности,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (teacher, speciality) => new TeacherUpd
                    {
                        Код_Преподавателя = teacher.cl.Код_Преподавателя,
                        Фамилия = teacher.cl.Фамилия,
                        Имя = teacher.cl.Имя,
                        Отчество = teacher.cl.Отчество,
                        Должность = speciality?.Название,
                        Телефон = teacher.cl.Телефон,
                        Табельный_номер = teacher.cl.Табельный_номер,
                        Пол = teacher.cl.Пол,
                        Стаж = teacher.cl.Стаж,
                    }).ToList();
                
                return result;
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
            var selectElement = grid.SelectedItem as TeacherUpd;
            using(ElectivesEntities db = new ElectivesEntities())
            {
                var findElement = db.Преподаватель.Where(x => x.Код_Преподавателя == selectElement.Код_Преподавателя).FirstOrDefault();
                db.Преподаватель.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddTeacher().Show();
            this.Close();
        }
    }
}
