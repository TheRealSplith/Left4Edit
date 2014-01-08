$(function () {
    function IndexVM() {
        // Members
        this.newCustomer = ko.validatedObservable(new Left4Edit.customer(null, null, null));
        this.customers = ko.observableArray([]);
        // Warning
        this.showWarning = ko.observable(false);
        this.warningMessage = ko.observable("");
        // Error
        this.showError = ko.observable(false);
        this.errorMessage = ko.observable("");
        // Visibility Assignment
        var self = this;
        var hub = $.connection.customerHub;
        var customers = this.customers;
        // End of Members

        // Constructor
        this.init = function () {
            hub.server.register("customers");
            hub.server.getCustomers();
        }
        // End of Constructor

        // Client functions (Server called)
        hub.client.returnCustomers = function(customerSet) {
            customers([]);
            customerSet.forEach(function(item) {
                customers.push(new Left4Edit.customer(item.ID, item.Name, item.Symbol, item.Contacts));
            });
        }
        hub.client.refreshCustomers = function () {
            self.showWarning(true);
            self.warningMessage("Customers collection has changed!");
        }
        hub.client.raiseError = function (exName, exMessage) {
            self.showError(true);
            self.errorMessage(exName + ": " + exMessage);
        }
        // End of Client Functions

        // Commands
        this.refreshCustomers = function () {
            hub.server.getCustomers();
            self.showWarning(false);
        }
        this.deleteCustomer = function (args) {
            hub.server.deleteCustomer(args.ID());
            self.customers.remove(args);
        }
        this.commitCustomer = function () {
            if (self.newCustomer.isValid()) {
                hub.server.addCustomer({ "Symbol": self.newCustomer().Symbol(), "Name": self.newCustomer().Name() });
                self.newCustomer = ko.validatedObservable(new Left4Edit.customer(null, null, null));
            }
            else
            {
                self.showError(true);
                self.errorMessage("Data not saved, model state invalid");
            }
        }
        this.hideError = function () {
            self.showError(false);
            self.errorMessage("");
        }
        // End of Commands
    }

    ko.validation.configure({
        registerExtenders: true,
        insertMessages: false
    });

    var vm = new IndexVM();
    $.connection.hub.start(function () { vm.init() });
    ko.validation.init({ grouping: { deep: true, observable: true } });
    ko.applyBindings(vm);
});