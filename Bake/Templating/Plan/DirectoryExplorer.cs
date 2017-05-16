using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bake.Templating.Plan
{
    public class DirectoryExplorer
    {
        private static readonly Regex DirectoriesToIgnore = new Regex(@".*(\.git|\.vs|bin|obj|packages|build|deploy)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex FilesToSearch = new Regex(@".*(\.csproj|package\.json)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IEnumerable<ParsableItem> Explore(string path)
        {
            foreach (var dir in GetDirectories(path))
            {
                var files = GetParsableItems(dir);
                var searchInSubdirectories = true;
                foreach (var file in files)
                {
                    yield return file;
                    if (!ShouldSearchInSubdirectories(file))
                    {
                        searchInSubdirectories = false;
                    }
                }
                if (!searchInSubdirectories)
                {
                    continue;
                }
                foreach (var parsableItem in Explore(dir))
                {
                    yield return parsableItem;
                }
            }
        }

        private IEnumerable<string> GetDirectories(string basePath)
        {
            return Directory.EnumerateDirectories(basePath, "*", SearchOption.TopDirectoryOnly)
                .Where(path => !DirectoriesToIgnore.IsMatch(path));
        }

        private IEnumerable<ParsableItem> GetParsableItems(string dirPath)
        {
            return Directory.EnumerateFiles(dirPath, "*", SearchOption.TopDirectoryOnly)
                .Where(path => FilesToSearch.IsMatch(path))
                .Select(GetParsableItem);
        }

        private ParsableItem GetParsableItem(string path)
        {
            return new ParsableItem(GetParsableItemType(path), path);
        }

        private ParsableItem.ItemType GetParsableItemType(string path)
        {
            var pathLower = path.ToLower();
            if (pathLower.EndsWith("sln"))
            {
                return ParsableItem.ItemType.Solution;
            }
            if (pathLower.EndsWith("csproj"))
            {
                return ParsableItem.ItemType.MsBuildProject;
            }
            if (pathLower.EndsWith("package.json"))
            {
                return ParsableItem.ItemType.NpmProject;
            }
                
            return ParsableItem.ItemType.Unknown;
        }

        private bool ShouldSearchInSubdirectories(ParsableItem parsableItem)
        {
            return parsableItem.Type != ParsableItem.ItemType.NpmProject;
        }
    }

}
