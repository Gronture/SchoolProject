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
    /// Логика взаимодействия для AddDirectory.xaml
    /// </summary>
    public partial class AddDirectory : Window
    {
        public AddDirectory()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            new DirectoryView().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                Справочник_Факультативов directory = new Справочник_Факультативов();
                directory.Название = Name.Text;
                directory.Объём_лекций = int.Parse(LekHour.Text);
                directory.Объём_практик = int.Parse(PractHour.Text);
                directory.Объём_лабораторных_работ = int.Parse(LabHour.Text);
                db.Справочник_Факультативов.Add(directory);
                db.SaveChanges();
            }
            new DirectoryView().Show();
            this.Close();
        }
    }
}
