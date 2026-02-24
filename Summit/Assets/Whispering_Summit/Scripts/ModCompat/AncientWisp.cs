using R2API;
using Summit;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AncientWisp;

namespace Summit
{
    public class AncientWispCompat
    {
        public static void AddEnemies()
        {
            var spawnInfo = new AncientWisp.StageSpawnInfo(Summit.RegularSceneName, 0);
            var spawnInfoLoop = new AncientWisp.StageSpawnInfo(Summit.RegularSceneName, 5);

            var simuSpawnInfo = new AncientWisp.StageSpawnInfo(Summit.SimuSceneName, 0);
            var simuSpawnInfoLoop = new AncientWisp.StageSpawnInfo(Summit.SimuSceneName, 5);

            var snowySpawnInfo = new AncientWisp.StageSpawnInfo(Summit.SnowySceneName, 0);
            var snowySpawnInfoLoop = new AncientWisp.StageSpawnInfo(Summit.SnowySceneName, 5);

            if (Summit.toggleArchaicWisp.Value && !AncientWispPlugin.StageList.Contains(spawnInfo) && !AncientWispPlugin.StageList.Contains(spawnInfoLoop)) //checking if the stage isn't already in the stage list to avoid adding an extra spawn card
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(AncientWisp.AWContent.AncientWispCard, false, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                //Log.Info("Ancient Wisp added to Hollow Crest's spawn pool.");
            }
            if (Summit.toggleArchaicWisp.Value && !AncientWispPlugin.StageList.Contains(snowySpawnInfo) && !AncientWispPlugin.StageList.Contains(snowySpawnInfoLoop))
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(AncientWisp.AWContent.AncientWispCard, false, DirectorAPI.Stage.Custom, Summit.SnowySceneName);
                //Log.Info("Ancient Wisp added to Frozen Crest's spawn pool.");
            }
            if (Summit.toggleArchaicWisp.Value && !AncientWispPlugin.StageList.Contains(simuSpawnInfo) && !AncientWispPlugin.StageList.Contains(simuSpawnInfoLoop))
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(AncientWisp.AWContent.AncientWispCard, false, DirectorAPI.Stage.Custom, Summit.SimuSceneName);
                //Log.Info("Ancient Wisp added to Hollow Crest's Simulacrum spawn pool.");
            }
        }
    }
}