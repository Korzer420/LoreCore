using ItemChanger;

using ItemChanger.FsmStateActions;
using KorzUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoreCore.Locations.SpecialLocations;

public class SlyLocation : DialogueLocation
{
    protected override void OnLoad()
    {
        base.OnLoad();
        Events.AddFsmEdit(sceneName, new("Conversation Control"), ControlSpawn);
    }

    private void ControlSpawn(PlayMakerFSM fsm)
    {
        fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
        {
            Name = "Check",
            Actions = new HutongGames.PlayMaker.FsmStateAction[]
            {
                new Lambda(() => fsm.SendEvent(Placement.AllObtained() ? "DESTROY" : "FINISHED"))
            }
        });
        fsm.GetState("Init").AdjustTransition("FINISHED", "Check");
        fsm.GetState("Check").AddTransition("FINISHED", "Audio");
        fsm.GetState("Check").AddTransition("DESTROY", "Destroy");
    }
}
