//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：BGM再生
	/// </summary>
	internal class AdvCommandBgm : AdvCommand
	{
		public AdvCommandBgm(StringGridRow row, AdvSettingDataManager dataManager)
		{
			string label = AdvParser.ParseCell<string>(row, AdvColumnName.Arg1);
			if (!dataManager.SoundSetting.Contains(label, SoundType.Bgm))
			{
				Debug.LogError(row.ToErrorString(label + " is not contained in file setting"));
			}

			this.file = AddLoadFile(dataManager.SoundSetting.LabelToFilePath(label, SoundType.Bgm));
		}
		public override void DoCommand(AdvEngine engine)
		{
			engine.SoundManager.Play(SoundManager.StreamType.Bgm, file, true, false );
		}
		AssetFile file;
	}
}