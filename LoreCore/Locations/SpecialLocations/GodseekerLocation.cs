using MonoMod.Cil;
using System;

namespace LoreCore.Locations.SpecialLocations;

internal class GodseekerLocation : DialogueLocation
{
    protected override void OnLoad()
    {
        base.OnLoad();
        IL.BossDoorCompletionStates.Start += BossDoorCompletionStates_Start;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        IL.BossDoorCompletionStates.Start -= BossDoorCompletionStates_Start;
    }

    private void BossDoorCompletionStates_Start(ILContext il)
    {
        ILCursor iLCursor = new(il);
        iLCursor.Goto(0);
        iLCursor.TryGotoNext(MoveType.After,
            x => x.MatchLdfld<bool>("completed"));
        iLCursor.EmitDelegate<Func<bool, bool>>(x => true);
    }
}
