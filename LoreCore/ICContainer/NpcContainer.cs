using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Components;
using KorzUtils.Helper;
using LoreCore.Locations;
using System.Collections.Generic;
using UnityEngine;

namespace LoreCore.ICContainer;

public class NpcContainer : Container
{
    #region Properties

    public override string Name => "Npc";

    public override bool SupportsInstantiate => true;

    public override bool SupportsDrop => true;

    #endregion

    #region Methods

    public override GameObject GetNewContainer(ContainerInfo info)
    {
        LogHelper.Write("Called");
        string npcName = null;
        foreach (AbstractItem item in info.giveInfo.placement.Items)
        {
            string itemName = item.name.ToLower();
            if (itemName.Contains("quirrel"))
                npcName = "Quirrel";
            else if (itemName.Contains("zote"))
                npcName = "Zote";
            else if (itemName.Contains("bretta"))
                npcName = "Bretta";
            else if (itemName.Contains("vespa"))
                npcName = "Vespa";
            else if (itemName.Contains("mask_maker"))
                npcName = "Mask_Maker";
            else if (itemName.Contains("gravedigger"))
                npcName = "Gravedigger";
            else if (itemName.Contains("poggy"))
                npcName = "Poggy";
            else if (itemName.Contains("joni"))
                npcName = "Joni";
            else if (itemName.Contains("myla"))
                npcName = "Myla";
            else if (itemName.Contains("emilitia"))
                npcName = "Emilitia";
            else if (itemName.Contains("willoh"))
                npcName = "Willoh";
            else if (itemName.Contains("fluke_hermit"))
                npcName = "Fluke_Hermit";
            else if (itemName.Contains("marissa"))
                npcName = "Marissa";
            else if (itemName.Contains("grasshopper"))
                npcName = "Grasshopper";
            else if (itemName.Contains("sly"))
                npcName = "Sly";
            else if (itemName.Contains("iselda"))
                npcName = "Iselda";
            else if (itemName.Contains("little_fool"))
                npcName = "Little_Fool";
            else if (itemName.Contains("salubra"))
                npcName = "Salubra";
            else if (itemName.Contains("brumm"))
                npcName = "Brumm";
            else if (itemName.Contains("tiso"))
                npcName = "Tiso";
            else if (itemName.Contains("cloth"))
                npcName = "Cloth";
            else if (itemName.Contains("Hornet"))
                npcName = "Hornet";

            if (!string.IsNullOrEmpty(npcName))
                break;
        }
        

        if (string.IsNullOrEmpty(npcName))
            return base.GetNewContainer(info);

        GameObject newNpc = GameObject.Instantiate(LoreCore.Instance.PreloadedObjects[npcName]);
        newNpc.AddComponent<DropIntoPlace>();

        PlayMakerFSM ghostDeath = newNpc.LocateMyFSM("ghost_npc_death");
        if (ghostDeath != null)
        {
            // Prevent killing the ghost.
            ghostDeath.GetState("Idle").ClearTransitions();

            // Remove dream nail requirement.
            newNpc.LocateMyFSM("Conversation Control").GetState("Init").RemoveActions<PlayerDataBoolTest>();
            newNpc.LocateMyFSM("Appear").GetState("Init").RemoveActions<PlayerDataBoolTest>();
        }
        DialogueLocation.ModifyDialogue(newNpc.LocateMyFSM("Conversation Control"), info.giveInfo.placement);
        TravellerLocation.RemoveSpawnConditions(newNpc);
        newNpc.SetActive(true);

        return newNpc;
    }

    #endregion
}
