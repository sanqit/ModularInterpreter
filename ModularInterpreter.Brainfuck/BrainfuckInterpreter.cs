using ModularInterpreter.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ModularInterpreter.Brainfuck
{
	public class BrainfuckInterpreter : AbstractModularInterpreter
	{
		private static readonly Regex CleanCommandRegex = new Regex("[^-+.,<>\\[\\]]",
			RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Compiled);

		private byte[] _memory;
		private int _commandPointer;
		private char[] _commands;
		private string _commandText;
		private string _input;
		private int _memoryPointer;
		private List<byte> _outputBytes;
		private int _readBytes;

		public BrainfuckInterpreter(Func<object> inputFunction, Action<byte> outputAction, int countMemories = 3)
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
			_outputBytes = new List<byte>();
			_memoryPointer = 0;
			_commandPointer = 0;

			_commands = _commandText.ToCharArray();
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
					switch (_commands[_commandPointer])
					{
						case '>':
							_memoryPointer++;
							break;
						case '<':
							_memoryPointer--;
							break;
						case '+':
							ExtendMemoryIfNeed(ref _memory, ref _memoryPointer);
							_memory[_memoryPointer]++;
							break;
						case '-':
							ExtendMemoryIfNeed(ref _memory, ref _memoryPointer);
							_memory[_memoryPointer]--;
							break;
						case '.':
							ExtendMemoryIfNeed(ref _memory, ref _memoryPointer);
							InvokeOutputAction(_memory[_memoryPointer]);
							break;
						case ',':
							ExtendMemoryIfNeed(ref _memory, ref _memoryPointer);
							InvokeInputFunc(ref _memory[_memoryPointer]);
							break;
						case '[':
							ExtendMemoryIfNeed(ref _memory, ref _memoryPointer);
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
							ExtendMemoryIfNeed(ref _memory, ref _memoryPointer);
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
			catch (Exception ex)
			{
				return new ExecutionResult(false) { Errors = new List<string> { ex.Message } };
			}
		}

		private static void ExtendMemoryIfNeed(ref byte[] bytes, ref int pointer)
		{
			if (pointer >= bytes.Length)
				Array.Resize(ref bytes, pointer + 1);
			else if (pointer < 0)
			{
				Array.Resize(ref bytes, bytes.Length + (-1 * pointer));
				pointer = 0;
			}

		}

		private void InvokeOutputAction(byte b)
		{
			OutputAction?.Invoke(b);
			_outputBytes.Add(b);
		}

		private void InvokeInputFunc(ref byte b)
		{
			if (InputFunction != null)
			{
				if (_input == null)
					_input = InputFunction.Invoke().ToString();
				if (_input.Length <= _readBytes)
					return;
				b = (byte)_input[_readBytes];
				_readBytes++;
			}
			else
			{
				b = (byte)_input[_readBytes];
				_readBytes++;
			}
		}
	}
}