using System;

namespace ModularInterpreter.Core
{
	public abstract class AbstractModularInterpreter
	{
		protected Func<object> InputFunction;
		protected Action<byte> OutputAction;

		protected AbstractModularInterpreter(Action<byte> outputAction) : this(null, outputAction)
		{
		}

		protected AbstractModularInterpreter(Func<object> inputFunction, Action<byte> outputAction)
		{
			InputFunction = inputFunction;
			OutputAction = outputAction;
		}

		public abstract void SetCommand(string commandText);
		public abstract IExecutionResult Execute();
	}
}