using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Web;
using Left4Edit.Models.Repo;
using Left4Edit.Models;

namespace Left4Edit.Hubs
{
    public class CustomerHub : Hub
    {
        private ICustomerRepo repo;

        public CustomerHub(ICustomerRepo repo)
        {
            if (repo == null)
                throw new ArgumentException("repo cannot be null");

            this.repo = repo;
        }

        public CustomerHub() : this(new EFCustomerRepo()) { }

        #region "Utility"
        public void Register(String id)
        {
            Groups.Add(Context.ConnectionId, id);
        }
        #endregion

        #region "Customers"
        public void GetCustomers()
        {
            var ret = repo.GetCustomers().ToArray();
            this.Clients.Caller.returnCustomers(ret);
        }

        public void GetCustomerByID(Int32 id)
        {
            this.Clients.Caller.returnCustomerByID(repo.GetCustomer(id));
        }

        public void AddCustomer(Customer customer)
        {
            repo.AddCustomer(customer);
            repo.SaveChanges();

            this.Clients.Group("customers").refreshCustomers();
        }

        public void UpdateCustomer(Customer customer)
        {
            repo.UpdateCustomer(customer);
            repo.SaveChanges();

            this.Clients.Group("customers").refreshCustomers();
            this.Clients.Group("customer:" + customer.ID).returnUpdateCustomer(customer);
        }

        public void DeleteCustomer(Int32 customerID)
        {
            try
            {
                repo.DeleteCustomer(repo.GetCustomer(customerID));
                repo.SaveChanges();

                this.Clients.Group("customers").refreshCustomers();
                this.Clients.Group("customer:" + customerID).removed();
            }
            catch (Exception ex)
            {
                this.Clients.Caller.raiseError(ex.GetType().Name, ex.Message);
            }
        }
        #endregion

        #region "Credential"
        public void GetCredentials()
        {
            this.Clients.Caller.returnCredentials(repo.GetCredentials().ToArray());
        }

        public void GetCredentialByCustomer(Int32 customerID)
        {
        }

        public void UpdateCredential(Credential credential)
        {
            repo.UpdateCredential(credential);
            repo.SaveChanges();

            this.Clients.Group("credentials").refreshCredentials();
            if (credential.NodeID.HasValue)
                this.Clients.Group("credentials.NodeID:" + credential.NodeID).returnUpdateCredential(credential);
            else
                this.Clients.Group("credentials.CustomerID:" + credential.CustomerID.Value).returnUpdateCredential(credential);
        }

        public void DeleteCredential(Int32 credentialID)
        {
            try
            {
                var credential = repo.GetCredential(credentialID);
                if (credential.CustomerID.HasValue)
                {
                    var customerID = credential.CustomerID.Value;
                    repo.DeleteCrednetial(credential);
                    repo.SaveChanges();

                    this.Clients.Group("credentials").refreshCredentials(repo.GetCredentials());
                    this.Clients.Group("credentials.CustomerID:" + customerID).refreshCredentials(repo.GetCustomerCredentials(customerID));
                    this.Clients.Group("credential:" + credentialID).removed();
                }
                if (credential.NodeID.HasValue)
                {
                    var nodeID = credential.NodeID.Value;
                    repo.DeleteCrednetial(credential);
                    repo.SaveChanges();

                    this.Clients.Group("credentials").refreshCredentials(repo.GetCredentials());
                    this.Clients.Group("credentials.NodeID:" + nodeID).refreshCredentials(repo.GetNodeCredentials(nodeID));
                    this.Clients.Group("credential:" + credentialID).removed();
                }
            }
            catch (Exception ex)
            {
                this.Clients.Caller.raiseError(ex.GetType().Name, ex.Message);
            }
        }
        #endregion

        #region "Contact"
        public void UpdateContact(Contact contact)
        {
            repo.UpdateContact(contact);
            repo.SaveChanges();

            this.Clients.Group("contacts").refreshContacts();
            this.Clients.Group("contacts.CustomerID:" + contact.CustomerID).returnUpdateContact(contact);
        }

        public void DeleteContact(Int32 contactID)
        {
            try
            {
                var customerID = repo.GetContact(contactID).CustomerID.Value;
                repo.DeleteContact(repo.GetContact(contactID));
                repo.SaveChanges();

                this.Clients.Group("contacts").refreshContacts(repo.GetContacts());
                this.Clients.Group("contacts.CustomerID:" + customerID).refreshContacts(repo.GetCustomerContacts(customerID));
                this.Clients.Group("contact:" + contactID).removed();
            }
            catch (Exception ex)
            {
                this.Clients.Caller.raiseError(ex.GetType().Name, ex.Message);
            }
        }
        #endregion

        #region "Node"
        public void UpdateNode(Node node)
        {
            repo.UpdateNode(node);
            repo.SaveChanges();

            this.Clients.Group("nodes").refreshNodes();
            this.Clients.Group("nodes.CustomerID:" + node.CustomerID).returnUpdateNode(node);
        }

        public void AddNodeToCustomer(Node node, Int32 customerID)
        {
            repo.AddNodeToCustomer(node, customerID);
            repo.SaveChanges();

            this.Clients.Group("nodes").refreshNodes(repo.GetNodes());
            this.Clients.Group("nodes.CustomerID:" + customerID).refreshNodes(repo.GetCustomerNodes(customerID));
        }

        public void AddCredentialToCustomer(Credential cred, Int32 customerID)
        {
            repo.AddCredentialToCustomer(cred, customerID);
            repo.SaveChanges();

            this.Clients.Group("credentials").refreshCredentials(repo.GetNodes());
            this.Clients.Group("credentials.CustomerID:" + customerID).refreshCredentials(repo.GetCustomerCredentials(customerID));
        }

        public void AddContactToCustomer(Contact cont, Int32 customerID)
        {
            repo.AddContactToCustomer(cont, customerID);
            repo.SaveChanges();

            this.Clients.Group("contacts").refreshContacts(repo.GetContacts());
            this.Clients.Group("contacts.CustomerID:" + customerID).refreshContacts(repo.GetCustomerContacts(customerID));
        }

        public void DeleteNode(Int32 nodeID)
        {
            try
            {
                var customerID = repo.GetNode(nodeID).CustomerID.Value;
                repo.DeleteNode(repo.GetNode(nodeID));
                repo.SaveChanges();

                this.Clients.Group("nodes").refreshNodes(repo.GetNodes());
                this.Clients.Group("nodes.CustomerID:" + customerID).refreshNodes(repo.GetCustomerNodes(customerID));
                this.Clients.Group("node:" + nodeID).removed();
            }
            catch (Exception ex)
            {
                this.Clients.Caller.raiseError(ex.GetType().Name, ex.Message);
            }
        }
        #endregion
    }
}