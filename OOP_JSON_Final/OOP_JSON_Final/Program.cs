using OOP_JSON_Final.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OOP_JSON_Final
{
    class Program
    {
        /// <summary>
        /// Entry point for program
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        static void Main(string[] args)
        {
            if (!IsValidInput(args))
                return;
            // stopwatch is used to time how long the operation takes
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // only needed to get the name of the file used
            FileInfo fi = new FileInfo(args[0]);
            string file_name = fi.Name;
            // get the .json text that will be used for the rest of this program
            string json_in = File.ReadAllText(args[0]).Trim();
            // dynamic allows root to be any type we want
            dynamic root = GetBase(json_in[0]);
            // here is where the magic happens. If root is a JSON_Object
            // or a JSON_Array, then this will recursively fill it with
            // whatever data follows in the string. Otherwise we're done
            HandleBase(root, json_in);
            // This determines the weight of the JSON tree, defined as follows:
            /* JSON_Objects have a weight of 1 + the weight of all their values
             * JSON_Arrays have a weight of 1 + the weight of all their values
             * JSON_Bools, JSON_Numbers, and JSON_String have a weight of 1 each
             */
            int weight = root.GetWeight();
            // This next part calculates the number of JSON_Arrays and
            // JSON_Objects in this tree, including the root node
            int arrCount, objCount;
            if (root is JSON_Array)
            {
                arrCount = (root as JSON_Array).ArrayCount();
                objCount = (root as JSON_Array).ObjectCount();
            }
            else if (root is JSON_Object)
            {
                arrCount = (root as JSON_Object).ArrayCount();
                objCount = (root as JSON_Object).ObjectCount();
            }
            else
            {
                arrCount = objCount = 0;
            }
            // all calculations and processing of the .json file are complete
            sw.Stop();
            // Display the weight of the tree
            Console.WriteLine(file_name + " has a weight of " + weight + ".");
            NewLine();
            // Display the array and object counts of the tree
            Console.WriteLine("It contains " + arrCount +
                (arrCount == 1 ? " array" : " arrays") + ", and " + objCount +
                (objCount == 1 ? " object" : " objects") + ".");
            NewLine();
            // Display the time this process took
            Console.WriteLine("This operation took " + sw.ElapsedTicks +
                " ticks to complete, or " + sw.Elapsed.TotalMilliseconds +
                " milliseconds.");
            NewLine();
            NewLine();
            // Ask the user if they would like to see the tree
            Console.WriteLine("Would you like to see the printed form of " +
                "the .json file?");
            string response = Console.ReadLine();
            NewLine();
            // if yes, continue
            if (response[0] == 'y' || response[0] == 'Y')
            {
                // Ask the user how they would like to see the tree
                Console.WriteLine("Would you like it printed to the " +
                    "screen, or to a file?");
                Console.WriteLine("Press (1) for on-screen, or (2) to " +
                    "write to a file.");
                response = Console.ReadLine();
                NewLine();
                // If they want it sent to the screen, do so
                if (response[0] == '1')
                    Console.WriteLine(root);
                // If they want it sent to a file, continue
                else if (response[0] == '2')
                {
                    // Ask the user for the name of the file for the output
                    Console.WriteLine("What file name should it be given?");
                    string fileName = Console.ReadLine();
                    // Write the output to the file, overwriting if the file exists
                    File.WriteAllText(fileName, root.ToString());
                }
                // if they didn't enter 1 or 2, exit the program
                else
                    Console.WriteLine("That was an invalid selection.");
                NewLine();
                NewLine();
            }
            // if they don't want to see the tree skip to this point
            // Exit the program
            Console.WriteLine("Press 'Enter' to close the program.");
            // Let the user decide when to close the display
            Console.ReadLine();
        }

        /// <summary>
        /// Verifies that the input is valid.<para/>
        /// <paramref name="args"/> takes an array of strings.
        /// </summary>
        /// <param name="args">The input</param>
        /// <returns>
        /// <c>True</c> if input is valid, otherwise <c>False</c>
        /// </returns>
        static bool IsValidInput(string[] args)
        {
            if (args.Count() != 1)
            {
                Console.WriteLine("Invalid number of arguments passed.");
                Console.ReadLine();
                return false;
            }
            if (!args[0].EndsWith(".json", true,
                System.Globalization.CultureInfo.CurrentCulture))
            {
                Console.WriteLine("Invalid filename extension.");
                Console.ReadLine();
                return false;
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File does not exist.");
                Console.ReadLine();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Inserts a new line into the <see cref="Console"/> output.
        /// </summary>
        static void NewLine()
        {
            // I know this isn't strictly necessary, but it let me type less
            Console.WriteLine();
        }

        /// <summary>
        /// Gets a base object based off the input character
        /// </summary>
        /// <param name="initial">The <c>char</c> to check</param>
        /// <returns>
        /// Returns an object of type <see cref="JSON_Base"/>
        /// </returns>
        static JSON_Base GetBase(char initial)
        {
            switch (initial)
            {
                case '{':
                    return new JSON_Object();
                case '[':
                    return new JSON_Array();
                case '"':
                    return new JSON_String();
                case 't':
                    return new JSON_Bool() { Value = true };
                case 'T':
                    return new JSON_Bool() { Value = true };
                case 'f':
                    return new JSON_Bool() { Value = false };
                case 'F':
                    return new JSON_Bool() { Value = false };
                case 'n':
                    return new JSON_Bool() { Value = null };
                case 'N':
                    return new JSON_Bool() { Value = null };
                default:
                    return new JSON_Number();
            }
        }

        /// <summary>
        /// Handles a <see cref="JSON_Base"/> object properly
        /// <para/>
        /// <paramref name="node"/> requires an object with a base type of
        /// <seealso cref="JSON_Base"/>
        /// </summary>
        /// <param name="node">The base object to interpret</param>
        /// <param name="json">The json string being parsed</param>
        /// <returns>
        /// The number of characters to trim from <paramref name="json"/>
        /// </returns>
        static int HandleBase(dynamic node, string json)
        {
            string name = node.GetType().Name;
            int i;
            switch (name)
            {
                case nameof(JSON_Object):
                    // Create an empty Dictionary<string, JSON_Base>
                    node.Value = new Dictionary<string, JSON_Base>();
                    i = 1;
                    i += TrimString(ref json, i);
                    // Fill the Dictionary
                    i += FillObject(node, json, 0);
                    break;
                case nameof(JSON_Array):
                    // Create an empty List<JSON_Base>
                    node.Value = new List<JSON_Base>();
                    i = 1;
                    i += TrimString(ref json, i);
                    // Fill the List
                    i += FillArray(node, json, 0);
                    break;
                case nameof(JSON_String):
                    // Figure out where the string ends
                    i = GetStringEndIndex(json, 0);
                    // Set the string to the JSON_String.Value
                    node.Value = json.Substring(1, i - 1);
                    ++i;
                    break;
                case nameof(JSON_Number):
                    // Figure out where the number ends
                    i = GetNumberEndIndex(json, 0);
                    double d;
                    // If it successfully parses, ...
                    if (double.TryParse(json.Substring(0, i), out d))
                    {
                        // Set the number to the JSON_Number.Value
                        node.Value = d;
                        // Set the original string to the JSON_Number.StringValue
                        // This lets us print exactly what was there
                        node.StringValue = json.Substring(0, i);
                    }
                    break;
                default: // JSON_Bool
                    // This was already handled when the JSON_Bool was created
                    i = node.ToString().Length;
                    break;
            }
            return i;
        }

        /// <summary>
        /// Gets the end index of a string within a string
        /// </summary>
        /// <param name="json">The string being parsed</param>
        /// <param name="i">The starting index</param>
        /// <returns>The end index within the outer string</returns>
        static int GetStringEndIndex(string json, int i)
        {
            // Figure out where the (") is that truly signifies
            // the end of the string. If it has an odd number of
            // escape characters (\) in front of it, then the
            // (") is part of the internal string, not the end.
            char temp = json[0];
            if (temp != '\"')
            { }
            int count;
            do
            {
                count = 0;
                i = json.IndexOf('"', i + 1);
                for (int j = i; j > 0 && json[j - 1] == '\\'; --j, ++count) { }
            } while (count % 2 == 1);
            return i;
        }

        /// <summary>
        /// Gets the end index of a number within a string
        /// </summary>
        /// <param name="json">The string being parsed</param>
        /// <param name="i">The starting index</param>
        /// <returns>The end index within the outer string</returns>
        static int GetNumberEndIndex(string json, int i)
        {
            // These are the allowed characters in a .json number
            char[] numerals = { '+', '-', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'e', 'E' };
            while (numerals.Contains(json[++i])) ;
            return i;
        }

        /// <summary>
        /// Trims <c>index</c> characters from the front
        /// of a string, then trims the whitespace, while keeping
        /// track of the number of characters trimmed
        /// </summary>
        /// <param name="json">The string passed by reference</param>
        /// <param name="index">The number of characters to trim</param>
        /// <returns>The total number of characters trimmed</returns>
        static int TrimString(ref string json, int index)
        {
            // Cut the first index characters off, then trim whitespace
            json = json.Substring(index);
            index = 0;
            // I don't use string.Trim() because I need to keep track
            // of how many whitespace characters are removed.
            if (string.IsNullOrEmpty(json))
                return index;
            while (char.IsWhiteSpace(json[index]))
                ++index;
            json = json.Substring(index);
            return index;
            // Phase 2 of this program would be to pass the .json string
            // by reference throughout the program, eliminating the need
            // to keep track of this, allowing me to use string.Trim().
        }

        /// <summary>
        /// Fills the contents of a <seealso cref="JSON_Object.Value"/>
        /// </summary>
        /// <param name="obj">The <seealso cref="JSON_Object"/></param>
        /// <param name="json">The string being parsed</param>
        /// <param name="index">The starting index of <paramref name="json"/></param>
        /// <returns>
        /// The number of characters to trim from <paramref name="json"/>
        /// </returns>
        static int FillObject(JSON_Object obj, string json, int index)
        {
            int i = 0;
            char end_char = json[0];
            if (end_char == '}')
                return 1;
            do
            {
                // The key will always be a string
                string key;
                int key_index = GetStringEndIndex(json, 0);
                i += key_index;
                key = json.Substring(1, key_index - 1);
                ++i; // increment to match next line
                i += TrimString(ref json, key_index + 1);
                ++i; // increment to match next line
                i += TrimString(ref json, 1);
                // The value could be any kind of JSON type
                // This is where the recursion sets in
                dynamic value = GetBase(json[0]);
                index = HandleBase(value, json);
                i += index;
                i += TrimString(ref json, index);
                obj.Value.Add(key, value);
                end_char = json[0];
                ++i; // increment to match next line
                i += TrimString(ref json, 1);
            } while (end_char == ',');
            return i;
        }

        /// <summary>
        /// Fills the contents of a <seealso cref="JSON_Array.Value"/>
        /// </summary>
        /// <param name="arr">The <seealso cref="JSON_Array"/></param>
        /// <param name="json">The string being parsed</param>
        /// <param name="index">The starting index of <paramref name="json"/></param>
        /// <returns>
        /// The number of characters to trim from <paramref name="json"/>
        /// </returns>
        static int FillArray(JSON_Array arr, string json, int index)
        {
            int i = 0;
            char end_char = json[0];
            if (end_char == ']')
                return 1;
            do
            {
                // The value could be any kind of JSON type
                // This is where the recursion sets in
                dynamic value = GetBase(json[0]);
                index = HandleBase(value, json);
                i += index;
                i += TrimString(ref json, index);
                arr.Value.Add(value);
                end_char = json[0];
                ++i; // increment to match next line
                i += TrimString(ref json, 1);
            } while (end_char == ',');
            return i;
        }
    }
}
