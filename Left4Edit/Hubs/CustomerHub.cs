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

            this.Clients.Group("customers").customersChanged();
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

                this.Clients.Group("customers").customersChanged();
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
        #endregion

        #region "Contact"
        public void UpdateContact(Contact contact)
        {
            repo.UpdateContact(contact);
            repo.SaveChanges();

            this.Clients.Group("contacts").refreshContacts();
            this.Clients.Group("contacts.CustomerID:" + contact.CustomerID).returnUpdateContact(contact);
        }
        #endregion
    }
}