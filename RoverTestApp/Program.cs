using System;
using System.Collections.Generic;

namespace RoverTestApp
{
    public static class PlateauGrid
    {
        public static int leftCordinate { get; set; }
        public static int rightCordinate { get; set; }
    }

    public static class Rover
    {
        public static int leftCordinate { get; set; }
        public static int rightCordinate { get; set; }
        public static string DirectionFacing { get; set; }
    }

    /// <summary>
    /// This class is an extension defined to store Tuple<String, int> for holding Direction and Index data
    /// </summary>
    public static class TupleListExtensions
    {
        public static void Add<T1, T2>(this IList<Tuple<T1, T2>> list,
                T1 item1, T2 item2)
        {
            list.Add(Tuple.Create(item1, item2));
        }     
     }

    public class Program
    {
        static string ValidateCommandFormat(string commandArgument)
        {
            string output = "Correct Format";
            string provideCorrectCommands = "Kindly enter correct commands : " + Environment.NewLine + " 1. SetPosition(x, y, direction) "
                    + Environment.NewLine + " 2. Move(commands) " + Environment.NewLine;

            if (string.IsNullOrEmpty(commandArgument))
            {
                return provideCorrectCommands;
            }
            else
            {
                // Get the opening and closing parenthesis and extract entire string before that to find out which command was keyed in
                int firstParenthesisIndex = commandArgument.IndexOf('(');
                int lastParenthesisIndex = commandArgument.IndexOf(')');

                if (firstParenthesisIndex == -1 || lastParenthesisIndex == -1 || lastParenthesisIndex <= firstParenthesisIndex)
                {
                    return "Invalid Command Provided." + Environment.NewLine + provideCorrectCommands;
                }
            }

            return output;
        }

        static string ValidateInputsandProceed(string commandArgument)
        {
            string output = string.Empty;

            output = ValidateCommandFormat(commandArgument);

            if (output.Contains("Correct"))
            {
                // Get the opening and closing parenthesis and extract entire string before that to find out which command was keyed in
                int firstParenthesisIndex = commandArgument.IndexOf('(');
                int lastParenthesisIndex = commandArgument.IndexOf(')');

                // Extract entire string before first parenthesis to find out which command was keyed in
                string command = commandArgument.Substring(0, commandArgument.IndexOf('('));

                // Match with correct input commands
                switch (command.ToLower())
                {
                    case "getcurrentroverposition":
                        output = "Current Position of the Rover :" + Environment.NewLine +
                            "X = " + Rover.leftCordinate.ToString() + " and Y = " + Rover.rightCordinate.ToString() + " and Direction Facing = " + Rover.DirectionFacing + Environment.NewLine;
                        break;

                    case "setplateaugridsize":
                        output = SetPlateauGridSize(commandArgument);
                        break;

                    case "setposition":
                        output = SetPosition(commandArgument);
                        break;

                    case "move":
                        output = Move(commandArgument);
                        break;
                }
            }

            return output;
        }

