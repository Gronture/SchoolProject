using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
    /// Логика взаимодействия для EditDirectory.xaml
    /// </summary>
    public partial class EditDirectory : Window
    {
        Справочник_Факультативов _справочник = new Справочник_Факультативов();
        public EditDirectory(Справочник_Факультативов справочник)
        {
            InitializeComponent();
            _справочник = справочник;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var needObject = db.Справочник_Факультативов.Where(x => x.Код_Справочника == _справочник.Код_Справочника).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.Название = Name.Text;
                    needObject.Объём_лекций = int.Parse(LekHour.Text);
                    needObject.Объём_практик = int.Parse(PractHour.Text);
                    needObject.Объём_лабораторных_работ = int.Parse(LabHour.Text);
                    db.SaveChanges();
                }
            }
            new StudentView().Show();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new DirectoryView().Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Text = _справочник.Название;
            LekHour.Text = _справочник.Объём_лекций.ToString();
            PractHour.Text = _справочник.Объём_практик.ToString();
            LabHour.Text = _справочник.Объём_лабораторных_работ.ToString();
        }
    }
}
