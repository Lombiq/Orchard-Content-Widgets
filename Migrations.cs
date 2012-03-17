using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Piedone.ContentWidgets.Models;

namespace Piedone.ContentWidgets
{
    public class FacebookLikeButtonMigrations : DataMigrationImpl
    {
        public int Create()
        {
            // Creating table FacebookLikeButtonPartRecord
            SchemaBuilder.CreateTable(typeof(ContentWidgetsPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("ExcludedWidgetIdsDefinition")
            );

            ContentDefinitionManager.AlterPartDefinition(typeof(ContentWidgetsPart).Name,
                builder => builder.Attachable());


            return 1;
        }
    }
}
