using ItemChanger;

namespace LoreCore.Other;

internal class ClothTest : IBool
{
    public bool Value => PlayerData.instance.GetBool(nameof(PlayerData.instance.clothKilled));

    public IBool Clone() => new ClothTest();
}
