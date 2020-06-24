using System;
using Cmk.Devsolutions.Core.Types.Provider.Binder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Cmk.Devsolutions.Core.Types.Provider.Provider
{
    public class DateTimeBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Metadata.ModelType == typeof(DateTime)
                   || context.Metadata.ModelType == typeof(DateTime?)
                ? new BinderTypeModelBinder(typeof(DateTimeBinder))
                : null;
        }/*End of GetBinder method*/
    }/*End of DateTimeBinderProvider class*/
}/*End of BinGoServices.Models.Providers namespace*/
