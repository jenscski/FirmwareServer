using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FirmwareServer.TagHelpers
{
    public class PageHeaderTagHelper : TagHelper
    {
        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            output.Attributes.Add("class", "border-bottom mb-1 pb-1 d-flex");

            output.PreContent.SetHtmlContent($"<h1 class=\"m-0 mr-2 {Class}\">{Title ?? ViewContext.ViewData["Title"]}</h1>");

            base.Process(context, output);
        }
    }
}
