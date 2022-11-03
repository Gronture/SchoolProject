using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.ClassConnection
{
    public class EducationPlanUpd
    {
        public int ID_Учебного_плана { get; set; }
        public string ФамилияСтудента { get; set; }
        public string НазваниеФакультатива { get; set; }
        public int Курс { get; set; }
        public int Оценка { get; set; }
        public System.DateTime Дата { get; set; }
    }
}
