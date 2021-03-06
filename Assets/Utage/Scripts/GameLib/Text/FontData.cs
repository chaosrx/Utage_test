//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// フォントデータ
	/// </summary>
	public class FontData : ScriptableObject
	{
		/// <summary>
		/// フォント
		/// </summary>
		public Font Font { get { return font; } }
		[SerializeField]
		Font font;

		/// <summary>
		/// サイズ
		/// </summary>
		public int Size { get { return size; } }
		[SerializeField]
		int size = 16;

		/// <summary>
		/// フォントスタイル
		/// </summary>
		[SerializeField]
		FontStyle style = FontStyle.Normal;

		/// <summary>
		/// 描画のY位置のオフセット。
		/// フォントの種類によって、中央ではなく上が中心になっていたりするのでそれの修正用
		/// </summary>
		public float OffsetY { get { return offsetY; } }
		[SerializeField]
		float offsetY;

		Dictionary<char, FontRenderInfo> renderInfos = new Dictionary<char, FontRenderInfo>();

		/// <summary>
		/// フォントをテクスチャに書き込んでアトラスを作成
		/// </summary>
		/// <param name="characters"></param>
		public void MakeFontAtlas(string characters)
		{
			font.textureRebuildCallback = CallbackFontTextureRebuild;
			font.RequestCharactersInTexture(characters, size, style);
			font.textureRebuildCallback = null;
		}

		/// <summary>
		/// 指定の文字の描画情報取得
		/// もし、未作成だったら作成してから描画情報を返す
		/// </summary>
		/// <param name="c">文字</param>
		/// <returns>描画情報</returns>
		public FontRenderInfo GetRenderInfoCreateIfMissing(char c, float pixelsToUnits)
		{
			FontRenderInfo renderInfo;
			bool isMakeSprite = false;
			if (!renderInfos.TryGetValue(c, out renderInfo))
			{
				isMakeSprite = true;
			}
			else
			{
				if (null == renderInfo.Sprite) isMakeSprite = true;
			}

			if (isMakeSprite)
			{
				CharacterInfo info;
				if (font.GetCharacterInfo(c, out info, size, style))
				{
					Texture2D texture = font.material.mainTexture as Texture2D;
					float x = info.uv.x * texture.width;
					float w = info.uv.width * texture.width;
					float y = info.uv.y * texture.height;
					float h = info.uv.height * texture.height;
					Rect rect = new Rect(x, y, w, h);
					Vector2 pivot = new Vector2(0.5f, 0.5f);
					Sprite sprite = Sprite.Create(texture, rect, pivot,pixelsToUnits,0,SpriteMeshType.FullRect);
					renderInfo = new FontRenderInfo(c, info, sprite, OffsetY);
					if (renderInfos.ContainsKey(c))
					{
						renderInfos[c] = renderInfo;
					}
					else
					{
						renderInfos.Add(c, renderInfo);
					}
				}
				else
				{
					Debug.LogError( LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownFontData, c) );
					return null;
				}
			}
			return renderInfo;
		}

		void CallbackFontTextureRebuild()
		{
			renderInfos.Clear();
			TextArea2D[] texts = GameObject.FindObjectsOfType<TextArea2D>();
			foreach (TextArea2D text in texts)
			{
				text.RebuildFont(this);
			}
		}
	}
}