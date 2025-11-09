using System;
using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using T3MenuSharedApi;

namespace cs2_rockthevote
{
    public class T3MenuBridge : IPluginDependency<Plugin, Config>
    {
        private static IT3MenuManager? _manager;

        public static bool Available
        {
            get
            {
                TryResolve();
                return _manager != null;
            }
        }

        public static IT3MenuManager? Manager
        {
            get
            {
                TryResolve();
                return _manager;
            }
        }

        public void OnLoad(Plugin plugin)
        {
            TryResolve();
        }

        public void OnMapStart(string map)
        {
        }

        public void OnConfigParsed(Config config)
        {
        }

        public static void OpenMenu(CCSPlayerController player, string title, IEnumerable<(string Label, Action<CCSPlayerController> OnSelect, bool Disabled)> options, bool isSubMenu = false)
        {
            if (!Available)
                return;

            var menu = _manager!.CreateMenu(title, isSubMenu);
            foreach (var opt in options)
            {
                if (opt.Disabled)
                {
                    menu.AddTextOption(opt.Label);
                }
                else
                {
                    menu.AddOption(opt.Label, (p, o) => { opt.OnSelect(p); });
                }
            }
            _manager.OpenMainMenu(player, menu);
        }

        private static void TryResolve()
        {
            if (_manager == null)
            {
                try
                {
                    _manager = new PluginCapability<IT3MenuManager>("t3menu:manager").Get();
                }
                catch (KeyNotFoundException)
                {
                    _manager = null;
                }
                catch
                {
                    _manager = null;
                }
            }
        }
    }
}
