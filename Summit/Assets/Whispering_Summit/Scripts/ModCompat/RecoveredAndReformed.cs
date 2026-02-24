using R2API;
using RecoveredAndReformed;
using RoR2;
using Summit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Summit
{
    public class RecoveredAndReformedCompat
    {
        public static void AddEnemies()
        {
            if (Summit.toggleIotaConstructs.Value)
            {
                var majorConstructSpawnCard = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/DLC1/MajorAndMinorConstruct/cscMajorConstruct.asset").WaitForCompletion();

                var majorConstructCard = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)majorConstructSpawnCard,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = 1,
                    minimumStageCompletions = RecoveredAndReformed.Main.MajorConstructMinStage.Value
                };

                var majorConstructHolder = new DirectorAPI.DirectorCardHolder
                {
                    Card = majorConstructCard,
                    MonsterCategory = DirectorAPI.MonsterCategory.Champions
                };

                DirectorAPI.Helpers.AddNewMonsterToStage(majorConstructHolder, false, DirectorAPI.Stage.Custom, Summit.RegularSceneName);
                DirectorAPI.Helpers.AddNewMonsterToStage(majorConstructHolder, false, DirectorAPI.Stage.Custom, Summit.SnowySceneName);
                DirectorAPI.Helpers.AddNewMonsterToStage(majorConstructHolder, false, DirectorAPI.Stage.Custom, Summit.SimuSceneName);
                //Log.Info("Iota Construct added to Hollow/Frigid Crest's spawn pools.");

            }
        }
    }
}