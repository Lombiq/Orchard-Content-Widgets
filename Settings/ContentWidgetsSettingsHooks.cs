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

namespace Piedone.ContentWidgets.Settings
{
    public class ContentWidgetsTypePartSettings
    {
        /// <summary>
        /// JSON array of the ids of widgets that are attached to the content type's items
        /// </summary>
        public string AttachedWidgetIdsDefinition { get; set; }

        private IEnumerable<int> _attachedWidgetIds;
        public IEnumerable<int> AttachedWidgetIds
        {
            get
            {
                if (_attachedWidgetIds == null)
                {
                    if (String.IsNullOrEmpty(AttachedWidgetIdsDefinition)) _attachedWidgetIds = new int[0];
                    else _attachedWidgetIds = new JavaScriptSerializer().Deserialize<IEnumerable<int>>(AttachedWidgetIdsDefinition);
                }

                return _attachedWidgetIds;
            }

            set
            {
                _attachedWidgetIds = value;
                AttachedWidgetIdsDefinition = new JavaScriptSerializer().Serialize(_attachedWidgetIds);
            }
        }

        public IList<WidgetAttachment> Widgets { get; set; }
    }

    public class ContentWidgetsSettingsHooks : ContentDefinitionEditorEventsBase
    {
        private readonly IWidgetsService _widgetService;

        public ContentWidgetsSettingsHooks(IWidgetsService widgetService)
        {
            _widgetService = widgetService;
        }

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "ContentWidgetsPart")
                yield break;

            var model = definition.Settings.GetModel<ContentWidgetsTypePartSettings>();

            model.Widgets = (from widget in _widgetService.GetWidgets()
                             select new WidgetAttachment
                             {
                                 Id = widget.Id,
                                 Title = widget.Title,
                                 IsAttached = model.AttachedWidgetIds.Contains(widget.Id)
                             }).ToList();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "ContentWidgetsPart")
                yield break;

            var model = new ContentWidgetsTypePartSettings();
            updateModel.TryUpdateModel(model, "ContentWidgetsTypePartSettings", null, null);
            model.AttachedWidgetIds = (from widget in model.Widgets where widget.IsAttached select widget.Id);
            builder.WithSetting("ContentWidgetsTypePartSettings.AttachedWidgetIdsDefinition", model.AttachedWidgetIdsDefinition);

            yield return DefinitionTemplate(model);
        }
    }
}
