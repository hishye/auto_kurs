﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class user279_dbEntities1 : DbContext
    {
        public user279_dbEntities1()
            : base("name=user279_dbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Auto_ДолжностьСотрудника> Auto_ДолжностьСотрудника { get; set; }
        public virtual DbSet<Auto_Заказ> Auto_Заказ { get; set; }
        public virtual DbSet<Auto_Клиент> Auto_Клиент { get; set; }
        public virtual DbSet<Auto_Материал> Auto_Материал { get; set; }
        public virtual DbSet<Auto_Машина> Auto_Машина { get; set; }
        public virtual DbSet<Auto_Роль> Auto_Роль { get; set; }
        public virtual DbSet<Auto_Сотрудники> Auto_Сотрудники { get; set; }
        public virtual DbSet<Auto_СтатусЗаказа> Auto_СтатусЗаказа { get; set; }
        public virtual DbSet<Auto_Услуги> Auto_Услуги { get; set; }
        public virtual DbSet<Auto_УслугиМатериалы> Auto_УслугиМатериалы { get; set; }
    }
}
