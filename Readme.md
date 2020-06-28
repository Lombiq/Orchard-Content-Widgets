# Content Widgets Orchard module



## About

This Orchard module makes it possible to add arbitrary widgets to content types (with the option to disable per item).


## Features

- Select widgets to display for all items of a content type
- Disable widgets per content item
- Configure display of widgets with Placement.info
- Import/export support


## Documentation

**The module depends [Helpful Libraries](https://gallery.orchardproject.net/List/Modules/Orchard.Module.Piedone.HelpfulLibraries) (at least 1.6). Please install it prior to installing the module!**  
After installing there will be a new part, ContentWidgetsPart. Attach that to the content types you want to display widgets with. Settings are added to the content type editor UI and the item editor.  
**Note that widgets are only displayed if you specify their shape's** (the one listed by their checkbox item) **placement in a Placement.info!**  
Since Content Widgets uses the widgets' ids to identify them the change of ids cause problems. This can happen when importing or exporting widgets, so if you plan to do this, make sure that you understand the consequences: exporting widgets or importing new ones won't harm, but rehydrating a site from recipes can mess up settings.

You can install the module from the [Gallery](http://gallery.orchardproject.net/List/Modules/Orchard.Module.Piedone.ContentWidgets).

[Version History](Docs/VersionHistory.md)


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.