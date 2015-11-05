using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MinimalEF
{
    public static class Cmd
    {
        public static void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public static void Write(string value)
        {
            Console.Write(value);
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static void WriteLine(string line, ConsoleColor foreground)
        {
            Console.ForegroundColor = foreground;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        public static void Write(string line, ConsoleColor foreground)
        {
            Console.ForegroundColor = foreground;
            Console.Write(line);
            Console.ResetColor();
        }

        public static void WriteLine(string line, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        public static void Write(string line, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(line);
            Console.ResetColor();
        }

        public static string Prompt()
        {
            Write("> ");
            return Console.ReadLine();
        }

        public static void Pause()
        {
            Write("Press any key to continue...");
            Console.ReadKey();
        }

        public static string Prompt(string path)
        {
            Write(string.Format("{0}> ", path));
            return Console.ReadLine();
        }

        public static void WriteInfoLine(string line)
        {
            WriteLine(line, ConsoleColor.Cyan);
        }

        public static void WriteSuccessLine(string line)
        {
            WriteLine(line, ConsoleColor.Green);
        }

        public static void WriteWarningLine(string line)
        {
            WriteLine(line, ConsoleColor.Yellow);
        }

        public static void WriteErrorLine(string line)
        {
            WriteLine(line, ConsoleColor.Red);
        }

        public static void WriteException(Exception exception)
        {
            WriteErrorLine(exception.Message);
            WriteLine("More info? (n)");
            var showFullException = Prompt();
            if (showFullException.IsRoughly("y", "yes"))
            {
                WriteLine(exception.ToString());
            }
        }

        public static void WriteHeader(string header)
        {
            WriteInfoLine(new string('=', header.Length));
            WriteInfoLine(header);
            WriteInfoLine(new string('=', header.Length));
        }

        public static void WriteSubheader(string header)
        {
            WriteInfoLine(header);
            WriteInfoLine(new string('-', header.Length));
        }

        public static void Dump(IEnumerable<object> dumpValues)
        {
            var values = dumpValues.Select(x => x.ToDictionary()).ToArray();

            if (!values.Any())
            {
                WriteSubheader("No values");
                return;
            }

            WriteSubheader($"{values.Count()} values");

            var columns = values.First().Select(x => new
            {
                Title = x.Key,
                MaxWidth = Math.Max(values.Select(v => v[x.Key].ToString().Length).Max(), x.Key.Length)
            }).ToArray();
            var header = columns.Aggregate(string.Empty, (accum, x) => accum + x.Title.PadRight(x.MaxWidth + 1));
            WriteSubheader(header);

            foreach (var row in values)
            {
                var line = row.Aggregate(string.Empty, (accum, x) =>
                {
                    var length = columns.Single(c => c.Title == x.Key).MaxWidth;
                    return accum + x.Value.ToString().PadRight(length + 1);
                });
                WriteLine(line);
            }
        }

        public static void Dump(object value)
        {
            foreach (var item in value.ToDictionary())
            {
                WriteLine($"{item.Key}: {item.Value}");
            }
        }

        public static void DumpCollection(string name, IEnumerable<object> values)
        {
            var array = values.ToArray();

            if (!array.Any())
            {
                WriteSubheader($"{name}: No values");
                return;
            }

            WriteSubheader($"{array.Count()} values");
            WriteSubheader(name);
            foreach (var item in array)
            {
                WriteLine(item.ToString());
            }
        }
    }
}