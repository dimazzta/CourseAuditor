using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Helpers
{
    public class EventsManager
    {
        public static event EventHandler<ObjectChangedEventArgs> ObjectChangedEvent;

        public static void RaiseObjectChangedEvent(object objectAdded, ChangeType type) // возможно заменить на 3 отдельные Added Deleted Updated, но надо будет много логики реализовывать на стороне подписчика
        {
            ObjectChangedEvent?.Invoke(null, new ObjectChangedEventArgs(objectAdded, type));
        }
    }
}
