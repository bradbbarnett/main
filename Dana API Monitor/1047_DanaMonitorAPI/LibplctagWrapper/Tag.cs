using System.Configuration;
using System.Text;

namespace LibplctagWrapper
{
    public class Tag
    {
        static string ipAddress = ConfigurationManager.ConnectionStrings["ipAddressAB"].ConnectionString;
        static string path = ConfigurationManager.ConnectionStrings["pathAB"].ConnectionString;
        public string Name { get; }
        public int ElementSize { get; }
        public int ElementCount { get; }
        public string UniqueKey { get; }

        public Tag(string name, int elementSize, int elementCount)
        {
            Name = name;
            ElementSize = elementSize;
            ElementCount = elementCount;

            var sb = new StringBuilder();
            sb.Append($"protocol=ab_eip&gateway={ipAddress}");
            if (!string.IsNullOrEmpty(path))
            {
                sb.Append($"&path={path}");
            }
            sb.Append($"&cpu=LGX&elem_size={ElementSize}&elem_count={elementCount}&name={name}");

            UniqueKey = sb.ToString();
        }
    }
}