using ItemChanger;
using KorzUtils.Helper;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations.SpecialLocations;

internal class HornetEdgeLocation : DialogueLocation
{
    protected override void OnLoad()
    {
        Events.AddSceneChangeEdit("Deepnest_East_Hornet", SpawnReoccuring);
        base.OnLoad();
    }

    protected override void OnUnload()
    {
        Events.RemoveSceneChangeEdit("Deepnest_East_Hornet", SpawnReoccuring);
        base.Unload();
    }
    // Abyss_06_Core -> Hornet Abyss NPC
    // Deepnest_Spider_Town -> Hornet Beast Den NPC
    // Room_temple -> Hornet Black Egg NPC
    private void SpawnReoccuring(Scene scene)
    {
        if (Placement.AllObtained())
            return;
        if (TravellerLocation.Stages[Enums.Traveller.Hornet] < 2)
        {
            GameObject encounter = GameObject.Find("Hornet Outskirts Battle Encounter");
            if (encounter != null)
                GameObject.Destroy(encounter);
            return;
        }
        if (Placement.Items.All(x => x.WasEverObtained()))
            ItemHelper.SpawnShiny(new(26.7f, 28.42f), Placement);
    }
}
