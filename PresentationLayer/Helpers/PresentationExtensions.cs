using BusinessLayer.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using static BusinessLayer.Helpers.SharedEnums;

namespace PresentationLayer.Helpers
{
    public static class PresentationExtensions
    {
       
        public static List<SelectListItem> ConvertEnumToSelectListItems(Type t, LocalizationService l)
        {
            var x =new List<SelectListItem>();            
            var elements = Enum.GetValues(t);
            for (int i = 0; i < elements.Length; i++)
            {
                x.Add(new SelectListItem() {Text= l.GetLocalizedHtmlString(t.Name+"."+ elements.GetValue(i).ToString()) , Value=(i+1).ToString()} );

            }          
            return x;
        }
        public static List<SelectListItem> ConvertEnumToSelectListItemsStartFromZeroIndex(Type t, LocalizationService l)
        {
            var x = new List<SelectListItem>();
            var elements = Enum.GetValues(t);
            for (int i = 0; i < elements.Length; i++)
            {
                x.Add(new SelectListItem() { Text = l.GetLocalizedHtmlString(t.Name + "." + elements.GetValue(i).ToString()), Value = (i).ToString() });

            }
            return x;
        }
        /// <summary>
        /// convert enum elements to SelectListItem with text =enum element and value =element description
        /// </summary>
        /// <param name="t"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static List<SelectListItem> ConvertEnumToSelectListItems2(Type t, LocalizationService l)
        {           
            var x = new List<SelectListItem>();           
            var elements = Enum.GetValues(t);
            for (int i = 0; i < elements.Length; i++)
            {
                var enumValue = (Enum)elements.GetValue(i);
                string valueString = enumValue.GetDescription();
                x.Add(new SelectListItem() { Text = l.GetLocalizedHtmlString(t.Name + "." + elements.GetValue(i).ToString()), Value = valueString });

            }
            return x;
        }

        public static List<int> ConvertEnumToList(Type t)
        {
            var x = new List<int>();
            var elements = Enum.GetValues(t);
            for (int i = 0; i < elements.Length; i++)
            {
                x.Add(i);

            }
            return x;
        }
    }
}
