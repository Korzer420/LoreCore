using HutongGames.PlayMaker;
using ItemChanger;

using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Items;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations;

/// <summary>
/// A location which can spawn an inspect collider and opens a text box upon inspecting.
/// </summary>
internal class InspectLocation : ContainerLocation
{
    public float XPosition { get; set; }

    public float YPosition { get; set; }

    /// <summary>
    /// The name of the game object, if an inspect region already exists. Using a game object, will make <see cref="Position"/> redundant.
    /// </summary>
    public string GameObjectName { get; set; }

    protected override void OnLoad()
    {
        if (string.IsNullOrEmpty(GameObjectName))
        { 
            Events.AddSceneChangeEdit(sceneName, SpawnInspectRegion);
            Events.AddFsmEdit(sceneName, new(name, "inspect_region"), ModifyInspectRegion);
        }
        else
            Events.AddFsmEdit(sceneName, new(GameObjectName, "inspect_region"), ModifyInspectRegion);
        if (name == LocationList.Grimm_Machine)
            Events.AddSceneChangeEdit("Town", BackupSpawn);
        
    }

    protected override void OnUnload()
    {
        if (string.IsNullOrEmpty(GameObjectName))
        {
            Events.RemoveSceneChangeEdit(sceneName, SpawnInspectRegion);
            Events.RemoveFsmEdit(sceneName, new(name, "inspect_region"), ModifyInspectRegion);
        }
        else
            Events.RemoveFsmEdit(sceneName, new(GameObjectName, "inspect_region"), ModifyInspectRegion);
        if (name == LocationList.Grimm_Machine)
            Events.RemoveSceneChangeEdit("Town", BackupSpawn);
    }

    private void ModifyInspectRegion(PlayMakerFSM fsm)
    {
        if (Placement.Items.All(x => x.IsObtained()) || !ReadItem.CanRead)
        {
            fsm.GetState("Idle").ClearTransitions();
            return;
        }
        fsm.AddState(new FsmState(fsm.Fsm)
        {
            Name = "Give Items",
            Actions =
            [
                new Lambda(() => fsm.GetState("Idle").ClearTransitions()),
                new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
                {
                    FlingType = flingType,
                    Container = Container.Tablet,
                    MessageType = MessageType.Any
                }, callback), "CONVO_FINISH")
            ]
        });
        fsm.GetState("Give Items").AddTransition("CONVO_FINISH", "Look Up End?");
        fsm.GetState("Hero Look Up?").ClearTransitions();
        fsm.GetState("Hero Look Up?").AddTransition("FINISHED", "Give Items");
    }

    private void SpawnInspectRegion(Scene scene)
    {
        // Also spawn a computer for record bela.
        if (name == LocationList.Lore_Tablet_Record_Bela)
        {
            GameObject tablet = Object.Instantiate(LoreCore.Instance.PreloadedObjects["Glow Response Mage Computer"]);
            tablet.name = "Mage_Computer_2";
            tablet.transform.localPosition = new(70f, 6.21f, .02f);
            tablet.SetActive(true);
        }

        if (Placement.Items.All(x => x.IsObtained()))
            return;
        GameObject inspectRegion = Object.Instantiate(LoreCore.Instance.PreloadedObjects["Inspect Region"]);
        inspectRegion.name = name;
        inspectRegion.transform.localPosition = new(XPosition, YPosition);
        inspectRegion.SetActive(true);
    }

    private void BackupSpawn(Scene scene)
    {
        if (!Placement.Items.All(x => x.IsObtained()) && PDHelper.NymmInTown)
            ItemHelper.SpawnShiny(new(90.33f, 11.41f), Placement);
    }
}
