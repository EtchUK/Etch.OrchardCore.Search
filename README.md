# Etch.OrchardCore.Search

Module for [Orchard Core](https://github.com/OrchardCMS/OrchardCore) for adding site search.

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.Search.png?branch=main)](http://travis-ci.org/etchuk/Etch.OrchardCore.Search) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.Search.svg)](https://www.nuget.org/packages/Etch.OrchardCore.Search)

## Orchard Core Reference

This module is referencing a stable build of Orchard Core ([`1.1.0`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.1.0)).

## Installing

This module is [available on NuGet](https://www.nuget.org/packages/Etch.OrchardCore.Search). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.Search", ensuring include prereleases is checked.

Alternatively you can [download the source](https://github.com/etchuk/Etch.OrchardCore.Search/archive/main.zip) or clone the repository to your local machine. Add the project to your solution that contains an Orchard Core project and add a reference to Etch.OrchardCore.Search.

## Configuration

### TL;DR

-   Attach `SearchablePart` to content types you'd like to be included in the site search.
-   Ensure content types that have `SearchablePart` have any fields/parts that need to be searched on, included in index (suggest having "Stored", "Analyzed" & "Santized" checked).
-   Update "Search" index to include all content items to be included in the site search.
-   Check "SiteSearch" query template (go to "Configuration" -> "Queries") to ensure all the fields you'd like to be searched are included in the match definitions.
-   Create new "SiteSearch" content type with desired configuration.

### Enable Module

First step, as with any module, is to enable it. Go to "Configuration" then "Modules", enter "Search" in to the search box. You should see a feature labelled "Site Search", enable it if it's not already.

![Screen recording of enabling module](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-enable-module.gif)

### Searchable Part

One of the things provided by this module is the `Searchable` part that can be attached to content types. When attached, this part gives content editors additional options for controlling how search results are curated. After attaching `Searchable` to a content type, it's important to edit the part settings and ensure "Include this element index" is checked with "Stored", "Analyzed" & "Sanitized" all checked.

![Screen recording of attaching Searchable part](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-attaching-searchable-part.gif)

Firstly is the ability to flag when content items should be omitted from search results. By default, when using the site search query provided, content items with the `Searchable` will be included in the search results.

![Screen recording of exluding content item from search results](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-exclude-from-search-results.gif)

The `Searchable` part also has a field for specify keywords that when used as a search term it'll return the content item.

![Screen recording of specific keywords for matching search](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-keywords.gif)

### Indexes/Queries

The site search uses the [OrchardCore.Queries](https://orchardcore.readthedocs.io/en/latest/OrchardCore.Modules/OrchardCore.Queries/README/) module in combination with [OrchardCore.Lucene](https://orchardcore.readthedocs.io/en/latest/OrchardCore.Modules/OrchardCore.Lucene/README/) in order to scan indexed content items and present matches to the user.

When enabling the module, an example query ("SiteSearch") and index ("Search") are created. By default the "Search" index won't have any content types configured to be included so any content types that should be included in the search results should be included in the index. The next step is to go through all content types that are being searched and ensure any parts/fields that should be scanned for a match are included in the index (e.g. `TitlePart` & `HtmlBody` parts). Once you've configured the content types and included the various parts/fields in the index it'll be a good idea to rebuild the "Search" index.

The "SiteSearch" query that comes by default will perform the following search logic to determine whether a content item is included in the search results.

-   Must not be excluded from the search results
-   Must have a specific content type (currently only "Page")
-   Must have a match in at least one of the following
    -   Body content
    -   Html content (`Html` widget included via our [widgets module](https://github.com/etchuk/Etch.OrchardCore.Widgets))
    -   Paragraph content (`Paragraph` widget included via our [widgets module](https://github.com/etchuk/Etch.OrchardCore.Widgets))
    -   Title of content item
    -   Keywords field on `Searchable` part

The query also includes parameters for configuring paging (`from` & `size`).

It's likely that you'd want to edit which content types are included in the results or which fields are included in the `should` collection in the query.

![Screen recording of modifying site search query](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-modifying-default-query.gif)

### Site Search

When the module is enabled, a new "Site Search" content type has been defined. This content type represents the site search page to give content editors the ability to control the title, path and a whole host of configuration options for dictating how the search behaves (these have sensible defaults).

![Screen recording of creating site search](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-create-site-search.gif)

#### Display Type

There are a couple of ways to present the search results.

##### List

This is the default, which will display search results for all searchable content types in a single list mixed together. This requires a single query, which should cater for the different content types that should appear in the search results (use "SiteSearch" query that's created when enabling the module as an example). This comes with a simple next/previous paging control for when there are more matches than the configured page size.

![Screen recording of site search with list display type](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-site-search-list.gif)

##### Grouped

Results will be grouped by content type. When switching to "Grouped", the "Site Search" content item will present all the content types that contain the `Searchable` part. Each content type has it's own set of configuration settings. Each content type should have it's own query that'll ensure only that content type is returned as well as any additional fields that should be searched for a match.

![Screen recording of site search with grouped display type](https://github.com/etchuk/Etch.OrchardCore.Search/raw/main/docs/demo-site-search-grouped.gif)
