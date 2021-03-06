//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：パラメーターに数値代入
	/// </summary>
	internal class AdvCommandParam : AdvCommand
	{

		public AdvCommandParam(StringGridRow row, AdvSettingDataManager dataManger)
		{
			this.exp = dataManger.DefaultParam.CreateExpression(AdvParser.ParseCell<string>(row, AdvColumnName.Arg1));
			if (this.exp.ErrorMsg != null)
			{
				Debug.LogError(row.ToErrorString(this.exp.ErrorMsg));
			}
		}

		public override void DoCommand(AdvEngine engine)
		{
			engine.Param.CalcExpression(exp);
		}
		ExpressionParser exp;
	}
}