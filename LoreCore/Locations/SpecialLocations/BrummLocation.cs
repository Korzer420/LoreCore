using ItemChanger;
using KorzUtils.Helper;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations.SpecialLocations;

public class BrummLocation : DialogueLocation
{
    protected override void OnLoad()
    {
        Events.AddSceneChangeEdit(sceneName, ControlSpawn);
        base.OnLoad();
    }

    protected override void OnUnload()
    {
        Events.RemoveSceneChangeEdit(sceneName, ControlSpawn);
        base.OnUnload();
    }

    private void ControlSpawn(Scene scene)
    {
        GameObject brumm = Object.FindObjectsOfType<DeactivateIfPlayerdataTrue>(true).FirstOrDefault(x => x.gameObject.name == "Brum NPC")?.gameObject;
        if (brumm == null)
        {
            LogHelper.Write<LoreCore>("Failed to find brumm npc.", KorzUtils.Enums.LogType.Error);
            return;
        }
        if (Placement.AllObtained())
            brumm.SetActive(false);
        else
        { 
            Component.Destroy(brumm.GetComponent<DeactivateIfPlayerdataTrue>());
            brumm.SetActive(true);
        }
    }
}
