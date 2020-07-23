﻿using MouseoverPopup.Interop;
using SuperMemoAssistant.Interop.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseOverWiki
{
  public static class PluginBaseEx
  {
    public static bool RegisterProvider<T>(this SMAPluginBase<T> plugin, string name, string[] urlRegexes, IMouseoverContentProvider provider) where T : SMAPluginBase<T>
    {
      var svc = plugin.GetService<IMouseoverSvc>();

      if (svc == null)
        return false;

      return svc.RegisterProvider(name, urlRegexes, provider);
    }
  }
}
