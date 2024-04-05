namespace Simple.WinUI.Controls.TrackBar
{
    public class PaintStateEventArgs : PaintEventArgs
    {
        private TrackBarState _trackBarState;

        public PaintStateEventArgs(Graphics g, Rectangle clipRect, TrackBarState state) : base(g, clipRect)
        {
            _trackBarState = state;
        }

        public TrackBarState TrackBarState
        {
            get { return _trackBarState; }
        }
    }
}