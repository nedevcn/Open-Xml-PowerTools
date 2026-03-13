// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/***************************************************************************

Copyright (c) Microsoft Corporation 2010.

This code is licensed using the Microsoft Public License (Ms-PL).  The text of the license
can be found here:

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx

***************************************************************************/

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using Nedev.ImageSharp.Formats.Png;
using Nedev.ImageSharp.Formats.Gif;
using Nedev.ImageSharp.Formats.Bmp;
using Nedev.ImageSharp.Formats.Jpeg;
using System.Collections.Generic;

class WmlToHtmlConverterHelper
{
    static void Main(string[] args)
    {
        var n = DateTime.Now;
        var tempDi = new DirectoryInfo(string.Format("ExampleOutput-{0:00}-{1:00}-{2:00}-{3:00}{4:00}{5:00}", n.Year - 2000, n.Month, n.Day, n.Hour, n.Minute, n.Second));
        tempDi.Create();

        /*
         * This example loads each document into a byte array, then into a memory stream, so that the document can be opened for writing without
         * modifying the source document.
         */
        foreach (var file in Directory.GetFiles("../../", "*.docx"))
        {
            ConvertToHtml(file, tempDi.FullName);
        }
    }

    public static void ConvertToHtml(string file, string outputDirectory)
    {
        var fi = new FileInfo(file);
        Console.WriteLine(fi.Name);
        byte[] byteArray = File.ReadAllBytes(fi.FullName);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            memoryStream.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wDoc = WordprocessingDocument.Open(memoryStream, true))
            {
                var destFileName = new FileInfo(fi.Name.Replace(".docx", ".html"));
                if (outputDirectory != null && outputDirectory != string.Empty)
                {
                    DirectoryInfo di = new DirectoryInfo(outputDirectory);
                    if (!di.Exists)
                    {
                        throw new OpenXmlPowerToolsException("Output directory does not exist");
                    }
                    destFileName = new FileInfo(Path.Combine(di.FullName, destFileName.Name));
                }
                var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length - 5) + "_files";
                int imageCounter = 0;

                var pageTitle = fi.FullName;
                var part = wDoc.CoreFilePropertiesPart;
                if (part != null)
                {
                    pageTitle = (string)part.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? fi.FullName;
                }

                // TODO: Determine max-width from size of content area.
                WmlToHtmlConverterSettings settings = new WmlToHtmlConverterSettings()
                {
                    AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                    PageTitle = pageTitle,
                    FabricateCssClasses = true,
                    CssClassPrefix = "pt-",
                    RestrictToSupportedLanguages = false,
                    RestrictToSupportedNumberingFormats = false,
                    ImageHandler = imageInfo =>
                    {
                        ++imageCounter;
                        string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                        string mimeType;
                        string base64 = null;
                        try
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                switch (extension)
                                {
                                    case "png":
                                        imageInfo.Bitmap.Save(ms, new PngEncoder());
                                        mimeType = "image/png";
                                        break;
                                    case "gif":
                                        imageInfo.Bitmap.Save(ms, new GifEncoder());
                                        mimeType = "image/gif";
                                        break;
                                    case "bmp":
                                        imageInfo.Bitmap.Save(ms, new BmpEncoder());
                                        mimeType = "image/bmp";
                                        break;
                                    case "jpeg":
                                    case "jpg":
                                        imageInfo.Bitmap.Save(ms, new JpegEncoder());
                                        mimeType = "image/jpeg";
                                        break;
                                    case "tiff":
                                        extension = "gif";
                                        imageInfo.Bitmap.Save(ms, new GifEncoder());
                                        mimeType = "image/gif";
                                        break;
                                    default:
                                        return null;
                                }
                                var ba = ms.ToArray();
                                base64 = System.Convert.ToBase64String(ba);
                            }
                        }
                        catch (System.Runtime.InteropServices.ExternalException)
                        {
                            return null;
                        }

                        string imageSource = string.Format("data:{0};base64,{1}", mimeType, base64);

                        XElement img = new XElement(Xhtml.img,
                            new XAttribute(NoNamespace.src, imageSource),
                            imageInfo.ImgStyleAttribute,
                            imageInfo.AltText != null ?
                                new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                        return img;
                    }
                };
                XElement htmlElement = WmlToHtmlConverter.ConvertToHtml(wDoc, settings);

                // Produce HTML document with <!DOCTYPE html > declaration to tell the browser
                // we are using HTML5.
                var html = new XDocument(
                    new XDocumentType("html", null, null, null),
                    htmlElement);

                // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                // for detailed explanation.
                //
                // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                // must do it correctly, or entities will not be serialized properly.

                var htmlString = html.ToString(SaveOptions.DisableFormatting);
                File.WriteAllText(destFileName.FullName, htmlString, Encoding.UTF8);
            }
        }
    }
}
