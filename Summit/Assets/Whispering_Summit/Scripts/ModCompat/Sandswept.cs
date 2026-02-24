using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HG.Reflection;
using Microsoft.CodeAnalysis;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Navigation;
using RoR2BepInExPack.GameAssetPaths.Version_1_35_0;
using RoR2BepInExPack.GameAssetPathsBetter;
using Sandswept;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.DirectorAPI;
using static RoR2.Navigation.MapNodeGroup;

namespace Summit
{
    public class SandsweptCompat
    {
        public static CharacterSpawnCard CannonballJellyfishCard;

        //go through config files to find the intended monster's "enabled" config, then return its value as a string. return "false" if the config entry can't be found
        public static bool FindEnabledConfig(string configName)
        {
            var configArray = Sandswept.Main.config.GetConfigEntries();
            foreach (var entry in configArray)
            {
                var entryString = entry.Definition.ToString();
                if (entryString == configName)
                {
                    Log.Debug("Found Config Entry (Enable)");
                    return (bool) entry.BoxedValue;
                }
            }
            return false;
        }
        public static int FindCreditConfig(string configName)
        {
            var configArray = Sandswept.Main.config.GetConfigEntries();
            foreach (var entry in configArray)
            {
                var entryString = entry.Definition.ToString();
                if (entryString == configName)
                {
                    //Log.Debug("Found Config Entry (Credit)");
                    return (int) entry.DefaultValue;
                }
            }
            return 130;
        }

        public static void AddEnemies()
        {
            // Delta Construct. Largely copied from MoreVieldsOptions
            var deltaConstructValue = FindEnabledConfig("Enemies :: Delta Construct.Enabled");

            if (Summit.toggleDeltaConstructs.Value && deltaConstructValue) {
                CharacterSpawnCard DeltaConstructCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
                DeltaConstructCard.name = "cscMVODeltaConstruct";
                DeltaConstructCard.prefab = Main.assets.LoadAsset<GameObject>("DeltaConstructMaster.prefab");
                DeltaConstructCard.sendOverNetwork = true;
                DeltaConstructCard.hullSize = (HullClassification)0;
                DeltaConstructCard.nodeGraphType = (GraphType)1;
                DeltaConstructCard.requiredFlags = (NodeFlags)0;
                DeltaConstructCard.forbiddenFlags = (NodeFlags)2;
                DeltaConstructCard.directorCreditCost = 45;
                DeltaConstructCard.occupyPosition = false;
                DeltaConstructCard.loadout = new SerializableLoadout();
                DeltaConstructCard.noElites = false;
                DeltaConstructCard.forbiddenAsBoss = false;


                DirectorCardHolder cardHolder = new DirectorCardHolder
                    {
                    Card = new DirectorCard
                    {
                        spawnCard = (SpawnCard)(object)DeltaConstructCard,
                        selectionWeight = 1
                    },
                    MonsterCategory = MonsterCategory.Minibosses
                };
                DirectorAPI.Helpers.AddNewMonsterToStage(cardHolder, false, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                DirectorAPI.Helpers.AddNewMonsterToStage(cardHolder, false, DirectorAPI.Stage.Custom, Summit.SnowySceneName);
                DirectorAPI.Helpers.AddNewMonsterToStage(cardHolder, false, DirectorAPI.Stage.Custom, Summit.SimuSceneName);
                //Log.Debug("Delta Construct added to Hollow Crest/Frozen Crest spawn pools.");
            };
        }
    } 
}