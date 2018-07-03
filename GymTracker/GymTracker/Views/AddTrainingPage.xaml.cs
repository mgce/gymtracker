using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddTrainingPage : PopupPage
	{
		public AddTrainingPage ()
		{
			InitializeComponent ();
		}
	}
}