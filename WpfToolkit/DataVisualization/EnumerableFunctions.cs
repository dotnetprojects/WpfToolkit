// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace System.Windows.Controls.DataVisualization
{
    /// <summary>
    /// This class contains general purpose functions to manipulate the generic
    /// IEnumerable type.
    /// </summary>
    internal static class EnumerableFunctions
    {
        /// <summary>
        /// Attempts to cast IEnumerable to a list in order to retrieve a count 
        /// in order one.  It attempts to cast fail the sequence is enumerated.
        /// </summary>
        /// <param name="that">The sequence.</param>
        /// <returns>The number of elements in the sequence.</returns>
        public static int FastCount(this IEnumerable that)
        {
            IList list = that as IList;
            if (list != null)
            {
                return list.Count;
            }
            return that.Cast<object>().Count();
        }

        /// <summary>
        /// Returns whether a sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="that">The sequence.</param>
        /// <returns>Whether the sequence is empty or not.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> that)
        {
            IEnumerator<T> enumerator = that.GetEnumerator();
            return !enumerator.MoveNext();
        }

        /// <summary>
        /// Returns the minimum value in the stream based on the result of a
        /// project function.
        /// </summary>
        /// <typeparam name="T">The stream type.</typeparam>
        /// <param name="that">The stream.</param>
        /// <param name="projectionFunction">The function that transforms the
        /// item.</param>
        /// <returns>The minimum value or null.</returns>
        public static T MinOrNull<T>(this IEnumerable<T> that, Func<T, IComparable> projectionFunction)
            where T : class
        {
            IComparable result = null;
            T minimum = default(T);
            if (!that.Any())
            {
                return minimum;
            }

            minimum = that.First();
            result = projectionFunction(minimum);
            foreach (T item in that.Skip(1))
            {
                IComparable currentResult = projectionFunction(item);
                if (result.CompareTo(currentResult) > 0)
                {
                    result = currentResult;
                    minimum = item;
                }
            }

            return minimum;
        }

        /////// <summary>
        /////// Returns the maximum value in the sequence or the default value.
        /////// </summary>
        /////// <typeparam name="T">The stream type.</typeparam>
        /////// <param name="that">The stream.</param>
        /////// <returns>The maximum value or the default value.</returns>
        ////public static T MaxOrDefault<T>(this IEnumerable<T> that)
        ////{
        ////    if (!that.Any())
        ////    {
        ////        return default(T);
        ////    }
        ////    else
        ////    {
        ////        return that.Max();
        ////    }
        ////}

        /// <summary>
        /// Returns the sum of all values in the sequence or the default value.
        /// </summary>
        /// <param name="that">The stream.</param>
        /// <returns>The sum of all values or the default value.</returns>
        public static double SumOrDefault(this IEnumerable<double> that)
        {
            if (!that.Any())
            {
                return 0.0;
            }
            else
            {
                return that.Sum();
            }
        }

        /// <summary>
        /// Returns the maximum value in the stream based on the result of a
        /// project function.
        /// </summary>
        /// <typeparam name="T">The stream type.</typeparam>
        /// <param name="that">The stream.</param>
        /// <param name="projectionFunction">The function that transforms the
        /// item.</param>
        /// <returns>The maximum value or null.</returns>
        public static T MaxOrNull<T>(this IEnumerable<T> that, Func<T, IComparable> projectionFunction)
            where T : class
        {
            IComparable result = null;
            T maximum = default(T);
            if (!that.Any())
            {
                return maximum;
            }

            maximum = that.First();
            result = projectionFunction(maximum);
            foreach (T item in that.Skip(1))
            {
                IComparable currentResult = projectionFunction(item);
                if (result.CompareTo(currentResult) < 0)
                {
                    result = currentResult;
                    maximum = item;
                }
            }

            return maximum;
        }

        /////// <summary>
        /////// Performs a projection with the index of the item in the sequence.
        /////// </summary>
        /////// <typeparam name="T">The type of the sequence.</typeparam>
        /////// <typeparam name="R">The type of the returned sequence.</typeparam>
        /////// <param name="that">The sequence to apply the projection to.</param>
        /////// <param name="func">The function to apply to each item.</param>
        /////// <returns>A sequence of the returned values.</returns>
        ////public static IEnumerable<R> SelectWithIndex<T, R>(this IEnumerable<T> that, Func<T, int, R> func)
        ////{
        ////    int counter = 0;

        ////    foreach (T item in that)
        ////    {
        ////        yield return func(item, counter);
        ////        counter++;
        ////    }
        ////}

        /// <summary>
        /// Accepts two sequences and applies a function to the corresponding 
        /// values in the two sequences.
        /// </summary>
        /// <typeparam name="T0">The type of the first sequence.</typeparam>
        /// <typeparam name="T1">The type of the second sequence.</typeparam>
        /// <typeparam name="R">The return type of the function.</typeparam>
        /// <param name="enumerable0">The first sequence.</param>
        /// <param name="enumerable1">The second sequence.</param>
        /// <param name="func">The function to apply to the corresponding values
        /// from the two sequences.</param>
        /// <returns>A sequence of transformed values from both sequences.</returns>
        public static IEnumerable<R> Zip<T0, T1, R>(IEnumerable<T0> enumerable0, IEnumerable<T1> enumerable1, Func<T0, T1, R> func)
        {
            IEnumerator<T0> enumerator0 = enumerable0.GetEnumerator();
            IEnumerator<T1> enumerator1 = enumerable1.GetEnumerator();
            while (enumerator0.MoveNext() && enumerator1.MoveNext())
            {
                yield return func(enumerator0.Current, enumerator1.Current);
            }
        }

        /////// <summary>
        /////// Zips a each item in the sequence together with the previous item in
        /////// the stream.
        /////// </summary>
        /////// <typeparam name="T">The type of the sequence.</typeparam>
        /////// <param name="that">The sequence.</param>
        /////// <returns>A sequence of tuples containing the current item and the
        /////// previous item.</returns>
        ////public static IEnumerable<Tuple<T, T>> ZipWithPrevious<T>(this IEnumerable<T> that)
        ////{
        ////    T previous = default(T);
        ////    IEnumerator<T> enumerator = that.GetEnumerator();
        ////    if (enumerator.MoveNext())
        ////    {
        ////        previous = enumerator.Current;
        ////    }
        ////    else
        ////    {
        ////        yield break;
        ////    }

        ////    while (enumerator.MoveNext())
        ////    {
        ////        yield return new Tuple<T, T>(enumerator.Current, previous);
        ////        previous = enumerator.Current;
        ////    }
        ////    yield break;
        ////}

        /////// <summary>
        /////// Accepts two sequences and applies a function to the corresponding 
        /////// values in the two sequences.
        /////// </summary>
        /////// <typeparam name="T0">The type of the first sequence.</typeparam>
        /////// <typeparam name="T1">The type of the second sequence.</typeparam>
        /////// <param name="enumerable0">The first sequence.</param>
        /////// <param name="enumerable1">The second sequence.</param>
        /////// <returns>A sequence of transformed values from both sequences.</returns>
        ////public static IEnumerable<Tuple<T0, T1>> ZipTuple<T0, T1>(IEnumerable<T0> enumerable0, IEnumerable<T1> enumerable1)
        ////{
        ////    IEnumerator<T0> enumerator0 = enumerable0.GetEnumerator();
        ////    IEnumerator<T1> enumerator1 = enumerable1.GetEnumerator();
        ////    while (enumerator0.MoveNext() && enumerator1.MoveNext())
        ////    {
        ////        yield return new Tuple<T0, T1>(enumerator0.Current, enumerator1.Current);
        ////    }
        ////}

        /////// <summary>
        /////// Creates a tuple.
        /////// </summary>
        /////// <typeparam name="T0">The type of the first item.</typeparam>
        /////// <typeparam name="T1">The type of the second item.</typeparam>
        /////// <param name="t0">The first item.</param>
        /////// <param name="t1">The second item.</param>
        /////// <returns>The created tuple.</returns>
        ////public static Tuple<T0, T1> Tuple<T0, T1>(T0 t0, T1 t1)
        ////{
        ////    return new Tuple<T0, T1>(t0, t1);
        ////}

        /////// <summary>
        /////// Creates a triple.
        /////// </summary>
        /////// <typeparam name="T0">The type of the first item.</typeparam>
        /////// <typeparam name="T1">The type of the second item.</typeparam>
        /////// <typeparam name="T2">The type of the third item.</typeparam>
        /////// <param name="t0">The first item.</param>
        /////// <param name="t1">The second item.</param>
        /////// <param name="t2">The third item.</param>
        /////// <returns>The created triple.</returns>
        ////public static Triple<T0, T1, T2> Triple<T0, T1, T2>(T0 t0, T1 t1, T2 t2)
        ////{
        ////    return new Triple<T0, T1, T2>(t0, t1, t2);
        ////}

        /////// <summary>
        /////// Creates a quadruple.
        /////// </summary>
        /////// <typeparam name="T0">The type of the first item.</typeparam>
        /////// <typeparam name="T1">The type of the second item.</typeparam>
        /////// <typeparam name="T2">The type of the third item.</typeparam>
        /////// <typeparam name="T3">The type of the fourth item.</typeparam>
        /////// <param name="t0">The first item.</param>
        /////// <param name="t1">The second item.</param>
        ////    /// <param name="t2">The third item.</param>
        /////// <param name="t3">The fourth item.</param>
        /////// <returns>The created triple.</returns>
        ////public static Quadruple<T0, T1, T2, T3> Quadruple<T0, T1, T2, T3>(T0 t0, T1 t1, T2 t2, T3 t3)
        ////{
        ////    return new Quadruple<T0, T1, T2, T3>(t0, t1, t2, t3);
        ////}
         
        /// <summary>
        /// Creates a sequence of values by accepting an initial value, an 
        /// iteration function, and apply the iteration function recursively.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="value">The initial value.</param>
        /// <param name="nextFunction">The function to apply to the value.
        /// </param>
        /// <returns>A sequence of the iterated values.</returns>
        public static IEnumerable<T> Iterate<T>(T value, Func<T, T> nextFunction)
        {
            yield return value;
            while (true)
            {
                value = nextFunction(value);
                yield return value;
            }
        }

        /// <summary>
        /// Returns the index of an item in a sequence.
        /// </summary>
        /// <param name="that">The sequence.</param>
        /// <param name="value">The item to search for.</param>
        /// <returns>The index of the item or -1 if not found.</returns>
        public static int IndexOf(this IEnumerable that, object value)
        {
            int index = 0;
            foreach (object item in that)
            {
                if (object.ReferenceEquals(value, item) || value.Equals(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Executes an action for each item and a sequence, passing in the 
        /// index of that item to the action procedure.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="that">The sequence.</param>
        /// <param name="action">A function that accepts a sequence item and its
        /// index in the sequence.</param>
        public static void ForEachWithIndex<T>(this IEnumerable<T> that, Action<T, int> action)
        {
            int index = 0;
            foreach (T item in that)
            {
                action(item, index);
                index++;
            }
        }

        /////// <summary>
        /////// Returns the maximum value or null if sequence is empty.
        /////// </summary>
        /////// <param name="that">The sequence to retrieve the maximum value from.
        /////// </param>
        /////// <returns>The maximum value or null.</returns>
        ////public static T MaxOrNull<T>(this IEnumerable<T> that)
        ////    where T : class, IComparable
        ////{
        ////    if (!that.Any())
        ////    {
        ////        return null;
        ////    }
        ////    return that.Max();
        ////}

        /////// <summary>
        /////// Returns the minimum value or null if sequence is empty.
        /////// </summary>
        /////// <param name="that">The sequence to retrieve the minimum value from.
        /////// </param>
        /////// <returns>The minimum value or null.</returns>
        ////public static T MinOrNull<T>(this IEnumerable<T> that)
        ////    where T : class, IComparable
        ////{
        ////    if (!that.Any())
        ////    {
        ////        return null;
        ////    }
        ////    return that.Min();
        ////}

        /// <summary>
        /// Returns the maximum value or null if sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="that">The sequence to retrieve the maximum value from.
        /// </param>
        /// <returns>The maximum value or null.</returns>
        public static T? MaxOrNullable<T>(this IEnumerable<T> that)
            where T : struct, IComparable
        {
            if (!that.Any())
            {
                return null;
            }
            return that.Max();
        }

        /// <summary>
        /// Returns the minimum value or null if sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="that">The sequence to retrieve the minimum value from.
        /// </param>
        /// <returns>The minimum value or null.</returns>
        public static T? MinOrNullable<T>(this IEnumerable<T> that)
            where T : struct, IComparable
        {
            if (!that.Any())
            {
                return null;
            }
            return that.Min();
        }

        /////// <summary>
        /////// Returns distinct elements from a sorted sequence by using the default equality comparer to compare values.
        /////// </summary>
        /////// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /////// <param name="source">The sequence to remove duplicate elements from.</param>
        /////// <returns>An IEnumerable&lt;T&gt; that contains distinct elements from the source sequence.</returns>
        ////public static IEnumerable<TSource> DistinctOfSorted<TSource>(this IEnumerable<TSource> source)
        ////{
        ////    IEnumerator<TSource> enumerator = source.GetEnumerator();
        ////    if (enumerator.MoveNext())
        ////    {
        ////        TSource last = enumerator.Current;
        ////        yield return last;
        ////        while (enumerator.MoveNext())
        ////        {
        ////            if (!enumerator.Current.Equals(last))
        ////            {
        ////                last = enumerator.Current;
        ////                yield return last;
        ////            }
        ////        }
        ////    }
        ////}

        /// <summary>
        /// Attempts to retrieve an element at an index by testing whether a 
        /// sequence is randomly accessible.  If not, performance degrades to a 
        /// linear search.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="that">The sequence.</param>
        /// <param name="index">The index of the element in the sequence.</param>
        /// <returns>The element at the given index.</returns>
        public static T FastElementAt<T>(this IEnumerable that, int index)
        {
            {
                IList<T> list = that as IList<T>;
                if (list != null)
                {
                    return list[index];
                }
            }
            {
                IList list = that as IList;
                if (list != null)
                {
                    return (T) list[index];
                }
            }
            return that.Cast<T>().ElementAt(index);
        }

        /////// <summary>
        /////// Returns a cached sequence that stores each enumerated value to avoid
        /////// recalculating it twice.
        /////// </summary>
        /////// <typeparam name="T">The type of the sequence.</typeparam>
        /////// <param name="that">The sequence to cache.</param>
        /////// <returns>The cached sequence.</returns>
        ////public static IEnumerable<T> Cache<T>(this IEnumerable<T> that)
        ////{
        ////    return new CachedEnumerable<T>(that);
        ////}

        /////// <summary>
        /////// This class is a cached sequence of values.
        /////// </summary>
        /////// <typeparam name="T">The type of the sequence.</typeparam>
        ////private class CachedEnumerable<T> : IEnumerable<T>
        ////{
        ////    /// <summary>
        ////    /// Initializes a new instance of the CachedEnumerable class.
        ////    /// </summary>
        ////    /// <param name="enumerable">The inner sequence.</param>
        ////    public CachedEnumerable(IEnumerable<T> enumerable)
        ////    {
        ////        this.CachedValues = new List<T>();
        ////        this.Enumerable = enumerable;
        ////    }

        ////    /// <summary>
        ////    /// Gets or sets the inner sequence.
        ////    /// </summary>
        ////    private IEnumerable<T> Enumerable { get; set; }

        ////    /// <summary>
        ////    /// Gets or sets the list of cached values.
        ////    /// </summary>
        ////    private IList<T> CachedValues { get; set; }

        ////    /// <summary>
        ////    /// Returns an enumerator for enumerating the values of the 
        ////    /// sequence.
        ////    /// </summary>
        ////    /// <returns>An enumerator for enumerating the values of the 
        ////    /// sequence.</returns>
        ////    public IEnumerator<T> GetEnumerator()
        ////    {
        ////        IEnumerator<T> enumerator = Enumerable.GetEnumerator();
        ////        int index = 0;
        ////        int enumeratorIndex = 0;
        ////        while (true)
        ////        {
        ////            if (index < CachedValues.Count)
        ////            {
        ////                yield return CachedValues[index];
        ////            }
        ////            else
        ////            {
        ////                bool returnValue = false;
        ////                for (enumeratorIndex = 0; enumeratorIndex <= index; enumeratorIndex++)
        ////                {
        ////                    returnValue = enumerator.MoveNext();
        ////                }

        ////                if (returnValue)
        ////                {
        ////                    CachedValues.Add(enumerator.Current);
        ////                    yield return enumerator.Current;
        ////                }
        ////                else
        ////                {
        ////                    yield break;
        ////                }
        ////            }
        ////            index++;
        ////        }
        ////    }

        ////    /// <summary>
        ////    /// Returns an enumerator for enumerating the values of the 
        ////    /// sequence.
        ////    /// </summary>
        ////    /// <returns>
        ////    /// An enumerator for enumerating the values of the sequence.
        ////    /// </returns>
        ////    IEnumerator IEnumerable.GetEnumerator()
        ////    {
        ////        return ((IEnumerable<T>)this).GetEnumerator();
        ////    }
        ////}

        /////// <summary>
        /////// Pushes a value onto the front of the sequence.
        /////// </summary>
        /////// <typeparam name="T">The type of the sequence.</typeparam>
        /////// <param name="that">The sequence.</param>
        /////// <param name="value">The value to push onto the front of the
        /////// sequence.</param>
        /////// <returns>The new sequence.</returns>
        ////public static IEnumerable<T> Push<T>(this IEnumerable<T> that, T value)
        ////{
        ////    yield return value;
        ////    foreach (T item in that)
        ////    {
        ////        yield return item;
        ////    }
        ////}
        
        /////// <summary>
        /////// Accepts two sequences and returns a sequence of pairs containing one 
        /////// item from each sequence at a corresponding index.
        /////// </summary>
        /////// <typeparam name="T">The type of the sequences.</typeparam>
        /////// <param name="sequence0">The first sequence.</param>
        /////// <param name="sequence1">The second sequence.</param>
        /////// <returns>A sequence of pairs.</returns>
        ////public static IEnumerable<Pair<T>> ZipPair<T>(IEnumerable<T> sequence0, IEnumerable<T> sequence1)
        ////{
        ////    IEnumerator<T> enumerable0 = sequence0.GetEnumerator();
        ////    IEnumerator<T> enumerable1 = sequence1.GetEnumerator();
        ////    while (enumerable0.MoveNext() && enumerable1.MoveNext())
        ////    {
        ////        yield return new Pair<T>(enumerable0.Current, enumerable1.Current);
        ////    }
        ////}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="K"></typeparam>
        ///// <typeparam name="R"></typeparam>
        ///// <param name="that"></param>
        ///// <param name="keySelector"></param>
        ///// <param name="itemSelector"></param>
        ///// <returns></returns>
        ////public static Dictionary<KeyType, ItemType> ToTransformedDictionary<InputType, KeyType, ItemType>(this IEnumerable<InputType> that, Func<InputType, KeyType> keySelector, Func<InputType, ItemType> itemSelector)
        ////{
        ////    Dictionary<KeyType, ItemType> dictionary = new Dictionary<KeyType, ItemType>();
        ////    foreach (InputType item in that)
        ////    {
        ////        dictionary[keySelector(item)] = itemSelector(item);
        ////    }
        ////    return dictionary;
        ////}
    }
}