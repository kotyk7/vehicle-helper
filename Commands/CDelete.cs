using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Vehicles;
using SDG.Unturned;
using UnityEngine;
using Color = System.Drawing.Color;

namespace Hippisownia.VehicleHelper.Commands
{
    [Command("delete")] 
    [CommandDescription("Deletes the vehicle you are looking at")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandParent(typeof(CVehicleAdmin))]
    public class CDelete : UnturnedCommand
    {
        private readonly IConfiguration m_Configuration;
        
        public CDelete(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
        {
            m_Configuration = configuration;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var uPlayer = (UnturnedUser) Context.Actor;
            var player = uPlayer.Player;

            UnturnedVehicle? vehicle = player.CurrentVehicle;
            float distance = m_Configuration.GetSection("default_ray_range").Get<float>();
            
            await UniTask.SwitchToMainThread();
            vehicle = Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out var hit, distance, RayMasks.VEHICLE) 
                ? new(hit.transform.gameObject.GetComponent<InteractableVehicle>()) 
                : vehicle;

            await UniTask.SwitchToThreadPool();

            if (vehicle != null)
            {
                await player.PrintMessageAsync($"Deleting {vehicle.Asset.VehicleName}", Color.Yellow);
                await vehicle.DestroyAsync();
                await player.PrintMessageAsync($"Deleted {vehicle.Asset.VehicleName}", Color.Yellow);
            }
            else
            {
                await player.PrintMessageAsync("You are not sitting in a vehicle or looking at one", Color.Red);
            }
        }
    }
}