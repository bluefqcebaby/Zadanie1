//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zadanie1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product_Sale
    {
        public int ID { get; set; }
        public int ID_product { get; set; }
        public int ID_agents { get; set; }
        public System.DateTime date { get; set; }
        public int count { get; set; }
    
        public virtual Agents Agents { get; set; }
        public virtual Products Products { get; set; }
    }
}