using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Placements;
using ItemChanger.Tags;
using ItemChanger.Util;
using KorzUtils.Data;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Items;
using LoreCore.Modules;
using LoreCore.Resources.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Other;

public class ElderbugPlacement : AbstractPlacement, IMultiCostPlacement, IPrimaryLocationPlacement
{
    #region Constructors

    public ElderbugPlacement(string name) : base(name) { }

    #endregion

    #region Properties

    public PlaceableLocation Location { get; set; }

    AbstractLocation IPrimaryLocationPlacement.Location => Location;

    public int AcquiredLore { get; set; }

    public static ElderbugPlacement Instance { get; set; }

    public bool IsRando { get; set; } = true;

    public bool TalkedToElderbug { get; set; }

    #endregion

    #region Event handler

    private void AbstractItem_AfterGiveGlobal(ReadOnlyGiveEventArgs itemEventArgs)
    {
        if (itemEventArgs?.Item?.name?.StartsWith("Lore") == true && itemEventArgs.Item is not PowerLoreItem)
            AcquiredLore++;
    }

    private void CanGiveItems(ref string value) => value = ElderbugText.Enough;
    
    private void CannotGiveItems(ref string value) => value = ElderbugText.NotEnough;

    private void Introduction(ref string value) => value = ElderbugText.Introduction1;

    private void IntroductionSecondPart(ref string value) => value = ElderbugText.Introduction2;

    private void Preview(ref string value)
    {
        value = "If you show me that you've got that much knowledge, I'll reward you with:\n";
        int entry = 1;
        foreach (AbstractItem item in Items.Where(x => x.name != "Artifact_Lore"))
        {
            if (item.GetTag<CostTag>()?.Cost is LoreCost loreCost)
                value += $"{item.GetPreviewName(this)} - {loreCost.GetCostText()}";
            else
                value += "??? - " + item.GetPreviewName(this);
            entry++;
            if (entry == 3)
            {
                value += "<page>";
                entry = 0;
            }
            else
                value += "<br>";
        }
    }

