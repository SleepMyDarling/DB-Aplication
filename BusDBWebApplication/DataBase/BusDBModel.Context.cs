﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Bus_StationEntities : DbContext
    {
        public Bus_StationEntities()
            : base("name=Bus_StationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Buses> Buses { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Passengers> Passengers { get; set; }
        public virtual DbSet<Routes> Routes { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }
    }
}
