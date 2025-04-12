using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PresentationLayer.Helpers
{
    public static class HtmlExtensions
    {
        public static IHtmlContent DropDownControlFor<TModel, TValue>(this IHtmlHelper<TModel> helper,
                  Expression<Func<TModel, TValue>> expression, string ViewDataKey, object htmlAttributes = null,
                  string Icon = "", bool ShowLabel = true, bool MultiSelect = false, bool AllowSearch = true,
                  bool Enabled = true, string PlaceHolder = "", bool ShowRequired = false)
        {
            var itemList = helper.ViewData[ViewDataKey] as List<SelectListItem>;
            return DropDownControlFor(helper, expression, itemList, htmlAttributes, Icon, ShowLabel,
                MultiSelect, AllowSearch, Enabled, PlaceHolder, ShowRequired);
        }

        public static IHtmlContent DropDownControlFor<TModel, TValue>(this IHtmlHelper<TModel> helper,
           Expression<Func<TModel, TValue>> expression, List<SelectListItem> itemList,
           object htmlAttributes = null, string Icon = "", bool ShowLabel = true, bool MultiSelect = false,
           bool AllowSearch = true, bool Enabled = true, string PlaceHolder = "", bool ShowRequired = false)
        {
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attrs = AddFormControlClassToHtmlAttributes(attrs, !AllowSearch);
            if (!Enabled)
            {
                attrs["readonly"] = "readonly";
            }
            var editorHtml = "";
            helper.ViewData["PlaceHolder"] = PlaceHolder;
            if (MultiSelect)
            {
                helper.ViewData["SelectList"] = itemList;
                editorHtml = helper.EditorFor(expression, "MultiSelect").ToHtmlString();
            }
            else
            {
                editorHtml = helper.DropDownListFor(expression, itemList, attrs).ToHtmlString();
            }
            if (!string.IsNullOrEmpty(Icon))
            {
                editorHtml = $"<div class='input-group'>{editorHtml}<div class=\"input-group-addon\"><i class=\"{Icon}\"></i></div></div>";
            }
            string labelHtml = helper.LabelFor(expression).ToHtmlString();
            if (ShowRequired)
                labelHtml = "<span class='req'>*</span>" + labelHtml;
            if (!ShowLabel)
                labelHtml = "";//
            string ValidationHtml = helper.ValidationMessageFor(expression).ToHtmlString();
            // var result = string.Format("{0} {1}  <span class=\"help-block validationError\"></span>", labelHtml, editorHtml );
            var result = string.Format("{0} {1}  <span class=\"help-block validationError\">&nbsp;{2}</span>", labelHtml, editorHtml, ValidationHtml);

            result = string.Format("<div class=\"form-group \" >{0}</div>", result);

            return new HtmlString(result);
        }

        public static RouteValueDictionary AddFormControlClassToHtmlAttributes(IDictionary<string, object> htmlAttributes, bool AddDisableSearch = false)
        {
            if (!htmlAttributes.ContainsKey("class") || htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
                htmlAttributes["class"] = "form-control" + (AddDisableSearch ? " nosearch" : "");
            else
                if (!htmlAttributes["class"].ToString().Contains("form-control"))
                htmlAttributes["class"] += " form-control" + (AddDisableSearch ? " nosearch" : "");

            return new RouteValueDictionary(htmlAttributes);
        }

        public static string ToHtmlString(this IHtmlContent tag)
        {
            using (var writer = new StringWriter())
            {
                tag.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

    }
}
