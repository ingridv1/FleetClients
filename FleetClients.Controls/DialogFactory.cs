using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients.Controls
{
	public static class DialogFactory
	{
		public static OpenFileDialog GetOpenJsonDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog.Title = ("Open .txt file");
			openFileDialog.Multiselect = false;

			return openFileDialog;
		}

		public static SaveFileDialog GetSaveJsonDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
			saveFileDialog.Title = ("Save .txt file");

			return saveFileDialog;
		}

	}
}
