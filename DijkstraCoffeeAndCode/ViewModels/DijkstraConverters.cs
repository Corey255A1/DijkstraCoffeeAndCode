using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class DijkstraViewModelStateToBrush : IValueConverter
    {
        public Brush Base { get; set; }
        public Brush Selected { get; set; }
        public Brush Visited { get; set; }
        public Brush CurrentlyVisiting { get; set; }
        public Brush CurrentNeighbor { get; set; }
        public Brush Highlighted { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DijkstraNodeViewModel dijkstraViewModel)
            {
                if (dijkstraViewModel.Selected)
                {
                    return Selected;
                }
                else if (dijkstraViewModel.Highlighted)
                {
                    return Highlighted;
                }
                else if (dijkstraViewModel.Node.Visited)
                {
                    return Visited;
                }
            }
            return Base;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CoordinateOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double coordinate = (double)value;
            double offset = (double)parameter;  
            return coordinate - offset;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
