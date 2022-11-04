using SchoolProject.ClassConnection;
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
    /// Логика взаимодействия для EditElective.xaml
    /// </summary>
    public partial class EditElective : Window
    {
        ElectivesUpd _факультатив = new ElectivesUpd();
        public EditElective(ElectivesUpd факультатив)
        {
            InitializeComponent();
            _факультатив = факультатив;
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
                var needObject = db.Факультативов_в_семестре.Where(x => x.Код_Факультатива == _факультатив.Код_Факультатива).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.Код_справочника = db.Справочник_Факультативов.Where(x => x.Название == ElectiveNameBox.Text).FirstOrDefault().Код_Справочника;
                    needObject.Код_преподавателя = db.Преподаватель.Where(x => x.Фамилия == SecondNameBox.Text).FirstOrDefault().Код_Преподавателя;
                    needObject.Номер_семестра = int.Parse(SemestrNumber.Text);
                    needObject.ЛР = int.Parse(LRHour.Text);
                    needObject.Практика = int.Parse(PractHour.Text);
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
                var list2 = db.Преподаватель.ToList();
                foreach (var item in list2)
                    SecondNameBox.Items.Add(item.Фамилия);
            }

            ElectiveNameBox.Text = _факультатив.НазваниеФакультатива;
            SecondNameBox.Text = _факультатив.ФамилияПреподавателя;
            SemestrNumber.Text = _факультатив.Номер_семестра.ToString();
            LRHour.Text = _факультатив.ЛР.ToString();
            PractHour.Text = _факультатив.Практика.ToString();
        }
    }
}
