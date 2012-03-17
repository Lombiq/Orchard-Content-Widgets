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

        private IEnumerable<int> _excludedWidgetIds;
        public IEnumerable<int> ExcludedWidgetIds
        {
            get
            {
                if (_excludedWidgetIds == null)
                {
                    if (String.IsNullOrEmpty(ExcludedWidgetIdsDefinition)) _excludedWidgetIds = new int[0];
                    else _excludedWidgetIds = new JavaScriptSerializer().Deserialize<IEnumerable<int>>(ExcludedWidgetIdsDefinition);
                }

                return _excludedWidgetIds;
            }

            set
            {
                _excludedWidgetIds = value;
                ExcludedWidgetIdsDefinition = new JavaScriptSerializer().Serialize(_excludedWidgetIds);
            }
        }

        public IList<WidgetAttachment> Widgets { get; set; }
    }
}
