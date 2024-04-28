using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class ProtocolInsensitiveUrlEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            // 去除协议部分，仅比较主机和路径
            Uri uriX, uriY;
            if (Uri.TryCreate(x, UriKind.Absolute, out uriX) && Uri.TryCreate(y, UriKind.Absolute, out uriY))
            {
                return uriX.Host.Equals(uriY.Host, StringComparison.OrdinalIgnoreCase) &&
                       uriX.PathAndQuery.Equals(uriY.PathAndQuery, StringComparison.OrdinalIgnoreCase);
            }

            // 如果都不是有效的绝对URL，按普通字符串比较
            return x.Equals(y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            if (obj == null) return 0;

            // 取主机和路径的组合哈希码，忽略协议
            Uri uri;
            if (Uri.TryCreate(obj, UriKind.Absolute, out uri))
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(uri.Host) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(uri.PathAndQuery);
            }

            // 如果不是有效的绝对URL，按普通字符串哈希
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj);
        }
    }
}
