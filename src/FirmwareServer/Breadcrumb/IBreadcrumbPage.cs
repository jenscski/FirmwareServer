using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmwareServer.Breadcrumb
{
    interface IBreadcrumbPage
    {
        IEnumerable<Breadcrumb> Breadcrumbs();
    }
}
