using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using KorzUtils.Helper;
using LoreCore.Locations;
using LoreCore.Other;
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

public class LoreCore : Mod
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
        ("Ruins_Bathhouse", "Ghost NPC/Idle Pt"),
        // Npc for container (ToDo)
        //("Room_temple", "Quirrel"),
        //("Town", "_NPCs/Zote Town"),
        //("Fungus2_23", "Bretta Dazed"),
        //("Hive_05", "Battle Scene/Vespa NPC"),
        //("Room_Mask_Maker", "Maskmaker NPC"),
        //("Town", "_NPCs/Gravedigger NPC"),
        //("Ruins_Elevator", "Ghost NPC"),
        //("Cliffs_05", "Ghost Activator/Ghost NPC Joni"),
        //("Crossroads_45", "Miner"),
        //("Ruins_House_03", "Emilitia NPC"),
        //("Fungus2_34", "Giraffe NPC"),
        //("Room_GG_Shortcut", "Fluke Hermit"),
        //("Ruins_Bathhouse", "Ghost NPC"),
        //("Fungus1_24", "Ghost NPC"),
        //("Room_ruinhouse", "Sly Dazed"),
        //("Room_mapper", "Iselda"),
        //("Room_Colosseum_01", "Little Fool NPC"),
        //("Room_Charm_Shop", "Charm Slug"),
        //("Grimm_Main_Tent", "Brum NPC"),
        //("Town", "_NPCs/Tiso Town NPC"),
        //("Deepnest_14", "Cloth NPC 2"),
        //("Abyss_06_Core", "Hornet Abyss NPC")
    };

    /// <summary>
    /// Does the initialization needed for the mod.
    /// </summary>
    /// <param name="preloadedObjects"></param>
    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        try
        {
            // Due to the npc we have to manually assign them.
            PreloadedObjects = new()
            {
                {"Glow Response Mage Computer", preloadedObjects["Ruins1_23"]["Glow Response Mage Computer"]},
                {"Inspect Region", preloadedObjects["Ruins1_23"]["Inspect Region"]},
                {"Ghost NPC/Idle Pt", preloadedObjects["Ruins_Bathhouse"]["Ghost NPC/Idle Pt"]}
                //{"Quirrel", preloadedObjects["Room_temple"]["Quirrel"]},
                //{"Zote", preloadedObjects["Town"]["_NPCs/Zote Town"]},
                //{"Bretta", preloadedObjects["Fungus2_23"]["Bretta Dazed"]},
                //{"Vespa", preloadedObjects["Hive_05"]["Battle Scene/Vespa NPC"]},
                //{"Mask_Maker", preloadedObjects["Room_Mask_Maker"]["Maskmaker NPC"]},
                //{"Gravedigger", preloadedObjects["Town"]["_NPCs/Gravedigger NPC"]},
                //{"Poggy", preloadedObjects["Ruins_Elevator"]["Ghost NPC"]},
                //{"Joni", preloadedObjects["Cliffs_05"]["Ghost Activator/Ghost NPC Joni"]},
                //{"Myla", preloadedObjects["Crossroads_45"]["Miner"]},
                //{"Emilitia", preloadedObjects["Ruins_House_03"]["Emilitia NPC"]},
                //{"Willoh", preloadedObjects["Fungus2_34"]["Giraffe NPC"]},
                //{"Fluke_Hermit", preloadedObjects["Room_GG_Shortcut"]["Fluke Hermit"]},
                //{"Marissa", preloadedObjects["Ruins_Bathhouse"]["Ghost NPC"]},
                //{"Grasshopper", preloadedObjects["Fungus1_24"]["Ghost NPC"]},
                //{"Sly", preloadedObjects["Room_ruinhouse"]["Sly Dazed"]},
                //{"Iselda", preloadedObjects["Room_mapper"]["Iselda"]},
                //{"Little_Fool", preloadedObjects["Room_Colosseum_01"]["Little Fool NPC"]},
                //{"Salubra", preloadedObjects["Room_Charm_Shop"]["Charm Slug"]},
                //{"Brumm", preloadedObjects["Grimm_Main_Tent"]["Brum NPC"]},
                //{"Tiso", preloadedObjects["Town"]["_NPCs/Tiso Town NPC"]},
                //{"Cloth", preloadedObjects["Deepnest_14"]["Cloth NPC 2"]},
                //{"Hornet", preloadedObjects["Abyss_06_Core"]["Hornet Abyss NPC"]}
            };
            foreach (string key in PreloadedObjects.Keys)
                GameObject.DontDestroyOnLoad(PreloadedObjects[key]);
        }
        catch (Exception exception)
        {
            throw new Exception("Failed to preload objects: " + exception.ToString());
        }
        CreateICData();
    }

    #endregion

    #region Methods

    public void CreateVanillaCustomLore(bool generateSettings = false)
    {
        if (generateSettings)
            ItemChangerMod.CreateSettingsProfile(false);
        List<AbstractPlacement> placements = new();
        foreach (string item in LoreTablets)
            placements.Add(GeneratePlacement(item + "_Empowered", item));
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
            Finder.DefineCustomLocation(new DualLocation()
            {
                name = Cloth_End,
                trueLocation = Finder.GetLocation(Cloth_Ghost),
                falseLocation = Finder.GetLocation(Cloth_Town),
                Test = new ClothTest()
            });

            using Stream itemStream = ResourceHelper.LoadResource<LoreCore>("Data.Items.json");
            using StreamReader reader2 = new(itemStream);

            foreach (AbstractItem item in jsonSerializer.Deserialize<List<AbstractItem>>(new JsonTextReader(reader2)))
                Finder.DefineCustomItem(item);

            Finder.GetLocationOverride += Finder_GetLocationOverride;
            //Container.DefineContainer(new NpcContainer());
        }
        catch (Exception exception)
        {
            throw new Exception("Failed to load items/locations: " + exception.ToString());
        }
    }

    private void Finder_GetLocationOverride(GetLocationEventArgs locationData)
    {
        if (ShrineLocation.ShrineLocations.Contains(locationData.LocationName))
        {
            string shrineName = locationData.LocationName.Replace("_", "").Replace("-", "") + "Location";
            ShrineLocation shrineLocation = Activator.CreateInstance(typeof(LoreCore).Assembly.FullName, "LoreCore.Locations.ShrineLocations." + shrineName).Unwrap() as ShrineLocation;
            shrineLocation.name = locationData.LocationName;
            shrineLocation.sceneName = "Dream_Room_Believer_Shrine";
            shrineLocation.tags = new()
            {
                new InteropTag()
                {
                    Message = "RandoSupplementalMetadata",
                    Properties = new()
                    {
                        {"ModSource", "LoreCore" },
                        {"DoNotMakePin", true }
                    }
                }
            };
            shrineLocation.TabletPosition = ShrineLocation.SelectedTablets.IndexOf(locationData.LocationName);
            locationData.Current = shrineLocation;
        }
    }

    #endregion
}