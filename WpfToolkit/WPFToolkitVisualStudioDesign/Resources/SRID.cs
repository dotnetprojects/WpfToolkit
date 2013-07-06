//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Resources;

namespace Microsoft.Windows.Controls.Design.VisualStudio 
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

        public static SRID SortCategoryTitle
        {
            get { return new SRID("SortCategoryTitle"); }
        }

        public static SRID SelectionCategoryTitle
        {
            get { return new SRID("SelectionCategoryTitle"); }
        }
    }
}
