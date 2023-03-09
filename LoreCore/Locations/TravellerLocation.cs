using ItemChanger;
using ItemChanger.Extensions;
using KorzUtils.Helper;
using LoreCore.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations;

public class TravellerLocation : DialogueLocation
{
    #region Members

    private static readonly (string, string)[] _quirrelNames = new (string, string)[]
    {
        ("Room_temple", "Quirrel"),
        ("Room_Slug_Shrine", "Quirrel Slug Shrine"),
        ("Fungus2_01","Quirrel Station NPC"),
        ("Fungus2_14","Quirrel Mantis NPC"),
        ("Ruins1_02","/RestBench/Quirrel Bench"),
        ("Mines_13","Quirrel Mines"),
        ("Deepnest_30" ,"Quirrel Spa"),
        ("Fungus3_47" ,"Quirrel Archive Ext"),
        ("Fungus3_archive_02","/Dreamer Monomon/Quirrel Wounded"),
        ("Crossroads_50","Quirrel Lakeside")
    };

    private static readonly (string, string)[] _clothNames = new (string, string)[]
    {
        ("Fungus2_09","Cloth NPC 1"),
        ("Abyss_17","Cloth NPC Tramway"),
        ("Deepnest_14","Cloth NPC 2"),
        ("Fungus3_34","Cloth NPC QG Entrance"),
        ("Fungus3_23", "Cloth Ghost NPC"),
        ("Town","Cloth NPC Town")
    };

    private static readonly (string, string)[] _zoteNames = new (string, string)[]
    {
        ("Fungus1_20_v02", "Zote Buzzer Convo(Clone)"),
        ("Town","/_NPCs/Zote Town"),
        ("Ruins1_06","Zote Ruins"),
        ("Deepnest_33","/Zote Deepnest/Faller/NPC"),
        ("Room_Colosseum_02","Zote Colosseum"),
        ("Town","/_NPCs/Zote Final Scene/Zote Final")
    };

    private static readonly (string, string)[] _tisoNames = new (string, string)[]
    {
        ("Town","/_NPCs/Tiso Town NPC"),
        ("Crossroads_47","/_NPCs/Tiso Bench NPC"),
        ("Crossroads_50","Tiso Lake NPC"),
        ("Room_Colosseum_02","Tiso Col NPC"),
        ("Deepnest_East_07","tiso_corpse")
    };

    private static readonly (string, string)[] _hornetNames = new (string, string)[] 
    {
        ("Fungus1_04","Hornet Infected Knight Encounter"),
        ("Ruins1_27","Hornet Fountain Encounter"),
        ("Deepnest_East_Hornet", "Hornet Outskirts Battle Encounter"),
        ("Abyss_06_Core","Hornet Abyss NPC"),
        ("Deepnest_Spider_Town","Hornet Beast Den NPC"),
        ("Room_temple","Hornet Black Egg NPC")
    };

    #endregion

    #region Properties

    public Traveller TravellerName { get; set; }

    public static Dictionary<Traveller, int> Stages { get; set; } = new()
    {
        {Traveller.Quirrel , 0 },
        {Traveller.Cloth, 0 },
        {Traveller.Tiso, 0 },
        {Traveller.Zote, 0 },
        {Traveller.Hornet, 0 }
    };

    #endregion

    protected override void OnLoad()
    {
        Events.AddSceneChangeEdit(sceneName, ControlSpawn);
        base.OnLoad();
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

        // Remove all components which affect the spawn.
        foreach (DeactivateIfPlayerdataTrue item in npc.GetComponents<DeactivateIfPlayerdataTrue>())
            Component.Destroy(item);
        foreach (DeactivateIfPlayerdataFalse item in npc.GetComponents<DeactivateIfPlayerdataFalse>())
            Component.Destroy(item);
        foreach (PlayMakerFSM item in npc.GetComponents<PlayMakerFSM>()?.Where(x => x.FsmName.StartsWith("Destroy") || x.FsmName == "FSM"
        || x.FsmName == "Death" || x.FsmName.StartsWith("Leave")
        || x.FsmName.StartsWith("deactivate") || x.FsmName == "Appear"))
            Component.Destroy(item);
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
            {
                int npcIndex;
                // Cloth town and her ghost share a placement, which is why we take the ghost for town Cloth.
                if (TravellerName == Traveller.Cloth && scene.name == "Town")
                    npcIndex = _clothNames.Select(x => x.Item2).IndexOf("Cloth Ghost NPC");
                else
                    npcIndex = TravellerName switch
                    {
                        Traveller.Quirrel => _quirrelNames.Select(x => x.Item2).IndexOf(ObjectName),
                        Traveller.Cloth => _clothNames.Select(x => x.Item2).IndexOf(ObjectName),
                        Traveller.Tiso => _tisoNames.Select(x => x.Item2).IndexOf(ObjectName),
                        Traveller.Zote => _zoteNames.Select(x => x.Item2).IndexOf(ObjectName),
                        _ => _hornetNames.Select(x => x.Item2).IndexOf(ObjectName),
                    };
                npc.SetActive(npcIndex <= Stages[TravellerName]);
            }
        }
    }

    /// <summary>
    /// Tries to find the corresponding game object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private GameObject CheckComponent<T>(string name) where T : Component
    {
        // Since some traveller objects are child objects, we extract their simple name.
        string normalName = name.Split('/').Last();
        return Object.FindObjectsOfType<T>(true).FirstOrDefault(x => x.gameObject.name == normalName)?.gameObject;
    }

    /// <summary>
    /// Activates quirrel, if he got deactivated via bench rando (if the bench got destroyed)
    /// </summary>
    /// <param name="quirrel"></param>
    /// <returns></returns>
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