using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ModularInterpreter.Core;

namespace ModularInterpreter.Brainfuck
{
	public class BrainfuckInterpreter : AbstractModularInterpreter
	{
		private static readonly Regex CleanCommandRegex = new Regex("[^-+.,<>\\[\\]]",
			RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Compiled);

		private readonly byte[] _memory;
		private int _commandPointer;
		private byte[] _commands;
		private string _commandText;
		private string _input;
		private int _memoryPointer;
		private List<byte> _outputBytes;
		private int _readBytes;

		public BrainfuckInterpreter(Func<object> inputFunction, Action<byte> outputAction, int countMemories = 3000)
			: base(inputFunction, outputAction)
		{
			_memory = new byte[countMemories];
		}

		private void ClearMemory()
		{
			Array.Clear(_memory, 0, _memory.Length);
		}

		public override void SetCommand(string commandText)
		{
			ClearMemory();
			_commandText = CleanCommand(commandText);
			//			_commandText = Regex.Replace(commandText, "[^-+.,<>\\[\\]]", string.Empty);
			_outputBytes = new List<byte>();
			_memoryPointer = 0;
			_commandPointer = 0;

			_commands = new byte[_commandText.Length*2];
			Buffer.BlockCopy(_commandText.ToCharArray(), 0, _commands, 0, _commands.Length);
		}

		private static string CleanCommand(string commandText)
		{
			return CleanCommandRegex.Replace(commandText, string.Empty);
		}

		public override IExecutionResult Execute()
		{
			try
			{
				ClearMemory();
				var brc = 0;
				while (_commandPointer < _commands.Length)
				{
					switch ((char) _commands[_commandPointer])
					{
						case '>':
							_memoryPointer++;
							break;
						case '<':
							_memoryPointer--;
							break;
						case '+':
							_memory[_memoryPointer]++;
							break;
						case '-':
							_memory[_memoryPointer]--;
							break;
						case '.':
							InvokeOutputAction(_memory[_memoryPointer]);
							break;
						case ',':
							InvokeInputFunc(out _memory[_memoryPointer]);
							break;
						case '[':
							if (_memory[_memoryPointer] == 0)
							{
								++brc;
								while (brc != 0)
								{
									++_commandPointer;
									if (_commands[_commandPointer] == '[') ++brc;
									if (_commands[_commandPointer] == ']') --brc;
								}
							}
							break;

						case ']':
							if (_memory[_memoryPointer] != 0)
							{
								if (_commands[_commandPointer] == ']') brc++;
								while (brc != 0)
								{
									--_commandPointer;
									if (_commands[_commandPointer] == '[') brc--;
									if (_commands[_commandPointer] == ']') brc++;
								}
								--_commandPointer;
							}
							break;
					}
					_commandPointer++;
				}

				return new ExecutionResult(true);
			}
			catch (Exception)
			{
				return new ExecutionResult(false);
			}
		}

		private void InvokeOutputAction(byte b)
		{
			OutputAction?.Invoke(b);
			_outputBytes.Add(b);
		}

		private void InvokeInputFunc(out byte b)
		{
			if (InputFunction != null)
			{
				b = (byte) (_input ?? (_input = InputFunction.Invoke().ToString()))[_readBytes];
				_readBytes++;
			}
			else
			{
				b = (byte) _input[_readBytes];
				_readBytes++;
			}
		}
	}
}