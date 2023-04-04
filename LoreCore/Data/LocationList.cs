namespace LoreCore.Data;

public static class LocationList
{
    public const string Elderbug_Shop = "Elderbug_Shop";

    #region Lists

    public static string[] NpcLocations => new string[]
    {
        Bretta,
        Bardoon,
        Vespa,
        Midwife,
        Myla,
        Willoh,
        Marissa,
        Joni,
        Grasshopper,
        Mask_Maker,
        Emilitia,
        Fluke_Hermit,
        Moss_Prophet,
        Queen,
        Dung_Defender,
        Menderbug_Diary,
        Gravedigger,
        Poggy,
        Godseeker,
        Millibelle,
        Hidden_Moth
    };

    public static string[] DreamLocations => new string[]
    {
        Ancient_Nailsmith_Golem_Dream,
        Aspid_Queen_Dream,
        Crystalized_Shaman_Dream,
        Dashmaster_Statue_Dream,
        Dream_Shield_Statue_Dream,
        Dryya_Dream,
        Grimm_Summoner_Dream,
        Hopper_Dummy_Dream,
        Isma_Dream,
        Kings_Mould_Machine_Dream,
        Mine_Golem_Dream,
        Overgrown_Shaman_Dream,
        Pale_King_Dream,
        Radiance_Statue_Dream,
        Shade_Golem_Dream_Normal,
        Shade_Golem_Dream_Void,
        Shriek_Statue_Dream,
        Shroom_King_Dream,
        Snail_Shaman_Tomb_Dream
    };

    public static string[] PointOfInterestLocations => new string[]
    {
        City_Fountain,
        Dreamer_Tablet,
        Weaver_Seal,
        Grimm_Machine,
        Beast_Den_Altar,
        Garden_Golem,
        Grub_Seal,
        White_Palace_Nursery,
        Grimm_Summoner_Corpse,
        Stag_Nest,
        Lore_Tablet_Record_Bela,
        Traitor_Grave,
        Elder_Hu_Grave,
        Gorb_Grave,
        Marmu_Grave,
        Xero_Grave,
        No_Eyes_Statue,
        Markoth_Corpse,
        Galien_Corpse
    };

    public static string[] TravellerLocations => new string[]
    {
        Quirrel_Crossroads,
        Quirrel_Greenpath,
        Quirrel_Queen_Station,
        Quirrel_Mantis_Village,
        Quirrel_City,
        Quirrel_Deepnest,
        Quirrel_Peaks,
        Quirrel_Outside_Archive,
        Quirrel_After_Monomon,
        Quirrel_Blue_Lake,
        Cloth_Fungal_Wastes,
        Cloth_Basin,
        Cloth_Deepnest,
        Cloth_Garden,
        Cloth_End,
        Tiso_Dirtmouth,
        Tiso_Crossroads,
        Tiso_Blue_Lake,
        Tiso_Colosseum,
        Tiso_Corpse,
        Zote_Greenpath,
        Zote_Dirtmouth_Intro,
        Zote_City,
        Zote_Deepnest,
        Zote_Colosseum,
        Zote_Dirtmouth_After_Colosseum,
        Hornet_Greenpath,
        Hornet_Fountain,
        Hornet_Edge,
        Hornet_Abyss,
        Hornet_Deepnest,
        Hornet_Temple
    };

    #endregion

    #region NPC

    public const string Bretta = "Bretta";

    public const string Bardoon = "Bardoon";

    public const string Vespa = "Vespa";

    public const string Mask_Maker = "Mask_Maker";

    public const string Midwife = "Midwife";

    public const string Gravedigger = "Gravedigger";

    public const string Poggy = "Poggy";

    public const string Joni = "Joni";

    public const string Myla = "Myla";

    public const string Emilitia = "Emilitia";

    public const string Willoh = "Willoh";

    public const string Moss_Prophet = "Moss_Prophet";

    public const string Fluke_Hermit = "Fluke_Hermit";

    public const string Queen = "Queen";

    public const string Marissa = "Marissa";

    public const string Grasshopper = "Grasshopper";

    public const string Dung_Defender = "Dung_Defender";

    public const string Menderbug_Diary = "Menderbug_Diary";

    public const string Godseeker = "Godseeker";

    public const string Millibelle = "Millibelle";

    public const string Hidden_Moth = "Hidden_Moth";

    #endregion

    #region Dream Nail Locations
    // 18
    public const string Mine_Golem_Dream = "Mine_Golem_Dream";

    public const string Aspid_Queen_Dream = "Aspid_Queen_Dream";

    public const string Overgrown_Shaman_Dream = "Overgrown_Shaman_Dream";

    public const string Crystalized_Shaman_Dream = "Crystalized_Shaman_Dream";

    public const string Ancient_Nailsmith_Golem_Dream = "Ancient_Nailsmith_Golem_Dream";

    public const string Shade_Golem_Dream_Normal = "Shade_Golem_Dream_Normal";

    public const string Shade_Golem_Dream_Void = "Shade_Golem_Dream_Void";

    public const string Shroom_King_Dream = "Shroom_King_Dream";

    public const string Hopper_Dummy_Dream = "Hopper_Dummy_Dream";

    public const string Dryya_Dream = "Dryya_Dream";

    public const string Isma_Dream = "Isma_Dream";

    public const string Shriek_Statue_Dream = "Shriek_Statue_Dream";

    public const string Radiance_Statue_Dream = "Radiance_Statue_Dream";

    public const string Dashmaster_Statue_Dream = "Dashmaster_Statue_Dream";

    public const string Snail_Shaman_Tomb_Dream = "Snail_Shaman_Tomb_Dream";

