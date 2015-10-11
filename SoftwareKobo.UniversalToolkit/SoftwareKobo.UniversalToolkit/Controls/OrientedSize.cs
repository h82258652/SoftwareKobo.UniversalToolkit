using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    internal struct OrientedSize
    {
        private double _direct;

        private double _indirect;

        private Orientation _orientation;

        public OrientedSize(Orientation orientation) : this(orientation, 0.0d, 0.0d)
        {
        }

        public OrientedSize(Orientation orientation, double width, double height)
        {
            _orientation = orientation;

            _direct = 0.0d;
            _indirect = 0.0d;

            Width = width;
            Height = height;
        }

        public double Direct
        {
            get
            {
                return _direct;
            }
            set
            {
                _direct = value;
            }
        }

        public double Height
        {
            get
            {
                if (Orientation != Orientation.Horizontal)
                {
                    return Direct;
                }
                else
                {
                    return Indirect;
                }
            }
            set
            {
                if (Orientation != Orientation.Horizontal)
                {
                    Direct = value;
                }
                else
                {
                    Indirect = value;
                }
            }
        }

        public double Indirect
        {
            get
            {
                return _indirect;
            }
            set
            {
                _indirect = value;
            }
        }

        public Orientation Orientation
        {
            get
            {
                return this._orientation;
            }
        }

        public double Width
        {
            get
            {
                if (Orientation == Orientation.Horizontal)
                {
                    return Direct;
                }
                else
                {
                    return Indirect;
                }
            }
            set
            {
                if (Orientation == Orientation.Horizontal)
                {
                    Direct = value;
                }
                else
                {
                    Indirect = value;
                }
            }
        }
    }
}