using HG;
using MapSandbox.Content;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using RoR2.Networking;
using RoR2BepInExPack.GameAssetPaths;
using ShaderSwapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using static RoR2.Console;
using static UnityEngine.UI.Image;

namespace Summit.Content
{
    public static class SummitContent
    {

        internal const string ScenesAssetBundleFileName = "Scenes";
        internal const string AssetsAssetBundleFileName = "Assets";

        private static AssetBundle _scenesAssetBundle;
        private static AssetBundle _assetsAssetBundle;

        internal static UnlockableDef[] UnlockableDefs;
        internal static SceneDef[] SceneDefs;

        //Hollow Crest
        internal static SceneDef summitSceneDef;
        internal static Sprite summitSceneDefPreviewSprite;
        internal static Material summitBazaarSeer;

        //Brumal Crest
        internal static SceneDef snowySceneDef;
        internal static Sprite snowySceneDefPreviewSprite;
        internal static Material snowyBazaarSeer;

        //Simulacrum Hollow Crest
        internal static SceneDef simuSceneDef;
        internal static Sprite simuSceneDefPreviewSprite;
        internal static Material simuBazaarSeer;

        public static List<Material> SwappedMaterials = new List<Material>();

        internal static IEnumerator LoadAssetBundlesAsync(AssetBundle scenesAssetBundle, AssetBundle assetsAssetBundle, IProgress<float> progress, ContentPack contentPack)
        {
            _scenesAssetBundle = scenesAssetBundle;
            _assetsAssetBundle = assetsAssetBundle;
            
            var upgradeStubbedShaders = _assetsAssetBundle.UpgradeStubbedShadersAsync();
            while (upgradeStubbedShaders.MoveNext())
            {
                yield return upgradeStubbedShaders.Current;
            }
            
            yield return LoadAllAssetsAsync(assetsAssetBundle, progress, (Action<UnlockableDef[]>)((assets) =>
            {
                contentPack.unlockableDefs.Add(assets);
            }));
            

            
            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<Sprite[]>)((assets) =>
            {
                summitSceneDefPreviewSprite = assets.First(a => a.name == "texSUMScenePreview");
                snowySceneDefPreviewSprite = assets.First(a => a.name == "texSUMSnowyScenePreview");
                simuSceneDefPreviewSprite = assets.First(a => a.name == "texSUMScenePreview");
            }));
            

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<SceneDef[]>)((assets) =>
            {
                SceneDefs = assets;
                summitSceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == Summit.RegularSceneName);
                snowySceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == Summit.SnowySceneName);
                simuSceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == Summit.SimuSceneName);
                Log.Debug(summitSceneDef.nameToken);
                Log.Debug(snowySceneDef.nameToken);
                Log.Debug(simuSceneDef.nameToken);
                contentPack.sceneDefs.Add(assets);
            }));

            summitSceneDef.portalMaterial = R2API.StageRegistration.MakeBazaarSeerMaterial((Texture2D)summitSceneDef.previewTexture);
            snowySceneDef.portalMaterial = R2API.StageRegistration.MakeBazaarSeerMaterial((Texture2D)snowySceneDef.previewTexture);
            simuSceneDef.portalMaterial = R2API.StageRegistration.MakeBazaarSeerMaterial((Texture2D)simuSceneDef.previewTexture);

            var summitTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/MusicTrackDefs/muGameplayBase_09.asset");
            while (!summitTrackDefRequest.IsDone)
            {
                yield return null;
            }
            var summitBossTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/MusicTrackDefs/muSong16.asset");
            while (!summitBossTrackDefRequest.IsDone)
            {
                yield return null;
            }


            summitSceneDef.mainTrack = summitTrackDefRequest.Result;
            summitSceneDef.bossTrack = summitBossTrackDefRequest.Result;
            snowySceneDef.mainTrack = summitTrackDefRequest.Result;
            snowySceneDef.bossTrack = summitBossTrackDefRequest.Result;
            simuSceneDef.mainTrack = summitTrackDefRequest.Result;
            simuSceneDef.bossTrack = summitBossTrackDefRequest.Result;

            // if if if if if if if if if if if if if 
            if (Summit.enableRegular.Value && Summit.enableVariant.Value && Summit.loopExclusiveVariant.Value && !Summit.swapVariantPlaces.Value)
            {
                //both variants are enabled and the snowy variant only appears after looping
                summitSceneDef.loopedSceneDef = snowySceneDef;
                snowySceneDef.loopedSceneDef = null;
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(summitSceneDef, StageRegistration.defaultWeight, true, false);
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(snowySceneDef, StageRegistration.defaultWeight, false, true);
                Log.Debug("Hollow Summit and Frozen Summit registered. Hollow pre-loop, Frozen post-loop");
            } else if (Summit.enableRegular.Value && Summit.enableVariant.Value && Summit.loopExclusiveVariant.Value && Summit.swapVariantPlaces.Value)
            {
                //both variants are enabled but their places be swarped
                snowySceneDef.loopedSceneDef = summitSceneDef;
                summitSceneDef.loopedSceneDef = null;
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(summitSceneDef, StageRegistration.defaultWeight, false, true);
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(snowySceneDef, StageRegistration.defaultWeight, true, false);
                Log.Debug("Hollow Summit and Frozen Summit registered. Frozen pre-loop, Hollow post-loop");
            } else if (Summit.enableRegular.Value && Summit.enableVariant.Value && !Summit.loopExclusiveVariant.Value)
            {
                //both variants are enabled but the variant can appear at any time
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(summitSceneDef, (StageRegistration.defaultWeight / 2));
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(summitSceneDef, (StageRegistration.defaultWeight / 2));
                Log.Debug("Hollow Summit and Frozen Summit registered. Both stages appear pre & post loop (weight halved)");
            } else if (Summit.enableRegular.Value && !Summit.enableVariant.Value)
            {
                //only Hollow Summit is enabled
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(summitSceneDef);
                Log.Debug("Hollow Summit registered only");
            } else if (!Summit.enableRegular.Value && Summit.enableVariant.Value)
            {
                //only Frozen Summit is enabled
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(summitSceneDef);
                Log.Debug("Frozen Summit registered only");
            }


            if (Summit.enableSimulacrum.Value && Summit.simulacrumStage1.Value)
            {
                Simulacrum.RegisterSceneToSimulacrum(simuSceneDef);
            }
            else if (Summit.enableSimulacrum.Value && !Summit.simulacrumStage1.Value)
            {
                Simulacrum.RegisterSceneToSimulacrum(simuSceneDef, false);
            }
        }

internal static void Unload()
        {
            _assetsAssetBundle.Unload(true);
            _scenesAssetBundle.Unload(true);
        }

        private static IEnumerator LoadAllAssetsAsync<T>(AssetBundle assetBundle, IProgress<float> progress, Action<T[]> onAssetsLoaded) where T : UnityEngine.Object
        {
            var sceneDefsRequest = assetBundle.LoadAllAssetsAsync<T>();
            while (!sceneDefsRequest.isDone)
            {
                progress.Report(sceneDefsRequest.progress);
                yield return null;
            }

            onAssetsLoaded(sceneDefsRequest.allAssets.Cast<T>().ToArray());

            yield break;
        }
    }
}