    public const string Pale_King_Dream = "Pale_King_Dream";

    public const string Grimm_Summoner_Dream = "Grimm_Summoner_Dream";

    public const string Kings_Mould_Machine_Dream = "Kings_Mould_Machine_Dream";

    public const string Dream_Shield_Statue_Dream = "Dream_Shield_Statue_Dream";

    #endregion

    #region Point of Interest Locations

    public const string City_Fountain = "City_Fountain";

    public const string Dreamer_Tablet = "Dreamer_Tablet";

    public const string Weaver_Seal = "Weaver_Seal";

    public const string Grimm_Machine = "Grimm_Machine";

    public const string Beast_Den_Altar = "Beast_Den_Altar";

    public const string Garden_Golem = "Garden_Golem";

    public const string Grub_Seal = "Grub_Seal";

    public const string White_Palace_Nursery = "White_Palace_Nursery";

    public const string Grimm_Summoner_Corpse = "Grimm_Summoner_Corpse";

    public const string Stag_Nest = "Stag_Nest";

    public const string Path_of_Pain_End_Scene = "Path_of_Pain-End_Scene";

    public const string Lore_Tablet_Record_Bela = "Lore_Tablet-Record_Bela";

    public const string Traitor_Grave = "Traitor_Grave";

    public const string Elder_Hu_Grave = "Elder_Hu_Grave";

    public const string Xero_Grave = "Xero_Grave";

    public const string Gorb_Grave = "Gorb_Grave";

    public const string No_Eyes_Statue = "No_Eyes_Statue";

    public const string Marmu_Grave = "Marmu_Grave";

    public const string Markoth_Corpse = "Markoth_Corpse";

    public const string Galien_Corpse = "Galien_Corpse";

    #endregion

    #region Traveller Locations
    // 28
    public const string Zote_Greenpath = "Zote_Greenpath";

    public const string Zote_Dirtmouth_Intro = "Zote_Dirtmouth_Intro";

    public const string Zote_City = "Zote_City";

    public const string Zote_Deepnest = "Zote_Deepnest";

    public const string Zote_Colosseum = "Zote_Colosseum";

    public const string Zote_Dirtmouth_After_Colosseum = "Zote_Dirtmouth_After_Colosseum";

    public const string Cloth_Fungal_Wastes = "Cloth_Fungal_Wastes";

    public const string Cloth_Deepnest = "Cloth_Deepnest";

    public const string Cloth_Basin = "Cloth_Basin";

    public const string Cloth_Garden = "Cloth_Garden";

    public const string Cloth_End = "Cloth_End";

    public const string Cloth_Ghost = "Cloth_Ghost";

    public const string Cloth_Town = "Cloth_Town";

    public const string Tiso_Dirtmouth = "Tiso_Dirtmouth";

    public const string Tiso_Crossroads = "Tiso_Crossroads";

    public const string Tiso_Blue_Lake = "Tiso_Blue_Lake";

    public const string Tiso_Colosseum = "Tiso_Colosseum";

    public const string Tiso_Corpse = "Tiso_Corpse";

    public const string Quirrel_Crossroads = "Quirrel_Crossroads";

    public const string Quirrel_Greenpath = "Quirrel_Greenpath";

    public const string Quirrel_Queen_Station = "Quirrel_Queen_Station";

    public const string Quirrel_Mantis_Village = "Quirrel_Mantis_Village";

    public const string Quirrel_City = "Quirrel_City";

    public const string Quirrel_Deepnest = "Quirrel_Deepnest";

    public const string Quirrel_Peaks = "Quirrel_Peaks";

    public const string Quirrel_Outside_Archive = "Quirrel_Outside_Archive";

    public const string Quirrel_After_Monomon = "Quirrel_After_Monomon";

    public const string Quirrel_Blue_Lake = "Quirrel_Blue_Lake";

    public const string Hornet_Greenpath = "Hornet_Greenpath";

    public const string Hornet_Fountain = "Hornet_Fountain";

    public const string Hornet_Edge = "Hornet_Edge";

    public const string Hornet_Abyss = "Hornet_Abyss";

    public const string Hornet_Deepnest = "Hornet_Deepnest";

    public const string Hornet_Temple = "Hornet_Temple";

    #endregion

    #region Shrine of Believers Locations

    public const string AllGrubsShrine = "All_Grubs-Shrine";

    public const string DrownShrine = "Drown-Shrine";

    public const string FlowerShrine = "Flower-Shrine";

    public const string FullStagShrine = "Full_Stag-Shrine";

    public const string GenocideShrine = "Genocide-Shrine";

    public const string GeoShrine = "Geo-Shrine";

    public const string HalfGrubsShrine = "Half_Grubs-Shrine";

    public const string HerrahShrine = "Herrah-Shrine";

    public const string LongestNailShrine = "Longest_Nail-Shrine";

    public const string LurienShrine = "Lurien-Shrine";

    public const string MenderbugShrine = "Menderbug-Shrine";

    public const string MonomonShrine = "Monomon-Shrine";

    public const string MylaShrine = "Myla-Shrine";

    public const string NailsmithShrine = "Nailsmith-Shrine";

    public const string RespectElderShrine = "Respect_Elder-Shrine";

    public const string ScammedShrine = "Scammed-Shrine";

    public const string ShadeKillShrine = "Shade_Kill-Shrine";

    public const string ShamanShrine = "Shaman-Shrine";

    public const string SlugInTubShrine = "Slug_In_Tub-Shrine";

    public const string ZoteShrine = "Zote-Shrine";

    public const string DeepDiveShrine = "Deep_Dive-Shrine";

    #endregion
}

