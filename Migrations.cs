using System.Linq;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Data.Migration.Records;
using Piedone.ContentWidgets.Models;

namespace Piedone.ContentWidgets
{
    public class Migrations : DataMigrationImpl
    {
        private readonly IRepository<DataMigrationRecord> _migrationRepository;

        public Migrations(IRepository<DataMigrationRecord> migrationRepository)
        {
            _migrationRepository = migrationRepository;
        }

        public int Create()
        {
            var existingMigration = _migrationRepository.Fetch(record => record.DataMigrationClass == "Piedone.ContentWidgets.FacebookLikeButtonMigrations").FirstOrDefault();

            if (existingMigration != null)
            {
                _migrationRepository.Delete(existingMigration);
            }
            else
            {
                SchemaBuilder.CreateTable(typeof(ContentWidgetsPartRecord).Name,
                        table => table
                            .ContentPartRecord()
                            .Column<string>("ExcludedWidgetIdsDefinition", column => column.Unlimited())
                    );

                ContentDefinitionManager.AlterPartDefinition(typeof(ContentWidgetsPart).Name,
                    builder => builder.Attachable());
            }


            return 1;
        }
    }
}
