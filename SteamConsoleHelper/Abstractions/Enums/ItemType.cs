﻿using System.ComponentModel;

namespace SteamConsoleHelper.Abstractions.Enums
{
    public enum ItemType
    {
        Undefined,

        Gems,

        [Description("Trading Card")]
        TradingCard,

        [Description("Booster pack")]
        BoosterPack,

        [Description("Profile Background")]
        ProfileBackground,
        Emoticon
    }
}