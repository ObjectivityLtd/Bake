using Cake.Core.IO;

namespace Bake.Templating.Plan
{
    public class ParsableItem
    {
        public enum ItemType
        {
            Unknown,
            Solution,
            MsBuildProject,
            NpmProject
        }

        public FilePath Path { get; }

        public ItemType Type { get; }

        public ParsableItem(ItemType type, FilePath filePath)
        {
            this.Type = type;
            this.Path = filePath;
        }
    }
}
