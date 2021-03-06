//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------



namespace Utage
{
	/// <summary>
	/// バックログのデータ
	/// </summary>
	public class AdvBacklog
	{
		/// <summary>
		/// テキスト
		/// </summary>
		public string Text { get { return this.text; } }
		string text;

		/// <summary>
		/// キャラ名
		/// </summary>
		public string CharacterName { get { return this.characterName; } }
		string characterName;

		/// <summary>
		///  ボイスファイル
		/// </summary>
		public AssetFile VoiceFile { get { return voiceFile; } }
		AssetFile voiceFile;

		/// <summary>
		/// ボイスつきか
		/// </summary>
		public bool IsVoice { get { return voiceFile != null; } }

		/// <summary>
		/// バックログのデータのコンストラクタ
		/// </summary>
		/// <param name="text">テキスト</param>
		/// <param name="characterName">キャラ名</param>
		/// <param name="voiceFile">ボイスファイル</param>
		public AdvBacklog(string text, string characterName, AssetFile voiceFile)
		{
			this.text = text;
			this.characterName = characterName;
			this.voiceFile = voiceFile;
		}
	}
}
