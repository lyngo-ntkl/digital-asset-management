using System.Text;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public sealed class AbsolutePathCreationHelper
    {
        private const string BaseFolder = "Files";
        public const string Separator = "/";

        public static string CreateAbsolutePath(string name, string? parentPath = null)
        {
            StringBuilder builder = new StringBuilder();
            if (parentPath != null)
            {
                builder.Append(parentPath).Append(Separator);
            }
            else
            {
                builder.Append(Separator).Append(BaseFolder).Append(Separator);
            }
            builder.Append(name);
            return builder.ToString();
        }

        public static string GetNewAbsolutePath(string oldAbsolutePath, string newParentAbsolutePath)
        {
            var name = oldAbsolutePath.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Last();
            var newAbsolutePath = CreateAbsolutePath(name, newParentAbsolutePath);
            return newAbsolutePath;
        }
    }
}
