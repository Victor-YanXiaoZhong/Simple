using System;

namespace Simple.WinUI.Controls.PopupControl
{
    public delegate void OldNewEventHandler<T>(object sender, OldNewEventArgs<T> e);

    public class OldNewEventArgs<T> : EventArgs
    {
        private T m_oldValue = default(T);

        private T m_newValue = default(T);

        public OldNewEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue
        {
            get { return this.m_oldValue; }
            protected set { this.m_oldValue = value; }
        }

        public T NewValue
        {
            get { return this.m_newValue; }
            protected set { this.m_newValue = value; }
        }
    }
}