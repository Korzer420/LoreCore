using ItemChanger;
using ItemChanger.Locations;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Locations;
using LoreCore.Other;
using LoreCore.SaveManagment;
using Modding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using static LoreCore.Data.ItemList;
using static LoreCore.Data.LocationList;

namespace LoreCore;

public class LoreCore : Mod, ILocalSettings<LocalSettings>
{
    #region Constructors

    public LoreCore() => Instance = this;
    
    #endregion

    #region Properties

    public static LoreCore Instance { get; set; }

    /// <summary>
    /// Gets or sets the preloaded object, used by various different powers.
    /// </summary>
    public Dictionary<string, GameObject> PreloadedObjects { get; set; } = new Dictionary<string, GameObject>();

    #endregion

    #region Configuration

    /// <summary>
    /// Get the version of the mod.
    /// </summary>
    /// <returns></returns>
    public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

    /// <summary>
    /// Gets the names (objects) that need to be preloaded.
    /// </summary>
    /// <returns></returns>
    public override List<(string, string)> GetPreloadNames() => new()
    {
        ("Ruins1_23", "Glow Response Mage Computer"), // Soul sanctum lore tablet.
        ("Ruins1_23", "Inspect Region"), // Inspect region for soul sanctum tablet.
    };

    /// <summary>
    /// Does the initialization needed for the mod.
    /// </summary>
    /// <param name="preloadedObjects"></param>
    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        try
        {
            foreach (string key in preloadedObjects.Keys)
                foreach (string subKey in preloadedObjects[key].Keys)
                    if (!PreloadedObjects.ContainsKey(subKey))
                    {
                        GameObject toAdd = preloadedObjects[key][subKey];
                        PreloadedObjects.Add(subKey, toAdd);
                        GameObject.DontDestroyOnLoad(toAdd);
                    }
        }
        catch (Exception exception)
        {
            throw new Exception("Failed to preload objects: " + exception.ToString());
        }
        CreateICData();
    }

    #endregion

    #region Save Managment

    public void OnLoadLocal(LocalSettings saveSettings)
    {
        if (saveSettings?.Stages != null)
            TravellerLocation.Stages = saveSettings.Stages;
    }

    public LocalSettings OnSaveLocal() => new() { Stages = TravellerLocation.Stages };

    #endregion

    #region Methods

    /// <summary>
    /// Creates and adds all placement with their vanilla items to the save file.
    /// </summary>
    /// <param name="generateSettings">If <paramref name="generateSettings"/> the item changer settings will be generated. Only use this, if rando is not unused.</param>
    public void CreateVanillaPlacements(bool generateSettings = false)
    {
        if (generateSettings)
            ItemChangerMod.CreateSettingsProfile(false);
        List<AbstractPlacement> placements = new()
        {
            // Npc
            GeneratePlacement(Dialogue_Bardoon, Bardoon),
            GeneratePlacement(Dialogue_Bretta_Diary, Bretta_Diary),
            GeneratePlacement(Dialogue_Dung_Defender, Dung_Defender),
            GeneratePlacement(Dialogue_Emilitia, Emilitia),
            GeneratePlacement(Dialogue_Fluke_Hermit, Fluke_Hermit),
            GeneratePlacement(Dialogue_Grasshopper, Grasshopper),
            GeneratePlacement(Dialogue_Gravedigger, Gravedigger),
            GeneratePlacement(Dialogue_Joni, Joni),
            GeneratePlacement(Dialogue_Marissa, Marissa),
            GeneratePlacement(Dialogue_Mask_Maker, Mask_Maker),
            GeneratePlacement(Dialogue_Menderbug_Diary, Menderbug_Diary),
            GeneratePlacement(Dialogue_Midwife, Midwife),
            GeneratePlacement(Dialogue_Moss_Prophet, Moss_Prophet),
            GeneratePlacement(Dialogue_Myla, Myla),
            GeneratePlacement(Dialogue_Poggy, Poggy),
            GeneratePlacement(Dialogue_Queen, Queen),
            GeneratePlacement(Dialogue_Vespa, Vespa),
            GeneratePlacement(Dialogue_Willoh, Willoh),
            // Dream dialogue
            GeneratePlacement(Dream_Dialogue_Ancient_Nailsmith_Golem, Ancient_Nailsmith_Golem_Dream),
            GeneratePlacement(Dream_Dialogue_Aspid_Queen, Aspid_Queen_Dream),
            GeneratePlacement(Dream_Dialogue_Crystalized_Shaman, Crystalized_Shaman_Dream),
            GeneratePlacement(Dream_Dialogue_Dashmaster_Statue, Dashmaster_Statue_Dream),
            GeneratePlacement(Dream_Dialogue_Dream_Shield_Statue, Dream_Shield_Statue_Dream),
            GeneratePlacement(Dream_Dialogue_Dryya, Dryya_Dream),
            GeneratePlacement(Dream_Dialogue_Grimm_Summoner, Grimm_Summoner_Dream),
            GeneratePlacement(Dream_Dialogue_Hopper_Dummy, Hopper_Dummy_Dream),
            GeneratePlacement(Dream_Dialogue_Isma, Isma_Dream),
            GeneratePlacement(Dream_Dialogue_Kings_Mould_Machine, Kings_Mould_Machine_Dream),
            GeneratePlacement(Dream_Dialogue_Mine_Golem, Mine_Golem_Dream),
            GeneratePlacement(Dream_Dialogue_Overgrown_Shaman, Overgrown_Shaman_Dream),
            GeneratePlacement(Dream_Dialogue_Pale_King, Pale_King_Dream),
            GeneratePlacement(Dream_Dialogue_Radiance_Statue, Radiance_Statue_Dream),
            GeneratePlacement(Dream_Dialogue_Shade_Golem_Normal, Shade_Golem_Dream_Normal),
            GeneratePlacement(Dream_Dialogue_Shade_Golem_Void, Shade_Golem_Dream_Void),
            GeneratePlacement(Dream_Dialogue_Shriek_Statue, Shriek_Statue_Dream),
            GeneratePlacement(Dream_Dialogue_Shroom_King, Shroom_King_Dream),
            GeneratePlacement(Dream_Dialogue_Snail_Shaman_Tomb, Snail_Shaman_Tomb_Dream),
            GeneratePlacement(Dream_Dialogue_Tiso_Corpse, Tiso_Corpse),
            // Point of interest
            GeneratePlacement(Inscription_City_Fountain, City_Fountain),
            GeneratePlacement(Inscription_Dreamer_Tablet, Dreamer_Tablet),
            GeneratePlacement(Inspect_Stag_Egg, Stag_Nest),
            GeneratePlacement(ItemList.Lore_Tablet_Record_Bela, LocationList.Lore_Tablet_Record_Bela),
            GeneratePlacement(Inspect_Beast_Den_Altar, Beast_Den_Altar),
            GeneratePlacement(Inspect_Garden_Golem, Garden_Golem),
            GeneratePlacement(Inspect_Grimm_Machine, Grimm_Machine),
            GeneratePlacement(Inspect_Grimm_Summoner_Corpse, Grimm_Summoner_Corpse),
            GeneratePlacement(Inspect_Grub_Seal, Grub_Seal),
            GeneratePlacement(Inspect_White_Palace_Nursery, White_Palace_Nursery),
            GeneratePlacement(Inspect_Gorb, Gorb_Grave),
            GeneratePlacement(Inspect_Galien, Galien_Corpse),
            GeneratePlacement(Inspect_Elder_Hu, Elder_Hu_Grave),
            GeneratePlacement(Inspect_Markoth, Markoth_Corpse),
            GeneratePlacement(Inspect_Marmu, Marmu_Grave),
            GeneratePlacement(Inspect_No_Eyes, No_Eyes_Statue),
            GeneratePlacement(Inspect_Xero, Xero_Grave),
            // Traveller
            // Quirrel
            GeneratePlacement(Dialogue_Quirrel_Archive, Quirrel_After_Monomon),
            GeneratePlacement(Dialogue_Quirrel_Blue_Lake, Quirrel_Blue_Lake),
            GeneratePlacement(Dialogue_Quirrel_Peaks, Quirrel_Peaks),
            GeneratePlacement(Dialogue_Quirrel_City, Quirrel_City),
            GeneratePlacement(Dialogue_Quirrel_Deepnest, Quirrel_Deepnest),
            GeneratePlacement(Dialogue_Quirrel_Crossroads, Quirrel_Crossroads),
            GeneratePlacement(Dialogue_Quirrel_Greenpath, Quirrel_Greenpath),
            GeneratePlacement(Dialogue_Quirrel_Mantis_Village, Quirrel_Mantis_Village),
            GeneratePlacement(Dialogue_Quirrel_Queen_Station, Quirrel_Queen_Station),
            GeneratePlacement(Dialogue_Quirrel_Outside_Archive, Quirrel_Outside_Archive),
            // Tiso
            GeneratePlacement(Dialogue_Tiso_Blue_Lake, Tiso_Blue_Lake),
            GeneratePlacement(Dialogue_Tiso_Colosseum, Tiso_Colosseum),
            GeneratePlacement(Dialogue_Tiso_Crossroads, Tiso_Crossroads),
            GeneratePlacement(Dialogue_Tiso_Dirtmouth, Tiso_Dirtmouth),
            // Zote
            GeneratePlacement(Dialogue_Zote_Greenpath, Zote_Greenpath),
            GeneratePlacement(Dialogue_Zote_Dirtmouth_Intro, Zote_Dirtmouth_Intro),
            GeneratePlacement(Dialogue_Zote_City, Zote_City),
            GeneratePlacement(Dialogue_Zote_Deepnest, Zote_Deepnest),
            GeneratePlacement(Dialogue_Zote_Colosseum, Zote_Colosseum),
            GeneratePlacement(Dialogue_Zote_Dirtmouth_After_Colosseum, Zote_Dirtmouth_After_Colosseum),
            // Cloth
            GeneratePlacement(Dialogue_Cloth_Fungal_Wastes, Cloth_Fungal_Wastes),
            GeneratePlacement(Dialogue_Cloth_Basin, Cloth_Basin),
            GeneratePlacement(Dialogue_Cloth_Deepnest, Cloth_Deepnest),
            GeneratePlacement(Dialogue_Cloth_Garden, Cloth_Garden),
            new DualLocation()
            {
                name = Cloth_End,
                trueLocation = Finder.GetLocation(Cloth_Ghost),
                falseLocation = Finder.GetLocation(Cloth_Town),
                Test = new ClothTest()
            }.Wrap().Add(Finder.GetItem(Cloth_End))
        };

        string[] items = new string[]
        {
            ItemNames.Wanderers_Journal,
            ItemNames.Wanderers_Journal,
            ItemNames.Wanderers_Journal,
            ItemNames.Charm_Notch,
            ItemNames.Hallownest_Seal,
            ItemNames.Hallownest_Seal,
            ItemNames.Kings_Idol,
            ItemNames.Charm_Notch,
            ItemNames.Arcane_Egg
        };
        for (int i = 1; i < 10; i++)
            placements.Add(GeneratePlacement(items[i - 1], Elderbug_Reward_Prefix + i));
        ItemChangerMod.AddPlacements(placements);
    }

    private AbstractPlacement GeneratePlacement(string item, string location)
        => Finder.GetLocation(location).Wrap().Add(Finder.GetItem(item));

    private void CreateICData()
    {
        try
        {
            using Stream locationStream = ResourceHelper.LoadResource<LoreCore>("Data.Locations.json");
            using StreamReader reader = new(locationStream);
            JsonSerializer jsonSerializer = new()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            foreach (AbstractLocation location in jsonSerializer.Deserialize<List<AbstractLocation>>(new JsonTextReader(reader)))
                Finder.DefineCustomLocation(location);

            using Stream itemStream = ResourceHelper.LoadResource<LoreCore>("Data.Items.json");
            using StreamReader reader2 = new(itemStream);

            foreach (AbstractItem item in jsonSerializer.Deserialize<List<AbstractItem>>(new JsonTextReader(reader2)))
                Finder.DefineCustomItem(item);
        }
        catch (Exception exception)
        {
            throw new Exception("Failed to load items/locations: " + exception.ToString());
        }
    }

    #endregion
}