using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SoftwareKobo.UniversalToolkit.Behaviors
{
    [TypeConstraint(typeof(ListViewBase))]
    [ContentProperty(Name = nameof(Actions))]
    public sealed class ItemClickBehaviors : DependencyObject, IBehavior
    {
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(nameof(Actions), typeof(ActionCollection), typeof(ItemClickBehaviors), new PropertyMetadata(null));

        public ActionCollection Actions
        {
            get
            {
                ActionCollection actions = (ActionCollection)this.GetValue(ActionsProperty);
                if (actions == null)
                {
                    actions = new ActionCollection();
                    this.SetValue(ActionsProperty, actions);
                }
                return actions;
            }
        }

        public DependencyObject AssociatedObject
        {
            get;
            private set;
        }

        public void Attach(DependencyObject associatedObject)
        {
            this.AssociatedObject = associatedObject;
            ListViewBase view = this.AssociatedObject as ListViewBase;
            if (view != null)
            {
                view.ItemClick += this.OnItemClick;
            }
        }

        public void Detach()
        {
            ListViewBase view = this.AssociatedObject as ListViewBase;
            if (view != null)
            {
                view.ItemClick -= this.OnItemClick;
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            Interaction.ExecuteActions(sender, Actions, e.ClickedItem);
        }
    }
}