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
                                          .HasForeignKey(con => con.CustomerID);

            // Contacts contain Nodes
            modelBuilder.Entity<Node>().HasOptional(node => node.MyCustomer)
                                       .WithMany(cus => cus.Nodes)
                                       .HasForeignKey(node => node.CustomerID);

            // Contacts contain Credentials
            modelBuilder.Entity<Credential>().HasOptional(cred => cred.MyCustomer)
                                             .WithMany(cus => cus.Credentials)
                                             .HasForeignKey(cred => cred.CustomerID);

            // Nodes contain Credentails
            modelBuilder.Entity<Credential>().HasOptional(cred => cred.MyNode)
                                             .WithMany(node => node.Credentials)
                                             .HasForeignKey(cred => cred.NodeID);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Credential> Credentials { get; set; }
    }
}