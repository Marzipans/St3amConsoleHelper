﻿using System.Collections.Generic;
using System.Linq;

using SteamConsoleHelper.Abstractions.Cache;
using SteamConsoleHelper.Abstractions.Enums;
using SteamConsoleHelper.Abstractions.Inventory;
using SteamConsoleHelper.Abstractions.Market;
using SteamConsoleHelper.ApiModels.Responses.Inventory;

namespace SteamConsoleHelper.Helpers
{
    public static class InventoryHelper
    {
        public static List<InventoryItem> FilterByMarketable(this List<InventoryItem> inventoryItems)
            => inventoryItems.FindAll(x => x.Marketable);

        public static List<InventoryItem> FilterByCommodity(this List<InventoryItem> inventoryItems)
            => inventoryItems.FindAll(x => x.Commodity);

        public static List<InventoryItem> FilterByType(this List<InventoryItem> inventoryItems, ItemType itemType)
            => inventoryItems.FindAll(x => x.ItemType == itemType);

        public static List<(InventoryAssetResponseModel asset, InventoryDescriptionResponseModel description)> MapAssets(
            List<InventoryAssetResponseModel> assets,
            List<InventoryDescriptionResponseModel> descriptions)
        {
            var result = new List<(InventoryAssetResponseModel, InventoryDescriptionResponseModel)>();
            var lookup = descriptions.ToLookup(x => x.ClassId);

            foreach (var asset in assets)
            {
                var description = lookup[asset.ClassId].First(x => x.InstanceId == asset.InstanceId);
                result.Add((asset, description));
            }

            return result;
        }

        public static List<(ItemWithPrice itemWithPrice, MarketListing listing)> MapPrices(
            List<ItemWithPrice> marketPrices,
            List<MarketListing> listings)
        {
            // todo: parse html tag too and get prices from it
            // code below works only with active cache
            // but if item was sold manually it wont find it
            var result = new List<(ItemWithPrice, MarketListing)>();
            var lookup = listings.ToLookup(x => x.ClassId);

            foreach (var itemWithPrice in marketPrices)
            {
                var asset = lookup[itemWithPrice.Item.ClassId]
                    .FirstOrDefault(x => x.AssetId == itemWithPrice.Item.AssetId);

                result.Add((itemWithPrice, asset));
            }

            return result;
        }
    }
}