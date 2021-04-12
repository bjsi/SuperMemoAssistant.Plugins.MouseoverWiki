using Anotar.Serilog;
using MouseoverPopupInterfaces;
using PluginManager.Interop.Sys;
using SuperMemoAssistant.Extensions;
using SuperMemoAssistant.Interop.SuperMemo.Elements.Builders;
using SuperMemoAssistant.Plugins.MouseOverWiki.Models;
using SuperMemoAssistant.Sys.Remoting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseOverWiki
{
  [Serializable]
  public class ContentService : PerpetualMarshalByRefObject, IMouseoverContentProvider
  {

    private string ArticleExtractUrl = @"https://{0}.wikipedia.org/api/rest_v1/page/summary/{1}";
    private readonly HttpClient _httpClient;

    public ContentService()
    {
      _httpClient = new HttpClient();
      _httpClient.DefaultRequestHeaders.Accept.Clear();
      _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Dispose()
    {
      _httpClient?.Dispose();
    }

    public RemoteTask<PopupContent> FetchHtml(RemoteCancellationToken ct, string url)
    {
      try
      {
        if (!url.IsDesktopWikipediaUrl())
          return null;

        string title = url.ParseArticleTitle();
        string language = url.ParseArticleLanguage();
        return string.IsNullOrEmpty(title)
          ? null
          : GetWikipediaExtractAsync(ct, title, language);

      } 
      catch (TaskCanceledException) { }
      catch (Exception ex)
      {
        LogTo.Error($"Failed to FetchHtml for url {url} with exception {ex}");
        throw;
      }

      return null;
    }

    private async Task<PopupContent> GetWikipediaExtractAsync(RemoteCancellationToken ct, string title, string language)
    {
      string url = string.Format(ArticleExtractUrl, language, title);
      string response = await GetAsync(ct.Token(), url);
      var extract = response?.Deserialize<WikiExtract>();
      return CreatePopupHtml(extract);
    }

    private PopupContent CreatePopupHtml(WikiExtract extract)
    {

      if (extract == null)
        return null;

      string html = @"
          <html>
            <body>
              <h1>{0}</h1>
              <h4>{1}</h4>
              <div>
                {2}
                <p>{3}</p>
              </div>
            </body>
          </html>";

      string img = extract.thumbnail.IsNull() || extract.thumbnail.source.IsNullOrEmpty()
        ? string.Empty
        : $@"<p style=""float: left;"">
              <img src=""{extract.thumbnail.source}"" border=""3px"" >
             </p>";

      string title = extract.displaytitle;
      string desc = extract.description;
      string body = extract.extract_html;

      string content = string.Format(html, title, desc, img, body);

      // Add references
      var refs = new References();
      refs.Author = string.Empty;
      refs.Link = extract.content_urls.desktop.page;
      refs.Source = "Wikipedia";
      refs.Title = extract.displaytitle;

      return new PopupContent(refs, content, true, browserQuery: refs.Link);

    }

    private async Task<string> GetAsync(CancellationToken ct, string url)
    {
      HttpResponseMessage responseMsg = null;

      try
      {
        responseMsg = await _httpClient.GetAsync(url, ct);

        if (responseMsg.IsSuccessStatusCode)
        {
          return await responseMsg.Content.ReadAsStringAsync();
        }
        else
        {
          return null;
        }
      }
      catch (HttpRequestException)
      {
        if (responseMsg != null && responseMsg.StatusCode == System.Net.HttpStatusCode.NotFound)
          return null;
        else
          throw;
      }
      catch (OperationCanceledException)
      {
        return null;
      }
      finally
      {
        responseMsg?.Dispose();
      }
    }
  }
}
