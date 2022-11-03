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
    /// Логика взаимодействия для EditPosition.xaml
    /// </summary>
    public partial class EditPosition : Window
    {
        Должность _должность = new Должность();
        public EditPosition(Должность должность)
        {
            InitializeComponent();
            _должность = должность;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new PositionView().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var needObject = db.Должность.Where(x => x.ID_Должности == _должность.ID_Должности).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.Название = Name.Text;
                    needObject.Оклад = decimal.Parse(Salary.Text);
                    db.SaveChanges();
                }
            }
            new PositionView().Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Text = _должность.Название;
            Salary.Text = _должность.Оклад.ToString();
        }
    }
}