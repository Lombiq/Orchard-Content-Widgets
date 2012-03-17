using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Drivers;
using Piedone.ContentWidgets.Models;
using Orchard.ContentManagement;
using Piedone.ContentWidgets.Settings;
using Orchard.Widgets.Models;
using Piedone.ContentWidgets.ViewModels;

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
            var excludedWidgetIds = ContentWidgetsViewModel.DeserializeIds(part.ExcludedWidgetIdsDefinition);
            var displayedWidgetIds = from id in ContentWidgetsViewModel.DeserializeIds(settings.AttachedWidgetIdsDefinition)
                                     where !excludedWidgetIds.Contains(id)
                                     select id;

            var results = new List<DriverResult>();

            foreach (var id in displayedWidgetIds)
            {
                int widgetId = id;

                results.Add(
                    ContentShape("Parts_ContentWidgetsPart_Widget_" + id, 
                    () =>
                    {
                        var widget = _contentManager.Get(widgetId);
                        // This can only happen if a widget previously attached gets removed.
                        if (widget == null) return shapeHelper.Empty();
                        return _contentManager.BuildDisplay(widget);
                    })
                );
            }

            return Combined(results.ToArray());
        }

        // GET
        protected override DriverResult Editor(ContentWidgetsPart part, dynamic shapeHelper)
        {
            var viewModel = new ContentWidgetsViewModel();
            viewModel.Widgets = (from widget in _contentManager.GetMany<WidgetPart>(
                                ContentWidgetsViewModel.DeserializeIds(part.Settings.GetModel<ContentWidgetsTypePartSettings>().AttachedWidgetIdsDefinition),
                                VersionOptions.Published,
                                new QueryHints().ExpandRecords<WidgetPartRecord>())
                                select new ContentWidget
                                {
                                    Id = widget.Id,
                                    Title = widget.Title,
                                    IsAttached = !ContentWidgetsViewModel.DeserializeIds(part.ExcludedWidgetIdsDefinition).Contains(widget.Id)
                                }).ToList();

            return ContentShape("Parts_ContentWidgetsPart_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.ContentWidgets",
                    Model: viewModel,
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(ContentWidgetsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new ContentWidgetsViewModel();
            updater.TryUpdateModel(viewModel, Prefix, null, null);

            part.ExcludedWidgetIdsDefinition = viewModel.GetIdsSerialized(widget => !widget.IsAttached);

            return Editor(part, shapeHelper);
        }
    }
}
