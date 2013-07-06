// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using Microsoft.Windows.Design.PropertyEditing;

namespace System.Windows.Controls.Design.Common
{
    /// <summary>
    /// Dummy editor to make a property read only in designer.
    /// </summary>
    internal partial class EmptyEditor : PropertyValueEditor
    {
        /// <summary>
        /// Preserve the constructor prototype from PropertyValueEditor.
        /// </summary>
        /// <param name="inlineEditorTemplate">Inline editor template.</param>
        public EmptyEditor(DataTemplate inlineEditorTemplate)
            : base(inlineEditorTemplate)
        { }

        /// <summary>
        /// Default constructor builds the default TextBox inline editor template.
        /// </summary>
        public EmptyEditor()
        {
            FrameworkElementFactory textBox = new FrameworkElementFactory(typeof(TextBox));
            textBox.SetValue(TextBox.IsReadOnlyProperty, true);
            DataTemplate dt = new DataTemplate();
            dt.VisualTree = textBox;
            InlineEditorTemplate = dt;
        }
    }
}
