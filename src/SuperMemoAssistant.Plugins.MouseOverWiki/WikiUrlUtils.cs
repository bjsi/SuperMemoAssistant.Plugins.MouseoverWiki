using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseOverWiki
{
  public static class WikiUrlUtils
  {
    // Example url: https://en.wikipedia.org/wiki/Hello_World
    public static bool IsDesktopWikipediaUrl(this string url)
    {

      if (string.IsNullOrEmpty(url))
        return false;

      if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        return false;

      Uri uri = new Uri(url);

      string[] splitUri = uri?.Host?.Split('.');
      if (splitUri == null || splitUri.Length != 3)
        return false;

      return splitUri[1] == "wikipedia" ? true : false;

    }

    // TODO: Return the segment after /wiki instead?
    public static string ParseArticleTitle(this string url)
    {
      return new Uri(url)?.Segments?.LastOrDefault();
    }

    public static string ParseArticleLanguage(this string url)
    {
      return url.IsDesktopWikipediaUrl()
        ? new Uri(url).Host.Split('.')[0]
        : null; 
    }
  }
}
