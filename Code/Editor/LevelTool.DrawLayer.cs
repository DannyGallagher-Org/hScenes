using System.Collections.Generic;
using System.Linq;
using Extensions;
using Levels;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public partial class LevelTool
    {
        private static void DrawLayer(string layerName, KeyValuePair<string, AdditiveScene> gameScene,
            int layerCount, string req = null)
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

                GUILayout.FlexibleSpace();

                GUI.color = RedColor2;
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    var path = $"Assets/Data/Global/Scenes/{gameScene.Key}.asset";
                    Debug.Log(path);
                    var scene = AssetDatabase.LoadAssetAtPath<AdditiveScene>(path);

                    var layersList = additiveScene.Layers.ToList();
                    layersList.Remove(layerName);

                    scene.Layers = layersList.ToArray();

                    EditorUtility.SetDirty(scene);
                    EditorUtility.SetDirty(additiveScene);
                }

                GUILayout.Space(20);

                GUI.color = prevColor;
            }
            GUILayout.EndHorizontal();
        }
    }
}