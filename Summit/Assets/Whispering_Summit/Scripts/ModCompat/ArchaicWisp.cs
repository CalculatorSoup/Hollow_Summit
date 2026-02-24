using R2API;
using Summit;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchaicWisp;

namespace Summit
{
    public class ArchaicWispCompat
    {
        public static void AddEnemies()
        {
            var spawnInfo = new ArchaicWisp.StageSpawnInfo(Summit.RegularSceneName, 0);
            var spawnInfoLoop = new ArchaicWisp.StageSpawnInfo(Summit.RegularSceneName, 5);

            var simuSpawnInfo = new ArchaicWisp.StageSpawnInfo(Summit.SimuSceneName, 0);
            var simuSpawnInfoLoop = new ArchaicWisp.StageSpawnInfo(Summit.SimuSceneName, 5);

            var snowySpawnInfo = new ArchaicWisp.StageSpawnInfo(Summit.SnowySceneName, 0);
            var snowySpawnInfoLoop = new ArchaicWisp.StageSpawnInfo(Summit.SnowySceneName, 5);

            if (Summit.toggleArchaicWisp.Value && !ArchaicWispPlugin.StageList.Contains(spawnInfo) && !ArchaicWispPlugin.StageList.Contains(spawnInfoLoop)) //checking if the stage isn't already in the stage list to avoid adding an extra spawn card
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(ArchaicWisp.ArchaicWispContent.ArchaicWispCard, false, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                //Log.Info("Archaic Wisp added to Hollow Crest's spawn pool.");
                DirectorAPI.Helpers.RemoveExistingMonsterFromStage(DirectorAPI.Helpers.MonsterNames.GreaterWisp, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                //Log.Info("Greater Wisps removed from Hollow Crest's spawn pool.");
            }
            if (Summit.toggleArchaicWisp.Value && !ArchaicWispPlugin.StageList.Contains(snowySpawnInfo) && !ArchaicWispPlugin.StageList.Contains(snowySpawnInfoLoop))
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(ArchaicWisp.ArchaicWispContent.ArchaicWispCard, false, DirectorAPI.Stage.Custom, Summit.SnowySceneName);
                //Log.Info("Archaic Wisp added to Frozen Crest's spawn pool.");
                DirectorAPI.Helpers.RemoveExistingMonsterFromStage(DirectorAPI.Helpers.MonsterNames.GreaterWisp, DirectorAPI.Stage.Custom, Summit.SnowySceneName);
                //Log.Info("Greater Wisps removed from Frozen Crest's spawn pool.");
            }
            if (Summit.toggleArchaicWisp.Value && !ArchaicWispPlugin.StageList.Contains(simuSpawnInfo) && !ArchaicWispPlugin.StageList.Contains(simuSpawnInfoLoop))
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(ArchaicWisp.ArchaicWispContent.ArchaicWispCard, false, DirectorAPI.Stage.Custom, Summit.SimuSceneName);
                //Log.Info("Archaic Wisp added to Hollow Crest's Simulacrum spawn pool.");
                DirectorAPI.Helpers.RemoveExistingMonsterFromStage(DirectorAPI.Helpers.MonsterNames.GreaterWisp, DirectorAPI.Stage.Custom, Summit.SimuSceneName);
                //Log.Info("Greater Wisps removed from Hollow Crest's Simulacrum spawn pool.");
            }
        }
    }
}