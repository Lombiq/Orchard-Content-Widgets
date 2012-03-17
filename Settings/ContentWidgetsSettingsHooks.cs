using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.ViewModels;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using System.Web.Script.Serialization;
using Piedone.ContentWidgets.Models;
using Piedone.ContentWidgets.ViewModels;

namespace Piedone.ContentWidgets.Settings
{
    public class ContentWidgetsTypePartSettings
    {
        /// <summary>
        /// JSON array of the ids of widgets that are attached to the content type's items
        /// </summary>
        public string AttachedWidgetIdsDefinition { get; set; }
    }

    public class ContentWidgetsSettingsHooks : ContentDefinitionEditorEventsBase
    {
        private readonly IWidgetsService _widgetService;

        private string Prefix
        {
            get { return "ContentWidgetsTypePartSettings"; }
        }

        public ContentWidgetsSettingsHooks(IWidgetsService widgetService)
        {
            _widgetService = widgetService;
        }

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "ContentWidgetsPart")
                yield break;

            var settings = definition.Settings.GetModel<ContentWidgetsTypePartSettings>();
            var attachedWidgetIds = ContentWidgetsViewModel.DeserializeIds(settings.AttachedWidgetIdsDefinition);
            var viewModel = new ContentWidgetsViewModel();
            viewModel.Widgets = (from widget in _widgetService.GetWidgets()
                                 select new ContentWidget
                                 {
                                     Id = widget.Id,
                                     Title = widget.Title,
                                     IsAttached = attachedWidgetIds.Contains(widget.Id)
                                 }).ToList();

            yield return DefinitionTemplate(viewModel, "ContentWidgetsTypePartSettings", Prefix);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "ContentWidgetsPart")
                yield break;

            var viewModel = new ContentWidgetsViewModel();
            updateModel.TryUpdateModel(viewModel, Prefix, null, null);

            builder.WithSetting("ContentWidgetsTypePartSettings.AttachedWidgetIdsDefinition", viewModel.GetIdsSerialized(widget => widget.IsAttached));

            yield return DefinitionTemplate(viewModel, "ContentWidgetsTypePartSettings", Prefix);
        }
    }
}
