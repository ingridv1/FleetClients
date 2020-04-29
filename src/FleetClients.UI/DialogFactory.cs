using Microsoft.Win32;

namespace FleetClients.UI
{
	public static class DialogFactory
	{
		public static OpenFileDialog GetOpenJsonDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
				Title = ("Open .txt file"),
				Multiselect = false
			};

			return openFileDialog;
		}

		public static SaveFileDialog GetSaveJsonDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
				Title = ("Save .txt file")
			};

			return saveFileDialog;
		}
	}
}