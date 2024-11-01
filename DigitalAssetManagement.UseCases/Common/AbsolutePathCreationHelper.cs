using System.Text;
using System.Text.RegularExpressions;

namespace DigitalAssetManagement.UseCases.Common
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

        public static string ChangeParentPath(string oldAbsolutePath, string newParentAbsolutePath)
        {
            var name = oldAbsolutePath.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Last();
            return CreateAbsolutePath(name, newParentAbsolutePath);
        }

        public static string ChangeName(string newName, string oldPath)
        {
            var parentPath = oldPath.Substring(0, oldPath.LastIndexOf(Separator));
            return CreateAbsolutePath(newName, parentPath);
        }
    }
}
