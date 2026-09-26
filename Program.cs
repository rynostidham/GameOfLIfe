using System;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            string inputPath;
            string outputPath;
            bool graphics;

            if (args.Length == 0)
            {
                // Prompts the user for input and output file paths and graphics option
                Console.Write("Enter input file path: ");
                inputPath = Console.ReadLine() ?? "";

                Console.Write("Enter output file path: ");
                outputPath = Console.ReadLine() ?? "";

                Console.Write("Enable graphics? (y/n): ");

                string graphicsChoice =
                    Console.ReadLine()?.Trim().ToLower() ?? "n";

                graphics = graphicsChoice == "y";
            }
            else
            {
                inputPath = "";
                outputPath = "";
                graphics = false;

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--input")
                    {
                        if (i + 1 >= args.Length)
                        {
                            throw new ArgumentException(
                                "Missing input file path."
                            );
                        }
                        // Moves to next argument and stores the input file path
                        inputPath = args[++i];
                    }
                    else if (args[i] == "--output")
                    {
                        if (i + 1 >= args.Length)
                        {
                            throw new ArgumentException(
                                "Missing output file path."
                            );
                        }

                        outputPath = args[++i];
                    }
                    else if (args[i] == "--graphics")
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

            if (string.IsNullOrWhiteSpace(inputPath))
            {
                throw new ArgumentException(
                    "An input file is required."
                );
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException(
                    "An output file is required."
                );
            }

            // Reads the users input file and returns data 
            InputData data = InputFileReader.Read(inputPath);

            // Create the game
            GameOfLife game =
                new GameOfLife(data.Width, data.Height);

            // Places the living cells
            foreach ((int column, int row) in data.LiveCells)
            {
                game.SetAlive(column, row);
            }

            game.Run(data.Steps, graphics);

            // Saves the results to the output file
            game.WriteOutput(outputPath, data.Steps);

            Console.WriteLine();
            Console.WriteLine(
                $"Simulation complete. Results saved to {outputPath}"
            );
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine($"Error: {exception.Message}");
        }
    }
}