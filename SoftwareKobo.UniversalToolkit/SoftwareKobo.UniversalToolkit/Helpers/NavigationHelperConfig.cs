namespace SoftwareKobo.UniversalToolkit.Helpers
{
    public class NavigationHelperConfig
    {
        static NavigationHelperConfig()
        {
            IsDefaultNavigateBackByHardwareBackButton = true;
            IsDefaultNavigateBackByMouseXButton1 = true;
            IsDefaultNavigateBackBySlideToRight = true;
            IsDefaultNavigateBackBySystemBackButton = true;
            IsDefaultNavigateForwardByMouseXButton2 = true;
            IsDefaultNavigateForwardBySlideToLeft = true;
        }

        public NavigationHelperConfig()
        {
            IsNavigateBackByHardwareBackButton = IsDefaultNavigateBackByHardwareBackButton;
            IsNavigateBackByMouseXButton1 = IsDefaultNavigateBackByMouseXButton1;
            IsNavigateBackBySlideToRight = IsDefaultNavigateBackBySlideToRight;
            IsNavigateBackBySystemBackButton = IsDefaultNavigateBackBySystemBackButton;
            IsNavigateForwardByMouseXButton2 = IsDefaultNavigateForwardByMouseXButton2;
            IsNavigateForwardBySlideToLeft = IsDefaultNavigateForwardBySlideToLeft;
        }

        public static bool IsDefaultNavigateBackByHardwareBackButton
        {
            get;
            set;
        }

        public static bool IsDefaultNavigateBackByMouseXButton1
        {
            get;
            set;
        }

        public static bool IsDefaultNavigateBackBySlideToRight
        {
            get;
            set;
        }

        public static bool IsDefaultNavigateBackBySystemBackButton
        {
            get;
            set;
        }

        public static bool IsDefaultNavigateForwardByMouseXButton2
        {
            get;
            set;
        }

        public static bool IsDefaultNavigateForwardBySlideToLeft
        {
            get;
            set;
        }

        public bool IsNavigateBackByHardwareBackButton
        {
            get;
            set;
        }

        public bool IsNavigateBackByMouseXButton1
        {
            get;
            set;
        }

        public bool IsNavigateBackBySlideToRight
        {
            get;
            set;
        }

        public bool IsNavigateBackBySystemBackButton
        {
            get;
            set;
        }

        public bool IsNavigateForwardByMouseXButton2
        {
            get;
            set;
        }

        public bool IsNavigateForwardBySlideToLeft
        {
            get;
            set;
        }
    }
}