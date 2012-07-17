using System.Collections.Generic;

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
    }
}
