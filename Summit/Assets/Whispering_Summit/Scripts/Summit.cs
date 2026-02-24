using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HG;
using R2API;
using R2API.AddressReferencedAssets;
using R2API.Utils;
using RoR2;
using On.RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using RoR2BepInExPack.GameAssetPathsBetter;
using Summit.Content;
using Summit.ModChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Diagnostics;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using static Summit.ModChecks.IsClayMen;
//Copied from a private Unity project I use for testing maps copied from Ancient Observatory copied from Wetland Downpour copied from Fogbound Lagoon copied from Nuketown


#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace Summit
{
    [BepInPlugin(GUID, Name, Version)]
    public class Summit : BaseUnityPlugin
    {
        public const string Author = "wormsworms";

        public const string Name = "Hollow_Crest";

        public const string Version = "0.1.0";

        public const string GUID = Author + "." + Name;

        public const string RegularSceneName = "hollowsummit_wormsworms";

        public const string SnowySceneName = "hollowsummitnight_wormsworms";

        public const string SimuSceneName = "itsummit_wormsworms";

        public static Summit instance;

        public static ConfigEntry<bool> enableRegular;
        public static ConfigEntry<bool> enableVariant;
        public static ConfigEntry<bool> loopExclusiveVariant;
        public static ConfigEntry<bool> swapVariantPlaces;
        public static ConfigEntry<bool> enableSimulacrum;
        public static ConfigEntry<bool> simulacrumStage1;
        public static ConfigEntry<bool> toggleAncientWisp;
        public static ConfigEntry<bool> toggleArchaicWisp;
        public static ConfigEntry<bool> toggleClayMen;
        public static ConfigEntry<bool> toggleLynxTotems;
        public static ConfigEntry<bool> toggleIotaConstructs;
        public static ConfigEntry<bool> toggleDeltaConstructs;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);

            ConfigSetup();

            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;

            RoR2.Language.collectLanguageRootFolders += CollectLanguageRootFolders;

            RoR2.RoR2Application.onLoadFinished += AddModdedEnemies;

            SceneManager.sceneLoaded += SceneSetup;

            RoR2.Run.onRunStartGlobal += InitializeBazaarSeerValues;

            On.RoR2.Run.PickNextStageScene += SwapBazaarFilters;

        }

        public void InitializeBazaarSeerValues(RoR2.Run run)
        {
            //filtering variants out of bazaar manually to prevent them appearing when they should not. vanilla does not do this
            if (Summit.enableRegular.Value && Summit.enableVariant.Value && Summit.loopExclusiveVariant.Value && !Summit.swapVariantPlaces.Value)
            {
                RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = false;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(SnowySceneName).filterOutOfBazaar = true;
            }
            else if (Summit.enableRegular.Value && Summit.enableVariant.Value && Summit.loopExclusiveVariant.Value && Summit.swapVariantPlaces.Value)
            {
                RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = true;
                RoR2.SceneCatalog.GetSceneDefFromSceneName(SnowySceneName).filterOutOfBazaar = false;
            }
        }

        public void SwapBazaarFilters(On.RoR2.Run.orig_PickNextStageScene orig, RoR2.Run self, WeightedSelection<RoR2.SceneDef> choices)
        {
            orig.Invoke(self, choices);

            if (Summit.enableRegular.Value && Summit.enableVariant.Value && Summit.loopExclusiveVariant.Value && !Summit.swapVariantPlaces.Value)
            {
                //swap bazaar values after clearing 5 stages
                if (RoR2.Run.instance.stageClearCount >= 5 && RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar != true)
                {
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = true;
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(SnowySceneName).filterOutOfBazaar = false;
                    Log.Debug("Swapped bazaar filter-out values (hollow TRUE, frozen FALSE)");
                }
            }
            else if ((Summit.enableRegular.Value && Summit.enableVariant.Value && Summit.loopExclusiveVariant.Value && Summit.swapVariantPlaces.Value))
            {
                if (RoR2.Run.instance.stageClearCount >= 5 && RoR2.SceneCatalog.GetSceneDefFromSceneName(SnowySceneName).filterOutOfBazaar != true)
                {
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(RegularSceneName).filterOutOfBazaar = false;
                    RoR2.SceneCatalog.GetSceneDefFromSceneName(SnowySceneName).filterOutOfBazaar = true;
                    Log.Debug("Swapped bazaar filter-out values (hollow FALSE, frozen TRUE. my favoritest movie is forozen i love ana and els a)");
                }
            }
        }



        public static void AddModdedEnemies()
        {
            if (IsEnemiesReturns.enabled)
            {
                EnemiesReturnsCompat.AddEnemies(); //Lynx Totem
            }

            if (IsSandswept.enabled)
            {
                SandsweptCompat.AddEnemies(); //Delta Construct
            }

            if (IsAncientWisp.enabled)
            {
                AncientWispCompat.AddEnemies();
            }

            if (IsArchaicWisp.enabled)
            {
                ArchaicWispCompat.AddEnemies();
            }

            if (IsClayMen.enabled)
            {
                ClayMenCompat.AddEnemies();
            }

            if (IsRecoveredAndReformed.enabled || IsRecoveredAndReformedStripped.enabled)
            {
                RecoveredAndReformedCompat.AddEnemies(); //Iota Construct
            }
        }

        private void Destroy()
        {
            RoR2.Language.collectLanguageRootFolders -= CollectLanguageRootFolders;
        }

        private static void GiveToRoR2OurContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(new ContentProvider());
        }

        public void CollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(base.Info.Location), "Language"));
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(base.Info.Location), "Plugins/Language"));
        }

        private void SceneSetup(Scene newScene, LoadSceneMode loadSceneMode)
        {
            if (newScene.name == RegularSceneName || newScene.name == SnowySceneName || newScene.name == SimuSceneName)
            {

                Transform ruinsHolder = GameObject.Find("Ruins n Stuff").transform;
                for (int i = 0; i < ruinsHolder.childCount; i++)
                {
                    if (ruinsHolder.GetChild(i).gameObject.name.Contains("Xi Construct"))
                    {
                        ruinsHolder.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false); // disabling point lights attached to fallen xi constructs
                    }
                }
            }

            if (newScene.name == SnowySceneName)
            {

                Transform treeHolder = GameObject.Find("Trees").transform;
                for (int i = 0; i < treeHolder.childCount; i++)
                {
                    if (treeHolder.GetChild(i).GetChild(0).gameObject.name.Contains("NestBranch"))
                    {
                        treeHolder.GetChild(i).GetChild(0).GetChild(0).GetChild(0).gameObject.layer = 11; // setting layer for branches from Default to World
                    }
                }
            }

            if (newScene.name == RegularSceneName || newScene.name == SnowySceneName)
            {
                GameObject moonMesh = GameObject.Find("ShatteredMoonMesh");
                moonMesh.layer = 9; // moving this to the NoCollision layer so the "Moon Light" in the stage can illuminate it
                GameObject moonHolder = GameObject.Find("MoonHolder");
                moonHolder.transform.localScale = Vector3.one * 1.5f; // upscaling the moon
                GameObject donut = GameObject.Find("AtmosphereDonut");
                donut.SetActive(false); //removing halation (becomes offset and looks kinda bad when the moon is upscaled)
            }
        }

        private void ConfigSetup()
        {
            enableRegular =
                base.Config.Bind<bool>("00 - Hollow Summit",
                                       "Enable Hollow Summit",
                                       true,
                                       "If true, Hollow Summit can appear in runs.");
            enableVariant =
                base.Config.Bind<bool>("01 - Frozen Summit",
                                       "Enable Frozen Summit",
                                       true,
                                       "If true, Frozen Summit can appear in runs. If Hollow Summit is disabled, the Loop Variant and Swap Places values are ignored and Frozen Summit can appear at any time, effectively replacing it.");
            loopExclusiveVariant =
                base.Config.Bind<bool>("01 - Frozen Summit",
                                       "Loop Variant",
                                       true,
                                       "If true, Frozen Summit replaces Hollow Summit after looping. If false, it can appear at any time (both variants will have their weight halved to prevent making Summit more common than other stages).");
            swapVariantPlaces =
                base.Config.Bind<bool>("01 - Frozen Summit",
                                       "Swap Places with Hollow Summit",
                                       false,
                                       "If true, Frozen Summit will appear before looping and Hollow Summit will replace it after looping.");
            enableSimulacrum =
                base.Config.Bind<bool>("02 - Simulacrum Variant",
                                       "Enable Simulacrum Variant",
                                       true,
                                       "If true, Hollow Summit can appear in the Simulacrum.");
            simulacrumStage1 =
                base.Config.Bind<bool>("02 - Simulacrum Variant",
                                       "Enable Simulacrum Variant on Stage 1",
                                       true,
                                       "If true, Hollow Summit can appear as the first stage in the Simulacrum. If false, it can only appear on Stage 2 or higher, like Commencement.");
            toggleAncientWisp =
                base.Config.Bind<bool>("03 - Modded Enemies - Ancient Wisp",
                                       "Enable Ancient Wisps",
                                       true,
                                       "If true, Ancient Wisps can appear in Hollow Summit and Frozen Summit.");
            toggleArchaicWisp =
                base.Config.Bind<bool>("04 - Modded Enemies - Archaic Wisp",
                                       "Enable Archaic Wisps",
                                       true,
                                       "If true, Archaic Wisps can appear in Hollow Summit and Frozen Summit (replaces Greater Wisps).");
            toggleClayMen =
                base.Config.Bind<bool>("05 - Modded Enemies - Clay Men",
                                       "Enable Clay Men",
                                       true,
                                       "If true, Clay Men can appear in Hollow Summit (replaces Imps).");
            toggleLynxTotems =
                base.Config.Bind<bool>("06 - Modded Enemies - EnemiesReturns",
                                       "Enable Lynx Totems",
                                       true,
                                       "If true, Lynx Totems can appear in Hollow Summit.");
            toggleIotaConstructs =
                base.Config.Bind<bool>("07 - Modded Enemies - RecoveredAndReformed",
                                       "Enable Iota Constructs",
                                       true,
                                       "If true, Iota Constructs can appear in Hollow Summit and Frozen Summit.");
            toggleDeltaConstructs =
                base.Config.Bind<bool>("08 - Modded Enemies - Sandswept",
                                       "Enable Delta Constructs",
                                       true,
                                       "If true, Delta Costructs can appear in Hollow Summit.");
        }




    }
}
