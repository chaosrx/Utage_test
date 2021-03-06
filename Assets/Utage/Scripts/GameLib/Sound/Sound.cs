using UnityEngine;

namespace Utage
{
/*
	/// <summary>
	/// サウンド処理
	/// </summary>
	public class Sound
	{
		const float DefaultFadeTime = 0.15f;
		const float DefaultVoiceFadeTime = 0.05f;
		const float DefaultVolume = 1.0f;

		/// <summary>
		/// サウンドの再生
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		/// <param name="file">サウンドファイル</param>
		/// <param name="isLoop">ループ再生するか</param>
		/// <param name="isReplay">直前が同じサウンドなら鳴らしなおすか</param>
		/// <returns>再生をしているサウンドストリーム</returns>
		public static void Play(SoundManager.StreamType type, AssetFile file, bool isLoop, bool isReplay = false)
		{
			float fadeTime = type == SoundManager.StreamType.Voice ? DefaultVoiceFadeTime : DefaultFadeTime;
			Play(type, file, isLoop, fadeTime, isReplay);
		}

		/// <summary>
		/// サウンドの再生
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		/// <param name="file">サウンドファイル</param>
		/// <param name="isLoop">ループ再生するか</param>
		/// <param name="fadeTime">フェード時間</param>
		/// <param name="isReplay">直前が同じサウンドなら鳴らしなおすか</param>
		/// <returns>再生をしているサウンドストリーム</returns>
		public static void Play(SoundManager.StreamType type, AssetFile file, bool isLoop, float fadeTime, bool isReplay = false)
		{
			if (!isReplay && IsPlaying(type, file.Sound))
			{
			}
			else
			{
				SoundManager manager = SoundManager.GetInstance();
				SoundStream stream = manager.Play(type, file.Sound, fadeTime, DefaultVolume, isLoop);
				if (null != stream)
				{
					file.AddReferenceComponet(stream.gameObject);
				}
			}
		}

		/// <summary>
		/// サウンドの再生
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		/// <param name="clip">オーディオクリップ</param>
		/// <param name="isLoop">ループ再生するか</param>
		/// <param name="isReplay">直前が同じサウンドなら鳴らしなおすか</param>
		/// <returns>再生をしているサウンドストリーム</returns>
		public static void Play(SoundManager.StreamType type, AudioClip clip, bool isLoop, bool isReplay = false)
		{
			float fadeTime = type == SoundManager.StreamType.Voice ? DefaultVoiceFadeTime : DefaultFadeTime;
			Play(type, clip, isLoop, fadeTime, isReplay);
		}

		/// <summary>
		/// サウンドの再生
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		/// <param name="clip">オーディオクリップ</param>
		/// <param name="isLoop">ループ再生するか</param>
		/// <param name="fadeTime">フェード時間</param>
		/// <param name="isReplay">直前が同じサウンドなら鳴らしなおすか</param>
		/// <returns>再生をしているサウンドストリーム</returns>
		public static void Play(SoundManager.StreamType type, AudioClip clip, bool isLoop, float fadeTime, bool isReplay = false)
		{
			if (!isReplay && IsPlaying(type, clip))
			{
			}
			else
			{
				SoundManager manager = SoundManager.GetInstance();
				manager.Play(type, clip, fadeTime, DefaultVolume, isLoop);
			}
		}

		/// <summary>
		/// 指定したタイプのサウンドの停止
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		public static void Stop(SoundManager.StreamType type )
		{
			float fadeTime = type == SoundManager.StreamType.Voice ? DefaultVoiceFadeTime : DefaultFadeTime;
			Stop(type, fadeTime);
		}

		/// <summary>
		/// 指定したタイプのサウンドの停止
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		/// <param name="fadeTime">フェードアウトの時間</param>
		public static void Stop(SoundManager.StreamType type, float fadeTime )
		{
			SoundManager manager = SoundManager.GetInstance();
			manager.Stop(type, fadeTime);
		}

		/// <summary>
		/// 指定したタイプのサウンドが停止しているか
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		/// <returns>鳴ってなければtrue。鳴ってればfalse</returns>
		public static bool IsStop(SoundManager.StreamType type)
		{
			SoundManager manager = SoundManager.GetInstance();
			return manager.IsStop(type);
		}

		/// <summary>
		/// SEの再生
		/// </summary>
		/// <param name="clip">オーディオクリップ</param>
		/// <returns>再生をしているサウンドストリーム</returns>
		public static AudioSource PlaySE(AudioClip clip)
		{
			SoundManager manager = SoundManager.GetInstance();
			return manager.PlaySE(clip, DefaultVolume);
		}

		/// <summary>
		/// SEの再生
		/// </summary>
		/// <param name="file">サウンドファイル</param>
		/// <returns>再生をしているサウンドストリーム</returns>
		public static AudioSource PlaySE(AssetFile file)
		{
			SoundManager manager = SoundManager.GetInstance();
			AudioSource audio = manager.PlaySE(file.Sound, DefaultVolume);
			file.AddReferenceComponet(audio.gameObject);
			return audio;
		}

		/// <summary>
		/// 指定のサウンドが鳴っているか
		/// </summary>
		/// <param name="type">サウンドのタイプ</param>
		/// <param name="clip">オーディオクリップ</param>
		/// <returns>鳴っていればtrue。鳴っていなければfalse</returns>
		public static bool IsPlaying(SoundManager.StreamType type, AudioClip clip)
		{
			SoundManager manager = SoundManager.GetInstance();
			return manager.IsPlaying(type, clip);
		}
	}
 */ 
}
