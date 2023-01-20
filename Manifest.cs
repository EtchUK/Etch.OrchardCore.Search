using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch UK",
    Category = "Content",
    Description = "Provides ability to setup site search.",
    Name = "Site Search",
    Version = "$(VersionNumber)",
    Website = "https://etchuk.com",
    Dependencies = new[] { "OrchardCore.Autoroute", "OrchardCore.Lucene", "OrchardCore.Queries", "OrchardCore.Title" }
)]