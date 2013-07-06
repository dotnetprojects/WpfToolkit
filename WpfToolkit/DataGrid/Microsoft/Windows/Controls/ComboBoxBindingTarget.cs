//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;

namespace Microsoft.Windows.Controls
{
    /// <summary>
    ///     Enum for selection type of ComboBox column
    /// </summary>
    public enum ComboBoxBindingTarget
    {
        /// <summary>
        /// SelectedItem of the ComboBox will be used
        /// for Binding
        /// </summary>
        SelectedItem,

        /// <summary>
        /// SelectedValue property of ComboBox will
        /// be used for Binding
        /// </summary>
        SelectedValue,

        /// <summary>
        /// Text property of ComboBox will be
        /// used for Binding
        /// </summary>
        Text
    }
}
