using System;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // ------------------------------------
            // PART 2 BENCHMARK
            // ------------------------------------

            if (args.Length == 1 &&
                args[0] == "--benchmark")
            {
                BenchmarkRunner.RunBenchmarks();
                return;
            }

            // ------------------------------------
            // PART 4 TEST FILE GENERATOR
            // ------------------------------------

            if (args.Length == 1 &&
                args[0] == "--generate-tests")
            {
                TestFileGenerator.GenerateTests();
                return;
            }

            // ------------------------------------
            // PART 4 DENSE VS. SPARSE COMPARISON
            // ------------------------------------

            if (args.Length == 1 &&
                args[0] == "--compare")
            {
                ComparisonRunner.RunComparisons();
                return;
            }

            // ------------------------------------
            // NORMAL GAME OF LIFE PROGRAM
            // ------------------------------------

            string inputPath;
            string outputPath;
            bool graphics;

            // If no command-line arguments were supplied,
            // ask the user for the required information.
            if (args.Length == 0)
            {
                Console.Write(
                    "Enter input file path: "
                );

                inputPath =
                    Console.ReadLine() ?? "";

                Console.Write(
                    "Enter output file path: "
                );

                outputPath =
                    Console.ReadLine() ?? "";

                Console.Write(
                    "Enable graphics? (y/n): "
                );

                string graphicsChoice =
                    Console.ReadLine()?
                        .Trim()
                        .ToLower()
                    ?? "n";

                graphics =
                    graphicsChoice == "y";
            }
            else
            {
                inputPath = "";
                outputPath = "";
                graphics = false;

                // Process command-line arguments.
                for (int i = 0;
                     i < args.Length;
                     i++)
                {
                    if (args[i] == "--input")
                    {
                        if (i + 1 >= args.Length)
                        {
                            throw new ArgumentException(
                                "Missing input file path."
                            );
                        }

                        inputPath =
                            args[++i];
                    }

                    else if (
                        args[i] == "--output")
                    {
                        if (i + 1 >= args.Length)
                        {
                            throw new ArgumentException(
                                "Missing output file path."
                            );
                        }

                        outputPath =
                            args[++i];
                    }

                    else if (
                        args[i] == "--graphics")
                    {
                        graphics = true;
                    }

                    else
                    {
                        throw new ArgumentException(
                            $"Unknown argument: {args[i]}"
                        );
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(
                inputPath))
            {
                throw new ArgumentException(
                    "An input file is required."
                );
            }

            if (string.IsNullOrWhiteSpace(
                outputPath))
            {
                throw new ArgumentException(
                    "An output file is required."
                );
            }

            // Read the starting universe.
            InputData data =
                InputFileReader.Read(
                    inputPath
                );

            // Create the baseline dense universe.
            GameOfLife game =
                new GameOfLife(
                    data.Width,
                    data.Height
                );

            // Add all initially living cells.
            foreach (
                (int column, int row)
                in data.LiveCells)
            {
                game.SetAlive(
                    column,
                    row
                );
            }

            // Run the requested generations.
            game.Run(
                data.Steps,
                graphics
            );

            // Save the final universe.
            game.WriteOutput(
                outputPath,
                data.Steps
            );

            Console.WriteLine();

            Console.WriteLine(
                "Simulation complete. " +
                $"Results saved to {outputPath}"
            );
        }
        catch (Exception exception)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"Error: {exception.Message}"
            );
        }
    }
}