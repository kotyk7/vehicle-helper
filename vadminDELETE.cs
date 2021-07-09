using System;
using System.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Vehicles;
using SDG.Unturned;
using UnityEngine;
using Command = OpenMod.Core.Commands.Command;

namespace VehicleHelper
{
    [Command("delete")] 
    [CommandDescription("Deletes the vehicle you are looking at")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandParent(typeof(VehicleAdmin))]
    public class vadminDELETE : Command
    {
        public vadminDELETE(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            var uPlayer = (UnturnedUser) Context.Actor;
            var player = uPlayer.Player;

            var vehicle = player.CurrentVehicle;
            
            vehicle = Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out var hit, 20f, RayMasks.VEHICLE) 
                ? new UnturnedVehicle(hit.transform.gameObject.GetComponent<InteractableVehicle>()) 
                : vehicle;
            
            if (vehicle != null)
            {
                await player.PrintMessageAsync($"Deleting {vehicle.Asset.VehicleName}");
                await vehicle.DestroyAsync();
                await player.PrintMessageAsync($"Deleted {vehicle.Asset.VehicleName}");
            }
            else
            {
                await player.PrintMessageAsync("You are not sitting in a vehicle or looking at one");
            }
        }
    }
}