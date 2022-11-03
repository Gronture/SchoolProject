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
    /// Логика взаимодействия для EducationPlaneView.xaml
    /// </summary>
    public partial class EducationPlaneView : Window
    {
        public EducationPlaneView()
        {
            InitializeComponent();
            UpdateView();
        }
        public void UpdateView()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                grid.ItemsSource = GetPlan();
            }
        }
        public List<EducationPlanUpd> GetPlan()
        {

            using (ElectivesEntities db = new ElectivesEntities())
            {
                var firstJoin = db.Учебный_план.ToList().GroupJoin(
                    db.Студенты.ToList(),
                    cl => cl.Код_студента,
                    ci => ci.Код_студента,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (plan, student) => new EducationPlanUpd
                    {
                        ID_Учебного_плана = plan.cl.ID_Учебного_плана,
                        ФамилияСтудента = student.Фамилия,
                    }).ToList();


                var secondJoin = db.Учебный_план.ToList().GroupJoin(
                    db.Факультативов_в_семестре.ToList(),
                    cl => cl.Код_факультатива,
                    ci => ci.Код_Факультатива,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (plan, elective) => new EducationPlanUpd
                    {
                        НазваниеФакультатива = db.Справочник_Факультативов.Where(x => x.Код_Справочника == elective.Код_справочника).FirstOrDefault().Название,
                        Курс = plan.cl.Курс,
                        Оценка = plan.cl.Оценка,
                        Дата = plan.cl.Дата,

                    }).ToList();


                for (int i = 0; i < firstJoin.Count; i++)
                {
                    firstJoin[i].НазваниеФакультатива = secondJoin[i].НазваниеФакультатива;
                    firstJoin[i].Курс = secondJoin[i].Курс;
                    firstJoin[i].Оценка = secondJoin[i].Оценка;
                    firstJoin[i].Дата = secondJoin[i].Дата;

                }

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
            var selectedElement = grid.SelectedItem as EducationPlanUpd;
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var findElement = db.Учебный_план.Where(x => x.ID_Учебного_плана == selectedElement.ID_Учебного_плана).FirstOrDefault();
                db.Учебный_план.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddEducationPlan().Show();
            this.Close();

        }
    }
}
