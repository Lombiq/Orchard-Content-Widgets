using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace Piedone.ContentWidgets.Models
{
    public class ContentWidgetsPartRecord : ContentPartRecord
    {
        /// <summary>
        /// JSON array of the ids of the widgets that shouldn't be attached to this content item
        /// </summary>
        [StringLengthMax]
        public virtual string ExcludedWidgetIdsDefinition { get; set; }
    }
}
