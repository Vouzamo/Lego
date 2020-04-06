using System.Collections.Generic;
using System.Linq;

namespace Lego.Core.Extensions
{
    public static class ByteExtensions
    {
        public static IEnumerable<byte> GetRange(this IEnumerable<byte> bytes, int skip, int take)
        {
            return bytes.Skip(skip).Take(take).ToList();
        }
    }
}
