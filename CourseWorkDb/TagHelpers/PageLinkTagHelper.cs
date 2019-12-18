using CourseWorkDb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace CourseWorkDb.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);

            if(PageModel.HasSecondPreviousPage)
            {
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 2, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            if (PageModel.HasPreviousPage)
            {
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            tag.InnerHtml.AppendHtml(currentItem);

            if (PageModel.HasNextPage)
            {
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }

            if (PageModel.HasSecondNextPage)
            {
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 2, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }

            TagBuilder nav = new TagBuilder("nav");
            nav.InnerHtml.AppendHtml(tag);
            output.Content.AppendHtml(nav);
        }

        private TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            if (pageNumber == PageModel.PageNumber)
            {
                item.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
