using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Utility
{
	public class TextMessageDialoguePopup : EditorWindow 
	{
		private static string _text;
		private static Func<bool> _action;
		private string _input;
		private static TextMessageDialoguePopup _window;
		private static string _buttonText;

		public static void Init(string text, string buttonText, Func<bool> action)
		{
			_text = text;
			_buttonText = buttonText;
			_action = action;
			
			if(_window)
				_window.Close();
			
			_window = CreateInstance<TextMessageDialoguePopup>();
			var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			_window.position = new Rect(mousePos.x, mousePos.y, 250, 80);
			_window.ShowPopup();
		}

		private void OnGUI()
		{
			EditorGUILayout.TextField(_text, EditorStyles.wordWrappedLabel);
			
			GUILayout.Space(30);

			if (!GUILayout.Button(_buttonText)) return;
			_action();
			_window.Close();
		}
	}
}
