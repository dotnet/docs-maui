using Microsoft.Maui.Controls.PlatformConfiguration;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    public class ContactsTest
    {
        public ContactsTest()
        {
        }

        public async void PickContact()
        {
            try
            {
                // TODO: Wont compile
                //var contact = await Contacts.PickContactAsync();

                //if (contact == null)
                //    return;

                //var id = contact.Id;
                //var namePrefix = contact.NamePrefix;
                //var givenName = contact.GivenName;
                //var middleName = contact.MiddleName;
                //var familyName = contact.FamilyName;
                //var nameSuffix = contact.NameSuffix;
                //var displayName = contact.DisplayName;
                //var phones = contact.Phones; // List of phone numbers
                //var emails = contact.Emails; // List of email addresses
            }
            catch (Exception ex)
            {
                // Handle exception here.
            }
        }

        public async void GetAllContacts()
        {
            ObservableCollection<Contact> contactsCollect = new ObservableCollection<Contact>();

            try
            {
                // TODO: Wont compile
                //// cancellationToken parameter is optional
                //var cancellationToken = default(CancellationToken);
                //var contacts = await Contacts.GetAllAsync(cancellationToken);

                //if (contacts == null)
                //    return;

                //foreach (var contact in contacts)
                //    contactsCollect.Add(contact);
            }
            catch (Exception ex)
            {
                // Handle exception here.
            }
        }
    }
}
