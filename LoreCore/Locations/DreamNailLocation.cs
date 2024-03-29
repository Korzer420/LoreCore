using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;

using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using System.Linq;
using UnityEngine;

namespace LoreCore.Locations;

/// <summary>
/// A location which needs to be dream nailed to give the items.
/// </summary>
internal class DreamNailLocation : AutoLocation
{
    public string GameObjectName { get; set; }

    protected override void OnLoad()
    {
        Events.AddFsmEdit(sceneName, new(GameObjectName, "npc_dream_dialogue"), ModifyDreamDialogue);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(sceneName, new(GameObjectName, "npc_dream_dialogue"), ModifyDreamDialogue);
    }

    private void ModifyDreamDialogue(PlayMakerFSM fsm)
    {
        if (!Placement.Items.All(x => x.IsObtained()))
        {
            fsm.AddState(new FsmState(fsm.Fsm)
            {
                Name = "Give Items",
                Actions = new FsmStateAction[]
                {
                    new Lambda(() =>
                    {
                        GameObject hint = GameObject.Find("Dream Hint");
                        if (hint is not null)
                            Object.Destroy(hint);
                    }),
                    new Lambda(() => fsm.GetState("Idle").ClearTransitions()),
                    new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
                    {
                        FlingType = flingType,
                        Container = Container.Tablet,
                        MessageType = MessageType.Any
                    }, callback), "CONVO_FINISH")
                }
            });
            fsm.GetState("Give Items").AddTransition("CONVO_FINISH", "Box Down");
            fsm.GetState("Impact").AdjustTransition("FINISHED", "Give Items");
            // Remove the box down event (the textbox will be handled in the UIDef)
            fsm.GetState("Box Down").RemoveFirstAction<SendEventByName>();

            GameObject hint = Object.Instantiate(LoreCore.Instance.PreloadedObjects["Ghost NPC/Idle Pt"]);
            hint.name = "Dream Hint";
            hint.SetActive(true);
            hint.GetComponent<ParticleSystem>().enableEmission = true;
            hint.transform.position = fsm.transform.position;
            hint.transform.position -= new Vector3(0f,0f, 3f);
        }
        else
            fsm.GetState("Idle").ClearTransitions();
    }
}
