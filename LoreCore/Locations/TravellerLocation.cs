using ItemChanger;
using ItemChanger.Extensions;
using KorzUtils.Helper;
using LoreCore.Enums;
using LoreCore.Modules;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations;

public class TravellerLocation : DialogueLocation
{
    #region Properties

    public Traveller TravellerName { get; set; }

    public int RequiredLevel { get; set; }

    #endregion

    protected override void OnLoad()
    {
        Events.AddSceneChangeEdit(sceneName, ControlSpawn);
        base.OnLoad();
        ItemChangerMod.Modules.GetOrAdd<TravellerControlModule>();
    }

    protected override void OnUnload()
    {
        Events.RemoveSceneChangeEdit(sceneName, ControlSpawn);
        base.OnUnload();
    }

    /// <summary>
    /// Controls if the npc should spawn at all.
    /// </summary>
    protected virtual void ControlSpawn(Scene scene)
    {
        string objectName = ObjectName.Contains("Final Scene") ? "_NPCs/Zote Final Scene" : ObjectName;
        GameObject npc = GameObject.Find(objectName);
        if (npc == null)
        {
            npc = CheckComponent<DeactivateIfPlayerdataTrue>(objectName);
            if (npc == null)
                npc = CheckComponent<DeactivateIfPlayerdataFalse>(objectName);
            if (npc == null)
            {
                LogHelper.Write<LoreCore>("Couldn't find " + objectName + ".", KorzUtils.Enums.LogType.Error);
                return;
            }
        }
        if (ObjectName == "/RestBench/Quirrel Bench")
        {
            npc.transform.SetParent(null);
            GameManager.instance.StartCoroutine(ActivateQuirrel(npc));
        }

        RemoveSpawnConditions(npc);
        if (Placement.Items.All(x => x.IsObtained()))
        {
            npc.SetActive(false);
            return;
        }
        else
        {
            if (TravellerName == Traveller.Quirrel && ObjectName == "Quirrel Wounded" && !PlayerData.instance.GetBool(nameof(PlayerData.instance.summonedMonomon)))
                npc.SetActive(false);
            else
                npc.SetActive(RequiredLevel <= ItemChangerMod.Modules.GetOrAdd<TravellerControlModule>().Stages[TravellerName]);
        }
    }

    internal static void RemoveSpawnConditions(GameObject traveller)
    {
        // Remove all components which affect the spawn.
        foreach (DeactivateIfPlayerdataTrue item in traveller.GetComponents<DeactivateIfPlayerdataTrue>())
            Object.Destroy(item);
        foreach (DeactivateIfPlayerdataFalse item in traveller.GetComponents<DeactivateIfPlayerdataFalse>())
            Object.Destroy(item);
        foreach (PlayMakerFSM item in traveller.GetComponents<PlayMakerFSM>()?.Where(x => x.FsmName.StartsWith("Destroy") || x.FsmName == "FSM"
        || x.FsmName == "Death" || x.FsmName.StartsWith("Leave")
        || x.FsmName.StartsWith("deactivate") || x.FsmName == "Appear"))
            Object.Destroy(item);
    }

    /// <summary>
    /// Tries to find the corresponding game object.
    /// </summary>
    private GameObject CheckComponent<T>(string name) where T : Component
    {
        // Since some traveller objects are child objects, we extract their simple name.
        string normalName = name.Split('/').Last();
        return Object.FindObjectsOfType<T>(true)
            .FirstOrDefault(x => x.gameObject.name == normalName)?.gameObject;
    }

    /// <summary>
    /// Activates quirrel, if he got deactivated via bench rando (if the bench got destroyed)
    /// </summary>
    private IEnumerator ActivateQuirrel(GameObject quirrel)
    {
        yield return new WaitForSeconds(.5f);
        if (quirrel != null)
            foreach (Component component in quirrel.GetComponents<Component>())
            {
                if (component is tk2dSprite sprite)
                    sprite.enabled = true;
                else if (component is tk2dSpriteAnimator animator)
                    animator.enabled = true;
                else if (component is BoxCollider2D collider)
                    collider.enabled = true;
                else if (component is PlayMakerFSM fsm)
                    fsm.enabled = true;
                else if (component is PlayMakerFixedUpdate update)
                    update.enabled = true;
            }
    }
}