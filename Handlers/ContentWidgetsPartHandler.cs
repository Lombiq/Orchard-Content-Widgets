using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Piedone.ContentWidgets.Models;

namespace Piedone.ContentWidgets.Handlers
{
    public class ContentWidgetsPartHandler : ContentHandler
    {
        public ContentWidgetsPartHandler(IRepository<ContentWidgetsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
