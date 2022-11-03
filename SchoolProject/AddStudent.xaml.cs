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
    /// Логика взаимодействия для AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        public AddStudent()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new StudentView().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using(ElectivesEntities db = new ElectivesEntities())
            {
                Студенты student = new Студенты();
                student.Фамилия = secondName.Text;
                student.Имя = firstName.Text;
                student.Отчество = patronymic.Text;
                student.Адрес = adress.Text;
                student.Телефон = phone.Text;
                student.Зачётная_книжка = int.Parse(zachet.Text);
                db.Студенты.Add(student);
                db.SaveChanges();
            }
            new StudentView().Show();
            this.Close();
        }
    }
}
