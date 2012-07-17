using Orchard.UI.Resources;

namespace Piedone.ContentWidgets
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineStyle("ContentWidgets_Admin").SetUrl("piedone-content-widgets-admin.css");
        }
    }
}
