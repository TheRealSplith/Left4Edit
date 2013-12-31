using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Left4Edit.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Left4Edit.Tests.Models
{
    [TestClass]
    public class CustomerRepoTest
    {
        public static IEnumerable<Customer> CreateTestCustomer_Mem()
        {
            // Preparation
            var customer = new Customer();
            var contact1 = new Contact();
            var contact2 = new Contact();
            var node1 = new Node();
            var node2 = new Node();
            var node3 = new Node();
            var credential1 = new Credential();
            var credential2 = new Credential();
            var credential3 = new Credential();

            // Build customer
            customer.Name = "Test Customer";
            customer.Symbol = "ASA";

            // Build contact1
            contact1.FirstName = "Anita";
            contact1.LastName = "Neal";
            contact1.Email = "contact1@billmcg.com";
            contact1.MyCustomer = customer;

            // Build contact2
            contact2.FirstName = "Monster";
            contact2.LastName = "Energy";
            contact2.Email = "contact2@billmcg.com";
            contact2.MyCustomer = customer;

            // build node1
            node1.Address = "node1.billmcg.com";
            node1.Name = "Node1 is best node!";
            node1.ConnectionComment = "No comment here";
            node1.MyCustomer = customer;

            // build node2
            node2.Address = "node2.billmcg.com";
            node2.Name = "Node2 FTW!";
            node2.ConnectionComment = "Worst Connection US";
            node2.MyCustomer = customer;

            // build node3
            node2.Address = "node3.billmcg.com";
            node2.Name = "Test Server";
            node3.ConnectionComment = "Use RDP fool";
            node3.MyCustomer = customer;

            // build credential1
            credential1.Domain = "SPLITH";
            credential1.UserName = "Bill";
            credential1.Password = "TestPassword1";

            // build credential2
            credential2.Domain = "AURORA_NT";
            credential2.UserName = "wmcgraw";
            credential2.Password = "TestPassword2";

            // build credential3
            credential3.Domain = "AURORA_NT";
            credential3.UserName = "someoneElse";
            credential3.Password = "TestPassword3";

            // assembly
            customer.Contacts.Add(contact1);
            customer.Contacts.Add(contact2);

            customer.Credentials.Add(credential2);
            customer.Credentials.Add(credential3);

            customer.Nodes.Add(node1);
            customer.Nodes.Add(node2);
            customer.Nodes.Add(node3);

            node1.Credentials.Add(credential1);

            return new List<Customer>(new Customer[] { customer });
        }
        public static IEnumerable<Customer> CreateTestCustomer_EF()
        {
            // Preparation
            using (var cc = new CustomerContext())
            {
                var customer = cc.Customers.Create();
                var contact1 = cc.Contacts.Create();
                var contact2 = cc.Contacts.Create();
                var node1 = cc.Nodes.Create();
                var node2 = cc.Nodes.Create();
                var node3 = cc.Nodes.Create();
                var credential1 = cc.Credentials.Create();
                var credential2 = cc.Credentials.Create();
                var credential3 = cc.Credentials.Create();

                // Build customer
                customer.Name = "Test Customer";
                customer.Symbol = "ASA";

                // Build contact1
                contact1.FirstName = "Anita";
                contact1.LastName = "Neal";
                contact1.Email = "contact1@billmcg.com";

                // Build contact2
                contact2.FirstName = "Monster";
                contact2.LastName = "Energy";
                contact2.Email = "contact2@billmcg.com";

                // build node1
                node1.Address = "node1.billmcg.com";
                node1.Name = "Node1 is best node!";
                node1.ConnectionComment = "No comment here";

                // build node2
                node2.Address = "node2.billmcg.com";
                node2.Name = "Node2 FTW!";
                node2.ConnectionComment = "Worst Connection US";

                // build node3
                node2.Address = "node3.billmcg.com";
                node2.Name = "Test Server";
                node3.ConnectionComment = "Use RDP fool";

                // build credential1
                credential1.Domain = "SPLITH";
                credential1.UserName = "Bill";
                credential1.Password = "TestPassword1";

                // build credential2
                credential2.Domain = "AURORA_NT";
                credential2.UserName = "wmcgraw";
                credential2.Password = "TestPassword2";

                // build credential3
                credential3.Domain = "AURORA_NT";
                credential3.UserName = "someoneElse";
                credential3.Password = "TestPassword3";

                // assembly
                customer.Contacts.Add(contact1);
                customer.Contacts.Add(contact2);

                customer.Credentials.Add(credential2);
                customer.Credentials.Add(credential3);

                customer.Nodes.Add(node1);
                customer.Nodes.Add(node2);
                customer.Nodes.Add(node3);

                node1.Credentials.Add(credential1);

                return new List<Customer>(new Customer[] {customer});
            }
        }

        [TestMethod]
        public void EF_Include()
        {
            // preperation
            var EFcustomer = CreateTestCustomer_EF().First();

            using (var cc = new CustomerContext())
            {
                cc.Database.Delete();
                cc.Database.Create();

                cc.Customers.Add(EFcustomer);
                cc.SaveChanges();

                EFcustomer = cc.Customers
                    .Include(c => c.Contacts)
                    .Include(c => c.Credentials)
                    .Include(c => c.Nodes).FirstOrDefault();
            }

            // Make sure Include properties work
            Assert.IsNotNull(EFcustomer.Contacts);
            Assert.IsNotNull(EFcustomer.Credentials);
            Assert.IsNotNull(EFcustomer.Nodes);
        }

        [TestMethod]
        public void Mem_Customer_Create()
        {
            var repo = new Left4Edit.Models.Repo.MemCustomerRepo();
            var customer = CreateTestCustomer_Mem().ElementAt(0);
            customer.ID = 0;

            repo.NewCustomer(customer);
            repo.SaveChanges();

            var retCust = repo.GetCustomer(0);
            Assert.IsNotNull(retCust);
            Assert.IsNotNull(retCust.Credentials);
            Assert.IsNotNull(retCust.Contacts);
            Assert.IsNotNull(retCust.Nodes);
        }

        [TestMethod]
        public void EF_Customer_GetCustomer()
        {
            using (var cc = new CustomerContext())
            {
                cc.Database.Delete();
                cc.Database.Create();
            }
            Left4Edit.Models.Repo.ICustomerRepo repo = new Left4Edit.Models.Repo.EFCustomerRepo();
            repo.AddCustomer(CreateTestCustomer_EF().First());
            repo.SaveChanges();

            var cust = repo.GetCustomer(repo.GetCustomers().First().ID);
            Assert.IsNotNull(cust);
            Assert.IsNotNull(cust.Credentials);
            Assert.IsNotNull(cust.Contacts);
            Assert.IsNotNull(cust.Nodes);
            Assert.IsNotNull(
                cust.Nodes.Where(n => n.Address == "node1.billmcg.com")
                    .First().Credentials
            );
        }

        [TestMethod]
        public void EF_Customer_AddItems()
        {
            using (var cc = new CustomerContext())
            {
                cc.Database.Delete();
                cc.Database.Create();
            }
            var repo = new Left4Edit.Models.Repo.EFCustomerRepo();
            var customer = CreateTestCustomer_EF().ElementAt(0);
            repo.AddCustomer(customer);
            repo.SaveChanges();

            var newCredential = repo.CreateCredential();
            newCredential.Domain = "+DOMAIN";
            newCredential.UserName = "Splith";
            newCredential.Password = "This should be on cust and node";
            repo.AddCredentialToCustomer(newCredential, customer.ID);
            repo.SaveChanges();

            var newContact = repo.CreateContact();
            newContact.FirstName = "Added";
            newContact.LastName = "Contact";
            newContact.Email = "ac@billmcg.com";
            repo.AddContactToCustomer(newContact, customer.ID);
            //repo.SaveChanges();

            var newNode = repo.CreateNode();
            newNode.Name = "Added node";
            newNode.Address = "added.billmcg.com";
            repo.AddNodeToCustomer(newNode, customer.ID);
            repo.AddCredentialToNode(newCredential, newNode.ID);
            repo.SaveChanges();

            // Assert tiem
            var node = repo.GetNodes().Where(n => n.Name == "Added node").First();
            Assert.IsTrue(node.Credentials.Any(c => c.Domain == "+DOMAIN"));

            var contact = repo.GetCustomerContacts(customer.ID)
                                .Where(c => c.Email == "ac@billmcg.com")
                                .First();
            Assert.IsTrue(contact.LastName == "Contact");

            var sharedCredential = customer.Credentials.Where(c => c.Domain == "+DOMAIN").First();
            Assert.IsTrue(
                sharedCredential.ID == 
                    node.Credentials.Where(
                        c => c.Domain == "+DOMAIN"
                    ).First().ID
            );
        }

        [TestMethod]
        public void EF_Customer_Poco_Add()
        {
            using (var cc = new CustomerContext())
            {
                cc.Database.Delete();
                cc.Database.Create();
            }
            Left4Edit.Models.Repo.ICustomerRepo repo =
                new Left4Edit.Models.Repo.EFCustomerRepo();

            Customer pocoCustomer = new Customer()
            {
                Name = "Poco Inc",
                Symbol = "POI",
                Contacts = new List<Contact>() 
                { 
                    new Contact()
                    {
                        FirstName = "No way",
                        LastName = "This works!",
                        Email = "poco@billmcg.com"
                    }
                }
            };

            repo.AddCustomer(pocoCustomer);
            repo.SaveChanges();

            Assert.IsTrue(repo.GetCustomers().First().Name == "Poco Inc");
            Assert.IsTrue(repo.GetCustomers().First().Contacts.First().FirstName == "No way");

        }

        [TestMethod]
        public void UpdateCustomer()
        {
            using (var cc = new CustomerContext())
            {
                cc.Database.Delete();
                cc.Database.Create();
            }

            var repo = new Left4Edit.Models.Repo.EFCustomerRepo();
            repo.AddCustomer(CreateTestCustomer_EF().First());
            repo.SaveChanges();
            var customer = repo.GetCustomers().First();

            Int32 customerKey = customer.ID;
            String customerName = customer.Name;
            String newCustomerName = "APPLE SAUCE!";

            customer.Name = newCustomerName;
            repo.UpdateCustomer(customer);
            repo.SaveChanges();

            customer = repo.GetCustomer(customerKey);
            Assert.IsTrue(customer.Name == newCustomerName);
        }

        [TestMethod]
        public void DeleteCustomer_FromPOCO()
        {
            using (var cc = new CustomerContext())
            {
                cc.Database.Delete();
                cc.Database.Create();
            }

            var repo = new Left4Edit.Models.Repo.EFCustomerRepo();
            repo.AddCustomer(CreateTestCustomer_EF().First());
            repo.SaveChanges();

            var existingCustomer = repo.GetCustomers().First();
            Customer customerToDelete = new Customer()
            {
                ID = existingCustomer.ID,
                Name = "Test Customer",
                Symbol = "ASA"
            };

            repo.DeleteCustomer(customerToDelete);
            repo.SaveChanges();

            Assert.IsTrue(repo.GetCustomers().Count() == 0);
        }

        [TestMethod]
        public void UpdateCustomer_FromPOCO()
        {
            using (var cc = new CustomerContext())
            {
                cc.Database.Delete();
                cc.Database.Create();
            }

            var repo = new Left4Edit.Models.Repo.EFCustomerRepo();
            repo.AddCustomer(CreateTestCustomer_Mem().First());
            repo.SaveChanges();

            // Make sure the POCO customer was accepted
            Assert.IsTrue(repo.GetCustomers().Count() == 1 &&
                repo.GetCustomers().First().Symbol == "ASA");

            var customer = repo.GetCustomers().First();
            customer.Name = "Made some Changes!";
            repo.AddCredentialToCustomer(
                new Credential()
                {
                    Domain = "Update Test",
                    UserName = "UpdateCustomer_FromPOCO",
                    Password = "Not doing this"
                },
                customer.ID
            );
            repo.UpdateCustomer(customer);
            repo.SaveChanges();

            Assert.IsTrue(
                repo.GetCustomers().First()
                .Credentials.Where(c => c.Domain == "Update Test")
                .Count() == 1
            );
            Assert.IsTrue(
                repo.GetCustomers().Where(c => c.Name == "Made some Changes!").Count() == 1
            );
        }

        [TestMethod]
        public void Mem_Customer_AddItems()
        {
            var repo = new Left4Edit.Models.Repo.MemCustomerRepo();
            var customer = CreateTestCustomer_Mem().ElementAt(0);
            customer.ID = 0;

            repo.NewCustomer(customer);

            var contact = new Contact()
            {
                FirstName = "Test",
                LastName = "User",
                Email = "billmcg90@gmail.com",
                MyCustomer = customer,
                ID = 100
            };

            repo.AddContactToCustomer(contact, customer.ID);

            Assert.IsTrue(
                repo.GetCustomer(0)
                    .Contacts
                    .Where(c => c.ID == 100)
                    .First().FirstName == "Test"
            );
        }
    }
}
