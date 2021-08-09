using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch UK",
    Category = "Content",
    Description = "Provides ability to setup site search.",
    Name = "Site Search",
    Version = "1.0.0",
    Website = "https://etchuk.com",
    Dependencies = new[] { "OrchardCore.Autoroute", "OrchardCore.Lucene", "OrchardCore.Queries", "OrchardCore.Title" }
)]