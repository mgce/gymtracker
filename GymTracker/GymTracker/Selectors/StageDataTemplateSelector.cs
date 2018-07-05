using GymTracker.Views.MainPageItems;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GymTracker.Selectors
{
    public class StageDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var mytemplate = new DataTemplate(typeof(StagePageViewCell));
            mytemplate.SetValue(StagePageViewCell.ParentBindingContextProperty, container.BindingContext);

            return mytemplate;
        }
    }
}
