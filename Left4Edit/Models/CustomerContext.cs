using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Left4Edit.Models
{
    public class CustomerContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Contacts contain Customers
            modelBuilder.Entity<Contact>().HasOptional(con => con.MyCustomer)
                                          .WithMany(cus => cus.Contacts)
                                          .HasForeignKey(con => con.CustomerID)
                                          .WillCascadeOnDelete(true);

            // Contacts contain Nodes
            modelBuilder.Entity<Node>().HasOptional(node => node.MyCustomer)
                                       .WithMany(cus => cus.Nodes)
                                       .HasForeignKey(node => node.CustomerID)
                                       .WillCascadeOnDelete(true);

            // Contacts contain Credentials
            modelBuilder.Entity<Credential>().HasOptional(cred => cred.MyCustomer)
                                             .WithMany(cus => cus.Credentials)
                                             .HasForeignKey(cred => cred.CustomerID)
                                             .WillCascadeOnDelete(false);

            // Nodes contain Credentails
            modelBuilder.Entity<Credential>().HasOptional(cred => cred.MyNode)
                                             .WithMany(node => node.Credentials)
                                             .HasForeignKey(cred => cred.NodeID)
                                             .WillCascadeOnDelete(true);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Credential> Credentials { get; set; }
    }
}