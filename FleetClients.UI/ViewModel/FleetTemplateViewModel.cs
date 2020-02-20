using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetClients.UI.Message;
using GACore;
using GACore.Architecture;
using GACore.Utility;

namespace FleetClients.UI.ViewModel
{
	public class FleetTemplateViewModel : AbstractCollectionViewModel<FleetTemplate, AGVTemplateViewModel, AGVTemplate>
	{
		public override AGVTemplateViewModel GetViewModelForModel(AGVTemplate model)
		{
			return ViewModels.FirstOrDefault(e => e.IPV4String == model.IPV4String);
		}

		public FleetTemplateViewModel()
		{
			Messenger.Default.Register<TemplateUpdatedMessage>(this, OnTemplateUpdatedMessageRecieved);
		}
			

		private void OnTemplateUpdatedMessageRecieved(TemplateUpdatedMessage templateUpdatedMessage)
		{
			try
			{
				Model = templateUpdatedMessage.FleetTemplate;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}
	}
}
