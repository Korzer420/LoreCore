using ItemChanger;
using ItemChanger.Items;
using KorzUtils.Helper;
using LoreCore.Locations;
using LoreCore.SaveManagment;
using Modding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

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

    #region Private Methods

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