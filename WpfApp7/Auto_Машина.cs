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
    
    public partial class Auto_Машина
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auto_Машина()
        {
            this.Auto_Заказ = new HashSet<Auto_Заказ>();
        }
    
        public int id { get; set; }
        public Nullable<int> id_client { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public Nullable<System.DateTime> date_of_issue { get; set; }
        public string VIN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Auto_Заказ> Auto_Заказ { get; set; }
        public virtual Auto_Клиент Auto_Клиент { get; set; }
    }
}
