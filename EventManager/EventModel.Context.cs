﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventManager
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class EVENTS_MGR_TESTING_Entities : DbContext
    {
        public EVENTS_MGR_TESTING_Entities()
            : base("name=EVENTS_MGR_TESTING_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
    
        public virtual ObjectResult<EVENTS_LAST_10_DAYS_Result> EVENTS_LAST_10_DAYS()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<EVENTS_LAST_10_DAYS_Result>("EVENTS_LAST_10_DAYS");
        }
    
        public virtual ObjectResult<EVENTS_LAST_6_MONTHS_Result> EVENTS_LAST_6_MONTHS()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<EVENTS_LAST_6_MONTHS_Result>("EVENTS_LAST_6_MONTHS");
        }
    
        public virtual ObjectResult<Event> EVENTS_LAST_10_DAYS1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Event>("EVENTS_LAST_10_DAYS1");
        }
    
        public virtual ObjectResult<Event> EVENTS_LAST_10_DAYS1(MergeOption mergeOption)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Event>("EVENTS_LAST_10_DAYS1", mergeOption);
        }
    
        public virtual ObjectResult<Event> EVENTS_LAST_6_MONTHS1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Event>("EVENTS_LAST_6_MONTHS1");
        }
    
        public virtual ObjectResult<Event> EVENTS_LAST_6_MONTHS1(MergeOption mergeOption)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Event>("EVENTS_LAST_6_MONTHS1", mergeOption);
        }
    
        public virtual ObjectResult<USER_OWNED_EVENTS_Result> USER_OWNED_EVENTS(Nullable<int> user)
        {
            var userParameter = user.HasValue ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USER_OWNED_EVENTS_Result>("USER_OWNED_EVENTS", userParameter);
        }
    
        public virtual ObjectResult<Event> USER_OWNED_EVENTS1(Nullable<int> user)
        {
            var userParameter = user.HasValue ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Event>("USER_OWNED_EVENTS1", userParameter);
        }
    
        public virtual ObjectResult<Event> USER_OWNED_EVENTS1(Nullable<int> user, MergeOption mergeOption)
        {
            var userParameter = user.HasValue ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Event>("USER_OWNED_EVENTS1", mergeOption, userParameter);
        }
    }
}
