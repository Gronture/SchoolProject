using SchoolProject.ClassConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для EditEducationPlan.xaml
    /// </summary>
    public partial class EditEducationPlan : Window
    {
        EducationPlanUpd _план = new EducationPlanUpd();
        public EditEducationPlan(EducationPlanUpd план)
        {
            InitializeComponent();
            _план = план;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new EducationPlaneView().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var needObject = db.Учебный_план.Where(x => x.ID_Учебного_плана == _план.ID_Учебного_плана).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.Код_факультатива = db.Справочник_Факультативов.Where(x => x.Название == ElectiveNameBox.Text).FirstOrDefault().Код_Справочника;
                    needObject.Код_студента = db.Студенты.Where(x => x.Фамилия == SecondNameBox.Text).FirstOrDefault().Код_студента;
                    needObject.Курс = int.Parse(CourseNumber.Text);
                    needObject.Оценка = int.Parse(Mark.Text);
                    needObject.Дата = (DateTime) DatePick.SelectedDate;
                    db.SaveChanges();
                }
            }
            new ElectivesView().Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var list = db.Справочник_Факультативов.ToList();
                foreach (var item in list)
                    ElectiveNameBox.Items.Add(item.Название);
                var list2 = db.Студенты.ToList();
                foreach (var item in list2)
                    SecondNameBox.Items.Add(item.Фамилия);
            }

            ElectiveNameBox.Text = _план.НазваниеФакультатива;
            SecondNameBox.Text = _план.ФамилияСтудента;
            CourseNumber.Text = _план.Курс.ToString();
            Mark.Text = _план.Оценка.ToString();
            DatePick.SelectedDate = _план.Дата;
        }
    }
}
