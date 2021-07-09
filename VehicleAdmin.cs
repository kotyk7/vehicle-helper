using System;
using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
#pragma warning disable 1998

namespace Hippisownia
{
    [Command("vehicleadmin")]
    [CommandAlias("vadmin")]
    [CommandDescription("Moving/deleting vehicles")]
    [CommandActor(typeof(UnturnedUser))]
    public class VehicleAdmin : UnturnedCommand
    {
        public VehicleAdmin(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async UniTask OnExecuteAsync()
        {
            await Context.Actor.PrintMessageAsync("Command usage: /vadmin delete/move");
        }
    }
}