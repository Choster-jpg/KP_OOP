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
    using System.Windows.Media;

    public partial class TASK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TASK()
        {
            this.TASK_CREATURES = new HashSet<TASK_CREATURES>();
            this.TASK_MEMBER = new HashSet<TASK_MEMBER>();
        }
    
        public Color TASK_COLOR { get; set; }
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public string TASK_START { get; set; }
        public string TASK_END { get; set; }
        public Nullable<decimal> TASK_PROFIT { get; set; }
        public Nullable<int> TASK_GUILD_ID { get; set; }
        public string TASK_DESCRIPTION { get; set; }
        public bool IS_TASK_RUNNING { get; set; }
    
        public virtual GUILD GUILD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASK_CREATURES> TASK_CREATURES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASK_MEMBER> TASK_MEMBER { get; set; }
    }
}