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
    /// Логика взаимодействия для EditStudent.xaml
    /// </summary>
    public partial class EditStudent : Window
    {
        Студенты _студенты = new Студенты();
        public EditStudent(Студенты студенты)
        {
            InitializeComponent();
            _студенты = студенты;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new StudentView().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var needObject = db.Студенты.Where(x => x.Код_студента == _студенты.Код_студента).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.Фамилия = secondName.Text;
                    needObject.Имя = firstName.Text;
                    needObject.Отчество = patronymic.Text;
                    needObject.Адрес = adress.Text;
                    needObject.Телефон = phone.Text;
                    needObject.Зачётная_книжка = int.Parse(zachet.Text);
                    db.SaveChanges();
                }
            }
            new StudentView().Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            secondName.Text = _студенты.Фамилия;
            firstName.Text = _студенты.Имя;
            patronymic.Text = _студенты.Отчество;
            adress.Text = _студенты.Адрес;
            phone.Text = _студенты.Телефон;
            zachet.Text = _студенты.Зачётная_книжка.ToString();
        }
    }
}
