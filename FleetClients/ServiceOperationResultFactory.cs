using BaseClients;
using FleetClients.FleetManagerServiceReference;
using System;

namespace FleetClients
{
	internal static class ServiceOperationResultFactory
	{
		public static ServiceOperationResult FromFleetManagerServiceCallData(ServiceCallData serviceCallData)
		{
			Exception serviceException = string.IsNullOrEmpty(serviceCallData.Message) ? null : new Exception(serviceCallData.Message);

			return new ServiceOperationResult
				(
					(uint)serviceCallData.ServiceCode,
					serviceCallData.ServiceCode.ToString(),
					serviceException,
					null
				);
		}
	}
}