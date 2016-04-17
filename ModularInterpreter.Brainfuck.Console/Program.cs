using System;
using ModularInterpreter.Core;

namespace ModularInterpreter.Brainfuck.Console
{
	public class Program
	{
		public static void Main()
		{
			while (true)
			{
				AbstractModularInterpreter interpreter = new BrainfuckInterpreter(ReadFunction, WriteAction);
				System.Console.WriteLine("Enter your command");
				interpreter.SetCommand(System.Console.ReadLine());
				var result = interpreter.Execute();
				if(!result.IsSuccess)
					System.Console.WriteLine(string.Join(Environment.NewLine, result.Errors));
				break;
			}

			System.Console.ReadLine();
		}

		private static void WriteAction(byte c)
		{
			System.Console.Write((char)c);
		}

		public static object ReadFunction()
		{
			return System.Console.ReadLine();
		}
	}
}
