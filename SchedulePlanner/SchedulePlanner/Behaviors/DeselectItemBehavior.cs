using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SchedulePlanner
{
    /// <summary>
    /// Denotes behavior for when items in a ListView are interacted with.
    /// <para>Items that are selected will immedietly be deselected.</para>
    /// </summary>
    /// <remarks>
    /// Will get rid of item highlight that would ordinarily persist.
    /// </remarks>
    public class DeselectItemBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.ItemSelected += ListView_ItemSelected;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.ItemSelected -= ListView_ItemSelected;
        }
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
