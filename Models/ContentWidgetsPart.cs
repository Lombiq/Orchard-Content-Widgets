using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using System.Web.Script.Serialization;

namespace Piedone.ContentWidgets.Models
{
    public class ContentWidgetsPart : ContentPart<ContentWidgetsPartRecord>
    {
        public string ExcludedWidgetIdsDefinition
        {
            get { return Record.ExcludedWidgetIdsDefinition; }
            set { Record.ExcludedWidgetIdsDefinition = value; }
        }
    }
}
