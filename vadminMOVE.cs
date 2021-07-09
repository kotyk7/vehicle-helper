using System;
using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using UnityEngine;
using Color = System.Drawing.Color;

namespace VehicleHelper
{
    [Command("move")] 
    [CommandDescription("Moves the vehicle you are sitting in to y+10")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandParent(typeof(VehicleAdmin))] // set "awesome" as parent.
    public class vadminMOVE : UnturnedCommand
    {
        public vadminMOVE(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async UniTask OnExecuteAsync()
        {
            var uPlayer = (UnturnedUser) Context.Actor;
            var player = uPlayer.Player;


            Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out var hit, 25f,
                RayMasks.VEHICLE);
            var vehicle = hit.transform.gameObject.GetComponent<InteractableVehicle>();

            if (player.CurrentVehicle != null)
            {
                await player.PrintMessageAsync("You can't sit in a vehicle doing this (it won't move)", Color.Red);
                return;
            }

            if (vehicle != null)
            {
                var way = await Context.Parameters.GetAsync<string>(0);
                var howMuch = await Context.Parameters.GetAsync<int>(1);

                await UniTask.SwitchToMainThread();
                
                switch (way)
                {
                    case "x":
                        vehicle.transform.position += new Vector3(howMuch, 0);
                        break;
                    case "y":
                        vehicle.transform.position += new Vector3(0, howMuch);
                        break;
                    case "z":
                        vehicle.transform.position += new Vector3(0, 0, howMuch);
                        break;
                    default:
                        await UniTask.SwitchToThreadPool();
                        await player.PrintMessageAsync("Usage: /vadmin move x/y/z int");
                        return;
                }

                await UniTask.SwitchToThreadPool();
                
                await player.PrintMessageAsync($"{vehicle.asset.name} - {way} += {howMuch}", Color.LawnGreen);
            }
            else
            {
                await player.PrintMessageAsync("You are not looking at a vehicle", Color.Red);
            }
        }
    }
}