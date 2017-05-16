﻿using System.Collections.Generic;
using Cake.Core.IO;

namespace Bake.API.ScriptTaskFactory
{
    public class SolutionInfo
    {
        public FilePath SolutionFilePath { get; }

        public bool BuildSolution { get; }

        public IEnumerable<ProjectInfo> Projects { get; }

        public SolutionInfo(FilePath solutionFilePath, bool buildSolution, IEnumerable<ProjectInfo> projects)
        {
            this.SolutionFilePath = solutionFilePath;
            this.BuildSolution = buildSolution;
            this.Projects = projects;
        }

    }
}