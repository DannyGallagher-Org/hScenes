using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Utility
{
	public class InputTextFieldDialoguePopup : EditorWindow
	{
		private static string _text;
		private static Func<string, bool> _action;
		private string _input;
		private static InputTextFieldDialoguePopup _window;

		public static void Init(string text, Func<string, bool> action)
		{
			_text = text;
			_action = action;

			_window = FindObjectOfType<InputTextFieldDialoguePopup>();
			
			if(_window)
				_window.Close();
			
			_window = CreateInstance<InputTextFieldDialoguePopup>();
			
			var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			_window.position = new Rect(mousePos.x, mousePos.y, 250, 100);
			_window.ShowPopup();
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField(_text, EditorStyles.wordWrappedLabel);
			_input = EditorGUILayout.TextField(_input);
			
			GUILayout.Space(20);
			
			if (GUILayout.Button("Accept"))
			{
				_action(_input);
				Close();
			}

			if (!GUILayout.Button("Cancel")) 
				return;
			
			Close();
		}
	}
}
