//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace Utage
{

	/// <summary>
	/// サウンドのタイプ
	/// </summary>
	public enum SoundType
	{	
		/// <summary>BGM</summary>
		Bgm,
		/// <summary>SE</summary>
		Se,
		/// <summary>環境音</summary>
		Ambience,
		/// <summary>ボイス</summary>
		Voice,
		/// <summary>タイプの数</summary>
		Max,
	};


	/// <summary>
	/// サウンドファイル設定（ラベルとファイルの対応）
	/// </summary>
	[System.Serializable]
	public partial class AdvSoundSettingData : SerializableDictionaryFileReadKeyValue
	{
		/// <summary>
		/// サウンドのタイプ
		/// </summary>
		public SoundType Type { get { return this.type; } }
		[SerializeField]
		SoundType type;

		/// <summary>
		/// 表示タイトル
		/// </summary>
		public string Title { get { return this.title; } }
		[SerializeField]
		string title;

		/// <summary>
		/// ファイル名
		/// </summary>
		[SerializeField]
		string fileName;

		/// <summary>
		/// ファイル名
		/// </summary>
		public string FilePath { get { return this.filePath; } }
		string filePath;

		/// <summary>
		/// ストリーミングするか
		/// </summary>
		public bool IsStreaming { get { return this.isStreaming; } }
		[SerializeField]
		bool isStreaming;

		/// <summary>
		/// バージョン
		/// </summary>
		public int Version { get { return this.version; } }
		[SerializeField]
		int version;

		/// <summary>
		/// StringGridの一行からデータ初期化
		/// </summary>
		/// <param name="row">初期化するためのデータ</param>
		/// <returns>成否</returns>
		public override bool InitFromStringGridRow(StringGridRow row)
		{
			if (row.IsEmpty) return false;

			string key = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
			if (string.IsNullOrEmpty(key))
			{
				return false;
			}
			else
			{
				InitKey(key);
				this.type = AdvParser.ParseCell<SoundType>(row, AdvColumnName.Type);
				this.fileName = AdvParser.ParseCell<string>(row, AdvColumnName.FileName);
				this.isStreaming = AdvParser.ParseCellOptional<bool>(row, AdvColumnName.Streaming, false);
				this.version = AdvParser.ParseCellOptional<int>(row, AdvColumnName.Version, 0);
				this.title = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Title, "");
				return true;
			}
		}

		/// <summary>
		/// 起動時の初期化
		/// </summary>
		/// <param name="settingData">設定データ</param>
		public void BootInit(AdvBootSetting settingData)
		{
			filePath = FileNameToPath(fileName, settingData);
		}

		string FileNameToPath(string fileName, AdvBootSetting settingData)
		{
			switch (type)
			{
				case SoundType.Se:
					return settingData.SeDirInfo.FileNameToPath(fileName);
				case SoundType.Ambience:
					return settingData.AmbienceDirInfo.FileNameToPath(fileName);
				case SoundType.Bgm:
				default:
					return settingData.BgmDirInfo.FileNameToPath(fileName);
			}
		}
	}


	/// <summary>
	/// サウンドの設定
	/// </summary>
	[System.Serializable]
	public partial class AdvSoundSetting : SerializableDictionaryFileRead<AdvSoundSettingData>
	{
		/// <summary>
		/// 起動時の初期化
		/// </summary>
		/// <param name="settingData">設定データ</param>
		public void BootInit(AdvBootSetting settingData)
		{
			foreach (AdvSoundSettingData data in List)
			{
				data.BootInit(settingData);

				AssetFile file = AssetFileManager.GetFileCreateIfMissing(data.FilePath);
				file.Version = data.Version;
				//ロードタイプをストリーミングにする場合、
				//oggはUnityのバグがあるのでストリーム読み込みを無効にする
				//バグの内容は、曲の長さがとれず一度鳴らすと終了しなくなるというもの。
				if (data.IsStreaming && !ExtensionUtil.CheckExtention(data.FilePath, ExtensionUtil.Ogg))
				{
					file.AddLoadFlag(AssetFileLoadFlags.Streaming);
				}
			}
		}

		/// <summary>
		/// 全てのリソースをダウンロード
		/// </summary>
		public void DownloadAll()
		{
			//ファイルマネージャーにバージョンの登録
			foreach (AdvSoundSettingData data in List)
			{
				AssetFileManager.Download(data.FilePath);
			}
		}


		/// <summary>
		/// ラベルが登録されているか
		/// </summary>
		/// <param name="label">ラベル</param>
		/// <param name="type">サウンドのタイプ</param>
		/// <returns>ファイルパス</returns>
		public bool Contains(string label, SoundType type)
		{
			//既に絶対URLならそのまま
			if (UtageToolKit.IsAbsoluteUri(label))
			{
				return true;
			}
			else
			{
				AdvSoundSettingData data = FindData(label);
				if (data == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// ラベルからファイルパスを取得
		/// </summary>
		/// <param name="label">ラベル</param>
		/// <param name="type">サウンドのタイプ</param>
		/// <returns>ファイルパス</returns>
		public string LabelToFilePath(string label, SoundType type)
		{
			//既に絶対URLならそのまま
			if (UtageToolKit.IsAbsoluteUri(label))
			{
				//プラットフォームが対応する拡張子にする
				return ExtensionUtil.ChangeSoundExt(label);
			}
			else
			{
				AdvSoundSettingData data = FindData(label);
				if (data == null)
				{
					//ラベルをそのままファイル名扱いに
					return label;
				}
				else
				{
					return data.FilePath;
				}
			}
		}

		//ラベルからデータを取得
		AdvSoundSettingData FindData(string label)
		{
			AdvSoundSettingData data;
			if (!TryGetValue(label, out data))
			{
				return null;
			}
			else
			{
				return data;
			}
		}

		/// <summary>
		/// サウンドルームに表示するデータのリスト
		/// </summary>
		/// <returns></returns>
		public List<AdvSoundSettingData> GetSoundRoomList()
		{
			List<AdvSoundSettingData> list = new List<AdvSoundSettingData>();
			foreach (AdvSoundSettingData item in List)
			{
				if (!string.IsNullOrEmpty(item.Title))
				{
					list.Add(item);
				}
			}
			return list;
		}
	}
}