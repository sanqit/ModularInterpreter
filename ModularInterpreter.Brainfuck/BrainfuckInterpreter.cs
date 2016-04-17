using ModularInterpreter.Core;
using System;
using System.Collections.Generic;

namespace ModularInterpreter.Brainfuck
{
	public class BrainfuckInterpreter : AbstractModularInterpreter
	{
		private int _readBytes = 0;
		private readonly byte[] _memory;
		private byte[] _commands;
		private int _commandPointer;
		private int _memoryPointer;
		private List<byte> _outputBytes;
		private string _input;

		public BrainfuckInterpreter(Func<object> inputFunction, Action<byte> outputAction, int countMemories = 3000) : base(inputFunction, outputAction)
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
			_outputBytes = new List<byte>();
			_memoryPointer = 0;
			_commandPointer = 0;

			_commands = new byte[commandText.Length * 2];
			Buffer.BlockCopy(commandText.ToCharArray(), 0, _commands, 0, _commands.Length);
		}

		public override IExecutionResult Execute()
		{
			try
			{
				ClearMemory();
				var brc = 0;
				while (_commandPointer < _commands.Length)
				{
					switch ((char)_commands[_commandPointer])
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
				b = (byte)((_input ?? (_input = InputFunction.Invoke().ToString()))[_readBytes]);
				_readBytes++;
			}
			else
			{
				b = (byte)(_input[_readBytes]);
				_readBytes++;
			}
		}
	}
}