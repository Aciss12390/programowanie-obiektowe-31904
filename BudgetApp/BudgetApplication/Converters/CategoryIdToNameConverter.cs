using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using BudzetDomowy.Models;

namespace BudgetApplication.Converters
{
    public class CategoryIdToNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return "";

            if (values[0] is not int categoryId)
                return "";

            if (values[1] is not System.Collections.IEnumerable categoriesEnumerable)
                return categoryId.ToString();

            var category = categoriesEnumerable.Cast<Category>().FirstOrDefault(c => c.Id == categoryId);
            return category?.Name ?? categoryId.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Nie edytujemy tego w tabeli
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
