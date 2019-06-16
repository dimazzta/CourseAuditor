using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CourseAuditor.Helpers
{
    public static class Ext
    {
        public static bool InRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }
        public static IEnumerable<T> FindVisualDescendants<T>(this Visual parent, Func<T, bool> predicate, bool deepSearch) where T : Visual
        {
            var visualChildren = new List<Visual>();
            var visualChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var childIndex = 0; childIndex < visualChildrenCount; childIndex++)
            {
                visualChildren.Add((Visual)VisualTreeHelper.GetChild(parent, childIndex));
            }

            foreach (var child in visualChildren)
            {
                var typedChild = child as T;
                if ((typedChild != null) && (predicate == null || predicate.Invoke(typedChild)))
                {
                    yield return typedChild;
                    if (deepSearch)
                    {
                        foreach (var foundVisual in FindVisualDescendants(child, predicate, true))
                        {
                            yield return foundVisual;
                        }
                    }
                    else
                    {
                        foreach (var foundVisual in FindVisualDescendants(child, predicate, deepSearch))
                        {
                            yield return foundVisual;
                        }
                    }
                }

                yield break;
            }
        }
    }
}
