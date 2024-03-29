﻿using ASP_Meeting_18.Infrostructure.ModelBinders;
using ASP_Meeting_18.Models.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASP_Meeting_18.Infrostructure.ModelBinderProviders
{
    public class CartModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(Cart) ? new CartModelBinder() : null;
        }
    }
}