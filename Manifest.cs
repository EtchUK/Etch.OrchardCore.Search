using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch UK",
    Category = "Content",
    Description = "Provides ability to setup site search.",
    Name = "Site Search",
    Version = "0.1.1",
    Website = "https://etchuk.com",
    Dependencies = new[] { "OrchardCore.Autoroute", "OrchardCore.Lucene", "OrchardCore.Queries", "OrchardCore.Title" }
)]