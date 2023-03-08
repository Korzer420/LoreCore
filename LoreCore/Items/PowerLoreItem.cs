using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Items;
using ItemChanger.UIDefs;
using LoreCore.Locations;
using LoreCore.Manager;
using LoreCore.Resources.Text;
using LoreCore.UIDefs;

namespace LoreCore.Items;

/// <summary>
/// An lore item which adds itself to the obtained powers and activates it. Also plays a custom sound file.
/// </summary>
internal class PowerLoreItem : LoreItem
{
    public delegate string ProcessPowerItem(string key, string originalText);

    public event ProcessPowerItem AcquirePowerItem;
     
    /// <summary>
    /// Gets or sets the name of the sound file which should be played.
    /// </summary>
    public string SoundClipName { get; set; } = "Lore";
    
    public override void GiveImmediate(GiveInfo info)
    {
        // Check if item is an actual power.
        string text = string.IsNullOrEmpty(loreKey) 
            ? InspectText.ResourceManager.GetString(name)
            : Language.Language.Get(loreKey, loreSheet);
        string finalText = AcquirePowerItem?.Invoke(loreKey, text);

        if (UIDef is LoreUIDef lore)
            lore.lore = new BoxedString(finalText ?? text);
        else if (UIDef is DreamLoreUIDef dream)
            dream.Text = finalText ?? text;
        // Plays the given sound file.
        if (SoundClipName == "Lore")
            SoundManager.Instance.PlayClipAtPoint("LoreSound", HeroController.instance.transform.localPosition);
        else
            SoundEffectManager.Manager.PlayClipAtPoint(SoundClipName, HeroController.instance.transform.position);
        if (ElderbugLocation.Instance != null)
            ElderbugLocation.Instance.AcquiredLore++;
    }
}
