using EnemiesReturns;
using EnemiesReturns.Configuration;
using R2API;
using Summit;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemiesReturns.Enemies.LynxTribe.Totem;
using EnemiesReturns.Configuration.LynxTribe;
using EnemiesReturns.Enemies.Ifrit;

namespace Summit
{
    public class EnemiesReturnsCompat
    {
        public static void AddEnemies()
        {
            // Lynx Totem
            if (Summit.toggleLynxTotems.Value && General.EnableLynxTotem.Value)
            {
                var totemCard = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)TotemBody.SpawnCards.cscLynxTotemDefault,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = LynxTotem.SelectionWeight.Value,
                    minimumStageCompletions = LynxTotem.MinimumStageCompletion.Value
                };

                var totemHolder = new DirectorAPI.DirectorCardHolder
                {
                    Card = totemCard,
                    MonsterCategory = DirectorAPI.MonsterCategory.Champions
                };

                if (!LynxTotem.DefaultStageList.Value.Contains(Summit.RegularSceneName)) //Checking whether default stage list has Lynx Totems to avoid adding a duplicate spawn card
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(totemHolder, false, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                    //Log.Info("Lynx Totem added to Hollow Summit's spawn pool.");
                }
                if (!LynxTotem.DefaultStageList.Value.Contains(Summit.SimuSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(totemHolder, false, DirectorAPI.Stage.Custom, Summit.SimuSceneName);
                    //Log.Info("Lynx Totem added to Hollow Summit's simulacrum spawn pool.");
                }

            }

            // Ifrit
            if (Summit.toggleIfrit.Value && General.EnableIfrit.Value)
            {
                var card = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)IfritBody.SpawnCards.cscIfritDefault,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = Ifrit.SelectionWeight.Value,
                    minimumStageCompletions = Ifrit.MinimumStageCompletion.Value
                };

                var holder = new DirectorAPI.DirectorCardHolder
                {
                    Card = card,
                    MonsterCategory = DirectorAPI.MonsterCategory.Champions
                };

                if (!Ifrit.DefaultStageList.Value.Contains(Summit.SnowySceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(holder, false, DirectorAPI.Stage.Custom, Summit.SnowySceneName);
                    //Log.Info("Ifrit added to Hollow Summit's spawn pool.");
                }
                if (!Ifrit.DefaultStageList.Value.Contains(Summit.SimuSceneName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(holder, false, DirectorAPI.Stage.Custom, Summit.SimuSceneName);
                    //Log.Info("Ifrit added to Hollow Summit's simulacrum spawn pool.");
                }

            }

        }
    }
}