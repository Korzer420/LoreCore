using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.FsmStateActions;
using KorzUtils.Helper;
using System.Linq;
using UnityEngine;

namespace LoreCore.Locations.SpecialLocations;

internal class ZoteGreenpathLocation : DialogueLocation
{
    protected override void OnLoad()
    {
        base.OnLoad();
        Events.AddFsmEdit(sceneName, new("Giant Buzzer", "FSM"), ManualZoteSpawn);
        Events.AddFsmEdit(sceneName, new("Giant Buzzer", "Encounter Control"), ManualZoteSpawn);
        Events.AddFsmEdit(sceneName, new("Corpse Giant Buzzer(Clone)", "corpse"), SetZoteFlag);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Events.RemoveFsmEdit(sceneName, new("Giant Buzzer", "FSM"), ManualZoteSpawn);
        Events.RemoveFsmEdit(sceneName, new("Giant Buzzer", "Encounter Control"), ManualZoteSpawn);
        Events.RemoveFsmEdit(sceneName, new("Corpse Giant Buzzer", "corpse"), SetZoteFlag);
    }

    private void ManualZoteSpawn(PlayMakerFSM fsm)
    {
        fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
        {
            Name = "Check Zote Spawn",
            Actions = new HutongGames.PlayMaker.FsmStateAction[]
            {
                new Lambda(() =>
                {
                    if (!Placement.Items.All(x => x.IsObtained()))
                    {
                        GameObject zote = Object.Instantiate(fsm.gameObject.LocateMyFSM("Big Buzzer").GetState("Unfurl").GetFirstAction<CreateObject>().gameObject.Value);
                        zote.SetActive(true);
                        zote.transform.position = new Vector3(47.5214f, 13.4081f, 0.004f);
                        zote.LocateMyFSM("Zote Buzzer Control").GetState("Land").AddActions(() => PlayMakerFSM.BroadcastEvent("SAVE ZOTE"));
                    }
                })
            }
        });
        if (fsm.FsmName == "FSM")
        {
            fsm.GetState("Check").AdjustTransition("DESTROY", "Check Zote Spawn");
            fsm.GetState("Check Zote Spawn").AddTransition("FINISHED", "Destroy");
        }
        else
        {
            fsm.GetState("State 1").AdjustTransition("KILLED", "Check Zote Spawn");
            fsm.GetState("Check Zote Spawn").AddTransition("FINISHED", "State 2");
        }
    }

    private void SetZoteFlag(PlayMakerFSM fsm)
    {
        fsm.GetState("Blow").AddActions(() => PlayerData.instance.SetBool(nameof(PlayerData.instance.zoteRescuedBuzzer), true));
    }
}
