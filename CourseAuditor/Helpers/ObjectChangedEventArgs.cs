using System;

namespace CourseAuditor.Helpers
{
    public enum ChangeType
    {
        Deleted,
        Updated,
        Added
    };
    public class ObjectChangedEventArgs
    {
        public object ObjectChanged { get; protected set; }
        public ChangeType Type { get; protected set; }
        public ObjectChangedEventArgs(object objectChanged, ChangeType type)
        {
            ObjectChanged = objectChanged;
            Type = type;
        }
    }
}