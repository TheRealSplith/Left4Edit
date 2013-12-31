using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Left4Edit.Models.Repo
{
    public class MemCustomerRepo : ICustomerRepo
    {
        public IList<Customer> Customers { get; set; }
        public MemCustomerRepo(IEnumerable<Customer> customers = null)
        {
            if (customers == null)
                Customers = new List<Customer>();
            else
                Customers = new List<Customer>(customers);
        }

        public void NewCustomer(Customer newCust)
        {
            Customers.Add(newCust);
        }

        public void UpdateCustomer(Customer updateCust)
        {
            foreach (var cust in Customers)
            {
                if (cust.ID == updateCust.ID)
                {
                    Customers.Remove(cust);
                    Customers.Add(updateCust);
                    break;
                }
            }
            throw new ArgumentException(String.Format("No customer with key {0}", updateCust.ID));
        }

        public Customer GetCustomer(int id)
        {
            return Customers.Where(c => c.ID == id).FirstOrDefault();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return Customers;
        }

        public IEnumerable<Contact> GetContacts()
        {
            return Customers.SelectMany(c => c.Contacts);
        }

        public IEnumerable<Contact> GetCustomerContacts(int customerID)
        {
            return Customers.Where(c => c.ID == customerID).SelectMany(c => c.Contacts);
        }

        public void AddContactToCustomer(Contact contact, int customerID)
        {
            var customer = Customers.Where(c => c.ID == customerID).First();
            contact.MyCustomer = customer;
            contact.CustomerID = customer.ID;

            customer.Contacts.Add(contact);
        }

        public IEnumerable<Node> GetNodes()
        {
            return Customers.SelectMany(c => c.Nodes);
        }

        public IEnumerable<Node> GetCustomerNodes(int customerID)
        {
            return Customers.Where(c => c.ID == customerID).SelectMany(c => c.Nodes);
        }

        public void AddNodeToCustomer(Node node, int customerID)
        {
            var customer = Customers.Where(c => c.ID == customerID).First();
            node.MyCustomer = customer;
            node.CustomerID = customer.ID;

            customer.Nodes.Add(node);
        }

        public IEnumerable<Credential> GetCredentials()
        {
            return Customers.SelectMany(c => c.Credentials)
                .Union(Customers.SelectMany(c => c.Nodes.SelectMany(cust => cust.Credentials)));
        }

        public IEnumerable<Credential> GetCustomerCredentials(int customerID)
        {
            return Customers.Where(c => c.ID == customerID)
                .SelectMany(c => c.Credentials);
        }

        public IEnumerable<Credential> GetNodeCredentials(int nodeID)
        {
            return Customers.SelectMany(c => c.Nodes)
                .Where(n => n.ID == nodeID)
                .SelectMany(node => node.Credentials);
        }

        public void AddCredentialToCustomer(Credential credential, int customerID)
        {
            var customer = Customers.Where(c => c.ID == customerID).First();
            credential.CustomerID = customer.ID;
            credential.MyCustomer = customer;

            customer.Credentials.Add(credential);
        }

        public void AddCredentialToNode(Credential credential, int nodeID)
        {
            var node = Customers.SelectMany(c => c.Nodes)
                .Where(n => n.ID == nodeID)
                .First();

            credential.MyNode = node;
            credential.NodeID = node.ID;

            node.Credentials.Add(credential);
        }

        public Contact GetContact(int contactID)
        {
            return Customers.SelectMany(c => c.Contacts)
                .Where(c => c.ID == contactID)
                .First();
        }

        public Node GetNode(int nodeID)
        {
            return Customers.SelectMany(c => c.Nodes)
                .Where(n => n.ID == nodeID)
                .First();
        }

        public Credential GetCredential(int credentialID)
        {
            return GetCredentials()
                .Where(c => c.ID == credentialID)
                .First();
        }

        public void UpdateContact(Contact contact)
        {
            foreach (var c in Customers.SelectMany(c => c.Contacts))
            {
                if (c.ID == contact.ID)
                {
                    c.MyCustomer.Contacts.Remove(c);
                    c.MyCustomer.Contacts.Add(contact);
                    break;
                }
            }
        }

        public void UpdateNode(Node node)
        {
            foreach (var n in Customers.SelectMany(c => c.Nodes))
            {
                if (n.ID == node.ID)
                {
                    n.MyCustomer.Nodes.Remove(n);
                    n.MyCustomer.Nodes.Add(node);
                    break;
                }
            }
        }

        public void UpdateCredential(Credential credential)
        {
            foreach (var cred in Customers.SelectMany(c => c.Credentials)
                                          .Union(
                                 Customers.SelectMany(c => c.Nodes).SelectMany(n => n.Credentials)))
            {
                if (cred.ID == credential.ID)
                {
                    if (cred.MyCustomer != null)
                    {
                        cred.MyCustomer.Credentials.Remove(cred);
                        cred.MyCustomer.Credentials.Add(credential);
                        break;
                    }
                    if (cred.MyNode != null)
                    {
                        cred.MyNode.Credentials.Remove(cred);
                        cred.MyNode.Credentials.Add(credential);
                    }
                }
            }
        }

        public void SaveChanges()
        {
            var truth = "Bill is awesome";
        }

        public Customer CreateCustomer()
        {
            return new Customer();
        }

        public void AddCustomer(Customer newCust)
        {
            Customers.Add(newCust);
        }

        public void DeleteCustomer(Customer targetCustomer)
        {
            Customers.Remove(targetCustomer);
        }

        public Contact CreateContact()
        {
            return new Contact();
        }

        public void DeleteContact(Contact targetContact)
        {
            foreach (var contact in Customers.SelectMany(c => c.Contacts))
            {
                if (contact.ID == targetContact.ID)
                {
                    contact.MyCustomer.Contacts.Remove(contact);
                    break;
                }
            }
        }

        public Node CreateNode()
        {
            return new Node();
        }

        public void DeleteNode(Node targetNode)
        {
            foreach (var node in  Customers.SelectMany(c => c.Nodes))
            {
                if (node.ID == targetNode.ID)
                {
                    node.MyCustomer.Nodes.Remove(node);
                    break;
                }
            }
        }

        public Credential CreateCredential()
        {
            return new Credential();
        }

        public void DeleteCrednetial(Credential targetCred)
        {
            foreach (var cred in Customers.SelectMany(c => c.Credentials)
                                    .Union(
                                 Customers.SelectMany(c => c.Nodes).SelectMany(n => n.Credentials)))
            {
                if (cred.ID == targetCred.ID)
                {
                    if (cred.MyCustomer != null)
                    {
                        cred.MyCustomer.Credentials.Remove(cred);
                        break;
                    }
                    if (cred.MyNode != null)
                    {
                        cred.MyNode.Credentials.Remove(cred);
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}