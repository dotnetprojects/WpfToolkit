// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Represents a collection of Customer objects.
    /// </summary>
    public class CustomerCollection : IList<Customer>, IList, INotifyCollectionChanged
    {
        /// <summary>
        /// The underlying list of customers.
        /// </summary>
        private List<Customer> _customers;

        /// <summary>
        /// Notifies of changes to the collection.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Initializes a CustomerCollection object with 25 customers.
        /// </summary>
        public CustomerCollection()
            : this(25)
        {
        }

        /// <summary>
        /// Initializes a CustomerCollection.
        /// </summary>
        /// <param name="customerCount">The number of customers to generate.</param>
        public CustomerCollection(int customerCount)
        {
            this._customers = new List<Customer>();

            // Fake Data
            if (customerCount > 0)
            {
                Add(new Customer("John", "Doe", Colors.Orange, 1, "Obere Str. 57, Berlin 12209, Germany", (decimal)12.25, false, null, 35, Status.Active, Status.Closed));
            }
            if (customerCount > 1)
            {
                Add(new Customer("Jane", "Doe", Colors.Purple, 5, "24, place Kléber, 67000 Strasbourg, France", (decimal)15.55, true, null, 57, Status.Resolved, null));
            }
            if (customerCount > 2)
            {
                Add(new Customer("Joe", "Anybody", Colors.Yellow, 1, "Strada Provinciale 124, 42100 Reggio Emilia, Italy", (decimal)11.5, true, false, null, Status.Closed, Status.Closed));
            }
            if (customerCount > 3)
            {
                Add(new Customer("Jill", "Anybody", Colors.Green, 1, "Geislweg 36, Salzburg 5020, Austria", (decimal)9.99, false, true, null, Status.Active, null));
            }
            for (int customer = 4; customer < customerCount; customer++)
            {
                Add(Customer.GetRandomCustomer(customer));
            }
        }

        #region ICollection Members

        /// <summary>
        /// Copies the collection.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="index">Index to copy to.</param>
        public void CopyTo(System.Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether the collection supports asynchronous operations.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets an object to use for synchronization.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region IList Members

        /// <summary>
        /// Gets a value indicating whether the collection has a fixed size.
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the customer at an index.
        /// </summary>
        /// <param name="index">Index to retrieve.</param>
        /// <returns>The customer at the given index in the collection.</returns>
        object IList.this[int index]
        {
            get
            {
                return this._customers[index];
            }
            set
            {
                Customer customer = value as Customer;
                if (customer == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._customers[index] = customer;
            }
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="value">The value to add to the collection.</param>
        /// <returns>The index at which the value was added.</returns>
        public int Add(object value)
        {
            Customer customer = value as Customer;
            if (customer == null)
            {
                throw new ArgumentException("Invalid argument to Add", "value");
            }
            Add(customer);
            return this.Count - 1;
        }

        /// <summary>
        /// Determines whether the collection contains the given value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if the collection contains the value.  False otherwise.</returns>
        public bool Contains(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return Contains(value as Customer);
        }

        /// <summary>
        /// Determines the index of the given value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The index of the value, or -1 if not found.</returns>
        public int IndexOf(object value)
        {
            return IndexOf(value as Customer);
        }

        /// <summary>
        /// Inserts an object at the given index.
        /// </summary>
        /// <param name="index">Index where the value should be inserted.</param>
        /// <param name="value">The value to insert.</param>
        public void Insert(int index, object value)
        {
            Insert(index, value as Customer);
        }

        /// <summary>
        /// Removes the given value from the collection.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        public void Remove(object value)
        {
            Remove(value as Customer);
        }

        #endregion

        #region IList<Customer> Members

        /// <summary>
        /// Determines the index of the given customer.
        /// </summary>
        /// <param name="item">The customer to search for.</param>
        /// <returns>The index of the customer, or -1 if not found.</returns>
        public int IndexOf(Customer item)
        {
            return this._customers.IndexOf(item);
        }

        /// <summary>
        /// Inserts a customer at the given index.
        /// </summary>
        /// <param name="index">Index where the value should be inserted.</param>
        /// <param name="item">Customer to insert.</param>
        public void Insert(int index, Customer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            this._customers.Insert(index, item);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Removes the item at the given index.
        /// </summary>
        /// <param name="index">Index at which the item should be removed.</param>
        public void RemoveAt(int index)
        {
            Customer customer = this._customers[index];
            this._customers.RemoveAt(index);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, customer, index));
        }

        /// <summary>
        /// Gets the customer at the given index.
        /// </summary>
        /// <param name="index">Index of the customer.</param>
        /// <returns>The customer at the given index.</returns>
        public Customer this[int index]
        {
            get
            {
                return this._customers[index];
            }
            set
            {
                if (this._customers[index] != value)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException("value");
                    }
                    this._customers[index] = value;
                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, index));
                }
            }
        }

        #endregion

        #region ICollection<Customer> Members

        /// <summary>
        /// Adds a customer to the collection.
        /// </summary>
        /// <param name="item">Customer to add.</param>
        public void Add(Customer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            this._customers.Add(item);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, this.Count - 1));
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            this._customers.Clear();
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Determines whether the collection contains the given customer.
        /// </summary>
        /// <param name="item">The customer to search for.</param>
        /// <returns>True if the collection contains the customer.  False otherwise.</returns>
        public bool Contains(Customer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return this._customers.Contains(item);
        }

        /// <summary>
        /// Copies customers from this collection into an array.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">Start index in the array into which customers will be copied.</param>
        public void CopyTo(Customer[] array, int arrayIndex)
        {
            this._customers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this._customers.Count;
            }
        }

        /// <summary>
        /// Removes a customer from the collection.
        /// </summary>
        /// <param name="item">Customer to remove.</param>
        /// <returns>True if the customer was found and removed.  False otherwise.</returns>
        public bool Remove(Customer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = this._customers.IndexOf(item);
            if (index > -1)
            {
                this._customers.RemoveAt(index);
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<Customer> Members

        /// <summary>
        /// Enumerates over the customers in the collection.
        /// </summary>
        /// <returns>An enumerator for the customers.</returns>
        public IEnumerator<Customer> GetEnumerator()
        {
            return this._customers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Enumerates over the customers in the collection.
        /// </summary>
        /// <returns>An enumerator for the customers.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._customers.GetEnumerator();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        #endregion Private methods
    }
}
