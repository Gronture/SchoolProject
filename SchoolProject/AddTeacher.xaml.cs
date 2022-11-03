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
using System.Xml.Linq;

namespace SchoolProject
{
    /// <summary>
    /// Логика взаимодействия для AddTeacher.xaml
    /// </summary>
    public partial class AddTeacher : Window
    {
        public AddTeacher()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new TeacherView().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                Преподаватель teacher = new Преподаватель();
                var positin = db.Должность.Where(x => x.Название == PositionBox.Text).FirstOrDefault();

                teacher.Имя = firstName.Text;
                teacher.Фамилия = secondName.Text;
                teacher.Отчество = patronymic.Text;
                teacher.ID_Должности = positin.ID_Должности;
                teacher.Телефон = phone.Text;
                teacher.Табельный_номер = int.Parse(tabNumber.Text);
                teacher.Пол = Pol.Text;
                teacher.Стаж = int.Parse(staj.Text);

                db.Преподаватель.Add(teacher);
                db.SaveChanges();
            }


            this.Close();
            new TeacherView().Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var position = new List<Должность>();
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var tmpList = db.Должность.ToList();
                position.AddRange(tmpList);
            }
            foreach (var i in position) 
            {
                PositionBox.Items.Add(i.Название);
            }

        }
    }
}
