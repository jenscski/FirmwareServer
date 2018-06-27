using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace FirmwareServer.Breadcrumb
{
    public class BreadcrumbPageFilter : IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            if (context.HandlerInstance is IBreadcrumbPage page)
            {
                var breadCrumbs = context.HttpContext.Items["FooBar"] as List<Breadcrumb> ?? new List<Breadcrumb>();

                breadCrumbs.AddRange(page.Breadcrumbs() ?? new Breadcrumb[0]);

                context.HttpContext.Items["FooBar"] = breadCrumbs;
            }
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }
    }
}
