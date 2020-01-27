# Generating swagger.json and ApiClient.cs (C# client code)

> dotnet msbuild -t:NSwag


This command uses the nswag.json configuration to generate a swagger.json open api definition as well associated C# client code. The client code (ApiClient.cs) is generated directly into the XTMF2.Web.Client project.
