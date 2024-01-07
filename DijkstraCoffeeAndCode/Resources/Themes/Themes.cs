using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DijkstraCoffeeAndCode.Resources.Themes
{
    public static class Themes
    {
        public static readonly Dictionary<string, string> ColorSchemeMap = new() {
            { "Day Mode", "Resources/Themes/DefaultColors.xaml" },
            { "Night Mode", "Resources/Themes/DarkColors.xaml" }
        };

        public static readonly List<KeyValuePair<string, string>> ColorSchemeList = ColorSchemeMap.ToList();

        public static int GetCurrentThemeIndex(Collection<ResourceDictionary> resources)
        {
            ResourceDictionary? resource = resources.FirstOrDefault((dictionary) => { return ColorSchemeMap.Values.Contains(dictionary.Source.OriginalString); });
            if (resource == null) { throw new Exception("Theme not found in ColorSchemeMap"); }

            return ColorSchemeList.FindIndex(keyValue => keyValue.Value == resource.Source.OriginalString);
        }

        public static int GetNext(int index, out KeyValuePair<string, string> colorScheme)
        {
            index += 1;
            if (index < 0 || index >= ColorSchemeList.Count)
            {
                colorScheme = ColorSchemeList[0];
                return 0;
            }

            colorScheme = ColorSchemeList[index];
            return index;
        }
    }
}
