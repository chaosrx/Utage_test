//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections;

namespace Utage
{

	/// <summary>
	/// ラジオボタングループコンポーネント
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu("Utage/Lib/UI/RadioButtonGroup")]
	public class RadioButtonGroup : MonoBehaviour
	{
		/// <summary>
		/// 管理化のラジオボタン
		/// 子にある全てのラジオボタン
		/// </summary>
		public CheckBox[] RadioButtons
		{
			get
			{
				if (radioButtons == null) radioButtons = this.GetComponentsInChildren<CheckBox>(true);
				return radioButtons;
			}
		}
		CheckBox[] radioButtons;

		[SerializeField]
		int radioIndex;

		/// <summary>
		/// ラジオボタンを押したときのメッセージの送り先
		/// </summary>
		public GameObject Target
		{
			get { return target; }
			set { target = value; }
		}
		[SerializeField]
		GameObject target;

		/// <summary>
		/// ラジオボタンを押したときに送られるメッセージ
		/// </summary>
		public string FunctionName
		{
			get { return functionName; }
			set { functionName = value; }
		}
		[SerializeField]
		string functionName = "OnTapRadioButton";

		/// <summary>
		/// チェックされてるラジオボタンのインデックス
		/// </summary>
		public int RadioIndex
		{
			get { return this.radioIndex; }
			set
			{
				this.radioIndex = value;
				Refresh();
			}
		}

		void Awake()
		{
			Refresh();
		}

		void OnEnable()
		{
			Refresh();
		}

		void OnValidate()
		{
			Refresh();
		}

		void Refresh()
		{
			radioButtons = this.GetComponentsInChildren<CheckBox>(true);
			foreach (CheckBox checkBox in RadioButtons)
			{
				checkBox.Target = this.gameObject;
				checkBox.FunctionName = "OnRadioButtonClick";
			}
			foreach (CheckBox checkBox in RadioButtons)
			{
				if (checkBox.Index == radioIndex)
				{
					checkBox.IsChecked = true;
				}
				else
				{
					checkBox.IsChecked = false;
				}
			}
		}

		void OnRadioButtonClick( CheckBox checkBox )
		{
			RadioIndex = checkBox.Index;
			UtageToolKit.SafeSendMessage(this, Target, FunctionName);
		}
	}
}