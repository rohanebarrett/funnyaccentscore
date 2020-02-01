using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Funny.Accents.Core.Types.Binders
{
    public class DateTimeBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var provider = CultureInfo.InvariantCulture;
            string[] dateTimeFormats =
            {
                "F", "f", "D", "d", "yyyyMMdd", "yyyy-MM-dd","s","u","o"
            };

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Try to fetch the value of the argument by name
            var modelName = bindingContext.ModelName;
            var valueProviderResult =
                bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName,
                valueProviderResult);

            var dateTimeString = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return Task.CompletedTask;
            }

            var (status, dateTime, _) = TryParseDateTime(dateTimeString, dateTimeFormats,
                provider, DateTimeStyles.AssumeLocal);

            if (!status || dateTime == null)
            {
                // Non-integer arguments result in model state errors
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    $"Provided date could not be parsed -> {dateTimeString}");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(dateTime.Value);

            return Task.CompletedTask;
        }/*End of BindModelAsync method*/

        private static (bool status, DateTime? dateResult,Exception exception) TryParseDateTime(string dateTimeString,
            string[] dateTimeFormats, IFormatProvider provider, DateTimeStyles dateTimeStyles)
        {
            try
            {
                DateTime.TryParseExact(dateTimeString, dateTimeFormats, provider,
                    dateTimeStyles, out var dateTimeResult);

                return (true, dateTimeResult,null);
            }
            catch (Exception ex)
            {
                return (false, null,ex);
            }
        }/*End of TryParseDateTime method*/
    }/*End of DateTimeBinder class*/
}/*End of BinGoServices.Models.Binders namespace*/
