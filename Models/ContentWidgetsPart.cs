using Orchard.ContentManagement;

namespace Piedone.ContentWidgets.Models
{
    public class ContentWidgetsPart : ContentPart<ContentWidgetsPartRecord>
    {
        public string ExcludedWidgetIdsDefinition
        {
            get { return Retrieve(x => x.ExcludedWidgetIdsDefinition); }
            set { Store(x => x.ExcludedWidgetIdsDefinition, value); }
        }
    }
}
