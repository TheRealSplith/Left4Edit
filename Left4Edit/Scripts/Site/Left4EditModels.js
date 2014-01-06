﻿var Left4Edit = (function (Left4Edit) {
    Left4Edit.contact = function(id, fName, lName, email, phone, fax, customerID) {
        var self = this;

        // Members
        this.ID = ko.observable(id);
        this.FirstName = ko.observable(fName);
        this.LastName = ko.observable(lName);
        this.Email = ko.observable(email);
        this.Phone = ko.observable(phone);
        this.Fax = ko.observable(fax);
        this.CustomerID = ko.observable(customerID);
        // Calculated
        this.cFullName = ko.computed(function () {
            return self.LastName() + ', ' + self.FirstName()
        });
        // Functions
        this.xCopy = function (contact) {
            // This should not need to change
            //this.ID(contact.ID);
            self.FirstName(contact.FirstName);
            self.LastName(contact.LastName);
            self.Email(contact.Email);
            self.Phone(contact.Phone);
            self.Fax(contact.Fax);
            self.CustomerID(contact.CustomerID);
        }
    }
    Left4Edit.customer = function(id, name, symbol) {
        var self = this;

        this.ID = ko.observable(id);
        this.Name = ko.observable(name);
        this.Symbol = ko.observable(symbol);
        this.DetailLink = ko.computed(function () {
            return '/Customer/Detail/' + self.ID()
        });
    }
    Left4Edit.node = function (id, name, address, comment, customerID) {
        var self = this;
        // Members
        this.ID = ko.observable(id);
        this.Name = ko.observable(name);
        this.Address = ko.observable(address);
        this.Comment = ko.observable(comment);
        this.CustomerID = ko.observable(customerID);
    }
    Left4Edit.credential = function (id, domain, userName, password, custID, nodeID) {
        var self = this;
        // Members
        this.ID = ko.observable(id);
        this.Domain = ko.observable(domain);
        this.UserName = ko.observable(userName);
        this.Password = ko.observable(password);
        this.CustomerID = ko.observable(custID)
        this.NodeID = ko.observable(nodeID);
        // Calculated
        this.cFullUN = ko.computed(function () {
            return self.Domain() + '\\' + self.UserName()
        });
        // Functions
        this.xCopy = function (credential) {
            self.Domain(credential.Domain);
            self.UserName(credential.UserName);
            self.Password(credential.Password);
        }
    }

    Left4Edit.IgnoreJS = function (key, value) {
        if (key.charAt(0) == 'c' || key.charAt(0) == 'x')
            return;
        else
            return value;
    }

    return Left4Edit
}(Left4Edit || {}));