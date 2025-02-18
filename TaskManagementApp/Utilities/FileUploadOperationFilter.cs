using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementApp.Utilities
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Look for parameters of type IFormFile in the action's parameters
            var fileParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.Type == typeof(IFormFile) ||
                            (p.ModelMetadata != null && p.ModelMetadata.ModelType == typeof(IFormFile)))
                .ToList();

            if (!fileParameters.Any())
            {
                return;
            }

            // Remove the existing file parameters from the operation parameters.
            foreach (var parameter in fileParameters)
            {
                var paramToRemove = operation.Parameters.FirstOrDefault(p => p.Name == parameter.Name);
                if (paramToRemove != null)
                {
                    operation.Parameters.Remove(paramToRemove);
                }
            }

            // Add a new request body with a multipart/form-data schema that describes the file upload.
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["file"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary",
                                    Description = "Upload File"
                                }
                            },
                            Required = new HashSet<string> { "file" }
                        }
                    }
                }
            };
        }
    }

}
