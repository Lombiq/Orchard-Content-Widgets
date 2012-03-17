using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Drivers;
using Piedone.ContentWidgets.Models;
using Orchard.ContentManagement;
using Piedone.ContentWidgets.Settings;
using Orchard.Widgets.Models;

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
            var typePartSettings = part.Settings.GetModel<ContentWidgetsTypePartSettings>();
            var displayedWidgetIds = from id in typePartSettings.AttachedWidgetIds where !part.ExcludedWidgetIds.Contains(id) select id;

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
            part.Widgets = (from widget in _contentManager.GetMany<WidgetPart>(
                                part.Settings.GetModel<ContentWidgetsTypePartSettings>().AttachedWidgetIds,
                                VersionOptions.Published,
                                new QueryHints().ExpandRecords<WidgetPartRecord>())
                             select new WidgetAttachment
                             {
                                 Id = widget.Id,
                                 Title = widget.Title,
                                 IsAttached = !part.ExcludedWidgetIds.Contains(widget.Id)
                             }).ToList();

            return ContentShape("Parts_ContentWidgetsPart_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.ContentWidgets",
                    Model: part,
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(ContentWidgetsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            part.ExcludedWidgetIds = from widget in part.Widgets where !widget.IsAttached select widget.Id;

            return Editor(part, shapeHelper);
        }
    }
}
