﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FarfetchDataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WebApiDbEntities : DbContext
    {
        public WebApiDbEntities()
            : base("name=WebApiDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Tokens> Tokens { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<FeatureToggle> FeatureToggle { get; set; }
        public virtual DbSet<ServiceFeatureToggle> ServiceFeatureToggle { get; set; }
    }
}