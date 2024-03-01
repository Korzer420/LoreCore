using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Items;
using ItemChanger.Modules;
using ItemChanger.UIDefs;
using LoreCore.Data;
using LoreCore.Manager;
using LoreCore.Other;
using LoreCore.Resources.Text;
using LoreCore.UIDefs;

namespace LoreCore.Items;

/// <summary>
/// An lore item which adds itself to the obtained powers and activates it. Also plays a custom sound file.
/// </summary>
public class PowerLoreItem : LoreItem
{
    public delegate string ProcessPowerItem(string key, string originalText);

    public static event ProcessPowerItem AcquirePowerItem;

    /// <summary>
    /// Gets or sets the name of the sound file which should be played.
    /// </summary>
    public string SoundClipName { get; set; } = "Lore";

    //public override string GetPreferredContainer() => "Npc";

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
        if (ElderbugPlacement.Instance != null)
            ElderbugPlacement.Instance.AcquiredLore++;
        if (name == ItemList.Dialogue_Bretta)
            PlayerData.instance.SetBool(nameof(PlayerData.instance.brettaRescued), true);
        else if (name == ItemList.Dream_Dialogue_Grimm_Summoner)
        {
            PlayerData.instance.SetBool(nameof(PlayerData.nightmareLanternAppeared), true);
            PlayerData.instance.SetBool(nameof(PlayerData.nightmareLanternLit), true);
            PlayerData.instance.SetBool(nameof(PlayerData.troupeInTown), true);
            PlayerData.instance.SetBool(nameof(PlayerData.divineInTown), true);
            PlayerData.instance.SetBool(nameof(PlayerData.metGrimm), true);
            PlayerData.instance.SetInt(nameof(PlayerData.flamesRequired), 3);
        }
        else if (name == ItemList.Dialogue_Sly)
        { 
            PlayerData.instance.SetBool(nameof(PlayerData.slyRescued), true);
            // ItemChanger handles the shop with an extra flag which we need to set as well.
            ItemChangerMod.Modules.GetOrAdd<SlyRescuedEvent>().SlyRescued = true;
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        if (name == ItemList.Dialogue_Iselda)
            // Skip first dialogue from Iselda
            PlayerData.instance.SetBool(nameof(PlayerData.metIselda), true);
        else if (name == ItemList.Dialogue_Salubra)
            // Skip first dialogue from Salubra
            PlayerData.instance.SetBool(nameof(PlayerData.metCharmSlug), true);
    }
}
