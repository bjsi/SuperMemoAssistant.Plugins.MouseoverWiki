using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseOverWiki.Models
{
  public enum WindowType
  {
    wikipedia,
    wiktionary
  }

  public class Titles
  {
    public string canonical { get; set; }
    public string normalized { get; set; }
    public string display { get; set; }
  }

  public class Thumbnail
  {
    public string source { get; set; }
    public int width { get; set; }
    public int height { get; set; }
  }

  public class Originalimage
  {
    public string source { get; set; }
    public int width { get; set; }
    public int height { get; set; }
  }

  public class Desktop
  {
    public string page { get; set; }
    public string revisions { get; set; }
    public string edit { get; set; }
    public string talk { get; set; }
  }

  public class Mobile
  {
    public string page { get; set; }
    public string revisions { get; set; }
    public string edit { get; set; }
    public string talk { get; set; }
  }

  public class ContentUrls
  {
    public Desktop desktop { get; set; }
    public Mobile mobile { get; set; }
  }

  public class ApiUrls
  {
    public string summary { get; set; }
    public string metadata { get; set; }
    public string references { get; set; }
    public string media { get; set; }
    public string edit_html { get; set; }
    public string talk_page_html { get; set; }
  }

  public class WikiExtract
  {
    public string type { get; set; }
    public string title { get; set; }
    public string displaytitle { get; set; }
    public string wikibase_item { get; set; }
    public Titles titles { get; set; }
    public int pageid { get; set; }
    public Thumbnail thumbnail { get; set; }
    public Originalimage originalimage { get; set; }
    public string lang { get; set; }
    public string dir { get; set; }
    public string revision { get; set; }
    public string tid { get; set; }
    public DateTime timestamp { get; set; }
    public string description { get; set; }
    public string description_source { get; set; }
    public ContentUrls content_urls { get; set; }
    public ApiUrls api_urls { get; set; }
    public string extract { get; set; }
    public string extract_html { get; set; }
  }
}