    private void ModifyElderbug(PlayMakerFSM self)
    {
        // Force Elderbug to always appear.
        self.GetState("Grimm?").AdjustTransition("DISABLE", "Idle");
        self.transform.localScale = new(2f, 2f);
        self.transform.localPosition = new(126.36f, 12.35f, self.transform.localPosition.z);
        self.gameObject.GetComponent<BoxCollider2D>().size = new(1.8361f, 0.2408f);
        self.AddState(new FsmState(self.Fsm)
        {
            Name = "Control",
            Actions =
            [
                new Lambda(() =>
                {
                    if (!IsRando && !TalkedToElderbug)
                        self.SendEvent("GRANT");
                    else if (!PlayerData.instance.GetBool(nameof(PlayerData.instance.hasXunFlower))
                    || PlayerData.instance.GetBool(nameof(PlayerData.instance.elderbugGaveFlower))
                    || PlayerData.instance.GetBool(nameof(PlayerData.instance.xunFlowerBroken)))
                        self.SendEvent("GENERIC");
                    else
                        self.SendEvent(PlayerData.instance.GetBool(nameof(PlayerData.instance.elderbugRequestedFlower))
                            ? "FLOWER REOFFER"
                            : "FLOWER OFFER");
                })
            ]
        });
        self.AddState(new FsmState(self.Fsm)
        {
            Name = Name,
            Actions =
            [
                new Lambda(() =>
                {
                    List<AbstractItem> itemsToGive = new();
                    for (int i = 0; i < Items.Count; i++)
                    {
                        AbstractItem item = Items[i];
                        if (!item.IsObtained() && item.GetTag<CostTag>()?.Cost is LoreCost cost
                        && !cost.Paid && cost.CanPay())
                            itemsToGive.Add(item);
                    }

                    string conversationName = Name;
                    conversationName = itemsToGive.Any()
                        ? $"Elderbug_Task_Successful"
                        : $"Elderbug_Task_Failed";
                    self.GetState("Sly Rescued")
                    .GetFirstAction<CallMethodProper>().gameObject.GameObject.Value
                    .GetComponent<DialogueBox>()
                    .StartConversation(conversationName, "Elderbug");
                })
            ]
        });
        self.AddState(new FsmState(self.Fsm)
        {
            Name = $"{Name} Throw",
            Actions =
            [
                new Lambda(() =>
                {
                    List<AbstractItem> itemsToGive = new();
                    for (int i = 0; i < Items.Count; i++)
                    {
                        AbstractItem item = Items[i];
                        if (!item.IsObtained() && item.GetTag<CostTag>()?.Cost is LoreCost cost
                        && !cost.Paid && cost.CanPay())
                            itemsToGive.Add(item);
                    }
                    if (itemsToGive.Any())
                        ItemHelper.FlingShiny(self.gameObject, this, itemsToGive);
                })
            ]
        });
        self.AddState(new FsmState(self.Fsm)
        {
            Name = "Introduction",
            Actions = 
            [
                new Lambda(() =>
                {
                    TalkedToElderbug = true;
                    self.GetState("Sly Rescued")
                        .GetFirstAction<CallMethodProper>().gameObject.GameObject.Value
                        .GetComponent<DialogueBox>()
                        .StartConversation("Introduction", "Elderbug");
                })
            ]
        });
        self.AddState(new FsmState(self.Fsm)
        {
            Name = "Power Artifact",
            Actions =
            [
                new AsyncLambda(callback => ItemUtility.GiveSequentially([Items.First(x => x.name == "Artifact_Lore")], this, new GiveInfo
                {
                    FlingType = FlingType.DirectDeposit,
                    Container = Container.Tablet,
                    MessageType = MessageType.Big,
                }, callback), "CONVO_FINISH")
            ]
        });
        self.AddState(new FsmState(self.Fsm)
        {
            Name = "Introduction 2",
            Actions =
            [
                new Lambda(() =>
                {
                    self.GetState("Sly Rescued")
                        .GetFirstAction<CallMethodProper>().gameObject.GameObject.Value
                        .GetComponent<DialogueBox>()
                        .StartConversation("Introduction2", "Elderbug");
                })
            ]
        });

        self.GetState("Box Up").AdjustTransition("FINISHED", "Control");
        self.GetState("Control").AddTransition("GENERIC", Name);
        self.GetState("Control").AddTransition("FLOWER OFFER", "Flower Offer");
        self.GetState("Control").AddTransition("FLOWER REOFFER", "Flower Reoffer");
        self.GetState("Control").AddTransition("GRANT", "Introduction");
        self.GetState(Name).AddTransition("CONVO_FINISH", $"{Name} Throw");
        self.GetState($"{Name} Throw").AddTransition("FINISHED", "Talk Finish");
        self.GetState("Introduction").AddTransition("CONVO_FINISH", "Power Artifact");
        self.GetState("Power Artifact").AddTransition("CONVO_FINISH", "Introduction 2");
        self.GetState("Introduction 2").AddTransition("CONVO_FINISH", "Talk Finish");
    }

    private void ElderbugPreview(Scene scene)
    {
        GameObject tablet = TabletUtility.MakeNewTablet(this, GetPreviewText);
        tablet.name = "Elderbug_Tablet";
        tablet.transform.localPosition = new(105.74f, 11.71f, 2.5f);
        tablet.SetActive(true);

        List<AbstractItem> alreadyThrownItems = new();
        for (int i = 0; i < Items.Count; i++)
        {
            AbstractItem item = Items[i];
            if (!item.IsObtained() && item.GetTag<CostTag>()?.Cost is LoreCost cost
            && cost.Paid)
                alreadyThrownItems.Add(item);
        }
        if (alreadyThrownItems.Any())
            ItemHelper.FlingShiny(tablet, this, alreadyThrownItems);
        GameManager.instance.StartCoroutine(WaitThenMoveBench());
    }

