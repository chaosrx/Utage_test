//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;

namespace Utage
{
	public enum Pivot
	{
		/// <summary>右上</summary>
		TopLeft,
		/// <summary>上</summary>
		Top,
		/// <summary>左上</summary>
		TopRight,
		/// <summary>左</summary>
		Left,
		/// <summary>中央</summary>
		Center,
		/// <summary>右</summary>
		Right,
		/// <summary>左下</summary>
		BottomLeft,
		/// <summary>下</summary>
		Bottom,
		/// <summary>右下</summary>
		BottomRight,
	};

	/// <summary>
	/// ピボット処理
	/// </summary>
	public static class PivotUtil
	{

		/// <summary>
		/// 文字列からピボットを解析する
		/// </summary>
		/// <param name="text">テキスト</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>解析したピボット値。文字列が設定されてなかったらデフォルト値を。解析できなかったら例外を投げる</returns>
		public static Vector2 ParsePivotOptional(string text, Vector2 defaultValue )
		{
			//何も設定がなければデフォルト値を
			if (string.IsNullOrEmpty(text))
			{
				return defaultValue;
			}

			Vector2 pivot;
			if (TryParsePivot(text, out pivot))
			{
				return pivot;
			}
			else
			{
				throw new System.Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.PivotParse, text));
			}
		}

		/// <summary>
		/// 文字列からカラーデータを解析する
		/// </summary>
		/// <param name="text">テキスト</param>
		/// <param name="color">カラー</param>
		/// <returns>解析に成功したらtrue。書式間違いなどで解析できなかったらfalse</returns>
		public static bool TryParsePivot(string text, out Vector2 pivot)
		{
			pivot = Vector2.one * 0.5f;
			if (string.IsNullOrEmpty(text)) return false;

			Pivot pivotEnum;
			if( UtageToolKit.TryParaseEnum<Pivot>(text, out pivotEnum) )
			{
				pivot = PivotEnumToVector2(pivotEnum);
				return true;
			}

			bool ret = false;
			string[] strings;
			{
				char[] separator = {' '};
				strings = text.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
			}
			foreach( string str in strings )
			{
				char[] separator = {'='};
				string[] tags = str.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
				if( tags.Length == 2 )
				{
					switch (tags[0])
					{
						case "x":
							if (!float.TryParse(tags[1], out pivot.x))
							{
								return false;
							}
							ret = true;
							break;
						case "y":
							if (!float.TryParse(tags[1], out pivot.y))
							{
								return false;
							}
							ret = true;
							break;
						default:
							return false;
					}
				}
				else
				{
					return false;
				}
			}
			return ret;
		}

		public static Vector2 PivotEnumToVector2(Pivot pivot)
		{
			switch (pivot)
			{
				case Pivot.TopLeft:
					return new Vector2(0.0f, 1.0f);
				case Pivot.Left:
					return new Vector2(0.0f, 0.5f);
				case Pivot.BottomLeft:
					return new Vector2(0.0f, 0.0f);
				case Pivot.Top:
					return new Vector2(0.5f, 1.0f);
				case Pivot.Center:
					return new Vector2(0.5f, 0.5f);
				case Pivot.Bottom:
					return new Vector2(0.5f, 0.0f);
				case Pivot.TopRight:
					return new Vector2(1.0f, 1.0f);
				case Pivot.Right:
					return new Vector2(1.0f, 0.5f);
				case Pivot.BottomRight:
					return new Vector2(1.0f, 0.0f);
				default:
					return new Vector2(0.5f, 0.5f);
			}
		}
	}
}