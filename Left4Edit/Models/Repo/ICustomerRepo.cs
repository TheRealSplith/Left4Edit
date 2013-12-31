using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Left4Edit.Models.Repo
{
    public interface ICustomerRepo : IDisposable
    {
        // Customer
        Customer CreateCustomer();
        void AddCustomer(Customer newCust);
        void UpdateCustomer(Customer updateCust);
        Customer GetCustomer(Int32 id);
        void DeleteCustomer(Customer targetCustomer);
        IEnumerable<Customer> GetCustomers();

        // Contact
        Contact CreateContact();
        Contact GetContact(Int32 contactID);
        IEnumerable<Contact> GetContacts();
        IEnumerable<Contact> GetCustomerContacts(Int32 customerID);
        void AddContactToCustomer(Contact contact, Int32 customerID);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact targetContact);

        // Node
        Node CreateNode();
        Node GetNode(Int32 nodeID);
        IEnumerable<Node> GetNodes();
        IEnumerable<Node> GetCustomerNodes(Int32 customerID);
        void AddNodeToCustomer(Node node, Int32 customerID);
        void UpdateNode(Node node);
        void DeleteNode(Node targetNode);

        // Credential
        Credential CreateCredential();
        Credential GetCredential(Int32 credentialID);
        IEnumerable<Credential> GetCredentials();
        IEnumerable<Credential> GetCustomerCredentials(Int32 customerID);
        IEnumerable<Credential> GetNodeCredentials(Int32 nodeID);
        void AddCredentialToCustomer(Credential credential, Int32 customerID);
        void AddCredentialToNode(Credential credential, Int32 nodeID);
        void UpdateCredential(Credential credential);
        void DeleteCrednetial(Credential targetCred);

        // Commit
        void SaveChanges();
    }
}
