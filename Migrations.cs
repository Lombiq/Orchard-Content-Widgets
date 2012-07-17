using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Piedone.ContentWidgets.Models;

namespace Piedone.ContentWidgets
{
    // Funny fail of copy-paste :-)
    public class FacebookLikeButtonMigrations : DataMigrationImpl
    {
        public int Create()
        {
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
