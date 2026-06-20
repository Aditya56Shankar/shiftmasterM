namespace Services.Implementation.Exceptions
{
	public class InvalidWorkflowStateException : Exception
	{
		public InvalidWorkflowStateException(string message) : base(message)
		{
		}
	}
}