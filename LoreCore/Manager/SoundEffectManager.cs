using ItemChanger.Internal;

namespace LoreCore.Manager;

internal static class SoundEffectManager
{
    public static SoundManager Manager { get; set; }

    static SoundEffectManager()
    {
        Manager = new(typeof(LoreCore).Assembly, "LoreCore.Resources.Sounds.");
    }
}