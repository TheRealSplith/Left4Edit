var Left4Edit = (function (Left4Edit) {
    Left4Edit.contact = function(id, fName, lName, email, phone, fax, customerID) {
        var self = this;

        this.ID = ko.observable(id);
        this.FirstName = ko.observable(fName);
        this.LastName = ko.observable(lName);
        this.Email = ko.observable(email);
        this.Phone = ko.observable(phone);
        this.Fax = ko.observable(fax);
        this.CustomerID = ko.observable(customerID);
        this.cFullName = ko.computed(function () {
            return self.LastName() + ', ' + self.FirstName()
        });
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
    Left4Edit.node = function (id, name, address, comment, customer) {
        var self = this;

        this.ID = ko.observable(id);
        this.Name = ko.observable(name);
        this.Address = ko.observable(address);
        this.Comment = ko.observable(comment);
        this.Customer = ko.observable(customer);
    }
    Left4Edit.credential = function (id, domain, userName, password) {
        var self = this;

        this.ID = ko.observable(id);
        this.Domain = ko.observable(domain);
        this.UserName = ko.observable(userName);
        this.Password = ko.observable(password);
    }

    return Left4Edit
}(Left4Edit || {}));