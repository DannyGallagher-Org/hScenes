using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Levels;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace hScenes.Levels
{
    internal static partial class LevelManagement 
    {
        private static IPromise LoadLevel(	AdditiveScene additiveScene)
		{
			var promise = new Promise();
			LoadLevelAsync(additiveScene, promise);
			return promise;
		}

		private static void LoadLevelAsync( AdditiveScene additiveScene, 
											IPendingPromise promise)
		{
			var listOfNewScenes = FindWhichLevelsNeedAdded(additiveScene);

			var listOfOldScenes = new List<string>();
			var listOfPreloadedScenes = new List<string>();

			CheckForOldAndPreloadedScenes(listOfPreloadedScenes, listOfNewScenes, listOfOldScenes);

			listOfNewScenes.RemoveAll(x => listOfPreloadedScenes.Contains(x));
			
			var asyncLoadList = BuildAsyncLoadList(listOfNewScenes);

			Promise.All(asyncLoadList)
				.Catch(Debug.LogWarning)
				.Then(() =>
				{
					var sceneByName = SceneManager.GetSceneByName(additiveScene.LightingScene.Value);
					SceneManager.SetActiveScene(sceneByName);
				})
				.Catch(Debug.LogWarning)
				
				.Finally(() => promise.Resolve());
		}

		private static void UnloadUnneccesaryScenes(ICollection<string> listOfOldScenes)
		{
			foreach (var sceneName in listOfOldScenes)
#pragma warning disable 618
				SceneManager.UnloadScene(sceneName);
#pragma warning restore 618

			listOfOldScenes.Clear();
		}

		private static IPromise[] BuildAsyncLoadList(IReadOnlyList<string> listOfNewScenes)
		{
			var asyncLoadList = new IPromise[listOfNewScenes.Count];

			for (var index = 0; index < listOfNewScenes.Count; index++)
			{
				var sceneName = listOfNewScenes[index];
				asyncLoadList[index] = PromiseLoadScene(sceneName, LoadSceneMode.Additive);
			}

			return asyncLoadList;
		}

		private static void CheckForOldAndPreloadedScenes(ICollection<string> listOfPreloadedScenes, 
														  ICollection<string> listOfNewScenes,
														  ICollection<string> listOfOldScenes)
		{
			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				var next = SceneManager.GetSceneAt(i);
				listOfPreloadedScenes.Add(next.name);

				if (listOfNewScenes.Contains(next.name))
				{
					listOfNewScenes.Remove(next.name);
					continue;
				}

				listOfOldScenes.Add(next.name);
			}
		}
	    
		private static List<string> FindWhichLevelsNeedAdded(AdditiveScene additiveScene)
		{
			var listOfNewScenes = new List<string>();

			listOfNewScenes.AddRange(additiveScene.Layers);
			listOfNewScenes.AddRange(additiveScene.StateLayers.Values);
			
			return listOfNewScenes;
		}

	    private static async void LoadSceneAsync(string sceneName, LoadSceneMode loadMode, Promise promise)
	    {
		    await SceneLoadTask(sceneName, loadMode);
		    promise.Resolve();
	    }

	    private static Task SceneLoadTask(string sceneName, LoadSceneMode loadMode)
	    {
		    return Task.Run(() => SceneManager.LoadSceneAsync(sceneName, loadMode));
	    }
    }
}
