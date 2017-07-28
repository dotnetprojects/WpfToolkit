using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;

namespace System.Windows.Controls
{
    public class ValueByStringHelper
    {
        public static string GetStringValue(string path, Binding binding, object @object)
        {
            // see : https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/propertypath-xaml-syntax

            if (@object == null)
                return null;
            var _sb = new StringBuilder(20);
            var retVal = @object;
            PropertyInfo indexerP = null;
            ParameterInfo[] indexers = null;
            object[] indexerValues = null;
            int currentIndexer = 0;

            for (int n = 0; n < path.Length; n++)
            {
                var ch = path[n];
                if (ch == '.')
                {
                    if (_sb.Length > 0)
                    {
                        var prp = retVal.GetType().GetProperty(_sb.ToString());
                        if (prp == null)
                            return null;
                        retVal = prp.GetValue(retVal, null);
                        _sb.Clear();
                    }
                }
                else if (ch == '[')
                {
                    var prp = retVal.GetType().GetProperty(_sb.ToString());
                    if (prp == null)
                        return null;
                    retVal = prp.GetValue(retVal, null);
                    _sb.Clear();
                    indexerP = retVal.GetType().GetProperties().FirstOrDefault(x => x.GetIndexParameters().Any());
                    if (indexerP == null)
                        return null;
                    indexers = indexerP.GetIndexParameters();
                    indexerValues = new object[indexers.Length];
                }
                else if (ch == ',')
                {
                    indexerValues[currentIndexer] = Convert.ChangeType(_sb.ToString(), indexers[currentIndexer].ParameterType);
                    currentIndexer++;
                    _sb.Clear();
                }
                else if (ch == ']')
                {
                    indexerValues[currentIndexer] = Convert.ChangeType(_sb.ToString(), indexers[currentIndexer].ParameterType);
                    _sb.Clear();

                    retVal = indexerP.GetValue(retVal, indexerValues);
                    if (retVal == null)
                        return null;

                    currentIndexer = 0;
                    indexers = null;
                    indexerValues = null;
                }
                else
                {
                    _sb.Append(ch);
                }
            }

            if (_sb.Length > 0)
            {
                var prp = retVal.GetType().GetProperty(_sb.ToString());
                if (prp == null)
                    return null;
                retVal = prp.GetValue(retVal, null);
            }

            if (binding != null)
            {
                if (binding.Converter != null)
                {
                    retVal = binding.Converter.Convert(retVal, null, binding.ConverterParameter, binding.ConverterCulture);
                }

                var formattableRetVal = retVal as IFormattable;
                if (formattableRetVal != null)
                {
                    return formattableRetVal.ToString(binding.StringFormat, binding.ConverterCulture);
                }

                var convertibleRetVal = retVal as IConvertible;
                if (convertibleRetVal != null)
                {
                    return convertibleRetVal.ToString(binding.ConverterCulture);
                }
            }

            return retVal != null ? retVal.ToString() : null;
        }
    }
}
