using System.Collections.Generic;
using JetBrains.Annotations;
using Levels;
using RSG;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace hScenes.Levels
{
	internal static partial class LevelManagement
	{
		public static IPromise PromiseLoadScene(string sceneName, LoadSceneMode additive)
		{
			var promise = new Promise();
			LoadSceneAsync(sceneName, additive, promise);
			return promise;
		}
		
		public static IPromise ChangeLevel( AdditiveScene loadScene)
		{
			var promise = new Promise();
			
			LoadLevel(loadScene)
				.Then(() => promise.Resolve())
				.Catch(Debug.LogError);

			return promise;
		}

		public static void UpdateLevel( AdditiveScene additiveScene, [NotNull] IPendingPromise promise)
		{
			var listOfNewScenes = FindWhichLevelsNeedAdded(additiveScene);
			
			var listOfOldScenes = new List<string>();
			var listOfPreloadedScenes = new List<string>();

			CheckForOldAndPreloadedScenes(listOfPreloadedScenes, listOfNewScenes, listOfOldScenes);

			listOfNewScenes.RemoveAll(x => listOfPreloadedScenes.Contains(x));
			
			var asyncLoadList = BuildAsyncLoadList(listOfNewScenes);

			Promise.All(asyncLoadList)
				.Then(() =>
				{
					SceneManager.SetActiveScene(SceneManager.GetSceneByName(additiveScene.LightingScene.Value));
					UnloadUnneccesaryScenes(listOfOldScenes);
				})
				.Finally(() => promise.Resolve())
				.Catch(promise.Reject);
		}
	}
}
