using UnityEditor;
using UnityEngine;

namespace Editor.Utility
{
    public static class CustomEditorStyles
    {
        public static Font DefaultFont => (Font) EditorGUIUtility.Load("Fonts/Gilroy-Light.otf");
        
        public static GUIStyle ToolLargeTitleStyle => new GUIStyle
        {
            font = (Font) EditorGUIUtility.Load("Fonts/Gilroy-ExtraBold.otf"),
            fontSize = 20,
            padding = new RectOffset(5, 5, 5, 5)
        };

        public static GUIStyle ToolSmallTitleStyle => new GUIStyle
        {
            font = (Font) EditorGUIUtility.Load("Fonts/Gilroy-ExtraBold.otf"),
            fontSize = 16,
            padding = new RectOffset(5, 5, 5, 5),
            normal =
            {
                textColor = new Color(0.79f, 0.75f, 0.75f)
            }
        };

        public static GUIStyle ToolSubtitleStyle => new GUIStyle
        {
            font = DefaultFont,
            fontSize = 10,
            fontStyle = FontStyle.Italic,
            padding = new RectOffset(15, 0, 0, 0),
            normal =
            {
                textColor = new Color(0.79f, 0.74f, 0.62f)
            }
        };
    }
}
