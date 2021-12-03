using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Vehicles;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Color = System.Drawing.Color;

namespace Hippisownia.VehicleHelper.Commands
{
    [Command("move")] 
    [CommandDescription("Moves the vehicle you are looking at")]
    [CommandSyntax("<x/y/z> <int>")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandParent(typeof(CVehicleAdmin))]
    public class CMove : UnturnedCommand
    {
        private readonly IConfiguration m_Configuration;
        
        public CMove(IServiceProvider serviceProvider, IConfiguration mConfiguration) : base(serviceProvider)
        {
            m_Configuration = mConfiguration;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var uPlayer = (UnturnedUser) Context.Actor;
            var player = uPlayer.Player;
            
            if (Context.Parameters.Length != 2)
                throw new CommandWrongUsageException(Context);

            float distance = m_Configuration.GetSection("default_ray_range").Get<float>();

            await UniTask.SwitchToMainThread();

            InteractableVehicle? vehicle = Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out var hit, distance, RayMasks.VEHICLE) 
                ? hit.transform.gameObject.GetComponent<InteractableVehicle>() 
                : null;

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
                        return;
                }

                await UniTask.SwitchToThreadPool();
                
                await player.PrintMessageAsync($"{vehicle.asset.name} - {way} += {howMuch}", Color.Yellow);
            }
            else
            {
                await player.PrintMessageAsync("You are not looking at a vehicle", Color.Red);
            }
        }
    }
}