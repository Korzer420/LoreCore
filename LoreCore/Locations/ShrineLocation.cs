using ItemChanger.Locations;
using KorzUtils.Helper;
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
    // Bottom left: 34.405 to 47.02 (y = 14.41)
    // Bottom right: 63.18 to 76.46
    // Small platform (right): 101.56, 25.08
    // Middle right: 72.67 to 91.6  (y = 30.41)
    // Middle left: 16.8 to 32.07
    // Middle left small platform: 26.13, 47.41
    // Second from top (left): 34,8 (y = 45.08) to 50.23
    // Small platform (middle): 57.34, 40.34
    // Second from top (right): 65.72 to 81.82 (y =45.08)
    // Small  top right: 87.88 56.09
    // Top right: 62.8 to 78.24 (y = 61.41)
    // Top left: 32.12 to 47.6 (y = 61.41)

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
        new(72.67f, 30.41f),
        new(82.135f, 30.41f),
        new(91.6f, 30.41f),
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

    [JsonIgnore]
    public virtual string Text => "It dreams... nothing";

    [JsonIgnore]
    public static List<GameObject> Tablets { get; set; } = new();

    #endregion

    #region Event handler

    private void Breakable_Break(On.Breakable.orig_Break orig, Breakable self, float flingAngleMin, float flingAngleMax, float impactMultiplier)
    {
        if (self.name == "Explanation Tablet" || (self.name == name && ConditionMet))
            return;
        orig(self, flingAngleMin, flingAngleMax, impactMultiplier);
        if (self.name == name)
        { 
            ItemHelper.FlingShiny(self.gameObject, Placement);
            self.gameObject.name = "Empty";
        }
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.name == "Dream_Room_Believer_Shrine")
            GameManager.instance.StartCoroutine(CreateTablet());
        else
            Tablets.Clear();
    }

    private string ModHooks_LanguageGetHook(string key, string sheetTitle, string orig)
    {
        if (key == name)
            orig = Text;
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
        original.name = "Explanation_Tablet";
        if (Placement.Items.All(x => x.IsObtained()))
            yield break;
        GameObject newTablet = GameObject.Instantiate(original);
        newTablet.name = name;
        newTablet.transform.position = _tabletPositions[Tablets.Count];
        Tablets.Add(newTablet);
        newTablet.SetActive(true);
        newTablet.transform.Find("Active/Inspect Region (1)").gameObject.LocateMyFSM("inspect_region").FsmVariables.FindFsmString("Game Text Convo").Value = name;
    }
}
