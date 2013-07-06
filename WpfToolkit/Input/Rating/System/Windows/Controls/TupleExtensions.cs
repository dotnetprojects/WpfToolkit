// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace System.Windows.Controls
{
    /// <summary>
    /// A set of tuple functions.
    /// </summary>
    internal static class Tuple
    {
        /// <summary>
        /// A method to create tuples.
        /// </summary>
        /// <typeparam name="T0">The type of the first item.</typeparam>
        /// <typeparam name="T1">The type of the second item.</typeparam>
        /// <param name="arg0">The type of the first argument.</param>
        /// <param name="arg1">The type of the second argument.</param>
        /// <returns>The tuple to return.</returns>
        public static Tuple<T0, T1> Create<T0, T1>(T0 arg0, T1 arg1)
        {
            return new Tuple<T0, T1>(arg0, arg1);
        }
    }
}
