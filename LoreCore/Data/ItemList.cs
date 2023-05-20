using static ItemChanger.ItemNames;

namespace LoreCore.Data;

public static class ItemList
{
    public const string Read_Ability = "Read_Ability";

    public const string Listen_Ability = "Listen_Ability";

    #region Lists

    public static string[] NpcItems => new string[]
    {
        Dialogue_Bretta,
        Dialogue_Bardoon,
        Dialogue_Vespa,
        Dialogue_Midwife,
        Dialogue_Myla,
        Dialogue_Willoh,
        Dialogue_Marissa,
        Dialogue_Joni,
        Dialogue_Grasshopper,
        Dialogue_Mask_Maker,
        Dialogue_Emilitia,
        Dialogue_Fluke_Hermit,
        Dialogue_Moss_Prophet,
        Dialogue_Queen,
        Dialogue_Dung_Defender,
        Dialogue_Menderbug_Diary,
        Dialogue_Gravedigger,
        Dialogue_Poggy,
        Dialogue_Godseeker,
        Dialogue_Millibelle,
        Dialogue_Hidden_Moth,
        Dialogue_Sly,
        Dialogue_Iselda,
        Dialogue_Brumm,
        Dialogue_Little_Fool,
        Dialogue_Salubra
    };

    public static string[] DreamItems => new string[]
    {
        Dream_Dialogue_Ancient_Nailsmith_Golem,
        Dream_Dialogue_Aspid_Queen,
        Dream_Dialogue_Crystalized_Shaman,
        Dream_Dialogue_Dashmaster_Statue,
        Dream_Dialogue_Dream_Shield_Statue,
        Dream_Dialogue_Dryya,
        Dream_Dialogue_Grimm_Summoner,
        Dream_Dialogue_Hopper_Dummy,
        Dream_Dialogue_Isma,
        Dream_Dialogue_Kings_Mould_Machine,
        Dream_Dialogue_Mine_Golem,
        Dream_Dialogue_Overgrown_Shaman,
        Dream_Dialogue_Pale_King,
        Dream_Dialogue_Radiance_Statue,
        Dream_Dialogue_Shade_Golem_Normal,
        Dream_Dialogue_Shade_Golem_Void,
        Dream_Dialogue_Shriek_Statue,
        Dream_Dialogue_Shroom_King,
        Dream_Dialogue_Snail_Shaman_Tomb,
        Dream_Dialogue_Lighthouse_Keeper,
        Dream_Dialogue_Key_Thief
    };

    public static string[] PointOfInterestItems => new string[]
    {
        Inscription_City_Fountain,
        Inscription_Dreamer_Tablet,
        Inspect_Weaver_Seal,
        Inspect_Grimm_Machine,
        Inspect_Beast_Den_Altar,
        Inspect_Garden_Golem,
        Inspect_Grub_Seal,
        Inspect_White_Palace_Nursery,
        Inspect_Grimm_Summoner_Corpse,
        ItemList.Lore_Tablet_Record_Bela,
        ItemList.Traitor_Grave,
        Inspect_Stag_Egg,
        Inspect_Elder_Hu,
        Inspect_Gorb,
        Inspect_Marmu,
        Inspect_Xero,
        Inspect_No_Eyes,
        Inspect_Markoth,
        Inspect_Galien
    };

    public static string[] TravellerItems => new string[]
    {
        Dialogue_Quirrel_Crossroads,
        Dialogue_Quirrel_Greenpath,
        Dialogue_Quirrel_Queen_Station,
        Dialogue_Quirrel_Mantis_Village,
        Dialogue_Quirrel_City,
        Dialogue_Quirrel_Deepnest,
        Dialogue_Quirrel_Peaks,
        Dialogue_Quirrel_Outside_Archive,
        Dialogue_Quirrel_Archive,
        Dialogue_Quirrel_Blue_Lake,
        Dialogue_Cloth_Fungal_Wastes,
        Dialogue_Cloth_Basin,
        Dialogue_Cloth_Deepnest,
        Dialogue_Cloth_Garden,
        Dialogue_Cloth_Ghost,
        Dialogue_Tiso_Dirtmouth,
        Dialogue_Tiso_Crossroads,
        Dialogue_Tiso_Blue_Lake,
        Dialogue_Tiso_Colosseum,
        Dream_Dialogue_Tiso_Corpse,
        Dialogue_Zote_Greenpath,
        Dialogue_Zote_Dirtmouth_Intro,
        Dialogue_Zote_City,
        Dialogue_Zote_Deepnest,
        Dialogue_Zote_Colosseum,
        Dialogue_Zote_Dirtmouth_After_Colosseum,
        Dialogue_Hornet_Greenpath,
        Dialogue_Hornet_Fountain,
        Dialogue_Hornet_Edge,
        Dialogue_Hornet_Abyss,
        Dialogue_Hornet_Deepnest,
        Dialogue_Hornet_Temple
    };

