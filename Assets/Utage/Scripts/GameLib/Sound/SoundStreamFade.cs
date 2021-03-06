using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// フェード処理に対応するサウンド再生ストリーム
	/// 基本はシステム内部で使うので外から呼ばないこと
	/// </summary>
	[AddComponentMenu("Utage/Lib/Sound/SoundStreamFade")]
	internal class SoundStreamFade : MonoBehaviour
	{
		SoundStream current;
		SoundStream next;

		public void SetMasterVolume(float volume)
		{
			if (null != current) current.SetMasterVolume(volume);
			if (null != next) next.SetMasterVolume(volume);
		}

		//曲が終わっているか
		public bool IsStop()
		{
			if (null != current) return false;
			if (null != next) return false;
			return true;
		}

		//指定のサウンドが再生中か
		public bool IsPlaying(AudioClip clip)
		{
			if (null == current) return false;
			else
			{
				return current.IsPlaying(clip);
			}
		}

		//再生（直前のBGMをフェードアウトしてから再生）
		internal SoundStream Play(AudioClip clip, float fadeTime, float masterVolume, float volume, bool isLoop, bool isStreaming )
		{
			return Play(clip,  fadeTime,  masterVolume, volume, isLoop, isStreaming, null );
		}
		internal SoundStream Play(AudioClip clip, float fadeTime, float masterVolume, float volume, bool isLoop, bool isStreaming, Action callBackEnd )
		{
			if (null != next) GameObject.Destroy(next.gameObject);
			
			if (null == current)
			{
				current = UtageToolKit.AddChildGameObjectComponent<SoundStream>(this.transform, clip.name);
				//即時再生
				current.Play(clip, masterVolume, volume, isLoop, isStreaming, callBackEnd);
				return current;
			}
			else
			{
				//フェードアウト後に再生
				current.FadeOut(fadeTime);
				next = UtageToolKit.AddChildGameObjectComponent<SoundStream>(this.transform, clip.name);
				next.Ready(clip, masterVolume, volume, isLoop, isStreaming, callBackEnd);
				return next;
			}
		}
		//サウンドを終了
		public void Stop(float fadeTime)
		{
			if (null != next) next.End();
			if (null != current) current.FadeOut(fadeTime);
		}

		void Update()
		{
			if (null != current) current.Update();

			if (null != next)
			{
				if (next.IsReady() && current == null)
				{
					current = next;
					current.Play();
					next = null;
				}
			}
		}


		bool IsPlaying()
		{
			if (current != null)
			{
				if (current.IsPlaying())
				{
					return true;
				}
			}
			return false;
		}
		const int VERSION = 0;
		//セーブデータ用のバイナリ書き込み
		public void WriteSaveData(BinaryWriter writer)
		{
			writer.Write(VERSION);
			if (IsPlaying())
			{
				writer.Write(current.FileName);
				writer.Write(current.RequestVolume);
				writer.Write(current.IsLoop);
				writer.Write(current.IsStreaming);
			}
			else
			{
				writer.Write("");
				writer.Write(0.0f);
				writer.Write(false);
				writer.Write(false);
			}
		}
		//セーブデータ用のバイナリ読み込み
		public void ReadSaveData(BinaryReader reader, float masterVolume)
		{
			int version = reader.ReadInt32();
			if (version == VERSION)
			{
				string fileName = reader.ReadString();
				float volume = reader.ReadSingle();
				bool isLoop = reader.ReadBoolean();
				bool isStreaming = reader.ReadBoolean();
				StartCoroutine(CoLoadAndPlayFile(fileName, masterVolume, volume, isLoop, isStreaming));
			}
			else
			{
				Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion,version));
			}
		}

		//指定のファイル名をロードして鳴らす
		IEnumerator CoLoadAndPlayFile(string path, float masterVolume, float volume, bool isLoop, bool isStreaming )
		{
			if (!string.IsNullOrEmpty(path))
			{
				AssetFile file = AssetFileManager.GetFileCreateIfMissing(path);
				if (isStreaming) file.AddLoadFlag( AssetFileLoadFlags.Streaming );
				AssetFileManager.Load(file, this);
				while (!file.IsLoadEnd) yield return 0;
				file.AddReferenceComponet(this.gameObject);
				file.Unuse(this);
				Play(file.Sound, 0.1f, masterVolume, volume, isLoop, isStreaming);
			}
		}
	}
}