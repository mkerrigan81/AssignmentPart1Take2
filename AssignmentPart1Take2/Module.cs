//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AssignmentPart1Take2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Module()
        {
            this.Results = new HashSet<Results>();
        }
    
        public int Id { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public int LecturerId { get; set; }
        public int CourseId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Results> Results { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public virtual Course Course { get; set; }
    }
}