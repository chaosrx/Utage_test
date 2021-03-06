//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Utage
{
	//ADVエンジンオブジェクト作成の管理エディタウイドウ
	public class CreateManagersWindow : EditorWindow
	{
		CustomProjectSetting customProjectSetting;
		int gameScreenWidth;
		int gameScreenHeight;

		/// <summary>
		/// エディタ上に保存してあるデータをロード
		/// </summary>
		void Load()
		{
			customProjectSetting = UtageEditorPrefs.LoadAsset<CustomProjectSetting>(
				UtageEditorPrefs.Key.CustomProjectSetting,
				"Assets/Utage/Examples/ScriptableObject/Example CustomProjectSetting.asset");
			gameScreenWidth = UtageEditorPrefs.LoadInt(UtageEditorPrefs.Key.GameScreenWidth, 800);
			gameScreenHeight = UtageEditorPrefs.LoadInt(UtageEditorPrefs.Key.GameScreenHegiht, 600);
		}

		/// <summary>
		/// エディタ上に保存してあるデータをセーブ
		/// </summary>
		void Save()
		{
			UtageEditorPrefs.SaveAsset(UtageEditorPrefs.Key.CustomProjectSetting, customProjectSetting);
			UtageEditorPrefs.SaveInt(UtageEditorPrefs.Key.GameScreenWidth, gameScreenWidth);
			UtageEditorPrefs.SaveInt(UtageEditorPrefs.Key.GameScreenHegiht, gameScreenHeight);
		}

		void OnFocus()
		{
			Load();
		}
		void OnLostFocus()
		{
			Save();
		}
		void OnDestroy()
		{
			Save();
		}

		void OnGUI()
		{
			UtageEditorToolKit.BeginGroup("Game Screen Size");

			CustomProjectSetting projectSetting;
			projectSetting = EditorGUILayout.ObjectField("Custom Project Setting", this.customProjectSetting, typeof(CustomProjectSetting), false) as CustomProjectSetting;
			if (this.customProjectSetting != projectSetting)
			{
				this.customProjectSetting = projectSetting;
				Save();
			}

			int width = EditorGUILayout.IntField("Width", gameScreenWidth);
			if (gameScreenWidth != width && width > 0)
			{
				gameScreenWidth = width;
				Save();
			}
			int height = EditorGUILayout.IntField("Hegiht", gameScreenHeight);
			if (gameScreenHeight != height && height > 0 )
			{
				gameScreenHeight = height;
				Save();
			}

			GUILayout.Space(4f);

			bool isEnable = true;
			EditorGUI.BeginDisabledGroup(!isEnable);
			if (GUILayout.Button("Create"))
			{
				CreateManagers(customProjectSetting,gameScreenWidth, gameScreenHeight);
			}
			EditorGUI.EndDisabledGroup();

			UtageEditorToolKit.EndGroup();
		}

		static void CreateManagers(CustomProjectSetting customProjectSetting, int width, int height)
		{
			GameObject go = new GameObject("Manageres");
			Selection.activeGameObject = go;
			BootCustomProjectSetting cutomProjectSetting = go.AddComponent<BootCustomProjectSetting>();
			cutomProjectSetting.CustomProjectSetting = customProjectSetting;
			go.AddComponent<DontDestoryOnLoad>();

			//サウンドマネージャー
			UtageToolKit.AddChildGameObjectComponent<SoundManager>(go.transform, "SoundManager");

			//ファイルマネージャー
			AssetFileManager fileManager = UtageToolKit.AddChildGameObjectComponent<AssetFileManager>(go.transform, "FileManager");
			fileManager.FileIOManger = fileManager.gameObject.AddComponent<FileIOManager>();

			//カメラマネージャー
			CameraManager cameraManager = UtageToolKit.AddChildGameObjectComponent<CameraManager>(go.transform, "CameraManager");
			cameraManager.InitOnCreate(width, height);
			//全体画面クリア用
			Camera clearCamera = UtageToolKit.AddChildGameObjectComponent<Camera>(cameraManager.transform, "ClearCamera");
			clearCamera.depth = -1;
			clearCamera.clearFlags = CameraClearFlags.SolidColor;
			clearCamera.cullingMask = 0;
			clearCamera.backgroundColor = new Color(0, 0, 0, 0);
			//2D画面用
			Camera camera2D = UtageToolKit.AddChildGameObjectComponent<Camera>(cameraManager.transform, "2DCamera");
			camera2D.clearFlags = CameraClearFlags.Nothing;
			camera2D.orthographic = true;
			camera2D.orthographicSize = 3;
			camera2D.cullingMask = 1 << LayerMask.NameToLayer("Default");
			camera2D.gameObject.AddComponent<AudioListener>();
			cameraManager.AddCamera2D(camera2D);
			camera2D.gameObject.AddComponent<CameraInput2D>();

			Selection.activeGameObject = go;

			Undo.RegisterCreatedObjectUndo(go, "CreateManageres");
		}
	}
}