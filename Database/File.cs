//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class File
    {
        public int Id { get; set; }
        public System.DateTime DateTime { get; set; }
        public byte[] Content { get; set; }
        public int SwitchId { get; set; }
    
        public virtual Switch Switch { get; set; }
    }
}
