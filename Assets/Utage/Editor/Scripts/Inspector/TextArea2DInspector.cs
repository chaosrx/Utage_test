//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Utage
{

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TextArea2D))]
	public class TextArea2DInspector : Node2DInspector
	{
		public override void DrawProperties()
		{

			TextArea2D obj = target as TextArea2D;

			//テキスト
			UtageEditorToolKit.BeginGroup("Text");
			string text = EditorGUILayout.TextArea(obj.text, GUILayout.Height(60f));
			if (text != obj.text)
			{
				Undo.RecordObject(obj, "Text Change");
				obj.text = text;
				EditorUtility.SetDirty(target);
			}


			UtageEditorToolKit.PropertyField(serializedObject, "lengthOfView", "Length");
			UtageEditorToolKit.PropertyField(serializedObject, "type", "Type");
			switch (obj.Type)
			{
				case TextArea2D.ViewType.Outline:
				case TextArea2D.ViewType.Shadow:
					UtageEditorToolKit.PropertyField(serializedObject, "colorEffect", "ColorEffect");
					break;
				case TextArea2D.ViewType.Default:
				default:
					break;
			}
			UtageEditorToolKit.EndGroup();


			UtageEditorToolKit.BeginGroup("Font");
			UtageEditorToolKit.PropertyField(serializedObject, "font", "Prefab");
			UtageEditorToolKit.PropertyField(serializedObject, "size", "Size");
			UtageEditorToolKit.PropertyField(serializedObject, "pixcelsToUnits", "Pixcels To Units");
			UtageEditorToolKit.EndGroup();

			UtageEditorToolKit.BeginGroup("Clip");
			UtageEditorToolKit.PropertyField(serializedObject, "layoutType", "Layout");
			UtageEditorToolKit.PropertyField(serializedObject, "maxWidth", "Width (px)");
			UtageEditorToolKit.PropertyField(serializedObject, "maxHeight", "Height (px)");
			UtageEditorToolKit.EndGroup();

			UtageEditorToolKit.BeginGroup("Style");
			UtageEditorToolKit.PropertyField(serializedObject, "isKerning", "Kerning");
			UtageEditorToolKit.PropertyField(serializedObject, "space", "Space (px)");
			UtageEditorToolKit.PropertyField(serializedObject, "letterSpace", "Letter Space (px)");
			UtageEditorToolKit.PropertyField(serializedObject, "lineSpace", "Line Space (px)");
			UtageEditorToolKit.EndGroup();

			UtageEditorToolKit.BeginGroup("WordWrap");
			UtageEditorToolKit.PropertyField(serializedObject, "wordWrap", "Type");
			UtageEditorToolKit.EndGroup();

			UtageEditorToolKit.BeginGroup("Debug");
			UtageEditorToolKit.PropertyField(serializedObject, "isDebugMode", "");
			UtageEditorToolKit.EndGroup();

			base.DrawProperties();
		}
	}
}