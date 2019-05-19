using System.Collections.Generic;
using Extensions;
using Levels;
using ScriptableVariables.Bools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public partial class LevelTool
    {
        private void DrawStateLayers(KeyValuePair<string, AdditiveScene> gameScene)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(4);
            GUILayout.Label("- State Layers -", _lesserTitleStyle);

            GUILayout.Space(10);

            GUI.color = Color.cyan;
            if (GUILayout.Button("Add State Layer", GUILayout.Width(200)))
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
                        gameScene.Value.StateLayers.Add(scene.name, string.Empty);

                    EditorUtility.SetDirty(scene);
                    EditorUtility.SetDirty(gameScene.Value);
                }
            }

            GUILayout.EndHorizontal();

            GUI.color = Color.white;
            var layerCount = 0;
            foreach (var layer in gameScene.Value.StateLayers)
            {
                var layerName = layer.Key;
                var req = layer.Value;
                EditorGUILayout.BeginHorizontal();
                
                DrawStateLayer(layerName, gameScene, layerCount, req);
                
                EditorGUILayout.EndHorizontal();
                layerCount++;
            }

            GUILayout.Space(25);
        }
        
        private static void DrawStateLayer(string layerName, KeyValuePair<string, AdditiveScene> gameScene,
            int layerCount, string req)
        {
            var prevColor = GUI.color;
            var rect = EditorGUILayout.BeginHorizontal();
            {
                GUI.color = layerCount % 2 == 0 ? Color.white : Color.grey;
                GUI.Box(rect, GUIContent.none);
                GUILayout.Space(40);

                GUI.color = prevColor;
                GUILayout.Label(layerName, GUILayout.Width(240));

                var additiveScene = gameScene.Value;

                GUI.color = GreenColor;
                if (GUILayout.Button("Load", GUILayout.Width(60)))
                {
                    LoadScenePart(additiveScene, layerName);
                }

                GUI.color = RedColor;
                if (GUILayout.Button("Unload", GUILayout.Width(50)))
                {
                    for (var i = 0; i < EditorSceneManager.loadedSceneCount; i++)
                    {
                        var next = SceneManager.GetSceneAt(i);

                        if (next.name == layerName)
                            EditorSceneManager.CloseScene(next, true);
                    }
                }

                GUI.color = new Color(0.5f, 0.77f, 1f);

                if (layerCount != 0)
                {
                    if (GUILayout.Button("▲", GUILayout.Height(20), GUILayout.Width(20)))
                    {
                        var additiveSceneLayers = additiveScene.Layers;
                        additiveSceneLayers.Swap(layerCount, layerCount - 1);
                        EditorUtility.SetDirty(additiveScene);
                        return;
                    }
                }
                else
                    GUILayout.Space(24);

                var sceneLayers = additiveScene.Layers;
                if (layerCount != sceneLayers.Length - 1)
                {
                    if (GUILayout.Button("▼", GUILayout.Height(20), GUILayout.Width(20)))
                    {
                        sceneLayers.Swap(layerCount, layerCount + 1);
                        EditorUtility.SetDirty(additiveScene);
                        return;
                    }
                }
                else
                    GUILayout.Space(24);
                
                GUILayout.Space(20);
                
                GUI.color = new Color(1f, 0.71f, 0.4f);
                if(!string.IsNullOrEmpty(req))
                    GUILayout.Label(req);
                else
                {
                    if (GUILayout.Button("Add Requirement"))
                    {
                        var path = EditorUtility.OpenFilePanel("Choose data for requirement",
                                $"{Application.dataPath}",
                                "asset")
                            .Replace(Application.dataPath, string.Empty);
                        if (path.Length != 0)
                        {
                            var data = AssetDatabase.LoadAssetAtPath<BoolVariable>($"Assets{path}");
                            Debug.Log(data);
                            if (data != null)
                                gameScene.Value.StateLayers[layerName] = data.name;

                            EditorUtility.SetDirty(gameScene.Value);
                        }
                    }
                }
                
                GUILayout.Space(20);

                GUILayout.FlexibleSpace();

                GUI.color = RedColor2;
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    additiveScene.StateLayers.Remove(layerName);
                    EditorUtility.SetDirty(additiveScene);
                }

                GUILayout.Space(20);

                GUI.color = prevColor;
            }
            GUILayout.EndHorizontal();
        }
    }
}