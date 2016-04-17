using ModularInterpreter.Brainfuck;
using ModularInterpreter.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ModularInterpreter.Tests
{
	[TestFixture]
	public class BrainfuckTest
	{
		[Test]
		[TestCaseSource(nameof(GetWordsTestData))]
		public void PrintWords(string command, string stringOutput, Func<object> inputFunc)
		{
			var result = string.Empty;
			Action<byte> outputAction = x =>
			{
				result += (char)x;
			};

			AbstractModularInterpreter interpreter = new BrainfuckInterpreter(inputFunc, outputAction);
			interpreter.SetCommand(command);
			interpreter.Execute();
			Assert.AreEqual(result, stringOutput);
		}

		public static IEnumerable<TestCaseData> GetWordsTestData()
		{
			yield return new TestCaseData("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++." +
										  "+++++++++++++++++++++++++++++." +
										  "+++++++." +
										  "." +
										  "+++." +
										  "-------------------------------------------------------------------." +
										  "------------." +
										  "+++++++++++++++++++++++++++++++++++++++++++++++++++++++." +
										  "++++++++++++++++++++++++." +
										  "+++." +
										  "------." +
										  "--------." +
										  "-------------------------------------------------------------------.", "Hello, World!", null);

			yield return new TestCaseData("++++++[>++++++++++++<-]>." +
										  ">++++++++++[>++++++++++<-]>+." +
										  "+++++++." +
										  "." +
										  "+++." +
										  ">++++[>+++++++++++<-]>." +
										  "<+++[>----<-]>." +
										  "<<<<<+++[>+++++<-]>." +
										  ">>." +
										  "+++." +
										  "------." +
										  "--------." +
										  ">>+.", "Hello, World!", null);

			yield return new TestCaseData("++++++++++>>>>>++++++++++<<<<<[>+++++++>++++++++++>+++>+>[-]++++++++++<<<<<-]>++." +
										  ">+." +
										  "+++++++." +
										  "." +
										  "+++." +
										  ">++." +
										  "<<+++++++++++++++." +
										  ">." +
										  "+++." +
										  "------." +
										  "--------." +
										  ">+.", "Hello World!", null);

			yield return new TestCaseData(",.", "#", (Func<object>)(() => '#'));
			yield return new TestCaseData(",.", "#", (Func<object>)(() => "#"));
			yield return new TestCaseData(",.,.,.,.,.", "Hello", (Func<object>)(() => "Hello"));
		}
	}
}