    /// <summary>
    /// Gets the name of each lore tablet.
    /// <para>The modified names just have "_Empowered" on the end.</para>
    /// </summary>
    public static string[] LoreTablets => new string[]
    {
        Lore_Tablet_Ancient_Basin,
        Lore_Tablet_Archives_Left,
        Lore_Tablet_Archives_Right,
        Lore_Tablet_Archives_Upper,
        Lore_Tablet_City_Entrance,
        Lore_Tablet_Dung_Defender,
        Lore_Tablet_Fungal_Core,
        Lore_Tablet_Fungal_Wastes_Below_Shrumal_Ogres,
        Lore_Tablet_Fungal_Wastes_Hidden,
        Lore_Tablet_Greenpath_Below_Toll,
        Lore_Tablet_Greenpath_Lifeblood,
        Lore_Tablet_Greenpath_Lower_Hidden,
        Lore_Tablet_Greenpath_QG,
        Lore_Tablet_Greenpath_Stag,
        Lore_Tablet_Greenpath_Upper_Hidden,
        Lore_Tablet_Howling_Cliffs,
        Lore_Tablet_Kingdoms_Edge,
        Lore_Tablet_Kings_Pass_Exit,
        Lore_Tablet_Kings_Pass_Focus,
        Lore_Tablet_Kings_Pass_Fury,
        Lore_Tablet_Mantis_Outskirts,
        Lore_Tablet_Mantis_Village,
        Lore_Tablet_Palace_Throne,
        Lore_Tablet_Palace_Workshop,
        Lore_Tablet_Path_of_Pain_Entrance,
        Lore_Tablet_Pilgrims_Way_1,
        Lore_Tablet_Pilgrims_Way_2,
        Lore_Tablet_Pleasure_House,
        Lore_Tablet_Sanctum_Entrance,
        Lore_Tablet_Sanctum_Past_Soul_Master,
        Lore_Tablet_Spore_Shroom,
        Lore_Tablet_Watchers_Spire,
        Lore_Tablet_World_Sense
    };

    #endregion

    #region NPC

    public const string Dialogue_Bretta = "Dialogue-Bretta";

    public const string Dialogue_Bardoon = "Dialogue-Bardoon";

    public const string Dialogue_Vespa = "Dialogue-Vespa";

    public const string Dialogue_Mask_Maker = "Dialogue-Mask_Maker";

    public const string Dialogue_Midwife = "Dialogue-Midwife";

    public const string Dialogue_Gravedigger = "Dialogue-Gravedigger";

    public const string Dialogue_Poggy = "Dialogue-Poggy";

    public const string Dialogue_Joni = "Dialogue-Joni";

    public const string Dialogue_Myla = "Dialogue-Myla";

    public const string Dialogue_Emilitia = "Dialogue-Emilitia";

    public const string Dialogue_Willoh = "Dialogue-Willoh";

    public const string Dialogue_Moss_Prophet = "Dialogue-Moss_Prophet";

    public const string Dialogue_Fluke_Hermit = "Dialogue-Fluke_Hermit";

    public const string Dialogue_Queen = "Dialogue-Queen";

    public const string Dialogue_Marissa = "Dialogue-Marissa";

    public const string Dialogue_Grasshopper = "Dialogue-Grasshopper";

    public const string Dialogue_Dung_Defender = "Dialogue-Dung_Defender";

