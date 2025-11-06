using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace NoteApp.Api.OpenApi;
public class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
	public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
	{
		document.Components ??= new OpenApiComponents();
		document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
		{
			Type = SecuritySchemeType.Http,
			Scheme = "bearer",
			BearerFormat = "JWT",
			In = ParameterLocation.Header,
			Name = "Authorization",
			Description = "Enter 'Bearer {token}'"
		};

		document.SecurityRequirements.Add(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Id = "Bearer",
						Type = ReferenceType.SecurityScheme
					}
				},
				new List<string>()
			}
		});

		return Task.CompletedTask;
	}
}
