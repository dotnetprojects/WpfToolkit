using System.Text;

namespace System.Windows.Controls
{
    internal class ValueByStringHelper
    {
        public static object GetValue(string path, object @object)
        {
            if (@object == null || string.IsNullOrEmpty(path))
                return @object;

            var _sb = new StringBuilder(20);
            var retVal = @object;
            for (int n = 0; n < path.Length; n++)
            {
                var ch = path[n];
                if (ch == '.')
                {
                    var prp = retVal.GetType().GetProperty(_sb.ToString());
                    if (prp == null)
                        return null;
                    retVal = prp.GetValue(retVal, null);
                }
                else if (ch == '[')
                {
                    throw new Exception("Indexer Propertypath not yet supported");
                }
                else
                {
                    _sb.Append(ch);
                }
            }

            return retVal;
        }
    }
}
