//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EleventhDay.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Halls
    {
        public int ID { get; set; }
        public double Square { get; set; }
        public int Windows { get; set; }
        public int Heating { get; set; }
        public string Target { get; set; }
        public int DepartmentID { get; set; }
        public int BuildingID { get; set; }
        public int ChiefID { get; set; }
    
        public virtual Buildings Buildings { get; set; }
        public virtual Chiefs Chiefs { get; set; }
        public virtual Departments Departments { get; set; }
    }
}