    public const string Dialogue_Godseeker = "Dialogue-Godseeker";

    public const string Dialogue_Hidden_Moth = "Dialogue-Hidden_Moth";

    public const string Dialogue_Millibelle = "Dialogue-Millibelle";

    public const string Dialogue_Menderbug_Diary = "Dialogue-Menderbug_Diary";

    public const string Dialogue_Sly = "Dialogue-Sly";

    public const string Dialogue_Iselda = "Dialogue-Iselda";

    public const string Dialogue_Little_Fool = "Dialogue-Little_Fool";

    public const string Dialogue_Salubra = "Dialogue-Salubra";

    public const string Dialogue_Brumm = "Dialogue-Brumm";

    #endregion

    #region Dream Dialogues

    public const string Dream_Dialogue_Mine_Golem = "Dream_Dialogue-Mine_Golem";

    public const string Dream_Dialogue_Aspid_Queen = "Dream_Dialogue-Aspid_Queen";

    public const string Dream_Dialogue_Overgrown_Shaman = "Dream_Dialogue-Overgrown_Shaman";

    public const string Dream_Dialogue_Crystalized_Shaman = "Dream_Dialogue-Crystalized_Shaman";

    public const string Dream_Dialogue_Ancient_Nailsmith_Golem = "Dream_Dialogue-Ancient_Nailsmith_Golem";

    public const string Dream_Dialogue_Shade_Golem_Normal = "Dream_Dialogue-Shade_Golem_Normal";

    public const string Dream_Dialogue_Shade_Golem_Void = "Dream_Dialogue-Shade_Golem_Void";

    public const string Dream_Dialogue_Shroom_King = "Dream_Dialogue-Shroom_King";

    public const string Dream_Dialogue_Hopper_Dummy = "Dream_Dialogue-Hopper_Dummy";

    public const string Dream_Dialogue_Dryya = "Dream_Dialogue-Dryya";

    public const string Dream_Dialogue_Isma = "Dream_Dialogue-Isma";

    public const string Dream_Dialogue_Shriek_Statue = "Dream_Dialogue-Shriek_Statue";

    public const string Dream_Dialogue_Radiance_Statue = "Dream_Dialogue-Radiance_Statue";

    public const string Dream_Dialogue_Dashmaster_Statue = "Dream_Dialogue-Dashmaster_Statue";

    public const string Dream_Dialogue_Snail_Shaman_Tomb = "Dream_Dialogue-Snail_Shaman_Tomb";

    public const string Dream_Dialogue_Pale_King = "Dream_Dialogue-Pale_King";

    public const string Dream_Dialogue_Kings_Mould_Machine = "Dream_Dialogue-Kings_Mould";

    public const string Dream_Dialogue_Grimm_Summoner = "Dream_Dialogue-Grimm_Summoner";

    public const string Dream_Dialogue_Dream_Shield_Statue = "Dream_Dialogue-Dream_Shield_Statue";

    public const string Dream_Dialogue_Lighthouse_Keeper = "Dream_Dialogue-Lighthouse_Keeper";

    public const string Dream_Dialogue_Key_Thief = "Dream_Dialogue-Key_Thief";

    public const string Dream_Dialogue_Spider_Victims = "Dream_Dialogue-Spider_Victims";

    #endregion

    #region Point of Interest

    public const string Inscription_City_Fountain = "Inscription-City_Fountain";

    public const string Inscription_Dreamer_Tablet = "Inscription-Dreamer_Tablet";

    public const string Inspect_Weaver_Seal = "Inspect-Weaver_Seal";

    public const string Inspect_Grimm_Machine = "Inspect-Grimm_Machine";

    public const string Inspect_Beast_Den_Altar = "Inspect-Beast_Den_Altar";

    public const string Inspect_Garden_Golem = "Inspect-Garden_Golem";

    public const string Inspect_Grub_Seal = "Inspect-Grub_Seal";

    public const string Inspect_White_Palace_Nursery = "Inspect-White_Palace_Nursery";

    public const string Inspect_Grimm_Summoner_Corpse = "Inspect-Grimm_Summoner_Corpse";

    public const string Inspect_Elder_Hu = "Inspect-Elder_Hu";

