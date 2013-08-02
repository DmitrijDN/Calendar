﻿//ViewModel
function UserContactsVm(model) {
    var self = this;

    //Variables
    self.model = model;

    //Methods
    self.addContact = function() {
        var contact = { Value: ko.observable(""), ContactType: ko.observable("") };

        contact.Value.subscribe(function(changedContact) {
            $.getJSON('GetContactType', { contact: changedContact }, function(data) {
                contact.ContactType(data);
            });
        });
                
        if (self.model.Contacts == null) self.model.Contacts = ko.observableArray([]);
                
        self.model.Contacts.push(contact);
    };

    self.removeContact = function(contact) {
        self.model.Contacts.remove(contact);
    };

    self.contactsIndexedName = function(index, parameter) {
        return "Contacts[" + index + "]." + parameter;
    };

    //Custom Bindings
    ko.bindingHandlers.contactImage = {
        update: function(element, valueAccessor) {
            var valueUnwrapped = ko.unwrap(valueAccessor());
            var newClass = "";

            switch (valueUnwrapped) {
                case 1:
                    newClass = "icon-mail"; break;
                case 2:
                    newClass = "icon-twitter"; break;
                case 3:
                    newClass = "icon-skype"; break;
                case 4:
                    newClass = "icon-phone"; break;
                case 5:
                    newClass = "icon-globe";
            }
            $(element).attr("class", newClass);
        }
    };
};

var mappingOption = {
    'Contacts': {
        create: function (options) {
            var contact = ko.mapping.fromJS(options.data);
            contact.Value.subscribe(function (changedContact) {
                $.getJSON('GetContactType', { contact: changedContact }, function (data) {
                    contact.ContactType(data);
                });
            });
            return contact;
        }
    }
};