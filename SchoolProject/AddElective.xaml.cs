using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для AddElective.xaml
    /// </summary>
    public partial class AddElective : Window
    {
        public AddElective()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new ElectivesView().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                Факультативов_в_семестре elective = new Факультативов_в_семестре();
                var directory = db.Справочник_Факультативов.Where(x => x.Название == ElectiveNameBox.Text).FirstOrDefault();
                var teacher = db.Преподаватель.Where(x => x.Фамилия == SecondNameBox.Text).FirstOrDefault();

                elective.Код_справочника = directory.Код_Справочника;
                elective.Код_преподавателя = teacher.Код_Преподавателя;
                elective.Номер_семестра = int.Parse(SemestrNumber.Text);
                elective.ЛР = int.Parse(LRHour.Text);
                elective.Практика = int.Parse(PractHour.Text);

                db.Факультативов_в_семестре.Add(elective);
                db.SaveChanges();
            }
            this.Close();
            new ElectivesView().Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var techer = new List<Преподаватель>();
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var tmpList = db.Преподаватель.ToList();
                techer.AddRange(tmpList);
            }
            foreach (var i in techer)
            {
                SecondNameBox.Items.Add(i.Фамилия);
            }
            var directory = new List<Справочник_Факультативов>();
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var tmpList = db.Справочник_Факультативов.ToList();
                directory.AddRange(tmpList);
            }
            foreach (var i in directory)
            {
                ElectiveNameBox.Items.Add(i.Название);
            }
        }
    }
    
}
