using ItemChanger;
using KorzUtils.Helper;
using System;
using UnityEngine;

namespace LoreCore;

[Serializable]
public class WrappedSprite : ISprite
{
    #region Constructors

    public WrappedSprite() { }

    public WrappedSprite(string key)
    {
        if (!string.IsNullOrEmpty(key))
            Key = key;
    }

    #endregion

    #region Properties

    public string Key { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public Sprite Value => SpriteHelper.CreateSprite<LoreCore>("Sprites." + Key.Replace("/", ".").Replace("\\", "."));

    #endregion

    public ISprite Clone() => new WrappedSprite(Key);
}
