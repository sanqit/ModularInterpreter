using ModularInterpreter.Core;
using System;

namespace ModularInterpreter.Brainfuck.Console
{
	public class Program
	{
		public static void Main()
		{
			do
			{
				AbstractModularInterpreter interpreter = new BrainfuckInterpreter(ReadFunction, WriteAction);
				System.Console.WriteLine("Enter your command");
				interpreter.SetCommand(System.Console.ReadLine());
				var result = interpreter.Execute();
				if (!result.IsSuccess)
					System.Console.WriteLine(string.Join(Environment.NewLine, result.Errors));
				System.Console.WriteLine("\r\nPress e for exit or enter to continue");
			} while (System.Console.ReadKey().Key != ConsoleKey.E);
		}

		private static void WriteAction(byte c)
		{
			System.Console.Write((char)c);
		}

		public static object ReadFunction()
		{
			System.Console.WriteLine("Write line please");
			return System.Console.ReadLine();
		}
	}
}
