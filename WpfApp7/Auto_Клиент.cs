//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp7
{
    using System;
    using System.Collections.Generic;
    
    public partial class Auto_Клиент
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auto_Клиент()
        {
            this.Auto_Машина = new HashSet<Auto_Машина>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string number { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public Nullable<int> id_role { get; set; }
    
        public virtual Auto_Роль Auto_Роль { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Auto_Машина> Auto_Машина { get; set; }
    }
}
