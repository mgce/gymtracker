using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymTracker.Views.MainPageItems
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPageViewCell : ViewCell
	{
	    public static BindableProperty ParentBindingContextProperty = BindableProperty.Create(nameof(ParentBindingContext), typeof(object), typeof(MainPageViewCell), null);

	    public object ParentBindingContext
	    {
	        get { return GetValue(ParentBindingContextProperty); }
	        set { SetValue(ParentBindingContextProperty, value); }
	    }

        public MainPageViewCell ()
		{
			InitializeComponent ();
		}
	}
}