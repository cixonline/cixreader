using System;

namespace TheArtOfDev.HtmlRenderer
{
    public static class MonoHelper
    {
        // https://stackoverflow.com/a/32028919
        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;
    }
}