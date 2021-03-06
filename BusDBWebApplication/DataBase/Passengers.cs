//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusDBWebApplication.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Passengers
    {
        public Passengers()
        {
            this.Tickets = new HashSet<Tickets>();
        }
    
        public int passenger_id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string message { get; set; }
        public string information
        {
            get
            {
                return String.Format("{0} {1} {2}. Паспортные данные: {3}-{4}",surname,name,patronymic,passport_series,passport_number);
            }
            set
            {
                
            }
        }
        [MinLength(4)]
        [MaxLength(4)]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите число")]
        public string passport_series { get; set; }
        [MinLength(6)]
        [MaxLength(6)]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите число")]
        public string passport_number { get; set; }
    
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