    public const string Inspect_Gorb = "Inspect-Gorb";

    public const string Inspect_Xero = "Inspect-Xero";

    public const string Inspect_Marmu = "Inspect-Marmu";

    public const string Inspect_Galien = "Inspect-Galien";

    public const string Inspect_No_Eyes = "Inspect-No_Eyes";

    public const string Inspect_Markoth = "Inspect-Markoth";

    public const string Inspect_Stag_Egg = "Inspect-Stag_Egg";

    public const string Traitor_Grave = "Traitor_Grave";

    public const string Lore_Tablet_Record_Bela = "Lore_Tablet-Record_Bela";

    #endregion

    #region Traveller

    public const string Dialogue_Zote_Greenpath = "Dialogue-Zote_Greenpath";

    public const string Dialogue_Zote_Dirtmouth_Intro = "Dialogue-Zote_Dirtmouth_Intro";

    public const string Dialogue_Zote_City = "Dialogue-Zote_City";

    public const string Dialogue_Zote_Deepnest = "Dialogue-Zote_Deepnest";

    public const string Dialogue_Zote_Colosseum = "Dialogue-Zote_Colosseum";

    public const string Dialogue_Zote_Dirtmouth_After_Colosseum = "Dialogue-Zote_Dirtmouth_After_Colosseum";

    public const string Dialogue_Cloth_Fungal_Wastes = "Dialogue-Cloth_Fungal_Wastes";

    public const string Dialogue_Cloth_Deepnest = "Dialogue-Cloth_Deepnest";

    public const string Dialogue_Cloth_Basin = "Dialogue-Cloth_Basin";

    public const string Dialogue_Cloth_Garden = "Dialogue-Cloth_Garden";

    public const string Dialogue_Cloth_Dirtmouth = "Dialogue-Cloth_Dirtmouth";

    public const string Dialogue_Cloth_Ghost = "Dialogue-Cloth_Ghost";

    public const string Dialogue_Tiso_Dirtmouth = "Dialogue-Tiso_Dirtmouth";

    public const string Dialogue_Tiso_Crossroads = "Dialogue-Tiso_Crossroads";

    public const string Dialogue_Tiso_Blue_Lake = "Dialogue-Tiso_Blue_Lake";

    public const string Dialogue_Tiso_Colosseum = "Dialogue-Tiso_Colosseum";

    public const string Dream_Dialogue_Tiso_Corpse = "Dream_Dialogue-Tiso_Corpse";

    public const string Dialogue_Quirrel_Crossroads = "Dialogue-Quirrel_Crossroads";

    public const string Dialogue_Quirrel_Greenpath = "Dialogue-Quirrel_Greenpath";

    public const string Dialogue_Quirrel_Queen_Station = "Dialogue-Quirrel_Queen_Station";

    public const string Dialogue_Quirrel_Mantis_Village = "Dialogue-Quirrel_Mantis_Village";

    public const string Dialogue_Quirrel_City = "Dialogue-Quirrel_City";

    public const string Dialogue_Quirrel_Deepnest = "Dialogue-Quirrel_Deepnest";

    public const string Dialogue_Quirrel_Peaks = "Dialogue-Quirrel_Peaks";

    public const string Dialogue_Quirrel_Outside_Archive = "Dialogue-Quirrel_Outside_Archive";

    public const string Dialogue_Quirrel_Archive = "Dialogue-Quirrel_Archive";

    public const string Dialogue_Quirrel_Blue_Lake = "Dialogue-Quirrel_Blue_Lake";

    public const string Dialogue_Hornet_Greenpath = "Dialogue-Hornet_Greenpath";

    public const string Dialogue_Hornet_Fountain = "Dialogue-Hornet_Fountain";

    public const string Dialogue_Hornet_Edge = "Dialogue-Hornet_Edge";

    public const string Dialogue_Hornet_Abyss = "Dialogue-Hornet_Abyss";

    public const string Dialogue_Hornet_Deepnest = "Dialogue-Hornet_Deepnest";

    public const string Dialogue_Hornet_Temple = "Dialogue-Hornet_Temple";

    #endregion
}
