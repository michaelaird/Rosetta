/// <summary>
/// Program.ConvertProject.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.Runner
{
    using System;

    using Rosetta.Executable;
    using AST;

    /// <summary>
    /// Part of program responsible for translating one single file.
    /// 
    /// TODO: Remove as a build task can be used instead.
    /// </summary>
    internal partial class Program
    {
        protected IRunner projectConversionRunner;

        protected IRunner ProjectConversionRunner
        {
            get
            {
                if (this.projectConversionRunner == null)
                {
                    this.projectConversionRunner = this.CreateProjectConversionRunner();
                }

                return this.projectConversionRunner;
            }
        }

        protected virtual IRunner CreateProjectConversionRunner()
        {
            return new ProjectConversionRunner(PerformProjectConversion, new ConversionArguments()
            {
                ProjectPath = this.projectPath,
                AssemblyPath = this.assemblyPath,
                OutputDirectory = this.outputFolder,
                Extension = Extension
            });
        }

        protected virtual void ConvertProject()
        {
            this.ProjectConversionRunner.Run();
        }

        protected static string PerformProjectConversion(ConversionArguments arguments)
        {
            var program = new ProgramWrapper(arguments.Source, arguments.AssemblyPath);

            return program.Output;
        }

        #region Helpers

        private static string GetOutputFolderForProject(string userInput)
        {
            if (userInput != null)
            {
                // User provided a path: check the path is all right
                if (FileManager.IsDirectoryPathCorrect(userInput))
                {
                    return userInput;
                }

                // Wrong path
                throw new InvalidOperationException("Invalid path provided!");
            }

            // User did not provide a path, we get the current path
            return FileManager.ApplicationExecutingPath;
        }

        #endregion
    }
}
