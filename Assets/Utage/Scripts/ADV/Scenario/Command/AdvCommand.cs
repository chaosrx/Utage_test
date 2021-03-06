//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// 各コマンドの基底クラス
	/// </summary>
	public abstract class AdvCommand
	{
		/// <summary>
		/// エディタ上のエラーチェックのために起動してるか
		/// </summary>
		static public bool IsEditorErrorCheck
		{
			get { return isEditorErrorCheck; }
			set { isEditorErrorCheck = value; }
		}
		static bool isEditorErrorCheck = false;

		//ロードの必要があるファイルリスト
		public List<AssetFile> LoadFileList { get { return loadFileList; } }
		List<AssetFile> loadFileList = null;

		///このシナリオからリンクするジャンプ先のシナリオラベル
		public virtual string GetJumpLabel() { return null; }
		///このシナリオに設定されているシナリオラベル
		public virtual string GetScenarioLabel() { return null; }

		//ロードの必要があるファイルがあるか
		public bool IsExistLoadFile()
		{
			if (null != loadFileList)
			{
				return loadFileList.Count > 0;
			}
			return false;
		}

		//ロードの必要があるファイルリスト
		public AssetFile AddLoadFile(string path)
		{
			AssetFile file = AssetFileManager.GetFileCreateIfMissing(path);
			if (loadFileList == null) loadFileList = new List<AssetFile>();
			loadFileList.Add(file);
			return file;
		}

		//DL処理する
		public void Download()
		{
			if (null != loadFileList)
			{
				foreach (AssetFile file in loadFileList)
				{
					AssetFileManager.Download(file);
				}
			}
		}

		//ロード処理
		public void Load()
		{
			if (null != loadFileList)
			{
				foreach (AssetFile file in loadFileList)
				{
					AssetFileManager.Load(file, this);
				}
			}
		}

		//ロードが終わったか
		public bool IsLoadEnd()
		{
			if (null != loadFileList)
			{
				foreach (AssetFile file in loadFileList)
				{
					if (!file.IsLoadEnd)
					{
						return false;
					}
				}
			}
			return true;
		}

		//コマンド実行
		public abstract void DoCommand(AdvEngine engine);

		//コマンド実行後に使ったファイル参照をクリア
		public void Unload()
		{
			if (null != loadFileList)
			{
				foreach (AssetFile file in loadFileList)
				{
					file.Unuse(this);
				}
			}
		}


		//コマンド終了待ち
		public virtual bool Wait(AdvEngine engine) { return false; }

		//ページ区切りのコマンドか
		public virtual bool IsTypePageEnd() { return false; }

		//次のコマンドと連続コマンドかチェック
		public bool CheckContinues(AdvCommand nextCommand)
		{
			if (nextCommand.IsIfCommand)
			{
				return true;
			}
			else
			{
				return (nextCommand.GetType() == this.GetType() );
			}
		}

		//連続タイプのコマンドか
		public virtual bool IsContinusCommand { get { return false; } }

		//IF文タイプのコマンドか
		public virtual bool IsIfCommand { get { return false; } }
	}
}