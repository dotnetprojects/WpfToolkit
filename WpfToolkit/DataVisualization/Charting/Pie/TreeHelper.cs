using System.Windows.Media;

namespace System.Windows.Controls.DataVisualization.Charting
{
	internal static class TreeHelper
	{
		/// <summary>
		/// Finds the first ancestor of the element passed as a parameter that has type T.
		/// </summary>
		/// <typeparam name="T">The type of the ancestor we're looking for.</typeparam>
		/// <param name="visual">The element where we start our search.</param>
		/// <returns>The first ancestor of element of type T.</returns>
		public static T FindAncestor<T>(DependencyObject element) where T : class
		{
			while (element != null)
			{
				DependencyObject parent = VisualTreeHelper.GetParent(element) as DependencyObject;
				T result = parent as T;
				if (result != null)
				{
					return result;
				}
				element = parent;
			}
			return null;
		}
	}
}
