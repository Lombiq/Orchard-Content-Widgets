using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace Piedone.ContentWidgets.Models
{
    public class ContentWidgetsPartRecord : ContentPartRecord
    {
        /// <summary>
        /// JSON array of the ids of the widgets that shouldn't be attached to this content item
        /// </summary>
        public virtual string ExcludedWidgetIdsDefinition { get; set; }
    }
}
