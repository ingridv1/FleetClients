using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using FleetClients.DemoApp.Model;
using GACore;
using GACore.Command;
using Markdig;

namespace FleetClients.DemoApp.ViewModel
{
	public class TutorialViewModel : AbstractViewModel<TutorialModel>
	{
		internal TutorialViewModel()
		{
			HandleLoadCommands();
		}
		public ICommand RequestNavigateCommand { get; set; }

		private void HandleLoadCommands()
		{
			RequestNavigateCommand = new CustomCommand(RequestNavigate, CanRequestNavigate);
		}

		private bool CanRequestNavigate(object obj) => true;

		private void RequestNavigate(object obj)
		{
			try
			{
				string html = Markdown.ToHtml(Properties.Resources.MainWindowDescription);

				WebBrowser webBrowser = (WebBrowser)obj;
				webBrowser.NavigateToString(html);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}
	}
}
