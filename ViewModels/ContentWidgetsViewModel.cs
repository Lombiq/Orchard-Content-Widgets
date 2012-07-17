using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
