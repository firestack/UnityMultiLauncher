using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UnityMultiLauncher.ViewModels.Utils
{
	abstract public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void UpdateProperty(string Name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
		}

		public void UpdateEverything()
		{
			foreach (var kv in propertyValues)
			{
				UpdateProperty(kv.Key);
			}
		}


		protected Dictionary<string, object> propertyValues = new Dictionary<string, object>();

		protected T SetProperty<T>(T value, [CallerMemberName] string property = null)
		{
			this.propertyValues[property] = value;
			UpdateProperty(property);
			return value;
		}

		protected object GetProperty([CallerMemberName] string property = null)
		{
			try
			{
				return this.propertyValues[property];
			}
			catch
			{
				return null;
			}
		}
	}
}
