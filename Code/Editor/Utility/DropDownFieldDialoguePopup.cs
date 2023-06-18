using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Utility
{
	public class DropDownFieldDialoguePopup : EditorWindow
	{
		private static DropDownFieldDialoguePopup _window;
		private static string _text;
		private static Func<int, bool> _func;
		private static string[] _options;
		private int _index;

		public static void Init(string text, string[] options, Func<int, bool> func)
		{
			_text = text;
			_func = func;
			_options = options;
			
			if(_window)
				_window.Close();
			
			_window = CreateInstance<DropDownFieldDialoguePopup>();
			var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			_window.position = new Rect(mousePos.x, mousePos.y, 250, 100);
			_window.ShowPopup();
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField(_text, EditorStyles.wordWrappedLabel);

			_index = EditorGUILayout.Popup(_index, _options);

			GUILayout.Space(20);
			if (GUILayout.Button("Accept"))
			{
				_func(_index);
				_window.Close();
			}
			
			if(GUILayout.Button("Cancel"))
				_window.Close();
		}
	}
}
