$(function () {
    function DetailVM(customerID) {
        // Members
        this.CustomerID = customerID;

        this.customer = ko.observable();
        this.credentials = ko.observableArray([]);
        this.nodes = ko.observableArray([]);
        this.contacts = ko.observableArray([]);

        this.newCredential = ko.observable();
        this.newContact = ko.observable();
        this.nweNode = ko.observable();

        this.activeCredential = ko.observable();
        this.activeContact = ko.observable();
        this.activeNode = ko.observable();
        // Warning
        this.showWarning = ko.observable(false);
        this.warningMessage = ko.observable("");
        // Error
        this.showError = ko.observable(false);
        this.warningMessage = ko.observable("");
        // Visibility Assignment
        var self = this;
        var credentials
        var hub = $.connection.customerHub;
        // End of Members

        // Client functions (Server called)
        hub.client.returnCustomerByID = function (customer) {
            self.customer(new Left4Edit.customer(customer.ID, customer.Name, customer.Symbol));
            self.nodes([]);
            customer.Nodes.forEach(function (item) {
                self.nodes.push(new Left4Edit.node(item.ID, item.Name, item.Address, item.Comment, self.CustomerID));
            });
            self.contacts([]);
            customer.Contacts.forEach(function (item) {
                self.contacts.push(new Left4Edit.contact(item.ID, item.FirstName, item.LastName, item.Email, item.Phone, item.Fax, item.CustomerID));
            });
            self.credentials([]);
            customer.Credentials.forEach(function (item) {
                self.credentials.push(new Left4Edit.credential(item.ID, item.Domain, item.UserName, item.Password, item.CustomerID, item.NodeID));
            });
        }

        hub.client.returnUpdateContact = function (contact) {
            for (var i = 0; i < self.contacts().length; i++) {
                if (self.contacts()[i].ID() == contact.ID) {
                    self.contacts()[i].xCopy(contact);
                }
            }
        }
        hub.client.returnUpdateCredential = function (credential) {
            for (var i = 0; i < self.credentials().length; i++) {
                if (self.credentials()[i].ID() == credential.ID) {
                    self.credentials()[i].xCopy(credential);
                }
            }
        }
        hub.client.returnUpdateNode = function (node) {
            for (var i = 0; i < self.nodes().length; i++) {
                if (self.nodes()[i].ID() == node.ID) {
                    self.nodes()[i].xCopy(node);
                }
            }
        }
        hub.client.refreshContacts = function (contacts) {
            self.contacts([]);
            contacts.forEach(function (item) {
                self.contacts.push(new Left4Edit.contact(item.ID, item.FirstName, item.LastName, item.Email, item.Phone, item.Fax, item.CustomerID));
            });
        }
        hub.client.refreshCredentials = function (credentials) {
            self.credentials([]);
            credentials.forEach(function (item) {
                self.credentials.push(new Left4Edit.credential(item.ID, item.Domain, item.UserName, item.Password, item.CustomerID, item.NodeID));
            });
        }
        hub.client.refreshNodes = function (nodes) {
            self.nodes([]);
            nodes.forEach(function (item) {
                self.nodes.push(new Left4Edit.node(item.ID, item.Name, item.Address, item.Comment, self.CustomerID));
            });
        }
        // End of Client Functions

        // Commands
        this.getCustomer = function (custID) {
            hub.server.getCustomerByID(custID);
        }
        this.selectContact = function (contact) {
            self.activeContact(
              new Left4Edit.contact(contact.ID(), contact.FirstName(), contact.LastName(), contact.Email(), contact.Phone(), contact.Fax(), contact.CustomerID())
            );
        }
        this.selectCredential = function (credential) {
            self.activeCredential(
              new Left4Edit.credential(credential.ID(), credential.Domain(), credential.UserName(), credential.Password(), credential.CustomerID(), credential.NodeID())
            );
        }
        this.selectNode = function (node) {
            self.activeNode(
              new Left4Edit.node(node.ID(), node.Name(), node.Address(), node.Comment(), node.CustomerID())
            );
        }
        this.commitContact = function () {
            var c = self.activeContact();
            // passed function omits calculated fields
            hub.server.updateContact(ko.toJS(c,Left4Edit.IgnoreJS));
        }
        this.commitCredential = function () {
            var c = self.activeCredential();
            hub.server.updateCredential(ko.toJS(c, Left4Edit.IgnoreJS));
        }
        this.commitNode = function () {
            var n = self.activeNode();
            hub.server.updateNode(ko.toJS(n, Left4Edit.IgnoreJS));
        }
        this.deleteContact = function (contact) {
            hub.server.deleteContact(contact.ID());
        }
        this.deleteCredential = function (credential) {
            hub.server.deleteCredential(credential.ID());
        }
        this.deleteNode = function (node) {
            hub.server.deleteNode(node.ID());
        }
        // End of Commands

        // Constructor
        this.init = function () {
            hub.server.register("customer:" + self.CustomerID);
            hub.server.register("contacts.CustomerID:" + self.CustomerID);
            hub.server.register("credentials.CustomerID:" + self.CustomerID);
            hub.server.register("nodes.CustomerID:" + self.CustomerID);
            self.getCustomer(self.CustomerID);
        }
        // End of Constructor
    }
    // init
    var vm = new DetailVM(MODEL.CustomerID);
    $.connection.hub.start(function () { vm.init() });
    ko.applyBindings(vm);
});