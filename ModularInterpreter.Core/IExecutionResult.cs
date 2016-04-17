using System.Collections.Generic;

namespace ModularInterpreter.Core
{
	public interface IExecutionResult
	{
		bool IsSuccess { get; }
		IEnumerable<string> Errors { get; set; }
	}

	public class ExecutionResult : IExecutionResult
	{
		public ExecutionResult(bool isSuccess)
		{
			IsSuccess = isSuccess;
		}

		public bool IsSuccess { get; }
		public IEnumerable<string> Errors { get; set; } = new List<string>();
	}
}