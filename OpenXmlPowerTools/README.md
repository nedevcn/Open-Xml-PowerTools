# Nedev.OpenXmlPowerTools

A cross-platform Open XML library for manipulating Office documents (Word, Excel, PowerPoint).

## Overview

This is a fork of [Open-Xml-PowerTools](https://github.com/OfficeDev/Open-Xml-PowerTools) with the following key changes:

- **Cross-platform support**: Replaced `System.Drawing.Common` dependency with `Nedev.ImageSharp` for true cross-platform compatibility
- **Modern .NET**: Targets .NET 8.0
- **NuGet packages**: Uses `Nedev.ImageSharp`, `Nedev.Fonts`, and `Nedev.ImageSharp.Drawing` for image and font handling

## Features

- **Document Builder**: Merge multiple Word documents, insert documents at specific locations
- **HTML Conversion**: Convert Word documents to HTML and vice versa
- **Document Comparison**: Compare two Word documents and identify differences
- **Spreadsheet Operations**: Read, write, and manipulate Excel workbooks
- **Presentation Builder**: Work with PowerPoint presentations
- **Text Replacement**: Find and replace text in Word documents
- **Markup Simplification**: Simplify Open XML markup for easier processing
- **Revision Handling**: Accept or reject revisions in Word documents

## Installation

```bash
dotnet add package Nedev.OpenXmlPowerTools --version 1.0.0
```

## Dependencies

- [DocumentFormat.OpenXml](https://www.nuget.org/packages/DocumentFormat.OpenXml/) (>= 3.4.1)
- [Nedev.ImageSharp](https://www.nuget.org/packages/Nedev.ImageSharp/) (>= 1.0.2)
- [Nedev.Fonts](https://www.nuget.org/packages/Nedev.Fonts/) (>= 1.0.0)
- [Nedev.ImageSharp.Drawing](https://www.nuget.org/packages/Nedev.ImageSharp.Drawing/) (>= 1.0.0)

## Quick Start

### Convert Word to HTML

```csharp
using Nedev.OpenXmlPowerTools;

var wmlDoc = new WmlDocument("document.docx");
var settings = new WmlToHtmlConverterSettings
{
    PageTitle = "My Document",
    FabricateCssClasses = true,
    CssClassPrefix = "doc-"
};
var html = WmlToHtmlConverter.ConvertToHtml(wmlDoc, settings);
Console.WriteLine(html.ToString());
```

### Merge Documents

```csharp
using Nedev.OpenXmlPowerTools;

var sources = new List<Source>
{
    new Source(new WmlDocument("part1.docx")),
    new Source(new WmlDocument("part2.docx"))
};
var mergedDoc = DocumentBuilder.BuildDocument(sources);
mergedDoc.SaveAs("merged.docx");
```

### Search and Replace Text

```csharp
using Nedev.OpenXmlPowerTools;

var doc = new WmlDocument("document.docx");
var modifiedDoc = TextReplacer.SearchAndReplace(doc, "old text", "new text", false);
modifiedDoc.SaveAs("modified.docx");
```

## License

This project is licensed under the MIT License - see the original [Open-Xml-PowerTools](https://github.com/OfficeDev/Open-Xml-PowerTools) for details.

## Credits

- Original [Open-Xml-PowerTools](https://github.com/OfficeDev/Open-Xml-PowerTools) by Microsoft
- [Nedev.ImageSharp](https://github.com/nedev/Nedev.ImageSharp) for cross-platform image processing
