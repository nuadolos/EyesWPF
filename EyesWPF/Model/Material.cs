//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EyesWPF.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            this.MaterialCountHistory = new HashSet<MaterialCountHistory>();
            this.ProductMaterial = new HashSet<ProductMaterial>();
            this.Supplier = new HashSet<Supplier>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public int CountInPack { get; set; }
        public string Unit { get; set; }
        public Nullable<double> CountInStock { get; set; }
        public double MinCount { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Image { get; set; }
        public int MaterialTypeID { get; set; }
    
        public virtual MaterialType MaterialType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialCountHistory> MaterialCountHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaterial> ProductMaterial { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Supplier> Supplier { get; set; }
    }
}