    /// <summary>
    /// Build the preview text of the Elderbug "shop". 
    /// Basically just taken from https://github.com/homothetyhk/HollowKnight.ItemChanger/blob/master/ItemChanger/Placements/CostChestPlacement.cs#L135-L178
    /// </summary>
    private string GetPreviewText()
    {
        StringBuilder stringBuilder = new("If you show me that you've got that much knowledge, I'll reward you with:");
        stringBuilder.Append("<br>");
        StringBuilder itemPreviewBuilder = new();
        MultiPreviewRecordTag recordTag = GetOrAddTag<MultiPreviewRecordTag>();
        recordTag.previewTexts = new string[Items.Count];
        for (int i = 0; i < Items.Count; i++)
        {
            AbstractItem item = Items[i];
            Cost cost = item.GetTag<CostTag>()?.Cost;

            itemPreviewBuilder.Append(item.GetPreviewName(this));
            itemPreviewBuilder.Append("  -  ");
            if (item.IsObtained())
                itemPreviewBuilder.Append(Language.Language.Get("OBTAINED", "IC"));
            else if (cost is null)
                itemPreviewBuilder.Append(Language.Language.Get("FREE", "IC"));
            else if (cost.Paid)
                itemPreviewBuilder.Append(Language.Language.Get("PURCHASED", "IC"));
            else if (HasTag<DisableCostPreviewTag>() || item.HasTag<DisableCostPreviewTag>())
                itemPreviewBuilder.Append(Language.Language.Get("???", "IC"));
            else
                itemPreviewBuilder.Append(cost.GetCostText());

            string text = itemPreviewBuilder.ToString();
            itemPreviewBuilder.Clear();

            recordTag.previewTexts[i] = text;
            stringBuilder.Append("<br>");
            stringBuilder.Append(text);
        }
        AddVisitFlag(VisitState.Previewed);
        return stringBuilder.ToString();
    }

    #endregion

    #region Methods

    protected override void OnLoad()
    {
        Events.AddFsmEdit("Town", new("Elderbug", "Conversation Control"), ModifyElderbug);
        Events.AddSceneChangeEdit("Town", ElderbugPreview);
        Events.AddLanguageEdit(new("Elderbug", "Elderbug_Task_Successful"), CanGiveItems);
        Events.AddLanguageEdit(new("Elderbug", "Elderbug_Task_Failed"), CannotGiveItems);
        Events.AddLanguageEdit(new("Elderbug", "Introduction"), Introduction);
        Events.AddLanguageEdit(new("Elderbug", "Introduction2"), IntroductionSecondPart);
        Events.AddLanguageEdit(new("Lore Tablets", "Elderbug_Preview"), Preview);
        AbstractItem.AfterGiveGlobal += AbstractItem_AfterGiveGlobal;
        ItemChangerMod.Modules.GetOrAdd<LoreProgressModule>();
        Instance = this;
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit("Town", new("Elderbug", "Conversation Control"), ModifyElderbug);
        Events.RemoveSceneChangeEdit("Town", ElderbugPreview);
        Events.RemoveLanguageEdit(new("Elderbug", "Elderbug_Task_Successful"), CanGiveItems);
        Events.RemoveLanguageEdit(new("Elderbug", "Elderbug_Task_Failed"), CannotGiveItems);
        Events.RemoveLanguageEdit(new("Elderbug", "Introduction"), Introduction);
        Events.RemoveLanguageEdit(new("Elderbug", "Introduction2"), IntroductionSecondPart);
        Events.RemoveLanguageEdit(new("Lore Tablets", "Elderbug_Preview"), Preview);
        AbstractItem.AfterGiveGlobal -= AbstractItem_AfterGiveGlobal;
        Instance = null;
    }

    private IEnumerator WaitThenMoveBench()
    {
        yield return null;
        GameObject bench = GameObject.Find("RestBench");
        if (bench != null)
            bench.transform.position += new Vector3(3f, 0f);
    }

    #endregion
}
