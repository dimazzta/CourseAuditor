using System;

namespace CourseAuditor.Helpers
{
    public class ObjectChangedEventArgs
    {
        public object ObjectChanged { get; protected set; }

        public ObjectChangedEventArgs(object objectChanged)
        {
            ObjectChanged = objectChanged;
        }
    }
}