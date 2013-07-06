using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace System.Windows.Controls.Samples
{
    public partial class ObjectCollection : Collection<Object>
    {
        public ObjectCollection()
            : base()
        {
        }

        public ObjectCollection(IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            foreach (Object item in enumerable)
            {
                Add(item);
            }
        }
    }
}
