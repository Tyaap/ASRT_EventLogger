﻿using System.ComponentModel;

namespace EventLogger
{
    public enum Character
    {
        [Description("AiAi")] AiAi,
        [Description("Alex Kidd")] AlexKidd,
        [Description("Amigo")] Amigo,
        [Description("Amy")] Amy,
        [Description("Avatar")] Avatar,
        [Description("B.D. Joe")] BDJoe,
        [Description("Beat")] Beat,
        [Description("Danica Patrick")] DanicaPatrick,
        [Description("Dr. Eggman")] DrEggman,
        [Description("Football Manager")] FootballManager,
        [Description("Gilius")] Gilius,
        [Description("Gum")] Gum,
        [Description("Joe Musashi")] JoeMusashi,
        [Description("Knuckles")] Knuckles,
        [Description("MeeMee")] MeeMee,
        [Description("Metal Sonic")] MetalSonic,
        [Description("Mii")] Mii,
        [Description("NiGHTS")] NiGHTS,
        [Description("Pudding")] Pudding,
        [Description("Reala")] Reala,
        [Description("AGES")] AGES,
        [Description("Shadow")] Shadow,
        [Description("Shogun")] Shogun,
        [Description("Sonic")] Sonic,
        [Description("Tails")] Tails,
        [Description("Team Fortress")] TeamFortress,
        [Description("Ulala")] Ulala,
        [Description("Vyse")] Vyse,
        [Description("Ralph")] Ralph,
        [Description("Ryo")] Ryo,
        [Description("Charmy Bee")] CharmyBee,
        [Description("General Winter")] GeneralWinter,
        [Description("Willemus")] Willemus,
        [Description("Yogscast")] Yogscast,
    }

    public enum Map : uint
    {
        [Description("Seasonal Shrines")] SeasonalShrines = 0x503C1CBC,
        [Description("Graffiti City")] GraffitiCity = 0xD936550C,
        [Description("Adder's Lair")] AddersLair = 0xDC93F18B,
        [Description("Chilly Castle")] ChillyCastle = 0xC5C9DEA1,
        [Description("Graveyard Gig")] GraveyardGig = 0xCD8017BA,
        [Description("Carrier Zone")] CarrierZone = 0xC72B3B98,
        [Description("Galactic Parade")] GalacticParade = 0x4E015AB6,
        [Description("Temple Trouble")] TempleTrouble = 0xE3121777,
        [Description("Sanctuary Falls")] SanctuaryFalls = 0x4A0FF7AE,
        [Description("Dream Valley")] DreamValley = 0x38A394ED,
        [Description("Race of Ages")] RaceOfAges = 0x94610644,
        [Description("Ocean View")] OceanView = 0xD4257EBD,
        [Description("Samba Studios")] SambaStudios = 0x32D305A8,
        [Description("Dragon Canyon")] DragonCanyon = 0x03EB7FFF,
        [Description("Burning Depths")] BurningDepths = 0x2DB91FC2,
        [Description("Roulette Road")] RouletteRoad = 0x17463C8D,
        [Description("Shibuya Downtown")] ShibuyaDowntown = 0xE87FDF22,
        [Description("Egg Hangar")] EggHangar = 0xFEBC639E,
        [Description("Sunshine Tour")] SunshineTour = 0xE6CD97F0,
        [Description("Rogue's Landing")] RoguesLanding = 0x7534B7CA,
        [Description("Outrun Bay")] OutrunBay = 0x1EF56CE1,
        [Description("Neon Docks")] NeonDocks = 0xB9B67B8F,
        [Description("Battle Bay")] BattleBay = 0x7583BBD6,
        [Description("Creepy Courtyard")] CreepyCourtyard = 0x997E9C42,
        [Description("Rooftop Rumble")] RooftopRumble = 0x8DABE769,
        [Description("Monkey Ball Park")] MonkeyBallPark = 0x38A73138,
    }

    public enum MapAcronym : uint
    {
        SeS = 0x503C1CBC,
        GC = 0xD936550C,
        AL = 0xDC93F18B,
        CC = 0xC5C9DEA1,
        GG = 0xCD8017BA,
        CZ = 0xC72B3B98,
        GP = 0x4E015AB6,
        TT = 0xE3121777,
        SF = 0x4A0FF7AE,
        DV = 0x38A394ED,
        RoA = 0x94610644,
        OV = 0xD4257EBD,
        SaS = 0x32D305A8,
        DC = 0x03EB7FFF,
        BD = 0x2DB91FC2,
        RR = 0x17463C8D,
        SD = 0xE87FDF22,
        EH = 0xFEBC639E,
        ST = 0xE6CD97F0,
        RL = 0x7534B7CA,
        OB = 0x1EF56CE1,
        ND = 0xB9B67B8F,
        BB = 0x7583BBD6,
        CrC = 0x997E9C42,
        RoR = 0x8DABE769,
        MBP = 0x38A73138,
    }

    public enum EventType : uint
    {
        [Description("Normal Race")] NormalRace = 0x5508FFC7,
        [Description("Battle Race")] BattleRace = 0xE64B5DD8,
        [Description("Boost Race")] BoostRace = 0x61FF5D42,
        [Description("Capture the Chao")] CaptureTheChao = 0xCCB41574,
        [Description("Battle Arena")] BattleArena = 0x447473BC,
    }

    public enum Completion
    {
        NA = 0,
        Finished = 1,
        Eliminated = 3,
        DNF = 5,
    }
}
