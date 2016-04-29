using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SoftwareKobo.UniversalToolkit.Behaviors
{
    [TypeConstraint(typeof(ListViewBase))]
    [ContentProperty(Name = nameof(Actions))]
    public sealed class ItemClickBehavior : DependencyObject, IBehavior
    {
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(nameof(Actions), typeof(ActionCollection), typeof(ItemClickBehavior), new PropertyMetadata(null));

        public ActionCollection Actions
        {
            get
            {
                var actions = (ActionCollection)GetValue(ActionsProperty);
                if (actions == null)
                {
                    actions = new ActionCollection();
                    SetValue(ActionsProperty, actions);
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
            AssociatedObject = associatedObject;
            var view = AssociatedObject as ListViewBase;
            if (view != null)
            {
                view.ItemClick += OnItemClick;
            }
        }

        public void Detach()
        {
            var view = AssociatedObject as ListViewBase;
            if (view != null)
            {
                view.ItemClick -= OnItemClick;
            }
            AssociatedObject = null;
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            Interaction.ExecuteActions(sender, Actions, e);
        }
    }
}