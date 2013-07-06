// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Indicates the status of a customer's payment or complaint.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Indicates that the item is closed.
        /// </summary>
        Closed,

        /// <summary>
        /// Indicates that the item is active.
        /// </summary>
        Active,

        /// <summary>
        /// Indicates that the item is resolved.
        /// </summary>
        Resolved
    }

    /// <summary>
    /// Represents a customer.
    /// </summary>
    public class Customer : INotifyPropertyChanged, IEditableObject
    {
        /// <summary>
        /// Random number generator.
        /// </summary>
        private static Random random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Holds the data backing a customer.
        /// </summary>
        private struct CustomerData
        {
            /// <summary>
            /// The customer's first name.
            /// </summary>
            internal string FirstName;

            /// <summary>
            /// The customer's last name.
            /// </summary>
            internal string LastName;

            /// <summary>
            /// The customer's preferred color.
            /// </summary>
            internal Color PreferredColor;

            /// <summary>
            /// The customer's rating.
            /// </summary>
            internal int Rating;

            /// <summary>
            /// The customer's renewal date.
            /// </summary>
            internal DateTime RenewalDate;

            /// <summary>
            /// The customer's full address.
            /// </summary>
            internal string FullAddress;

            /// <summary>
            /// The customer's yearly fees.
            /// </summary>
            internal decimal YearlyFees;

            /// <summary>
            /// The customer's registration status.
            /// </summary>
            internal bool IsRegistered;

            /// <summary>
            /// The customer entry's validity.
            /// </summary>
            internal bool? IsValid;

            /// <summary>
            /// The customer's age.
            /// </summary>
            internal int? Age;

            /// <summary>
            /// The customer's payment status.
            /// </summary>
            internal Status Payment;

            /// <summary>
            /// The customer's complaint status.
            /// </summary>
            internal Status? Complaint;
        }

        #region Data

        /// <summary>
        /// Stores the data for the customer.
        /// </summary>
        private CustomerData _data;

        /// <summary>
        /// Stores a backup of the customer's data during edits.
        /// </summary>
        private CustomerData _backupData;

        /// <summary>
        /// Indicates whether the customer is being edited.
        /// </summary>
        private bool _editing;

        /// <summary>
        /// Indicates whether property change notifications should be enabled.
        /// </summary>
        private bool _delayPropertyChangeNotifications;

        #endregion Data

        /// <summary>
        /// Initializes a customer object.
        /// </summary>
        public Customer()
        {
            this._delayPropertyChangeNotifications = true;
            this._data = new CustomerData();
            this._data.RenewalDate = DateTime.Today;
        }

        /// <summary>
        /// Initializes a customer object.
        /// </summary>
        /// <param name="firstName">Customer's first name.</param>
        /// <param name="lastName">Customer's last name.</param>
        /// <param name="preferredColor">Customer's preferred color.</param>
        /// <param name="rating">Customer's rating.</param>
        /// <param name="fullAddress">Customer's full address.</param>
        /// <param name="yearlyFees">Customer's yearly fees.</param>
        /// <param name="isRegistered">Customer's registration status.</param>
        /// <param name="isValid">Customer's validity.</param>
        /// <param name="age">Customer's age.</param>
        /// <param name="payment">Customer's payment status.</param>
        /// <param name="complaint">Customer's complaint status.</param>
        public Customer(
            string firstName,
            string lastName,
            Color preferredColor,
            int rating,
            string fullAddress,
            decimal yearlyFees,
            bool isRegistered,
            bool? isValid,
            int? age,
            Status payment,
            Status? complaint)
            : this()
        {
            this._data.FirstName = firstName;
            this._data.LastName = lastName;
            this._data.PreferredColor = preferredColor;
            this._data.Rating = rating;
            this._data.FullAddress = fullAddress;
            this._data.YearlyFees = yearlyFees;
            this._data.IsRegistered = isRegistered;
            this._data.IsValid = isValid;
            this._data.Age = age;
            this._data.Payment = payment;
            this._data.Complaint = complaint;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on the customer changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members

        #region IEditableObject Members

        /// <summary>
        /// Indicates that the contact will undergo a cancellable edit.
        /// </summary>
        public void BeginEdit()
        {
            if (!this._editing)
            {
                this._backupData = this._data;
                this._editing = true;
            }
        }

        /// <summary>
        /// Indicates that the edit was cancelled and that the old state should be restored.
        /// </summary>
        public void CancelEdit()
        {
            if (this._editing)
            {
                if (!this._delayPropertyChangeNotifications)
                {
                    if (string.Compare(this._data.FirstName, this._backupData.FirstName, StringComparison.CurrentCulture) != 0)
                    {
                        RaisePropertyChanged("FirstName");
                    }
                    if (string.Compare(this._data.LastName, this._backupData.LastName, StringComparison.CurrentCulture) != 0)
                    {
                        RaisePropertyChanged("LastName");
                    }
                    if (this._data.Rating != this._backupData.Rating)
                    {
                        RaisePropertyChanged("Rating");
                    }
                    if (this._data.RenewalDate != this._backupData.RenewalDate)
                    {
                        RaisePropertyChanged("RenewalDate");
                    }
                    if (string.Compare(this._data.FullAddress, this._backupData.FullAddress, StringComparison.CurrentCulture) != 0)
                    {
                        RaisePropertyChanged("FullAddress");
                    }
                    if (this._data.YearlyFees != this._backupData.YearlyFees)
                    {
                        RaisePropertyChanged("YearlyFees");
                    }
                    if (this._data.IsRegistered != this._backupData.IsRegistered)
                    {
                        RaisePropertyChanged("IsRegistered");
                    }
                    if (this._data.IsValid != this._backupData.IsValid)
                    {
                        RaisePropertyChanged("IsValid");
                    }
                    if (this._data.Payment != this._backupData.Payment)
                    {
                        RaisePropertyChanged("Payment");
                    }
                    if (this._data.Complaint != this._backupData.Complaint)
                    {
                        RaisePropertyChanged("Complaint");
                    }
                }
                this._data = this._backupData;
                this._editing = false;
            }
        }

        /// <summary>
        /// Indicates that the edit completed and that changed fields should be committed.
        /// </summary>
        public void EndEdit()
        {
            if (this._editing)
            {
                if (this._delayPropertyChangeNotifications)
                {
                    if (string.Compare(this._data.FirstName, this._backupData.FirstName, StringComparison.CurrentCulture) != 0)
                    {
                        RaisePropertyChanged("FirstName");
                    }
                    if (string.Compare(this._data.LastName, this._backupData.LastName, StringComparison.CurrentCulture) != 0)
                    {
                        RaisePropertyChanged("LastName");
                    }
                    if (this._data.Rating != this._backupData.Rating)
                    {
                        RaisePropertyChanged("Rating");
                    }
                    if (this._data.RenewalDate != this._backupData.RenewalDate)
                    {
                        RaisePropertyChanged("RenewalDate");
                    }
                    if (string.Compare(this._data.FullAddress, this._backupData.FullAddress, StringComparison.CurrentCulture) != 0)
                    {
                        RaisePropertyChanged("FullAddress");
                    }
                    if (this._data.YearlyFees != this._backupData.YearlyFees)
                    {
                        RaisePropertyChanged("YearlyFees");
                    }
                    if (this._data.IsRegistered != this._backupData.IsRegistered)
                    {
                        RaisePropertyChanged("IsRegistered");
                    }
                    if (this._data.IsValid != this._backupData.IsValid)
                    {
                        RaisePropertyChanged("IsValid");
                    }
                    if (this._data.Payment != this._backupData.Payment)
                    {
                        RaisePropertyChanged("Payment");
                    }
                    if (this._data.Complaint != this._backupData.Complaint)
                    {
                        RaisePropertyChanged("Complaint");
                    }
                }
                this._backupData = new CustomerData();
                this._editing = false;
            }
        }

        #endregion IEditableObject Members

        #region Public Properties

        /// <summary>
        /// Gets or sets the customer's Age.
        /// </summary>
        public int? Age
        {
            get
            {
                return this._data.Age;
            }
            set
            {
                if (this._data.Age != value)
                {
                    this._data.Age = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("Age");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer's complaint status.
        /// </summary>
        public Status? Complaint
        {
            get
            {
                return this._data.Complaint;
            }
            set
            {
                if (this._data.Complaint != value)
                {
                    this._data.Complaint = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("Complaint");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer's first name.
        /// </summary>
        public string FirstName
        {
            get
            {
                return this._data.FirstName;
            }
            set
            {
                if (this._data.FirstName != value)
                {
                    this._data.FirstName = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("FirstName");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer's full address.
        /// </summary>
        public string FullAddress
        {
            get
            {
                return this._data.FullAddress;
            }
            set
            {
                if (this._data.FullAddress != value)
                {
                    this._data.FullAddress = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("FullAddress");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is registered.
        /// </summary>
        public bool IsRegistered
        {
            get
            {
                return this._data.IsRegistered;
            }
            set
            {
                if (this._data.IsRegistered != value)
                {
                    this._data.IsRegistered = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("IsRegistered");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is valid.
        /// </summary>
        public bool? IsValid
        {
            get
            {
                return this._data.IsValid;
            }
            set
            {
                if (this._data.IsValid != value)
                {
                    this._data.IsValid = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("IsValid");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer's last name.
        /// </summary>
        public string LastName
        {
            get
            {
                return this._data.LastName;
            }
            set
            {
                if (this._data.LastName != value)
                {
                    this._data.LastName = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("LastName");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer's payment status.
        /// </summary>
        public Status Payment
        {
            get
            {
                return this._data.Payment;
            }
            set
            {
                if (this._data.Payment != value)
                {
                    this._data.Payment = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("Payment");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the customer's preferred color.
        /// </summary>
        public Color PreferredColor
        {
            get
            {
                return this._data.PreferredColor;
            }
        }

        /// <summary>
        /// Gets a random customer.
        /// </summary>
        public static Customer FakeCustomer
        {
            get
            {
                return CreateFakeCustomer(-1);
            }
        }

        /// <summary>
        /// Gets or sets the customer's rating.
        /// </summary>
        public int Rating
        {
            get
            {
                return this._data.Rating;
            }
            set
            {
                if (this._data.Rating != value)
                {
                    this._data.Rating = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("Rating");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer's renewal date.
        /// </summary>
        public DateTime RenewalDate
        {
            get
            {
                return this._data.RenewalDate;
            }
            set
            {
                if (this._data.RenewalDate != value)
                {
                    this._data.RenewalDate = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("RenewalDate");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer's yearly fees.
        /// </summary>
        public decimal YearlyFees
        {
            get
            {
                return this._data.YearlyFees;
            }
            set
            {
                if (this._data.YearlyFees != value)
                {
                    this._data.YearlyFees = value;
                    if (!this._delayPropertyChangeNotifications || !this._editing)
                    {
                        RaisePropertyChanged("YearlyFees");
                    }
                }
            }
        }

        /// <summary>
        /// Gets a collection of the characters in the customer's first name.
        /// </summary>
        public Collection<char> Chars
        {
            get
            {
                return new Collection<char>(this.FirstName.ToCharArray().ToList());
            }
        }
        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets a random customer.
        /// </summary>
        /// <param name="index">The index of the random customer.</param>
        /// <returns>A customer based upon the index.</returns>
        public static Customer GetRandomCustomer(int index)
        {
            return CreateFakeCustomer(index);
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Gets a random status.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>A random status.</returns>
        internal static Status GetFakeStatus(Random random)
        {
            switch (random.Next(0, 3000) % 3)
            {
                case 0:
                    return Status.Active;
                case 1:
                    return Status.Closed;
                default:
                    return Status.Resolved;
            }
        }

        /// <summary>
        /// Gets a random string of characters.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>A string of randomly selected characters.</returns>
        internal static string GetFakeString(Random random)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                stringBuilder.Append((char)random.Next(65, 90));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets a random bool value.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>A random bool value.</returns>
        internal static bool GetFakeBool(Random random)
        {
            return random.Next(0, 2000) % 2 == 0;
        }

        /// <summary>
        /// Gets a random nullable bool value.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>A random nullable bool value.</returns>
        internal static bool? GetFakeNullableBool(Random random)
        {
            switch (random.Next(0, 3000) % 3)
            {
                case 0:
                    return null;
                case 1:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a random integer with a 1 in 6 chance of being null.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>A random nullable integer.</returns>
        internal static int? GetFakeNullableInteger(Random random)
        {
            int i = random.Next(0, 100);
            return (i % 6 == 0) ? null : (int?)i;
        }

        /// <summary>
        /// Gets a random status, with a 1 in 4 chance of being null.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>A random nullable status.</returns>
        internal static Status? GetFakeNullableStatus(Random random)
        {
            switch (random.Next(0, 4000) % 4)
            {
                case 0:
                    return Status.Active;
                case 1:
                    return Status.Closed;
                case 2:
                    return Status.Resolved;
                default:
                    return null;
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Creates a customer with random data.
        /// </summary>
        /// <param name="index">Index of the customer to generate.</param>
        /// <returns>A random customer.</returns>
        private static Customer CreateFakeCustomer(int index)
        {
            return new Customer(
                GetFakeString(random) + ((index > -1) ? index.ToString(CultureInfo.CurrentCulture) : string.Empty),
                GetFakeString(random),
                GetFakeColor(random),
                random.Next(5),
                GetFakeString(random),
                (decimal)(random.Next(100, 10000) / 100.0),
                GetFakeBool(random),
                GetFakeNullableBool(random),
                GetFakeNullableInteger(random),
                GetFakeStatus(random),
                GetFakeNullableStatus(random));
        }

        /// <summary>
        /// Randomly chooses a color from {Blue, Brown, Cyan, Gray, Green, Magenta, Orange, Purple, Red, Yellow}.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>A random color.</returns>
        private static Color GetFakeColor(Random random)
        {
            switch (random.Next(0, 9))
            {
                case 0:
                    return Colors.Blue;
                case 1:
                    return Colors.Brown;
                case 2:
                    return Colors.Cyan;
                case 3:
                    return Colors.Gray;
                case 4:
                    return Colors.Green;
                case 5:
                    return Colors.Magenta;
                case 6:
                    return Colors.Orange;
                case 7:
                    return Colors.Purple;
                case 8:
                    return Colors.Red;
                default:
                    return Colors.Yellow;
            }
        }

        /// <summary>
        /// Raises a property changed notification.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Private Methods
    }
}
