// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Represents a department in an organization.
    /// </summary>
    [ContentProperty("Divisions")]
    public class Department
    {
        /// <summary>
        /// Initializes a new instance of the Department class.
        /// </summary>
        public Department()
        {
            Divisions = new Collection<Department>();
            Employees = new Collection<Employee>();
        }

        /// <summary>
        /// Gets or sets the title of the department.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets a collection of employees in the department.
        /// </summary>
        public Collection<Employee> Employees { get; private set; }

        /// <summary>
        /// Gets a collection of divisions inside the department.
        /// </summary>
        public Collection<Department> Divisions { get; private set; }

        /// <summary>
        /// Gets a sample hierarchy of departments and employees.
        /// </summary>
        public static IEnumerable<Department> AllDepartments
        {
            get
            {
                IEnumerable<object> data = Application.Current.Resources["DepartmentOrganization"] as IEnumerable<object>;
                return (data != null) ?
                    data.Cast<Department>() :
                    Enumerable.Empty<Department>();
            }
        }
    }
}