//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Resources;

namespace Microsoft.Windows.Controls.Design
{
    // A wrapper around string identifiers.
    internal struct SRID
    {
        private string _string;

        public string String
        {
            get { return _string; }
        }

        private SRID(string s)
        {
            _string = s;
        }

        public static SRID CalendarCategoryTitle
        {
            get { return new SRID("CalendarCategoryTitle"); }
        }

        public static SRID DatePickerCategoryTitle
        {
            get { return new SRID("DatePickerCategoryTitle"); }
        }

        public static SRID ColumnsCategoryTitle
        {
            get { return new SRID("ColumnsCategoryTitle"); }
        }

        public static SRID RowsCategoryTitle
        {
            get { return new SRID("RowsCategoryTitle"); }
        }

        public static SRID HeadersCategoryTitle
        {
            get { return new SRID("HeadersCategoryTitle"); }
        }

        public static SRID GridLinesCategoryTitle
        {
            get { return new SRID("GridLinesCategoryTitle"); }
        }

        public static SRID HeaderCategoryTitle
        {
            get { return new SRID("HeaderCategoryTitle"); }
        }

        public static SRID TextCategoryTitle
        {
            get { return new SRID("TextCategoryTitle"); }
        }
    }
}
