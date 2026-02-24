using R2API;
using Summit;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClayMen;

namespace Summit
{
    public class ClayMenCompat
    {
        public static void AddEnemies()
        {
            var spawnInfo = new ClayMen.StageSpawnInfo(Summit.RegularSceneName, 0);
            var spawnInfoLoop = new ClayMen.StageSpawnInfo(Summit.RegularSceneName, 5);

            if (Summit.toggleClayMen.Value && !ClayMenPlugin.StageList.Contains(spawnInfo) && !ClayMenPlugin.StageList.Contains(spawnInfoLoop)) //checking if the stage isn't already in the stage list to avoid adding an extra spawn card
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(ClayMen.ClayMenContent.ClayManCard, false, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                //Log.Info("Clay Man added to Hollow Crest's spawn pool.");
                DirectorAPI.Helpers.RemoveExistingMonsterFromStage(DirectorAPI.Helpers.MonsterNames.Imp, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                //Log.Info("Imps removed from Hollow Crest's spawn pool.");
            }
        }
    }
}