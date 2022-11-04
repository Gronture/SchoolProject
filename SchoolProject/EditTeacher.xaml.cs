using SchoolProject.ClassConnection;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
    /// Логика взаимодействия для EditTeacher.xaml
    /// </summary>
    public partial class EditTeacher : Window
    {
        TeacherUpd _преподаватель = new TeacherUpd();
        public EditTeacher(TeacherUpd преподаватель)
        {
            InitializeComponent();
            _преподаватель = преподаватель;
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
                var needObject = db.Преподаватель.Where(x => x.Код_Преподавателя == _преподаватель.Код_Преподавателя).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.Фамилия = secondName.Text;
                    needObject.Имя = firstName.Text;
                    needObject.Отчество = patronymic.Text;
                    needObject.ID_Должности = db.Должность.Where(x => x.Название == PositionBox.Text).FirstOrDefault().ID_Должности;
                    needObject.Телефон = phone.Text;
                    needObject.Табельный_номер = int.Parse(tabNumber.Text);
                    needObject.Пол = Pol.Text;
                    needObject.Стаж = int.Parse(staj.Text);
                    db.SaveChanges();
                }
            }
            new TeacherView().Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            using(ElectivesEntities db = new ElectivesEntities())
            {
                var list = db.Должность.ToList();
                foreach (var item in list)
                    PositionBox.Items.Add(item.Название);
            }


            secondName.Text = _преподаватель.Фамилия;
            firstName.Text = _преподаватель.Имя;
            patronymic.Text = _преподаватель.Отчество;
            PositionBox.Text = _преподаватель.Должность;
            phone.Text = _преподаватель.Телефон;
            tabNumber.Text = _преподаватель.Табельный_номер.ToString();
            Pol.Text = _преподаватель.Пол;
            staj.Text = _преподаватель.Стаж.ToString();
        }
    }
}
