﻿/// <summary>
/// Program.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.Runner
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;

    using Rosetta.AST;

    using Mono.Options;

    /// <summary>
    /// Main program.
    /// </summary>
    /// <remarks>
    /// Members protected for testability.
    /// </remarks>
    internal partial class Program
    {
        protected static Program instance;

        protected string filePath = null;         // File to convert
        protected string projectPath = null;      // Project to convert
        protected string outputFolder = null;     // The output folder path for destination files
        protected string fileName = null;         // The output file name
        protected bool verbose = false;           // Verbosity
        protected bool help = false;              // Show help message

        protected FileManager fileManager;

        public const string FileArgumentName        = "file";
        public const string FileArgumentChar        = "f";
        public const string ProjectArgumentName     = "project";
        public const string ProjectArgumentChar     = "p";
        public const string OutputArgumentName      = "output";
        public const string OutputArgumentChar      = "o";
        public const string FileNameArgumentName    = "filename";
        public const string FileNameArgumentChar    = "n";
        public const string VerboseArgumentName     = "verbose";
        public const string VerboseArgumentChar     = "v";
        public const string HelpArgumentName        = "help";
        public const string HelpArgumentChar        = "h";

        public string FileOption
        {
            get { return string.Format("{0}|{1}=", FileArgumentName, FileArgumentChar); }
        }

        public string ProjectOption
        {
            get { return string.Format("{0}|{1}=", ProjectArgumentName, ProjectArgumentChar); }
        }

        public string OutputOption
        {
            get { return string.Format("{0}|{1}=", OutputArgumentName, OutputArgumentChar); }
        }

        public string FileNameOption
        {
            get { return string.Format("{0}|{1}=", FileNameArgumentName, FileNameArgumentChar); }
        }

        public string VerboseOption
        {
            get { return string.Format("{0}|{1}", VerboseArgumentName, VerboseArgumentChar); }
        }

        public string HelpOption
        {
            get { return string.Format("{0}|{1}", HelpArgumentName, HelpArgumentChar); }
        }

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            instance = new Program(args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="args"></param>
        public Program(string[] args)
        {
            var options = new OptionSet()
            {
                { FileOption, "The path to the C# {FILE} to convert into TypeScript.",
                  value => this.filePath = value },
                { ProjectOption, "The path to the C# {PROJECT} to convert into TypeScript project.",
                  value => this.filePath = value },
                { OutputOption, "The {OUTPUT} folder path where Rosetta will emit all output files.",
                  value => this.outputFolder = value },
                { FileNameOption, "The {FILENAME} to use for output file. Valid only when {FILE} is specified.",
                  value => this.fileName = value },
                { VerboseOption, "Increase debug message {VERBOSE} level.",
                  value => this.verbose = value != null },
                { HelpOption,  "Show this message and exit.",
                  value => this.help = value != null },
            };

            List<string> extra;
            try
            {
                extra = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("An error occurred: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try using option `--help' for more information.");
                return;
            }

            // If user provided no input arguments, show help
            if (args.Length == 0)
            {
                Console.Write("No input provided!");
                this.ShowHelp(options);

                return;
            }

            this.Run(options);
        }

        /// <summary>
        /// Runs the main logic.
        /// </summary>
        /// <param name="options"></param>
        private void Run(OptionSet options)
        {
            // Priority to help
            if (help)
            {
                this.ShowHelp(options);
                return;
            }

            try
            {
                // We start by considering whether the user specified a file to convert
                // TODO: Option for not using any parameter to pass the file path
                if (this.filePath != null)
                {
                    this.ConvertFile();
                    return;
                }

                // Then we evaluate project
                if (this.projectPath != null)
                {
                    this.ConvertProject();
                    return;
                }

                // If we get to here, then basically nothing happens, the user needs to specify options
                Console.WriteLine("No options specified.");
                this.ShowHelp(options);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return;
            }
        }

        protected virtual void HandleError(Exception e)
        {
            Console.WriteLine("An error occurred: {0}!", e.Message);
#if DEBUG
            Console.WriteLine(e.StackTrace);
#endif
        }

        private static string PerformConversion(string source)
        {
            var program = new ProgramWrapper(source);

            return program.Output;
        }

        #region Helpers

        protected virtual void ShowHelp(OptionSet options)
        {
            Console.WriteLine("Usage: Rosetta [OPTIONS]+ message");
            Console.WriteLine("Converts C# files into TypeScript.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        #endregion
    }
}
