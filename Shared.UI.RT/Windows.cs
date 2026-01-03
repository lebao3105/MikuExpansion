#if WINDOWS_RT

using MikuExpansion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Controls;

namespace MikuExpansion.UI
{
    public struct WindowPosition : IEquatable<WindowPosition>
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        #region Implicit operators
        public static implicit operator WindowPosition(Size other)
            => new WindowPosition
            {
                Width = other.Width,
                Height = other.Height
            };

        public static implicit operator WindowPosition(DoublePoint other)
            => new WindowPosition
            {
                X = other.X,
                Y = other.Y
            };
        #endregion

        #region operator- and operator+ overloads
        public static WindowPosition operator -(WindowPosition target, WindowPosition other)
        {
            target.Width -= other.Width;
            target.Height -= other.Height;
            target.X -= other.X;
            target.Y -= other.Y;

            return target;
        }

        public static WindowPosition operator +(WindowPosition target, WindowPosition other)
        {
            target.Width += other.Width;
            target.Height += other.Height;
            target.X += other.X;
            target.Y += other.Y;

            return target;
        }

        public static WindowPosition operator -(WindowPosition target, Size other)
        {
            target.Width -= other.Width;
            target.Height -= other.Height;

            return target;
        }

        public static WindowPosition operator +(WindowPosition target, Size other)
        {
            target.Width += other.Width;
            target.Height += other.Height;

            return target;
        }

        public static WindowPosition operator -(WindowPosition target, DoublePoint other)
        {
            target.X -= other.X;
            target.Y -= other.Y;

            return target;
        }

        public static WindowPosition operator +(WindowPosition target, DoublePoint other)
        {
            target.X += other.X;
            target.Y += other.Y;

            return target;
        }
        #endregion

        public bool Equals(WindowPosition other)
            => other.X == X && other.Y == Y &&
               other.Width == Width && other.Height == Height;
    }

    public class ToolWindowEx : ToolWindow
    {
        #region Window position & size
        private WindowPosition _Position = new WindowPosition { };

        public WindowPosition Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        public WindowPosition BackupPosition { get; private set; }
            = new WindowPosition();

        public void RequestPositionBackup()
        {
            BackupPosition = Position;
        }

        public void RequestPositionRestore()
        {
            Position = BackupPosition;
        }

        public bool Focused => FocusState != FocusState.Unfocused;

        public new double X
        {
            get { return Position.X; }
            set
            {
                _Position.X = value;
                base.X = value;
            }
        }

        public new double Y
        {
            get { return Position.Y; }
            set
            {
                _Position.Y = value;
                base.Y = value;
            }
        }

        public new double Width
        {
            get { return Position.Width; }
            set
            {
                _Position.Width = value;
                base.Width = value;
            }
        }

        public new double Height
        {
            get { return Position.Height; }
            set
            {
                _Position.Height = value;
                base.Height = value;
            }
        }
        #endregion
    }

    public class WindowManager
    {
        protected List<ToolWindowEx> _Windows = new List<ToolWindowEx>();
        public List<ToolWindowEx> Windows
        {
            protected get { return _Windows; }
            set
            {
                _Windows = value;
                foreach (var wind in value)
                {
                    wind.GotFocus += (_, __) =>
                    {
                        WindowOnFocus = wind;
                    };
                    wind.LostFocus += (_, __) =>
                    {
                        WindowOnFocus = null;
                    };
                }
            }
        }
        private ToolWindowEx WindowOnFocus;

        public Dictionary<int, WindowPosition> DesiredDocks;

        public void HideAll()
        {
            foreach (ToolWindowEx wind in Windows)
                wind.MinimizeAsync();
        }

        public void CloseAll()
        {
            foreach (var wind in Windows)
                wind.Close();
        }

        /// <summary>
        /// Tile all the specified windows (*) in
        /// <see cref="DesiredDocks"/>, ignoring other windows.
        /// *: You have to make sure that windows you want to be tiled
        /// are still alive yourself
        /// </summary>
        public void TileAllSpecified()
        {
            foreach (var pair in DesiredDocks)
            {
                if (!Windows[pair.Key].Visibility.IsVisible())
                    Windows[pair.Key].Show();
                Windows[pair.Key].RequestPositionBackup();
                Windows[pair.Key].Position = pair.Value;
            }
        }

        public void UnTileAllSpecified()
        {
            foreach (var pair in DesiredDocks)
            {
                if (!Windows[pair.Key].Visibility.IsVisible())
                    Windows[pair.Key].Show();
                Windows[pair.Key].RequestPositionRestore();
            }
        }
    }
}

#endif
