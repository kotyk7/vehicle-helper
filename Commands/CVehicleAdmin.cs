using System;
using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;

namespace Hippisownia.VehicleHelper.Commands
{
    [Command("vehicleadmin")]
    [CommandAlias("vadmin")]
    [CommandDescription("Moving/deleting vehicles")]
    [CommandSyntax("<delete/move>")]
    [CommandActor(typeof(UnturnedUser))]
    public class CVehicleAdmin : UnturnedCommand
    {
        public CVehicleAdmin(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async UniTask OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}