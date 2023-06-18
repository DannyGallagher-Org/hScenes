using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Utility;
using Levels;
using RSG;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Editor
{
    public partial class LevelTool : EditorWindow 
    {
        
        #region private variables
        private GUIStyle _titleStyle;
        private static GUIStyle _lesserTitleStyle;
        private GUIStyle _subTitleStyle;
        private GUIStyle _levelElementStyle;
        
        private readonly Dictionary<string, AdditiveScene> _gameScenes = new Dictionary<string, AdditiveScene>();
        private readonly bool[] _showDetails = new bool[100];
        private readonly bool[] _showOptionalDetails = new bool[100];
        private GUIStyle _layerStyle;
        private Vector2 _totalScroll;
        
        private static readonly Color RedColor = new Color(0.92f, 0.49f, 0.49f);
        private static readonly Color RedColor2 = new Color(0.92f, 0.08f, 0.15f);
        private static readonly Color GreenColor = new Color(136f/255f, 197f/255f, 66f/255f);
        private static readonly Color BlueColor = new Color(48f/255f, 73f/255f, 155f/255f);

        #endregion

        [MenuItem ("How/hScenes/Level Tool")]
        public static void  ShowWindow () {
            GetWindow(typeof(LevelTool), false, "Levels");
        }

        private void OnEnable()
        {
            var defaultFont = CustomEditorStyles.DefaultFont;

            _titleStyle = CustomEditorStyles.ToolLargeTitleStyle;
            _titleStyle.normal.textColor = Color.white;
            _lesserTitleStyle = CustomEditorStyles.ToolSmallTitleStyle;
            _subTitleStyle = CustomEditorStyles.ToolSubtitleStyle;

            _levelElementStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.white
                },
                font = defaultFont,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(10, 10, 2, 5)
            };
        }

        private void OnGUI ()
        {
            DrawTitle();
            GUILayout.Space(10);
            DrawAddButton();
            GUILayout.Space(10);
            
            GetLevelData();
            _totalScroll = GUILayout.BeginScrollView(_totalScroll);
            DrawLevelData();
            GUILayout.EndScrollView();
        }

        private static void DrawAddButton()
        {
            CustomEditorObjects.DrawButtonInHorizontalBox("ADD NEW LEVEL", () =>
            {
                CustomEditorObjects.TextFieldDialogue("Level name", CreateLevelFunc);
            }, 
            Color.white, 300f, 15f);
        }
        
        private static readonly Func<string, bool> CreateLevelFunc = CreateLevel;
        private static string _deleteString;

        private static bool CreateLevel(string name)
        {
            try
            {
                var newLevel = ScriptableObjectUtility.CreateAsset<AdditiveScene>($"Assets/Data/Global/Scenes/{name}.asset");
                newLevel.Name = name;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void DrawTitle()
        {
            CustomEditorObjects.DrawCustomTitle("Levels", 
                "By Danny Gallagher for House of Wire",
                BlueColor,
                _titleStyle,
                _subTitleStyle);
        }

        private void DrawLevelData()
        {
            var count = 0;
            foreach (var scene in _gameScenes)
            {
                DrawScene(scene, count);
                count++;
            }
        }

        private void DrawScene(KeyValuePair<string, AdditiveScene> gameScene, int count)
        {
            var outerRect = EditorGUILayout.BeginVertical();
            {
                GUI.color = count % 2 == 0 ? Color.gray : new Color(0.75f, 0.75f, 0.75f);
                
                GUI.Box(outerRect, GUIContent.none);
                GUI.color = Color.white;
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"{gameScene.Value.name} ({gameScene.Value.Name})", _levelElementStyle, GUILayout.Width(500));

                    if (GUILayout.Button("Load All", GUILayout.Width(80)))
                        LoadLevel(gameScene.Value, null);


                    if (GUILayout.Button("Test", GUILayout.Width(40)))
                        TestLevel(gameScene.Value);

                    _showDetails[count] = EditorGUILayout.Foldout(_showDetails[count], "Show Details");
                }
                GUILayout.EndHorizontal();

                if (_showDetails[count])
                {
                    var topBoxRect = EditorGUILayout.BeginHorizontal();
                    {
                        GUI.Box(topBoxRect, GUIContent.none);
                        GUILayout.BeginVertical();
                        {
                            EditorGUI.BeginChangeCheck();
                            
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Name: ", GUILayout.Width(60));
                            GUILayout.Label(gameScene.Value.Name);
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Lighting: ", GUILayout.Width(60));
                            GUILayout.Label(gameScene.Value.LightingScene);
                            GUILayout.EndHorizontal();

                            if (EditorGUI.EndChangeCheck())
                                EditorUtility.SetDirty(gameScene.Value);
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                    
                    GUILayout.Space(25);
                    
                    DrawLayers(gameScene);

                    GUILayout.Space(4);
                    
                    DrawStateLayers(gameScene);
                    GUILayout.Space(20);
                    
                    EditorUtility.SetDirty(gameScene.Value);
                }
            }
            GUILayout.EndVertical();
        }

        private static void DrawLayers(KeyValuePair<string, AdditiveScene> gameScene)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(4);
            GUILayout.Label("- Layers -", _lesserTitleStyle);

            GUILayout.Space(10);

            GUI.color = Color.cyan;
            if (GUILayout.Button("Add Layer", GUILayout.Width(200)))
            {
                var path = EditorUtility.OpenFilePanel("Choose scene to add as a layer",
                        $"{Application.dataPath}",
                        "unity")
                    .Replace(Application.dataPath, string.Empty);
                if (path.Length != 0)
                {
                    var scene = AssetDatabase.LoadAssetAtPath<Object>($"Assets{path}");
                    Debug.Log(scene);
                    if (scene)
                    {
                        var oldLayers = gameScene.Value.Layers;
                        gameScene.Value.Layers = new string[oldLayers.Length + 1];

                        for (var i = 0; i < oldLayers.Length; i++)
                        {
                            gameScene.Value.Layers[i] = oldLayers[i];
                        }

                        gameScene.Value.Layers[oldLayers.Length] = scene.name;
                    }

                    EditorUtility.SetDirty(scene);
                    EditorUtility.SetDirty(gameScene.Value);
                }
            }

            GUILayout.EndHorizontal();

            GUI.color = Color.white;
            var layerCount = 0;
            foreach (var layer in gameScene.Value.Layers)
            {
                DrawLayer(layer, gameScene, layerCount);
                layerCount++;
            }

            GUILayout.Space(25);
        }

        private static void TestLevel(AdditiveScene value)
        {
            LoadLevel(value, () =>
            {
                EditorApplication.isPlaying = true;
            });
        }

        private static void LoadLevel(AdditiveScene additiveScene, Action onComplete)
        {
            EditorUtility.DisplayProgressBar("Loading Level", "Beginning", 0);
            var newScenes = additiveScene.Layers.Select(layer => layer).ToList();
            
            var oldScenes = new List<Scene>();
            var preloaded = new List<string>();
            
            for (var i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                var next = SceneManager.GetSceneAt(i);
                preloaded.Add(next.name);

                if (newScenes.Contains(next.name))
                {
                    newScenes.Remove(next.name);
                    continue;
                }
                
                oldScenes.Add(next);
            }
            
            var toLoadCount = newScenes.Count;
            var elapsedCount = 0f;

            if (newScenes.Count < 1)
            {
                
                CloseUneededLoadedScenes(oldScenes)
                    .Then(() => FinishLoading(onComplete, additiveScene));
                return;
            }

            EditorSceneManager.SceneOpenedCallback handler = null;
            
            handler = (scene, mode) =>
            {
                elapsedCount++;

                if (elapsedCount >= toLoadCount)
                {
                    CloseUneededLoadedScenes(oldScenes)
                        .Then(() => FinishLoading(onComplete, additiveScene));
                }
                
                EditorSceneManager.sceneOpened -= handler;
            };
            
            foreach (var sceneName in newScenes)
            {
                EditorSceneManager.sceneOpened += handler;

                if (preloaded.Contains(sceneName)) continue;
                EditorUtility.DisplayProgressBar("Loading Level", $"Loaded:{sceneName}", elapsedCount / toLoadCount);
                LoadScenePart(additiveScene, sceneName);
            }
        }

        private static void FinishLoading(Action onComplete, AdditiveScene additiveScene)
        {
            onComplete?.Invoke();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(additiveScene.LightingScene));
            EditorUtility.ClearProgressBar();
        }

        private static void LoadScenePart(AdditiveScene additiveScene, string sceneName)
        {
            try
            {
                var guid = AssetDatabase.FindAssets($"{sceneName} t:Scene").First();
                EditorSceneManager.OpenScene(
                    AssetDatabase.GUIDToAssetPath(guid),
                    OpenSceneMode.Additive);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                EditorUtility.ClearProgressBar();
            }
        }

        private static IPromise CloseUneededLoadedScenes(List<Scene> oldScenes)
        {
            for (var index = 0; index < oldScenes.Count; index++)
            {
                EditorUtility.DisplayProgressBar("Closing Old Level", $"Closing:{oldScenes[index]}", (float) index / oldScenes.Count);

                EditorSceneManager.CloseScene(oldScenes[index], true);
            }

            EditorUtility.ClearProgressBar();
            return Promise.Resolved();
        }

        private void GetLevelData()
        {
            if (!Directory.Exists($"{Application.dataPath}/Data/Global/Scenes"))
                Directory.CreateDirectory($"{Application.dataPath}/Data/Global/Scenes");
            
            var levelData = Directory.GetFiles($"{Application.dataPath}/Data/Global/Scenes", "*.asset").Where(a => !a.Contains(".meta"));

            var enumerable = levelData as string[] ?? levelData.ToArray();
            
            if(enumerable.ToList().Count != _gameScenes.Count)
                _gameScenes.Clear();
            
            foreach (var scene in enumerable)
            {
                AdditiveScene sceneObj;
                var fileName = Application.platform == RuntimePlatform.OSXEditor ? scene.Split('/').Last() : scene.Split('\\').Last();
                
                var finalLevelName = fileName.Split('.').First();

                if (_gameScenes.TryGetValue(finalLevelName, out sceneObj)) continue;
                var loadedObj = (AdditiveScene)AssetDatabase.LoadAssetAtPath($"Assets/Data/Global/Scenes/{fileName}", typeof(AdditiveScene));
                    
                _gameScenes.Add(finalLevelName, loadedObj);
            }
        }
    }
}