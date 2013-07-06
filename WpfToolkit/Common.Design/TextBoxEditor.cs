// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

extern alias Silverlight;
using System.Windows.Data;
using Microsoft.Windows.Design.PropertyEditing;

namespace System.Windows.Controls.Design.Common
{
    /// <summary>
    /// Simple TextBox inline editor.
    /// </summary>
    internal partial class TextBoxEditor : PropertyValueEditor
    {
        /// <summary>
        /// Preserve the constructor prototype from PropertyValueEditor.
        /// </summary>
        /// <param name="inlineEditorTemplate">Inline editor template.</param>
        public TextBoxEditor(DataTemplate inlineEditorTemplate)
            : base(inlineEditorTemplate)
        { }

        /// <summary>
        /// Default constructor builds the default TextBox inline editor template.
        /// </summary>
        public TextBoxEditor()
        {
            FrameworkElementFactory textBox = new FrameworkElementFactory(typeof(TextBox));
            Binding binding = new Binding();
            binding.Path = new PropertyPath("Value");
            binding.Mode = BindingMode.TwoWay;
            textBox.SetBinding(TextBox.TextProperty, binding);

            DataTemplate dt = new DataTemplate();
            dt.VisualTree = textBox;

            InlineEditorTemplate = dt;
        }
    }
}