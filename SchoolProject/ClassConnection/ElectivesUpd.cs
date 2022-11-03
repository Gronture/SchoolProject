using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.ClassConnection
{
    public class ElectivesUpd
    {
        public int Код_Факультатива { get; set; }
        public string ФамилияПреподавателя { get; set; }
        public string НазваниеФакультатива { get; set; }
        public Nullable<int> Количество_часов { get; set; }
        public int ЛР { get; set; }
        public int Практика { get; set; }
        public int Номер_семестра { get; set; }
    }
}
