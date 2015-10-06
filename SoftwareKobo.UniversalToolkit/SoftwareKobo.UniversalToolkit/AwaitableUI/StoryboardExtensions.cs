using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace SoftwareKobo.UniversalToolkit.AwaitableUI
{
    public static class StoryboardExtensions
    {
        public static Task BeginAsync(this Storyboard storyboard)
        {
            if (storyboard == null)
            {
                throw new ArgumentNullException(nameof(storyboard));
            }

            return EventAsync.FromEvent<object>(handler => storyboard.Completed += handler, handler => storyboard.Completed -= handler, storyboard.Begin);
        }
    }
}