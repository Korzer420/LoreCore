using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Items;
using LoreCore.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations;

internal class ElderbugLocation : AutoLocation
{
    #region Properties

    public int AcquiredLore { get; set; }

    public static ElderbugLocation Instance { get; set; }

    #endregion

    #region Methods

    protected override void OnLoad()
    {
        Events.AddFsmEdit("Town", new("Elderbug", "Conversation Control"), ModifyElderbug);
        Events.AddSceneChangeEdit("Town", ElderbugPreview);
        Events.AddLanguageEdit(new("Elderbug", "Elderbug_Task_Successful"), CanGiveItems);
        Events.AddLanguageEdit(new("Elderbug", "Elderbug_Task_Failed"), CannotGiveItems);
        Events.AddLanguageEdit(new("Elderbug", "Elderbug_Preview"), Preview);
        AbstractItem.AfterGiveGlobal += AbstractItem_AfterGiveGlobal;
        Instance = this;
    }

    private void AbstractItem_AfterGiveGlobal(ReadOnlyGiveEventArgs itemEventArgs)
    {
        if (itemEventArgs?.Item?.name?.StartsWith("Lore") == true && itemEventArgs.Item is not PowerLoreItem)
            AcquiredLore++;
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit("Town", new("Elderbug", "Conversation Control"), ModifyElderbug);
        Events.RemoveSceneChangeEdit("Town", ElderbugPreview);
        Events.RemoveLanguageEdit(new("Elderbug", "Elderbug_Task_Successful"), CanGiveItems);
        Events.RemoveLanguageEdit(new("Elderbug", "Elderbug_Task_Failed"), CannotGiveItems);
        Events.RemoveLanguageEdit(new("Elderbug", "Elderbug_Preview"), Preview);
        Instance = null;
    }

    private void CanGiveItems(ref string value) => value = "Ah, I see that you've acquired some more knowledge. Let me reward you for that.";

    private void CannotGiveItems(ref string value) => value = "It seems that you've not obtained enough knowledge yet. Come back to me once you a are a little... wiser";

    private void Preview(ref string value)
    {
        value = "If you show you that you've got that much knowledge, I'll reward you with:\n";
        foreach (AbstractItem item in Placement.Items)
        {
            if (item.GetTag<CostTag>()?.Cost is LoreCost loreCost)
                value += $"{item.GetPreviewName(Placement)} - {loreCost.GetCostText()}\n";
            else
                value += "??? - " + item.GetPreviewName(Placement) +"\n";
        }
    }

    private void ModifyElderbug(PlayMakerFSM self)
    {
        self.AddState(new FsmState(self.Fsm)
        {
            Name = Placement.Name,
            Actions = new FsmStateAction[]
            {
                new Lambda(() =>
                {
                    List<AbstractItem> itemsToGive = new();
                    for (int i = 0; i < Placement.Items.Count; i++)
                    {
                        AbstractItem item = Placement.Items[i];
                        if (!item.IsObtained() && item.GetTag<CostTag>()?.Cost is LoreCost cost
                        && !cost.Paid && cost.CanPay())
                        itemsToGive.Add(item);
                    }

                    string conversationName = Placement.Name;
                    conversationName = itemsToGive.Any() 
                    ? $"Elderbug_Task_Successful"
                    : $"Elderbug_Task_Failed";
                    self.GetState("Sly Rescued")
                    .GetFirstActionOfType<CallMethodProper>().gameObject.GameObject.Value
                    .GetComponent<DialogueBox>()
                    .StartConversation(conversationName, "Elderbug");
                })
            }
        });

        self.AddState(new FsmState(self.Fsm)
        {
            Name = $"{Placement.Name} Throw",
            Actions = new FsmStateAction[]
            {
                new Lambda(() =>
                {
                    List<AbstractItem> itemsToGive = new();
                    for (int i = 0; i < Placement.Items.Count; i++)
                    {
                        AbstractItem item = Placement.Items[i];
                        if (!item.IsObtained() && item.GetTag<CostTag>()?.Cost is LoreCost cost
                        && !cost.Paid && cost.CanPay())
                        itemsToGive.Add(item);
                    }
                    if (itemsToGive.Any())
                        ItemHelper.FlingShiny(self.gameObject, Placement, itemsToGive);
                })
            }
        });

        self.GetState("Convo Choice").AddTransition(Placement.Name, Placement.Name);
        self.GetState(Placement.Name).AddTransition("CONVO_FINISH", $"{Placement.Name} Throw");
        self.GetState($"{Placement.Name} Throw").AddTransition("FINISHED", "Talk Finish");
    }

    private void ElderbugPreview(Scene scene)
    {
        GameObject tablet = GameObject.Instantiate(LoreCore.Instance.PreloadedObjects["Glow Response Mage Computer"]);
        tablet.name = "Elderbug_Tablet";
        tablet.transform.localPosition = new(105.74f, 14.21f, 0.5f);
        tablet.SetActive(true);

        GameObject inspectRegion = GameObject.Instantiate(LoreCore.Instance.PreloadedObjects["Inspect Region"]);
        inspectRegion.name = "Inspect_Elderbug_Tablet";
        inspectRegion.transform.localPosition = new(105.74f, 12.11f);
        inspectRegion.SetActive(true);
        inspectRegion.LocateMyFSM("inspect_region").FsmVariables.FindFsmString("Game Text Convo").Value = "Elderbug_Preview";

        List<AbstractItem> alreadyThrownItems = new();
        for (int i = 0; i < Placement.Items.Count; i++)
        {
            AbstractItem item = Placement.Items[i];
            if (!item.IsObtained() && item.GetTag<CostTag>()?.Cost is LoreCost cost
            && cost.Paid)
                alreadyThrownItems.Add(item);
        }
        if (alreadyThrownItems.Any())
            ItemHelper.FlingShiny(tablet, Placement, alreadyThrownItems);
    }

    #endregion
}
