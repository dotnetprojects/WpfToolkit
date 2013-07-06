//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;

namespace Microsoft.Windows.Controls
{
    [Flags]

    /// <summary>
    /// Determines whether the row/column headers are shown or not.
    /// </summary>
    public enum DataGridHeadersVisibility
    {
        /// <summary>
        /// Show Row, Column, and Corner Headers
        /// </summary>
        All = Row | Column,

        /// <summary>
        /// Show only Column Headers with top-right corner Header
        /// </summary>
        Column = 0x01,

        /// <summary>
        /// Show only Row Headers with bottom-left corner
        /// </summary>
        Row = 0x02,

        /// <summary>
        /// Don’t show any Headers
        /// </summary>
        None = 0x00
    }
}
