using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;

[assembly: PluginMetadata("Hippisownia.VehicleHelper", 
    DisplayName = "Vehicle Helper Plugin",
    Author = "kotyk",
    Website = "https://github.com/kotyk7/vehicle-helper",
    Description = "Move or delete your vehicles with simple commands!")]

namespace Hippisownia.VehicleHelper
{
    public class VehicleHelperPlugin : OpenModUnturnedPlugin
    {
        private readonly ILogger<VehicleHelperPlugin> m_Logger;

        public VehicleHelperPlugin(
            ILogger<VehicleHelperPlugin> logger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_Logger = logger;
        }

        protected override async UniTask OnLoadAsync()
        {
            m_Logger.LogInformation("Vehicle Helper by kotyk v1.2");
            m_Logger.LogInformation("For support create an issue on https://github.com/kotyk7/vehicle-helper");
        }
    }
}