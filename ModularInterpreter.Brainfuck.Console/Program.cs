using ModularInterpreter.Core;

namespace ModularInterpreter.Brainfuck.Console
{
	public class Program
	{
		public static void Main()
		{
			var isSuccess = false;
			while (!isSuccess)
			{
				AbstractModularInterpreter interpreter = new BrainfuckInterpreter(ReadFunction, WriteAction);
				System.Console.WriteLine("Enter your command");
				interpreter.SetCommand(System.Console.ReadLine());
				isSuccess = interpreter.Execute().IsSuccess;
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
