//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class Преподаватель
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Преподаватель()
        {
            this.Факультативов_в_семестре = new HashSet<Факультативов_в_семестре>();
        }
    
        public int Код_Преподавателя { get; set; }
        public string Телефон { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public int Стаж { get; set; }
        public string Пол { get; set; }
        public int Табельный_номер { get; set; }
        public int ID_Должности { get; set; }
    
        protected virtual Должность Должность { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        protected virtual ICollection<Факультативов_в_семестре> Факультативов_в_семестре { get; set; }
    }
}
