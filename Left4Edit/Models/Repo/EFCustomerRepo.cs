using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Left4Edit.Models.Repo
{
    public class EFCustomerRepo : ICustomerRepo
    {
        private CustomerContext CustDB;

        public EFCustomerRepo()
        {
            CustDB = new CustomerContext();
            CustDB.Configuration.LazyLoadingEnabled = false;
        }
        public void NewCustomer(Customer newCust)
        {
            CustDB.Customers.Add(newCust);
        }

        public void UpdateCustomer(Customer updateCust)
        {
            CustDB.Customers.Attach(updateCust);
            CustDB.Entry(updateCust).State = System.Data.EntityState.Modified;
        }

        public Customer GetCustomer(int id)
        {
            var ret = CustDB.Customers
                .Include(c => c.Contacts)
                .Include(c => c.Credentials)
                .Include(c => c.Nodes)
                .Where(c => c.ID == id).FirstOrDefault();
            if (ret == null)
                throw new ArgumentException("Key not found");

            return ret;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return CustDB.Customers;
        }

        public Contact GetContact(int customerID)
        {
            var ret = CustDB.Contacts.Find(customerID);
            if (ret == null)
                 throw new ArgumentException("Key not found");

            return ret;
        }

        public IEnumerable<Contact> GetContacts()
        {
            return CustDB.Contacts.ToList();
        }

        public IEnumerable<Contact> GetCustomerContacts(int customerID)
        {
            var customer = CustDB.Customers.Find(customerID);
            if (customer == null)
                throw new ArgumentException("Key not found");

            return customer.Contacts.ToList();
        }

        public void AddContactToCustomer(Contact contact, int customerID)
        {
            var customer = CustDB.Customers.Find(customerID);
            if (customer == null)
                throw new ArgumentException("Key not found");

            customer.Contacts.Add(contact);
        }

        public Node GetNode(int nodeID)
        {
            var ret = CustDB.Nodes.Find(nodeID);
            if (ret == null)
                throw new ArgumentException("Key not found");

            return ret;
        }

        public IEnumerable<Node> GetNodes()
        {
            return CustDB.Nodes.ToList();
        }

        public IEnumerable<Node> GetCustomerNodes(int customerID)
        {
            var customer = CustDB.Customers.Find(customerID);
            if (customer == null)
                throw new ArgumentException("Key not found");

            return customer.Nodes.ToList();
        }

        public void AddNodeToCustomer(Node node, int customerID)
        {
            var customer = CustDB.Customers.Find(customerID);
            if (customer == null)
                throw new ArgumentException("Key not found");

            customer.Nodes.Add(node);
        }

        public Credential GetCredential(int credentialID)
        {
            var ret = CustDB.Credentials.Find(credentialID);
            if (ret == null)
                throw new ArgumentException("Key not found");

            return ret;
        }

        public IEnumerable<Credential> GetCredentials()
        {
            return CustDB.Credentials.ToList();
        }

        public IEnumerable<Credential> GetCustomerCredentials(int customerID)
        {
            var customer = CustDB.Customers.Find(customerID);
            if (customer == null)
                throw new ArgumentException("Key not found");

            return customer.Credentials.ToList();
        }

        public IEnumerable<Credential> GetNodeCredentials(int nodeID)
        {
            var node = CustDB.Nodes.Find(nodeID);
            if (node == null)
                throw new ArgumentException("Key not found");

            return node.Credentials.ToList();
        }

        public void AddCredentialToCustomer(Credential credential, int customerID)
        {
            var customer = CustDB.Customers.Find(customerID);
            if (customer == null)
                throw new ArgumentException("Key not found");

            customer.Credentials.Add(credential);
        }

        public void AddCredentialToNode(Credential credential, int nodeID)
        {
            var node = CustDB.Nodes.Find(nodeID);
            if (node == null)
                throw new ArgumentException("Key not found");

            node.Credentials.Add(credential);
        }

        public void UpdateContact(Contact contact)
        {
            if (!CustDB.Contacts.Any(c => c.ID == contact.ID))
                throw new ArgumentException("Key not found");
            CustDB.Contacts.Attach(contact);
            CustDB.Entry(contact).State = System.Data.EntityState.Modified;
        }

        public void UpdateNode(Node node)
        {
            if (!CustDB.Nodes.Any(n => n.ID == node.ID))
                throw new ArgumentException("Key not found");

            CustDB.Nodes.Attach(node);
            CustDB.Entry(node).State = System.Data.EntityState.Modified;
        }

        public void UpdateCredential(Credential credential)
        {
            if (!CustDB.Credentials.Any(c => c.ID == credential.ID))
                throw new ArgumentException("Key not found");

            CustDB.Credentials.Attach(credential);
            CustDB.Entry(credential).State = System.Data.EntityState.Modified;
        }

        public void SaveChanges()
        {
            CustDB.SaveChanges();
        }

        public void Dispose()
        {
            CustDB.Dispose();
        }

        public Customer CreateCustomer()
        {
            return CustDB.Customers.Create();
        }

        public void AddCustomer(Customer newCust)
        {
            CustDB.Customers.Add(newCust);
        }

        public void DeleteCustomer(Customer targetCustomer)
        {
            var customer = CustDB.Customers.Find(targetCustomer.ID);
            CustDB.Customers.Remove(customer);
        }

        public Contact CreateContact()
        {
            return CustDB.Contacts.Create();
        }

        public void DeleteContact(Contact targetContact)
        {
            CustDB.Contacts.Remove(targetContact);
        }

        public Node CreateNode()
        {
            return CustDB.Nodes.Create();
        }

        public void DeleteNode(Node targetNode)
        {
            CustDB.Nodes.Remove(targetNode);
        }

        public Credential CreateCredential()
        {
            return CustDB.Credentials.Create();
        }

        public void DeleteCrednetial(Credential targetCred)
        {
            CustDB.Credentials.Remove(targetCred);
        }
    }
}