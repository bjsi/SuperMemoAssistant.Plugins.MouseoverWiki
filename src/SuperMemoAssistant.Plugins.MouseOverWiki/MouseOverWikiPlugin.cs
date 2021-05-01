using Anotar.Serilog;
using SuperMemoAssistant.Plugins.MouseOverWiki;
using SuperMemoAssistant.Services;
using SuperMemoAssistant.Services.IO.HotKeys;
using SuperMemoAssistant.Services.Sentry;
using SuperMemoAssistant.Services.UI.Configuration;
using System.Diagnostics.CodeAnalysis;

#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   4/29/2021 6:07:25 PM
// Modified By:  james

#endregion




namespace SuperMemoAssistant.Plugins.MouseoverWiki
{
  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
  public class MouseoverWikiPlugin : SentrySMAPluginBase<MouseoverWikiPlugin>
  {
    #region Constructors

    /// <inheritdoc />
    public MouseoverWikiPlugin() : base("Enter your Sentry.io api key (strongly recommended)") { }

    #endregion




    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "MouseoverWiki";

    /// <inheritdoc />
    public override bool HasSettings => true;

    private const string WikipediaRegex = @"^https?\:\/\/([\w\.]+)wikipedia.org\/wiki\/([\w]+)+";
    private ContentService _contentProvider => new ContentService();
    private MouseoverWikiCfg Config { get; set; }


    #endregion

    private void LoadConfig()
    {
      Config = Svc.Configuration.Load<MouseoverWikiCfg>() ?? new MouseoverWikiCfg();
    }


    /// <inheritdoc />
    protected override void OnSMStarted(bool wasSMAlreadyStarted)
    {
      LoadConfig();
      if (!this.RegisterProvider(Name, new string[] { WikipediaRegex }, _contentProvider))
      {
        LogTo.Error($"Failed to Register provider {Name} with MouseoverPopup Service");
        return;
      }

      LogTo.Debug($"Successfully registered provider {Name} with MouseoverPopup Service");
      base.OnSMStarted(wasSMAlreadyStarted);
    }

    public override void ShowSettings()
    {
      ConfigurationWindow.ShowAndActivate("MouseoverWiki", HotKeyManager.Instance, Config);
    }
  }
}
