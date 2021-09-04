using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.Tools
{
    public interface ITool
    {
        ToolboxButtonInfo ButtonInfo { get; }
        void MouseEvent(MouseEventData eventData);
        void Activate();
        void Deactivate();
        void PopulateToolstrip(ToolStrip toolstrip);
    }
    public abstract class ToolBase : ITool
    {
        public abstract ToolboxButtonInfo ButtonInfo {  get;  }

        public abstract void MouseEvent(MouseEventData eventData);
        public abstract void Activate();
        public abstract void Deactivate();

        public abstract void PopulateToolstrip(ToolStrip toolStrip);
    }

    public class MouseEventData
    {
        public enum EventType
        {
            Click,
            DoubleClick,
            Down,
            Up,
            Move,
            Wheel
        }

        public EventType Event { get; private set; }
        public MouseEventArgs Args { get; private set; }

        public static MouseEventData FromMouseClickEvent(MouseEventArgs args)
        {
            return new MouseEventData { Event = EventType.Click, Args = args };
        }
        public static MouseEventData FromMouseDoubleClickEvent(MouseEventArgs args)
        {
            return new MouseEventData { Event = EventType.DoubleClick, Args = args };
        }
        public static MouseEventData FromMouseDownEvent(MouseEventArgs args)
        {
            return new MouseEventData { Event = EventType.Down, Args = args };
        }
        public static MouseEventData FromMouseUpEvent(MouseEventArgs args)
        {
            return new MouseEventData { Event = EventType.Up, Args = args };
        }
        public static MouseEventData FromMouseMoveEvent(MouseEventArgs args)
        {
            return new MouseEventData { Event = EventType.Move, Args = args };
        }
        public static MouseEventData FromMouseWheelEvent(MouseEventArgs args)
        {
            return new MouseEventData { Event = EventType.Wheel, Args = args };
        }
    }

    public class KeyEventData
    {
        
    }
}
