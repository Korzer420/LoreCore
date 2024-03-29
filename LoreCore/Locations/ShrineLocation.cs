﻿using HutongGames.PlayMaker;

using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Items;
using Modding;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations;

// Dream Room Believer Shrine
public abstract class ShrineLocation : AutoLocation
{
    #region Members

    private static Vector3[] _tabletPositions = new Vector3[]
    {
        // Big platform bottom left.
        new(34.405f, 14.41f),
        new(40.7125f, 14.41f),
        new(47.02f, 14.41f),
        // Big platform bottom right
        new(63.18f, 14.41f),
        new(69,82f, 14.41f),
        new(76.46f, 14.41f),
        // Small right
        new(101.56f, 25.08f),
        // Second level left
        new(16.8f, 30.41f),
        new(24.435f, 30.41f),
        new(32.07f, 30.41f),
        // Second level right
        new(76.135f, 30.41f),
        new(82.135f, 30.41f),
        new(89.135f, 30.41f),
        // Small left
        new(26.13f, 47.41f),
        // Third level left
        new(34.8f, 45.08f),
        new(42.515f, 45.08f),
        new(50.23f, 45.08f),
        // Small middle
        new(57.34f, 40.34f),
        // Third level right
        new(65.72f, 45.08f),
        new(73.77f, 45.08f),
        new(81.82f, 45.08f),
        // Small top
        new(87.88f, 56.09f),
        // Fourth level left
        new(32.12f, 61.41f),
        new(39.86f, 61.41f),
        new(47.6f, 61.41f),
        // Fourth level right
        new(62.8f, 61.41f),
        new(70.52f, 61.41f),
        new(78.24f, 61.41f)
    };

    #endregion

    #region Properties

    public bool ConditionMet { get; set; }

    public int TabletPosition { get; set; }

    [JsonIgnore]
    public virtual string Text => "It dreams... nothing.";

    [JsonIgnore]
    public static List<string> SelectedTablets { get; set; } = new();

    [JsonIgnore]
    public static List<string> ShrineLocations = new()
    {
        LocationList.AllGrubsShrine,
        LocationList.DrownShrine,
        LocationList.FlowerShrine,
        LocationList.FullStagShrine,
        LocationList.GenocideShrine,
        LocationList.GeoShrine,
        LocationList.HalfGrubsShrine,
        LocationList.HerrahShrine,
        LocationList.LongestNailShrine,
        LocationList.LurienShrine,
        //LocationList.MenderbugShrine,
        LocationList.MonomonShrine,
        LocationList.MylaShrine,
        LocationList.NailsmithShrine,
        LocationList.RespectElderShrine,
        LocationList.ScammedShrine,
        LocationList.ShadeKillShrine,
        LocationList.ShamanShrine,
        LocationList.SlugInTubShrine,
        LocationList.ZoteShrine,
        LocationList.DeepDiveShrine
    };

    #endregion

    #region Event handler

    private void Breakable_Break(On.Breakable.orig_Break orig, Breakable self, float flingAngleMin, float flingAngleMax, float impactMultiplier)
    {
        if (self.name == "Explanation_Tablet" || (self.name == name && !ConditionMet))
            return;
        orig(self, flingAngleMin, flingAngleMax, impactMultiplier);
        if (self.name == name)
        { 
            ItemHelper.SpawnShiny(self.transform.position, Placement);
            self.gameObject.name = "Empty";
        }
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.name == "Dream_Room_Believer_Shrine")
            GameManager.instance.StartCoroutine(CreateTablet());
    }

    private string ModHooks_LanguageGetHook(string key, string sheetTitle, string orig)
    {
        if (key == name)
        { 
            orig = Text + "<br>Reward: " + Placement.Items[0].GetPreviewName(Placement);
            Placement.AddVisitFlag(ItemChanger.VisitState.Previewed);
        }
        else if (key == "Explanation_Shrine")
            orig = "Fulfill their dreams to obtain the memory that they left in this world.";
        return orig;
    }

    #endregion

    protected override void OnLoad()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        On.Breakable.Break += Breakable_Break;
        ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;
    }

    protected override void OnUnload()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
        ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;
        On.Breakable.Break -= Breakable_Break;
    }

    private IEnumerator CreateTablet()
    {
        yield return null;
        GameObject original = GameObject.Find("Plaque_statue_02 (3)");
        if (original == null)
            original = GameObject.Find("Explanation_Tablet");
        else
        {
            Object.Destroy(original.GetComponent<PersistentBoolItem>());
            original.GetComponent<BoxCollider2D>().enabled = true;
            original.GetComponent<Breakable>().enabled = true;
            original.name = "Explanation_Tablet";
            original.transform.Find("Active/Inspect Region").gameObject.LocateMyFSM("inspect_region").FsmVariables.FindFsmString("Game Text Convo").Value = "Explanation_Shrine";
            foreach (Breakable breakable in Object.FindObjectsOfType<Breakable>())
                if (breakable.gameObject.name != "Explanation_Tablet")
                    Object.Destroy(breakable.gameObject);
        }
        
        if (Placement.Items.All(x => x.IsObtained()))
            yield break;
        GameObject newTablet = Object.Instantiate(original);
        newTablet.name = name;
        // Set the position with a few adjustments.
        newTablet.transform.position = _tabletPositions[TabletPosition] + new Vector3(0f, 1.6f, 0.3f);
        newTablet.SetActive(true);
        newTablet.transform.Find("Active/Inspect Region").gameObject.LocateMyFSM("inspect_region").FsmVariables.FindFsmString("Game Text Convo").Value = name;
        if (!ReadItem.CanRead)
        {
            PlayMakerFSM fsm = newTablet.transform.Find("Active/Inspect Region").gameObject.LocateMyFSM("inspect_region");
            // Try to display that the tablet is unreadable.
            fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
            {
                Name = "Show unreadable prompt",
                Actions = new HutongGames.PlayMaker.FsmStateAction[]
                {
                        new Lambda(() => GameHelper.DisplayMessage("You can't read this."))
                }
            });

            // Best try to make the tablets unreadable
            if (fsm.GetState("In Range") is FsmState)
                fsm.GetState("In Range").AdjustTransition("UP PRESSED", "Show unreadable prompt");
            else
                fsm.GetState("In Range?").AdjustTransition("UP PRESSED", "Show unreadable prompt");

            if (fsm.GetState("Idle") == null)
                fsm.GetState("Show unreadable prompt").AddTransition("FINISHED", "Out Of Range");
            else
                fsm.GetState("Show unreadable prompt").AddTransition("FINISHED", "Idle");
        }
        yield return null;
        if (newTablet.transform.position.y > 61.41f)
            newTablet.transform.position = new(77.74f, 63.01f, 0.1f);
        // A fail save
        if (newTablet.GetComponent<Breakable>() is null)
        {
            Object.Destroy(newTablet);
            ItemHelper.SpawnShiny(new(77.74f, 63.01f, 0.1f), Placement);
        }

    }
}
