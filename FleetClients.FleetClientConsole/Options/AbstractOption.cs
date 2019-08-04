using BaseClients;
using System;

namespace FleetClients.FleetClientConsole.Options
{
	public abstract class AbstractOption<T> where T : IClient
	{
		public ServiceOperationResult ExecuteOption(T client)
		{
			ServiceOperationResult result;

			try
			{
				result = HandleExecution(client);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				result =  ServiceOperationResult.FromClientException(ex);
			}

			if (!result.IsSuccessfull) Console.WriteLine(result.ToString());

			return result;
		}

		protected abstract ServiceOperationResult HandleExecution(T client);
	}
}