using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
    /// Логика взаимодействия для AddEducationPlan.xaml
    /// </summary>
    public partial class AddEducationPlan : Window
    {
        public AddEducationPlan()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                Учебный_план plan = new Учебный_план();
                var directory = db.Справочник_Факультативов.Where(x => x.Название == ElectiveNameBox.Text).FirstOrDefault();
                var student = db.Студенты.Where(x => x.Фамилия == SecondNameBox.Text).FirstOrDefault();
                plan.Код_факультатива = directory.Код_Справочника;
                plan.Код_студента = student.Код_студента;
                plan.Курс = int.Parse(CourseNumber.Text);
                plan.Оценка = int.Parse(Mark.Text);
                plan.Дата = (DateTime)DatePick.SelectedDate;
                

                db.Учебный_план.Add(plan);
                db.SaveChanges();
            }
            new EducationPlaneView().Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var list = db.Студенты.ToList();
                foreach(var item in list)
                    SecondNameBox.Items.Add(item.Фамилия);

                var secondList = db.Справочник_Факультативов.ToList();
                foreach (var item in secondList)
                    ElectiveNameBox.Items.Add(item.Название);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new EducationPlaneView().Show();
            this.Close();
        }
    }
}
