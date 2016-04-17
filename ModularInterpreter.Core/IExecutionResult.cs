namespace ModularInterpreter.Core
{
	public interface IExecutionResult
	{
		bool IsSuccess { get; }
	}

	public class ExecutionResult : IExecutionResult
	{
		public ExecutionResult(bool isSuccess)
		{
			IsSuccess = isSuccess;
		}

		public bool IsSuccess { get; }
	}
}