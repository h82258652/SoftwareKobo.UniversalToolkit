using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace SoftwareKobo.UniversalToolkit.AwaitableUI
{
    public static class StoryboardExtensions
    {
        public static async Task BeginAsync(this Storyboard storyboard)
        {
            if (storyboard == null)
            {
                throw new ArgumentNullException(nameof(storyboard));
            }

            await EventAsync.FromEvent<object>(eh => storyboard.Completed += eh, eh => storyboard.Completed -= eh, storyboard.Begin);
        }
    }
}