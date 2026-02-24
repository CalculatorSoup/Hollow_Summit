using BepInEx.Bootstrap;
using ClayMen;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.AddressableAssets;
using static R2API.DirectorAPI;

namespace Summit.ModChecks
{
    public class IsEnemiesReturns
    {
        private static bool? _enabled;
        public static bool enabled


        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Viliger.EnemiesReturns");
                }
                return (bool)_enabled;
            }

        }
    }

    public class IsSandswept
    {
        private static bool? _enabled;
        public static bool enabled


        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = Chainloader.PluginInfos.ContainsKey("com.TeamSandswept.Sandswept");
                }
                return (bool)_enabled;
            }

        }
    }

    public class IsClayMen
    {
        private static bool? _enabled;
        public static bool enabled


        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = Chainloader.PluginInfos.ContainsKey("com.Moffein.ClayMen");
                }
                return (bool)_enabled;
            }

        }

        public class IsArchaicWisp
        {
            private static bool? _enabled;
            public static bool enabled


            {
                get
                {
                    if (_enabled == null)
                    {
                        _enabled = Chainloader.PluginInfos.ContainsKey("com.Moffein.ArchaicWisp");
                    }
                    return (bool)_enabled;
                }

            }
        }

        public class IsAncientWisp
        {
            private static bool? _enabled;
            public static bool enabled


            {
                get
                {
                    if (_enabled == null)
                    {
                        _enabled = Chainloader.PluginInfos.ContainsKey("com.Moffein.AncientWisp");
                    }
                    return (bool)_enabled;
                }

            }
        }

        public class IsRecoveredAndReformed
        {
            private static bool? _enabled;
            public static bool enabled


            {
                get
                {
                    if (_enabled == null)
                    {
                        _enabled = Chainloader.PluginInfos.ContainsKey("prodzpod.RecoveredAndReformed");
                    }
                    return (bool)_enabled;
                }

            }
        }

        public class IsRecoveredAndReformedStripped
        {
            private static bool? _enabled;
            public static bool enabled


            {
                get
                {
                    if (_enabled == null)
                    {
                        _enabled = Chainloader.PluginInfos.ContainsKey("Phreel.RecoveredAndReformedStripped");
                    }
                    return (bool)_enabled;
                }

            }
        }
    }
}