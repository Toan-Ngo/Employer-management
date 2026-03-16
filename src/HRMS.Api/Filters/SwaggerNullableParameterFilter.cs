using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HRMS.Api.Filters
{
    public class SwaggerNullableParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (!parameter.Schema.Nullable &&
                context.ApiParameterDescription.ModelMetadata != null &&
                context.ApiParameterDescription.ModelMetadata.IsReferenceOrNullableType)
            {
                parameter.Schema.Nullable = true;
            }
        }
    }
}

