using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Widgets.Models;
using Piedone.ContentWidgets.Models;
using Piedone.ContentWidgets.Settings;
using Piedone.ContentWidgets.ViewModels;
using Piedone.HelpfulLibraries.Serialization;

namespace Piedone.ContentWidgets.Drivers
{
    public class ContentWidgetsPartDriver : ContentPartDriver<ContentWidgetsPart>
    {
        private readonly IContentManager _contentManager;

        protected override string Prefix
        {
            get { return "ContentWidgets"; }
        }


        public ContentWidgetsPartDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }


        protected override DriverResult Display(ContentWidgetsPart part, string displayType, dynamic shapeHelper)
        {
            var settings = part.Settings.GetModel<ContentWidgetsTypePartSettings>();
            var excludedWidgetIds = IdSerializer.DeserializeIds(part.ExcludedWidgetIdsDefinition);
            var displayedWidgetIds = from id in IdSerializer.DeserializeIds(settings.AttachedWidgetIdsDefinition)
                                     where !excludedWidgetIds.Contains(id)
                                     select id;

            var results = new List<DriverResult>();

            var widgets = _contentManager.GetMany<IContent>(displayedWidgetIds, VersionOptions.Published, new QueryHints().ExpandParts<WidgetPart>());
            foreach (var widget in widgets)
            {
                // This is needed so the lambda gets a fresh copy.
                var currentWidget = widget;

                results.Add(
                    ContentShape("Parts_ContentWidgetsPart_Widget_" + currentWidget.ContentItem.Id,
                        () =>
                        {
                            var shape = _contentManager.BuildDisplay(currentWidget);
                            foreach (var partShape in shape.Content.Items)
                            {
                                partShape.ParentContentItem = part.ContentItem;
                            }
                            return shape;
                        })
                );
            }

            return Combined(results.ToArray());
        }

        // GET
        protected override DriverResult Editor(ContentWidgetsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ContentWidgetsPart_Edit",
                () =>
                {
                    var excludedWidgetIds = IdSerializer.DeserializeIds(part.ExcludedWidgetIdsDefinition); 
                    var viewModel = new ContentWidgetsViewModel();
                    viewModel.Widgets = (from widget in _contentManager.GetMany<WidgetPart>(
                                            IdSerializer.DeserializeIds(part.Settings.GetModel<ContentWidgetsTypePartSettings>().AttachedWidgetIdsDefinition),
                                            VersionOptions.Published,
                                            new QueryHints().ExpandRecords<WidgetPartRecord>())
                                        select new ContentWidget
                                        {
                                            Id = widget.ContentItem.Id,
                                            Title = widget.Title,
                                            IsAttached = !excludedWidgetIds.Contains(widget.Id)
                                        }).ToList();

                    return shapeHelper.EditorTemplate(
                                TemplateName: "Parts.ContentWidgets",
                                Model: viewModel,
                                Prefix: Prefix);
                });
        }

        // POST
        protected override DriverResult Editor(ContentWidgetsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new ContentWidgetsViewModel();
            updater.TryUpdateModel(viewModel, Prefix, null, null);

            var excludedWidgetIds = viewModel.Widgets.Where(widget => !widget.IsAttached).Select(widget => widget.Id);

            part.ExcludedWidgetIdsDefinition = IdSerializer.SerializeIds(excludedWidgetIds);

            return Editor(part, shapeHelper);
        }

        protected override void Exporting(ContentWidgetsPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("ExcludedWidgetIdsDefinition", part.ExcludedWidgetIdsDefinition);
        }

        protected override void Importing(ContentWidgetsPart part, ImportContentContext context)
        {
            context.ImportAttribute(part.PartDefinition.Name, "ExcludedWidgetIdsDefinition", value => part.ExcludedWidgetIdsDefinition = value);
        }
    }
}
