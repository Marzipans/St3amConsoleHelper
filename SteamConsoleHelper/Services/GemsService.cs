﻿using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SteamConsoleHelper.Abstractions.Enums;
using SteamConsoleHelper.Abstractions.Inventory;
using SteamConsoleHelper.ApiModels.Requests;
using SteamConsoleHelper.Common;
using SteamConsoleHelper.Exceptions;
using SteamConsoleHelper.Resources;

namespace SteamConsoleHelper.Services
{
    public class GemsService
    {
        private readonly ILogger<GemsService> _logger;
        private readonly SteamUrlService _steamUrlService;
        private readonly WebRequestService _webRequestService;

        public GemsService(
            ILogger<GemsService> logger,
            SteamUrlService steamUrlService, 
            WebRequestService webRequestService)
        {
            _logger = logger;
            _steamUrlService = steamUrlService;
            _webRequestService = webRequestService;
        }

        public async Task GrindSackIntoGems(InventoryItem item)
        {
            if (item.ItemType != ItemType.SackOfGems)
            {
                throw new InternalException(InternalError.InventoryItemIsNotASackOfGems);
            }

            var url = _steamUrlService.GrindSackToGemsUrl();
            var data = new GrindSackIntoGemsPostModel(
                Settings.SessionId,
                item.AppId,
                item.AssetId,
                item.Amount);

            await _webRequestService.PostRequestAsync(url, data);
            _logger.LogDebug($"Ground sack of gems into '{item.Amount * 1000}' gems");
        }

        public async Task GrindItemIntoGems(InventoryItem item)
        {
            var url = _steamUrlService.GrindItemIntoGemsUrl();
            var data = new GrindItemIntoGemsPostModel(
                Settings.SessionId,
                item.AppId,
                item.AssetId,
                item.ContextId);

            await _webRequestService.PostRequestAsync(url, data);
            _logger.LogDebug($"Ground item '{item.MarketHashName}' into gems");
        }
    }
}