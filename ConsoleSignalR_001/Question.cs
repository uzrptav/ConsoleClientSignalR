//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleSignalR_001
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }
    
        public int id { get; set; }
        public System.DateTime createdOn { get; set; }
        public int createdBy { get; set; }
        public System.DateTime modifiedOn { get; set; }
        public int modifiedBy { get; set; }
        public int orderId { get; set; }
        public string text { get; set; }
        public Nullable<int> answerListId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual AnswerList AnswerList { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
