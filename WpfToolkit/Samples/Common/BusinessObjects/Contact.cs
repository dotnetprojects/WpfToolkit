// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

/* 
 * The example companies, organizations, products, domain names,
 * e-mail addresses, logos, people, places, and events depicted
 * herein are fictitious.  No association with any real company,
 * organization, product, domain name, email address, logo, person,
 * places, or events is intended or should be inferred.
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Represents a person's contact information.
    /// </summary>
    public class Contact : INotifyPropertyChanged, IEditableObject
    {
        /// <summary>
        /// Represents the first name of the contact.
        /// </summary>
        private string firstName;

        /// <summary>
        /// Represents the last name of the contact.
        /// </summary>
        private string lastName;

        /// <summary>
        /// Represents the contact's phone number.
        /// </summary>
        private string phone;

        /// <summary>
        /// Represents the contact's street address.
        /// </summary>
        private string street1;

        /// <summary>
        /// Represents the contact's extended street address (such as an apartment number of P.O. Box).
        /// </summary>
        private string street2;

        /// <summary>
        /// Represents the contact's city.
        /// </summary>
        private string city;

        /// <summary>
        /// Represents the contact's state.
        /// </summary>
        private string state;

        /// <summary>
        /// Represents the contact's e-mail address.
        /// </summary>
        private string email;

        /// <summary>
        /// Represents the contact's zip code.
        /// </summary>
        private int zip;

        /// <summary>
        /// Indicates whether the address for the contact is a business address.
        /// </summary>
        private bool isBusinessAddress;

        /// <summary>
        /// Initializes a new contact.
        /// </summary>
        public Contact()
        {
        }

        /// <summary>
        /// Gets or sets the first name of the contact.
        /// </summary>
        [Required]
        [Display(Name = "First Name", GroupName = "Name")]
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (value != firstName)
                {
                    firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the last name of the contact.
        /// </summary>
        [Required]
        [Display(Name = "Last Name", GroupName = "Name")]
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the phone number of the contact in the form (###) ###-####.
        /// </summary>
        [Display(Name = "Phone", Description = "Phone number of the form (###) ###-####")]
        [RegularExpression(@"^\(\d\d\d\) \d\d\d\-\d\d\d\d$", ErrorMessage = "Not a valid phone number.  Please enter a phone number that matches the format (###) ###-####")]
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (value != phone)
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Phone" });
                    phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }

        /// <summary>
        /// Gets or sets the street address of the contact.
        /// </summary>
        [Required]
        [Display(Name = "Street Address", GroupName = "Address")]
        public string Street1
        {
            get
            {
                return street1;
            }
            set
            {
                if (value != street1)
                {
                    street1 = value;
                    OnPropertyChanged("Street1");
                }
            }
        }

        /// <summary>
        /// Gets or sets the secondary street address information for the contact, such as apartment number or P.O. Box.
        /// </summary>
        [Display(Name = "Secondary Street Address", Description = "Additional street address information, such as apartment number or P.O. Box", GroupName = "Address")]
        public string Street2
        {
            get
            {
                return street2;
            }
            set
            {
                if (value != street2)
                {
                    street2 = value;
                    OnPropertyChanged("Street2");
                }
            }
        }

        /// <summary>
        /// Gets or sets the city of the contact.
        /// </summary>
        [Required]
        [Display(Name = "City", Description = "City of residence", GroupName = "Address")]
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        /// <summary>
        /// Gets or sets the abbreviated state name of the contact (e.g. CA or TN).
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z]$", ErrorMessage = "Not a valid state abbreviation (e.g. CA or TN)")]
        [Display(Name = "State", Description = "State abbreviation", GroupName = "Address")]
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                if (value != state)
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "State" });
                    state = value;
                    OnPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this contact's address is a business address.
        /// </summary>
        [Display(Name = "Business Address", Description = "Indicates that the address is a business address.")]
        public bool IsBusinessAddress
        {
            get
            {
                return isBusinessAddress;
            }
            set
            {
                if (value != isBusinessAddress)
                {
                    isBusinessAddress = value;
                    OnPropertyChanged("IsBusinessAddress");
                }
            }
        }

        /// <summary>
        /// Gets or sets the e-mail address (e.g. someone@somewhere.com) for the contact.
        /// </summary>
        [RegularExpression(@"^([a-zA-Z0-9\!\%\$\%\*\/\?\|\^\{\}\`\~\&\'\+\-\=_]\.?)*[a-zA-Z0-9\!\%\$\%\*\/\?\|\^\{\}\`\~\&\'\+\-\=_]@((([a-zA-Z0-9\!\%\$\%\*\/\?\|\^\{\}\`\~\&\'\+\-\=_]\.?)*[a-zA-Z0-9\!\%\$\%\*\/\?\|\^\{\}\`\~\&\'\+\-\=_])|(\[\d+\.\d+\.\d+\.\d+\]))$", ErrorMessage = "Not a valid e-mail address.")]
        [Display(Name = "Email", Description = "An e-mail address of the form <name>@<domain>, such as john@johndoe.com")]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != email)
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Email" });
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        /// <summary>
        /// Gets or sets the zip code of the contact.
        /// </summary>
        [Required]
        [RegularExpression(@"^\d\d\d\d\d$", ErrorMessage = "Zip codes must be 5-digit numbers.")]
        [Display(Name = "Zip", Description = "Five-digit zip code", GroupName = "Address")]
        public int Zip
        {
            get
            {
                return zip;
            }
            set
            {
                if (value != zip)
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Zip" });
                    zip = value;
                    OnPropertyChanged("Zip");
                }
            }
        }

        /// <summary>
        /// Raises a property changed notification for the specified property name.
        /// </summary>
        /// <param name="propName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on the contact changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Gets a stock "John Doe" contact.
        /// </summary>
        public static Contact JohnDoe
        {
            get
            {
                return new Contact
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "(555) 555-5555",
                    Street1 = "1 Anywhere Street",
                    City = "Anytown",
                    State = "WA",
                    Zip = 98000
                };
            }
        }

        /// <summary>
        /// Gets a stock group of contacts with valid contact information.
        /// </summary>
        public static IEnumerable<Contact> People
        {
            get
            {
                return new ObservableCollection<Contact>
                {
                    new Contact
                    {
                        FirstName = "Kim",
                        LastName = "Abercrombie",
                        Phone = "(555) 555-0000",
                        Street1 = "1 Anywhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 12345
                    },
                    new Contact
                    {
                        FirstName = "Hadaya",
                        LastName = "Sagiv",
                        Phone = "(555) 555-0001",
                        Street1 = "2 Anywhere Street",
                        City = "AnotherTown",
                        State = "HI",
                        Zip = 55555
                    },
                    new Contact
                    {
                        FirstName = "Jeff",
                        LastName = "Price",
                        Phone = "(555) 555-0002",
                        Street1 = "3 Somewhere Street",
                        City = "Anytown",
                        State = "OR",
                        Zip = 77777
                    },
                    new Contact
                    {
                        FirstName = "Chris",
                        LastName = "Hill",
                        Phone = "(555) 555-0431",
                        Street1 = "17 Anywhere Lane",
                        City = "AnotherTown",
                        State = "WA",
                        Zip = 28145
                    },
                    new Contact
                    {
                        FirstName = "Prithvi",
                        LastName = "Raj",
                        Phone = "(555) 555-8543",
                        Street1 = "9144 Somewhere Street",
                        City = "Anytown",
                        State = "OR",
                        Zip = 75812
                    },
                    new Contact
                    {
                        FirstName = "Ryan",
                        LastName = "Ihrig",
                        Phone = "(555) 555-1345",
                        Street1 = "19 Anywhere Lane",
                        City = "AnotherTown",
                        State = "OR",
                        Zip = 58104
                    },
                    new Contact
                    {
                        FirstName = "Jim",
                        LastName = "Ptaszynski",
                        Phone = "(555) 555-5832",
                        Street1 = "1 Anywhere Street",
                        City = "Anytown",
                        State = "NH",
                        Zip = 74584
                    },
                    new Contact
                    {
                        FirstName = "Dan",
                        LastName = "Bacon",
                        Phone = "(555) 555-1914",
                        Street1 = "144 Anywhere Lane",
                        City = "Anytown",
                        State = "WA",
                        Zip = 13412
                    },
                    new Contact
                    {
                        FirstName = "Steve",
                        LastName = "Kastner",
                        Phone = "(555) 555-4581",
                        Street1 = "121 Anywhere Street",
                        City = "AnotherTown",
                        State = "WA",
                        Zip = 45828
                    },
                    new Contact
                    {
                        FirstName = "Andy",
                        LastName = "Ruth",
                        Phone = "(555) 555-4258",
                        Street1 = "391 Somewhere Lane",
                        City = "Anytown",
                        State = "WA",
                        Zip = 38447
                    },
                    new Contact
                    {
                        FirstName = "Brian",
                        LastName = "Bredehoeft",
                        Phone = "(555) 555-4853",
                        Street1 = "1234 Anywhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 14834
                    },
                    new Contact
                    {
                        FirstName = "Esko",
                        LastName = "Sario",
                        Phone = "(555) 555-3248",
                        Street1 = "1 Someplace Street",
                        City = "Anytown",
                        State = "CA",
                        Zip = 41834
                    },
                    new Contact
                    {
                        FirstName = "Joel",
                        LastName = "Lachance",
                        Phone = "(555) 555-1482",
                        Street1 = "48 Anywhere Lane",
                        City = "Anytown",
                        State = "NY",
                        Zip = 91934
                    },
                    new Contact
                    {
                        FirstName = "Bonnie",
                        LastName = "Skelly",
                        Phone = "(555) 555-8582",
                        Street1 = "1 Anywhere Street",
                        City = "Anytown",
                        State = "NY",
                        Zip = 95812
                    },
                    new Contact
                    {
                        FirstName = "James",
                        LastName = "Doe",
                        Phone = "(555) 555-5555",
                        Street1 = "1 Anywhere Street",
                        City = "AnotherTown",
                        State = "WA",
                        Zip = 58433
                    },
                    new Contact
                    {
                        FirstName = "John",
                        LastName = "Somebody",
                        Phone = "(555) 555-5555",
                        Street1 = "1 Anywhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 15852
                    },
                    new Contact
                    {
                        FirstName = "Jane",
                        LastName = "Somebody",
                        Phone = "(555) 555-5555",
                        Street1 = "1 Anywhere Street",
                        City = "Anytown",
                        State = "HI",
                        Zip = 18824
                    },
                    new Contact
                    {
                        FirstName = "Andrew",
                        LastName = "Ma",
                        Phone = "(555) 555-2384",
                        Street1 = "1 Anywhere Drive",
                        City = "Anytown",
                        State = "OR",
                        Zip = 12420
                    },
                    new Contact
                    {
                        FirstName = "Shannon",
                        LastName = "Dascher",
                        Phone = "(555) 555-7378",
                        Street1 = "231 Anywhere Lane",
                        City = "Anytown",
                        State = "WA",
                        Zip = 14855
                    },
                    new Contact
                    {
                        FirstName = "William",
                        LastName = "Looney",
                        Phone = "(555) 555-5555",
                        Street1 = "145 Somwhere Street",
                        City = "Anytown",
                        State = "OR",
                        Zip = 19855
                    },
                    new Contact
                    {
                        FirstName = "Louise",
                        LastName = "Toubro",
                        Phone = "(555) 555-4832",
                        Street1 = "123 Somewhere Lane",
                        City = "AnotherTown",
                        State = "OR",
                        Zip = 12842
                    },
                    new Contact
                    {
                        FirstName = "Tim",
                        LastName = "Toyoshima",
                        Phone = "(555) 555-3849",
                        Street1 = "1234 Anywhere Street",
                        City = "Anytown",
                        State = "NH",
                        Zip = 12843
                    },
                    new Contact
                    {
                        FirstName = "Peter",
                        LastName = "Mullen",
                        Phone = "(555) 555-7758",
                        Street1 = "1233 Anywhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 12484
                    },
                    new Contact
                    {
                        FirstName = "Ming-Yang",
                        LastName = "Xie",
                        Phone = "(555) 555-9283",
                        Street1 = "1423 Somewhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 12348
                    },
                    new Contact
                    {
                        FirstName = "Smuel",
                        LastName = "Yair",
                        Phone = "(555) 555-4821",
                        Street1 = "138 Anywhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 48439
                    },
                    new Contact
                    {
                        FirstName = "John",
                        LastName = "Woods",
                        Phone = "(555) 555-5859",
                        Street1 = "9583 Anywhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 93205
                    },
                    new Contact
                    {
                        FirstName = "Pieter",
                        LastName = "Wycoff",
                        Phone = "(555) 555-9292",
                        Street1 = "321 Anywhere Lane",
                        City = "Anytown",
                        State = "CA",
                        Zip = 95839
                    },
                    new Contact
                    {
                        FirstName = "Michiel",
                        LastName = "Wories",
                        Phone = "(555) 555-8584",
                        Street1 = "199 Anywhere Street",
                        City = "AnotherTown",
                        State = "NY",
                        Zip = 39495
                    },
                    new Contact
                    {
                        FirstName = "John",
                        LastName = "Yokim",
                        Phone = "(555) 555-4843",
                        Street1 = "3241 Someplace Street",
                        City = "Anytown",
                        State = "NY",
                        Zip = 93258
                    },
                    new Contact
                    {
                        FirstName = "Yanlai",
                        LastName = "Guo",
                        Phone = "(555) 555-4999",
                        Street1 = "2241 Somewhere Street",
                        City = "Anytown",
                        State = "WA",
                        Zip = 18444
                    }
                };
            }
        }

        #region IEditableObject Members
        /// <summary>
        /// Keeps a copy of the original contact for editing.
        /// </summary>
        private Contact cache;

        /// <summary>
        /// Indicates that the contact will undergo a cancellable edit.
        /// </summary>
        public void BeginEdit()
        {
            cache = new Contact();
            cache.City = this.City;
            cache.Email = this.Email;
            cache.FirstName = this.FirstName;
            cache.LastName = this.LastName;
            cache.Phone = this.Phone;
            cache.State = this.State;
            cache.Street1 = this.Street1;
            cache.Street2 = this.Street2;
            cache.Zip = this.Zip;
        }

        /// <summary>
        /// Indicates that the edit was cancelled and that the old state should be restored.
        /// </summary>
        public void CancelEdit()
        {
            this.City = cache.City;
            this.Email = cache.Email;
            this.FirstName = cache.FirstName;
            this.LastName = cache.LastName;
            this.Phone = cache.Phone;
            this.State = cache.State;
            this.Street1 = cache.Street1;
            this.Street2 = cache.Street2;
            this.Zip = cache.Zip;
            cache = null;
        }

        /// <summary>
        /// Indicates that the edit completed and that changed fields should be committed.
        /// </summary>
        public void EndEdit()
        {
            cache = null;
        }

        #endregion
    }
}