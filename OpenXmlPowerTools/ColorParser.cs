// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Nedev.ImageSharp;
using System.Collections.Generic;

namespace Nedev.OpenXmlPowerTools
{
    public static class ColorParser
    {
        private static readonly Dictionary<string, Color> ColorNames = new Dictionary<string, Color>(System.StringComparer.OrdinalIgnoreCase)
        {
            { "Black", Color.Black },
            { "White", Color.White },
            { "Red", Color.Red },
            { "Green", Color.Green },
            { "Blue", Color.Blue },
            { "Yellow", Color.Yellow },
            { "Cyan", Color.Cyan },
            { "Magenta", Color.Magenta },
            { "Gray", Color.Gray },
            { "Orange", Color.Orange },
            { "Pink", Color.Pink },
            { "Purple", Color.Purple },
            { "Violet", Color.Violet },
            { "Brown", Color.Brown },
            { "Maroon", Color.Maroon },
            { "Olive", Color.Olive },
            { "Navy", Color.Navy },
            { "Teal", Color.Teal },
            { "Silver", Color.Silver },
            { "Lime", Color.Lime },
            { "Aqua", Color.Aqua },
            { "Fuchsia", Color.Fuchsia },
            { "Transparent", Color.Transparent },
        };

        public static Color FromName(string name)
        {
            if (ColorNames.TryGetValue(name, out var color))
                return color;
            return Color.Black;
        }

        public static bool TryFromName(string name, out Color color)
        {
            return ColorNames.TryGetValue(name, out color);
        }

        public static bool IsValidName(string name)
        {
            return ColorNames.ContainsKey(name);
        }
    }
}
