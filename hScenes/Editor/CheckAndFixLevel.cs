using System.Collections.Generic;
using System.Linq;
using GenericDataSet;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
	public static class CheckAndFixLevel 
	{
		private static DataSet _checklist;
		private static List<GameObject> _checkDict;
		private static bool[] _checkedBools;
		private static bool _doDebug;

		[MenuItem("How/Levels/CheckLevel")]
		public static void CheckLevel()
		{
			_doDebug = EditorUtility.DisplayDialog("Debug Good Results?",
				$"Would you like to see the output of results from checking scene {SceneManager.GetActiveScene()}" +
				$" including when everything is ok?",
				"Yes", "No");
			
			var path = EditorUtility.OpenFilePanel("Checklist", "", "asset")
				.Replace(Application.dataPath, string.Empty);
			
			path = $"Assets{path}";
			_checklist = AssetDatabase.LoadAssetAtPath<DataSet>(path);
			
			_checkDict = new List<GameObject>();
			_checkedBools = new bool[_checklist.Items.Count];
			
			foreach (var obj in _checklist.Items)
			{
				var go = obj as GameObject;
				if (go != null) 
					_checkDict.Add(go);
			}

			var sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();

			for (var i = 0; i < _checklist.Items.Count; i++)
			{
				if(_doDebug) Debug.Log($"-Checking <color=yellow>{_checklist.Items[i]}</color>-");
				
				var exists = false;
				var checkListGameObject = _checklist.Items[i] as GameObject;

				var checkGoPrefabRoot = PrefabUtility.FindPrefabRoot(checkListGameObject);
				
				foreach (var sceneObject in sceneObjects)
				{
					var sceneObjPrefabParent = PrefabUtility.GetPrefabParent(sceneObject);

					if (!sceneObjPrefabParent) continue;
					exists = checkGoPrefabRoot == sceneObjPrefabParent;
					if(exists) break;
				}
				
				if (exists)
				{
					_checkedBools[i] = true;
					if (checkListGameObject == null) continue;
					if(_doDebug)
						Debug.Log($"-{checkListGameObject.name} <color=green>exists</color>-");
				}
				else if (checkListGameObject != null)
				{
					_checkedBools[i] = false;
					Debug.Log($"-Scene Missing <color=red>{checkListGameObject.name}</color> from checkList");
				}
			}

			var wrongCount = _checkedBools.Count(a => a == false);

			if (wrongCount <= 0) return;
			if(EditorUtility.DisplayDialog("Fix Bad Results?",
				$"Looks like {SceneManager.GetActiveScene()} is missing {wrongCount} things from {_checklist}" +
				" shall I go ahead and fix these?",
				"Yes", "No"))
			{
				FixLevel();
			}
		}

		private static void FixLevel()
		{
			for (var i = 0; i < _checkedBools.Length; i++)
			{
				if (_checkedBools[i]) continue;
				Debug.Log($"-Sawning a <color=cyan>{_checklist.Items[i]}</color>.-");
				var newObj = Object.Instantiate(_checklist.Items[i] as GameObject);
				PrefabUtility.ConnectGameObjectToPrefab(newObj, PrefabUtility.FindPrefabRoot(_checklist.Items[i] as GameObject));
			}
			
			Debug.Log("All done! :)");
		}
	}
}