        /// <summary>
        /// Move command with its input string expected in format 'L2R2...'
        /// </summary>
        /// <param name="commandArgument"></param>
        /// <returns></returns>
        public static string Move(string commandArgument)
        {
            bool isValidInput = true;
            string output = string.Empty;
            string provideCorrectCommands = string.Empty;

            output = ValidateCommandFormat(commandArgument);

            if (output.Contains("Correct"))
            {
                // Get the opening and closing parenthesis and extract entire string before that to find out which command was keyed in
                int firstParenthesisIndex = commandArgument.IndexOf('(');
                int lastParenthesisIndex = commandArgument.IndexOf(')');

                // Extract entire string before first parenthesis to find out which command was keyed in
                string command = commandArgument.Substring(0, commandArgument.IndexOf('('));

                // Validate if Move is selected Command
                if (string.IsNullOrEmpty(command) || command.ToLower() != "move")
                {
                    return "Invalid Command Provided.";
                }

                // Get the closing parenthesis and extract entire string between opening and closing parenthesis to find out which command was keyed in
                string argumentList = commandArgument.Substring(firstParenthesisIndex + 1, (lastParenthesisIndex - firstParenthesisIndex) - 1);
                string[] argumentArray = argumentList.Split(',');

                if (string.IsNullOrEmpty(argumentList))
                {
                    isValidInput = false;
                }
                else
                {
                    // Validate if the string is of pattern L1R2....
                    char[] movementArray = argumentList.ToCharArray();

                    /* Logic 1: Extract the "L"s and "R"s from the command string in a tuple with its corresponding indices, would work for patterns like L202R1234... */
                    var directionIndexList = new List<Tuple<string, int>> { };

                    int counterCharacter = 0;
                    foreach (char inputChar in movementArray)
                    {
                        if (!string.IsNullOrEmpty(inputChar.ToString()) && inputChar.ToString().ToUpper() == "L" || inputChar.ToString().ToUpper() == "R")
                        {
                            directionIndexList.Add(inputChar.ToString().ToUpper(), counterCharacter);
                        }

                        counterCharacter++;
                    }

                    if (directionIndexList.Count == 0)
                    {
                        isValidInput = false;
                    }
                    else
                    {
                        var directionStepList_New = new List<Tuple<string, int>> { };

                        counterCharacter = 0;
                        var previousCharacter = string.Empty;
                        var previousIndex = 0;
                        int outputIndex = 0;

                        foreach (var directionIndex in directionIndexList)
                        {
                            if (counterCharacter == 0)
                            {
                                previousCharacter = directionIndex.Item1;
                                previousIndex = 0;
                            }
                            else
                            {
                                if (int.TryParse(argumentList.Substring(previousIndex + 1, directionIndex.Item2 - previousIndex - 1), out outputIndex))
                                {
                                    directionStepList_New.Add(previousCharacter, outputIndex);
                                    previousCharacter = directionIndex.Item1;
                                    previousIndex = directionIndex.Item2;
                                }
                                else
                                {
                                    isValidInput = false;
                                    break;
                                }
                            }

                            counterCharacter++;
                        }

                        if (isValidInput && int.TryParse(argumentList.Substring((previousIndex + 1), argumentList.Length - previousIndex - 1), out outputIndex))
                        {
                            directionStepList_New.Add(previousCharacter, outputIndex);
                        }
                        else
                        {
                            isValidInput = false;
                        }

                        if (isValidInput)
                        {
                            // algorithm to calculate final direction
                            foreach (var direction in directionStepList_New)
                            {
                                string newDirection = direction.Item1;
                                int stepsToMove = direction.Item2;

                                string newFinalDirection = string.Empty;

                                // Add steps to left co-ordinate or right coordinate based on current and new direction

                                // Fetch new direction based on old direction
                                switch (Rover.DirectionFacing)
                                {
                                    case "N":

                                        if (newDirection == "L")
                                            newFinalDirection = "W";
                                        else
                                            newFinalDirection = "E";

                                        break;

                                    case "E":

                                        if (newDirection == "L")
                                            newFinalDirection = "N";
                                        else
                                            newFinalDirection = "S";

                                        break;

                                    case "W":

                                        if (newDirection == "L")
                                            newFinalDirection = "S";
                                        else
                                            newFinalDirection = "N";
                                        break;

                                    case "S":

                                        if (newDirection == "L")
                                            newFinalDirection = "E";
                                        else
                                            newFinalDirection = "W";
                                        break;

                                    default:
                                        break;
                                }

                                int finalCoordinate = 0;

                                switch (newFinalDirection)
                                {
                                    case "N":

                                        // Check if the new position goes beyond plateau dimension
                                        finalCoordinate = Rover.rightCordinate + stepsToMove;

                                        if (finalCoordinate > PlateauGrid.rightCordinate)
                                        {
                                            isValidInput = false;
                                            provideCorrectCommands = "Rover goes beyond the plateau dimension."
                                                + Environment.NewLine + provideCorrectCommands;
                                        }
                                        else
                                        {
                                            Rover.rightCordinate += stepsToMove;
                                            Rover.DirectionFacing = newFinalDirection;
                                        }

                                        break;

                                    case "S":

                                        // Check if the new position goes beyond plateau dimension
                                        finalCoordinate = Rover.rightCordinate - stepsToMove;

                                        if (finalCoordinate < 0)
                                        {
                                            isValidInput = false;
                                            provideCorrectCommands = "Rover goes beyond the plateau dimension."
                                                + Environment.NewLine + provideCorrectCommands;
                                        }
                                        else
                                        {
                                            Rover.rightCordinate -= stepsToMove;
                                            Rover.DirectionFacing = newFinalDirection;
                                        }

                                        break;

                                    case "E":

                                        // Check if the new position goes beyond plateau dimension
                                        finalCoordinate = Rover.leftCordinate + stepsToMove;

                                        if (finalCoordinate > PlateauGrid.leftCordinate)
                                        {
                                            isValidInput = false;
                                            provideCorrectCommands = "Rover goes beyond the plateau dimension."
                                                + Environment.NewLine + provideCorrectCommands;
                                        }
                                        else
                                        {
                                            Rover.leftCordinate += stepsToMove;
                                            Rover.DirectionFacing = newFinalDirection;
                                        }

                                        break;

                                    case "W":

                                        // Check if the new position goes beyond plateau dimension
                                        finalCoordinate = Rover.leftCordinate - stepsToMove;

                                        if (finalCoordinate < 0)
                                        {
                                            isValidInput = false;
                                            provideCorrectCommands = "Rover goes beyond the plateau dimension."
                                                + Environment.NewLine + provideCorrectCommands;
                                        }
                                        else
                                        {
                                            Rover.leftCordinate -= stepsToMove;
                                            Rover.DirectionFacing = newFinalDirection;
                                        }

                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                if (isValidInput)
                    output = "Movement done. Final location of Rover now: " + Environment.NewLine +
                        "[ " + Rover.leftCordinate.ToString() + ", " + Rover.rightCordinate.ToString() + ", " + Rover.DirectionFacing + " ]" + Environment.NewLine;
                else
                    output = "Invalid Data Provided." + Environment.NewLine + provideCorrectCommands;

            }

            return output;
        }

        /// <summary>
        /// SetPosition command to set initial position of Rover
        /// </summary>
        /// <param name="commandArgument"></param>
        /// <returns></returns>
        public static string SetPosition(string commandArgument)
        {
            bool isValidInput = true;
            string output = string.Empty;

            output = ValidateCommandFormat(commandArgument);

            if (output.Contains("Correct"))
            {
                // Get the opening and closing parenthesis and extract entire string before that to find out which command was keyed in
                int firstParenthesisIndex = commandArgument.IndexOf('(');
                int lastParenthesisIndex = commandArgument.IndexOf(')');

                // Extract entire string before first parenthesis to find out which command was keyed in
                string command = commandArgument.Substring(0, commandArgument.IndexOf('('));

                // Validate if SetPosition is selected Command
                if (string.IsNullOrEmpty(command) || command.ToLower() != "setposition")
                {
                    return "Invalid Command Provided.";
                }

                // Get the closing parenthesis and extract entire string between opening and closing parenthesis to find out which command was keyed in
                string argumentList = commandArgument.Substring(firstParenthesisIndex + 1, (lastParenthesisIndex - firstParenthesisIndex) - 1);
                string[] argumentArray = argumentList.Split(',');

                if (argumentArray.Length < 3) // Less than 3 arguments provided
                {
                    isValidInput = false;
                }
                else
                {
                    // Check if X coordinate in not an integrer or not in range of 1 to 10000
                    if (int.TryParse(argumentArray[0], out int xCoordinate) &&
                        xCoordinate >= 1 && xCoordinate <= int.MaxValue)
                        Rover.leftCordinate = xCoordinate;
                    else
                        isValidInput = false;

                    // Check if Y coordinate in not an integrer or not in range of 1 to 10000
                    if (int.TryParse(argumentArray[1], out int yCoordinate) &&
                        yCoordinate >= 1 && yCoordinate <= int.MaxValue)
                        Rover.rightCordinate = yCoordinate;
                    else
                        isValidInput = false;

                    // Validate the direction if its one among "N", "E", "W", "S"
                    if (!string.IsNullOrEmpty(argumentArray[2]) &&
                            (argumentArray[2].ToUpper() == "N" ||
                            argumentArray[2].ToUpper() == "S" ||
                            argumentArray[2].ToUpper() == "E" ||
                            argumentArray[2].ToUpper() == "W")
                            )
                        Rover.DirectionFacing = argumentArray[2].ToUpper();
                    else
                        isValidInput = false;
                }

                if (isValidInput)
                    output = "Initial Position for Rover set." + Environment.NewLine +
                        "X = " + Rover.leftCordinate.ToString() + " and Y = " + Rover.rightCordinate.ToString() + " and Direction Facing = " + Rover.DirectionFacing + Environment.NewLine;
                else
                    output = "Invalid Data Provided.";

            }

            return output;
        }

        /// <summary>
        /// Sets the initial dimension for the plateau
        /// </summary>
        /// <param name="commandArgument"></param>
        /// <returns></returns>
        public static string SetPlateauGridSize(string commandArgument)
        {
            bool isValidInput = true;
            string output = string.Empty;

            output = ValidateCommandFormat(commandArgument);

            if (output.Contains("Correct"))
            {
                // Extract left, right coordinates and Validate them, they should be within 1 to 10000
                // Extract argumentList from command

                // Get the opening and closing parenthesis and extract entire string before that to find out which command was keyed in
                int firstParenthesisIndex = commandArgument.IndexOf('(');
                int lastParenthesisIndex = commandArgument.IndexOf(')');

                // Extract entire string before first parenthesis to find out which command was keyed in
                string command = commandArgument.Substring(0, commandArgument.IndexOf('('));

                // Validate if SetPosition is selected Command
                if (string.IsNullOrEmpty(command) || command.ToLower() != "setplateaugridsize")
                {
                    return "Invalid Command Provided.";
                }

                // Get the closing parenthesis and extract entire string between opening and closing parenthesis to find out which command was keyed in
                string argumentList = commandArgument.Substring(firstParenthesisIndex + 1, (lastParenthesisIndex - firstParenthesisIndex) - 1);
                string[] argumentArray = argumentList.Split(',');

                if (argumentArray.Length < 2) // Less than 2 arguments provided
                {
                    isValidInput = false;
                }
                else
                {
                    // Check if X coordinate in not an integrer or not in range of 1 to 10000
                    if (int.TryParse(argumentArray[0], out int xCoordinate) && xCoordinate >= 1 && xCoordinate <= int.MaxValue)
                        PlateauGrid.leftCordinate = xCoordinate;
                    else
                        isValidInput = false;

                    // Check if Y coordinate in not an integrer or not in range of 1 to 10000
                    if (int.TryParse(argumentArray[1], out int yCoordinate) && yCoordinate >= 1 && yCoordinate <= int.MaxValue)
                        PlateauGrid.rightCordinate = yCoordinate;
                    else
                        isValidInput = false;
                }

                if (isValidInput)
                    output = "Initial size for Plateau set. X = " + PlateauGrid.leftCordinate.ToString() + " and Y = " + PlateauGrid.rightCordinate.ToString() + Environment.NewLine;
                else
                    output = "Invalid Data Provided.";

            }
            
            return output;
        }

        /// <summary>
        /// Setting default value for Rover and Plateau
        /// Can be changed / overridden when running the console app
        /// </summary>
        public static void InitializeRoverAndPlateau()
        {
            // Default dimensions of PlateauGrid
            PlateauGrid.leftCordinate = 100;
            PlateauGrid.rightCordinate = 100;

            // Default Rover Position Size
            Rover.DirectionFacing = "N";
            Rover.leftCordinate = 10;
            Rover.rightCordinate = 10;
        }

        public static void Main(string[] args)
        {
            InitializeRoverAndPlateau();

            Console.WriteLine("Welcome to the Rover Test Program. Kindly enter following commands : " + Environment.NewLine + Environment.NewLine
                + " Command 1 : SetPlateauGridSize(X, Y)" + Environment.NewLine
                + "\t" + "[Sets the X and Y coordinates of the plateau grid [X and Y should be positive]"
                + Environment.NewLine
                + "\t" + "[Default size set for Plateau : 100 X 100 ]" + Environment.NewLine
                + " Command 2 : SetPosition(X, Y, direction) " + Environment.NewLine 
                + "\t" + "[Deploys the Rover to an initial grid location [X Y], where direction is the initial compass direction]"
                + Environment.NewLine 
                + "\t" + "[Default Rover coordinates set : X = 10, Y = 10, Direction Facing = N]" + Environment.NewLine 
                + " Command 3 : Move(commands) " + Environment.NewLine
                + "\t" + "[Moves the Rover by accepting a command string in the form 'L1R2'] " + Environment.NewLine

                + " Command 4 : GetCurrentRoverPosition() " + Environment.NewLine
                + "\t" + "[Gives the current X, Y , direction for the Rover] " + Environment.NewLine
                + Environment.NewLine
                + "Type 'exit' to leave the program");
            
            var inputCommand = Console.ReadLine();

            while (inputCommand.Trim().ToLower() != "exit")
            {
                string validationOutput = ValidateInputsandProceed(inputCommand);
                Console.WriteLine(validationOutput);
                inputCommand = Console.ReadLine();
            }

            Console.WriteLine("Exiting the Rover Test Program. Thanks for testing !!");
            Console.ReadLine();
        }
    }
}
