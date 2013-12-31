using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Left4Edit.Models
{
    public class CustomerContextInit : DropCreateDatabaseAlways<CustomerContext>
    {
        protected override void Seed(CustomerContext context)
        {
            InitializeTestCustomer(ref context);
            base.Seed(context);
        }

        private static void InitializeTestCustomer(ref CustomerContext context)
        {
            var customer = context.Customers.Create();
            customer.Name = "AGC North America";
            customer.Symbol = "ASA";
            customer.Contacts.Add(
                new Contact() { FirstName = "Bill", LastName = "McGraw", Email = "billmcg90@gmail.com" }
                );
            customer.Contacts.Add(
                new Contact() { FirstName = "Anita", LastName = "Neal", Email = "idk@idk.idk" }
                );
            customer.Credentials.Add(
                new Credential() { Domain = "AURORA_NT", UserName = "wmcgraw", Password = "NO!" }
                );

            var nodeWithChildren = context.Nodes.Create();
            nodeWithChildren.Address = "NodeWChildren.billmcg.com";
            nodeWithChildren.MyCustomer = customer;
            nodeWithChildren.Name = "Node with Children (I hope)";
            nodeWithChildren.Credentials.Add(
                new Credential() { Domain = "FakeNodeDomain", UserName = "awesomeGuy", Password = "fakePW" }
                );
            customer.Nodes.Add(nodeWithChildren);

            customer.Nodes.Add(
                new Node() { Address = "childlessNode.billmcg.com", Name = "Childless node" }
                );

            context.Customers.Add(customer);
            context.SaveChanges();
        }
    }
}