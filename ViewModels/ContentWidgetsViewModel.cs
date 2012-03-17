using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Piedone.ContentWidgets.ViewModels
{
    public class ContentWidget
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAttached { get; set; }
    }

    public class ContentWidgetsViewModel
    {
        public IList<ContentWidget> Widgets { get; set; }

        public string GetIdsSerialized(Predicate<ContentWidget> selector)
        {
            var ids = from widget in Widgets
                      where selector(widget)
                      select widget.Id;

            return new JavaScriptSerializer().Serialize(ids);
        }

        public static IEnumerable<int> DeserializeIds(string serializedIds)
        {
            if (String.IsNullOrEmpty(serializedIds)) return new int[0];
            else return new JavaScriptSerializer().Deserialize<IEnumerable<int>>(serializedIds);
        }
    }
}
