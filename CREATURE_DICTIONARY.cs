//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KP_OOP
{
    using System;
    using System.Collections.Generic;
    
    public partial class CREATURE_DICTIONARY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CREATURE_DICTIONARY()
        {
            this.TASK_CREATURES = new HashSet<TASK_CREATURES>();
        }
    
        public string CR_NAME { get; set; }
        public int CR_ID { get; set; }
        public Nullable<short> CR_DANGER_LVL { get; set; }
        public string CR_MOVEMENT_TYPE { get; set; }
        public Nullable<int> CR_ARMOUR { get; set; }
        public string CR_IMAGE { get; set; }
        public string CR_DESCRIPTION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASK_CREATURES> TASK_CREATURES { get; set; }
    }
}
