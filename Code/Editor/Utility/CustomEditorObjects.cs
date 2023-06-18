using System;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Editor.Utility
{
	public static class CustomEditorObjects
	{
		public static void DrawCustomTitle(string title, string subtitle, Color titleColor, GUIStyle titleStyle, GUIStyle subtitleStyle)
		{
			EditorGUILayout.BeginVertical();
			
			var rect = EditorGUILayout.BeginHorizontal();
			GUI.color = titleColor;
			GUI.Box(rect, GUIContent.none);
			GUILayout.Label(title, titleStyle);
			EditorGUILayout.EndHorizontal();
			
			var subRect = EditorGUILayout.BeginHorizontal();
			var subColor = titleColor;
			subColor.a *= 0.5f;
			GUI.color = new Color(1f, 0.92f, 0.65f);
			GUI.Box(subRect, GUIContent.none);
			GUILayout.BeginVertical();
			GUILayout.Space(4);
			GUILayout.Label(subtitle, subtitleStyle);
			GUILayout.Space(4);
			GUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
		}

		public static void DrawDisplayTextBoxedAsButton(string displayText, Action action, Color inColor = default(Color))
		{
			GUI.color = inColor == default(Color) ? Color.gray : inColor;
			
			var rect = EditorGUILayout.BeginHorizontal();
			GUI.Box(rect, GUIContent.none);
			
			if (GUILayout.Button(displayText,GUILayout.ExpandWidth(true)))
				action();
			
			EditorGUILayout.EndHorizontal();
		}

		public static void DrawButtonInHorizontalBox(string buttonText, Action action, Color color, float width, float height)
		{
			var rect = EditorGUILayout.BeginHorizontal();
			{
				GUI.color = new Color(136,197,66);
				rect.x = (rect.width - width) / 2f;
				rect.width = width;
				GUI.Box(rect, GUIContent.none);
				GUILayout.BeginVertical();
				GUILayout.Space(Mathf.Min(5, height*0.33f));
				GUI.color = color;
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button(buttonText, GUILayout.Width(width*0.66f), GUILayout.Height(height)))
					action();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.Space(Mathf.Min(5, height*0.33f));
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		public static void TextFieldDialogue(string text, Func<string, bool> action)
		{
			InputTextFieldDialoguePopup.Init(text, action);
		}

		public static void DropDownFieldDialogue(string text, string[] options, Func<int, bool> createLine)
		{
			DropDownFieldDialoguePopup.Init(text, options, createLine);
		}

		public static void TwoToneFoldOut(ref bool showOptionalDetail, string amode, string bmode)
		{
			showOptionalDetail = EditorGUILayout.Foldout(showOptionalDetail, showOptionalDetail ? bmode : amode);
		}
	}
}
