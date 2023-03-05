using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using LoreCore.Data;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations;

internal class ElderbugLocation : AutoLocation
{
    #region Properties

    /// <summary>
    /// Gets or sets the flag that indicates if Elderbug has already thrown an item.
    /// </summary>
    public static bool ItemThrown { get; set; }

    #endregion

    #region Methods

    protected override void OnLoad()
    {
        Events.AddFsmEdit("Town", new("Elderbug", "Conversation Control"), ModifyElderbug);
        if (name == LocationList.Elderbug_Reward_Prefix + "1")
            Events.AddSceneChangeEdit("Town", ElderbugPreview);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit("Town", new("Elderbug", "Conversation Control"), ModifyElderbug);
        if (name == LocationList.Elderbug_Reward_Prefix + "1")
            Events.RemoveSceneChangeEdit("Town", a =>
            {
                if (name == LocationList.Lore_Tablet_Record_Bela)
                {
                    GameObject tablet = GameObject.Instantiate(LoreCore.Instance.PreloadedObjects["Glow Response Mage Computer"]);
                    tablet.name = "Mage_Computer_2";
                    tablet.transform.localPosition = new(105.74f, 11.41f);
                    tablet.SetActive(true);
                }

                GameObject inspectRegion = GameObject.Instantiate(LoreCore.Instance.PreloadedObjects["Inspect Region"]);
                inspectRegion.name = name;
                inspectRegion.transform.localPosition = new(105.74f, 11.41f);
                inspectRegion.SetActive(true);
                inspectRegion.LocateMyFSM("inspect_region").FsmVariables.FindFsmString("Elderbug_Preview");
            });
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
                    string conversationName = Placement.Name;
                    conversationName = $"Elderbug_Task_{Convert.ToInt32(conversationName.Substring(16)) + 1}";
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
                    Container container = Container.GetContainer(Container.Shiny);
                    GameObject treasure = container.GetNewContainer(new ContainerInfo(container.Name, Placement, FlingType.StraightUp));
                    ShinyUtility.FlingShinyRight(treasure.LocateMyFSM("Shiny Control"));
                    container.ApplyTargetContext(treasure, self.gameObject, 0f);
                    ItemThrown = true;
                    self.gameObject.GetComponent<BoxCollider2D>().size = new(1.8361f, 0.2408f);
                })
            }
        });

        self.GetState("Convo Choice").AddTransition(Placement.Name, Placement.Name);
        self.GetState(Placement.Name).AddTransition("CONVO_FINISH", $"{Placement.Name} Throw");
        self.GetState($"{Placement.Name} Throw").AddTransition("FINISHED", "Talk Finish");
    }

    private static void ElderbugPreview(Scene scene)
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
    }

    #endregion
}
