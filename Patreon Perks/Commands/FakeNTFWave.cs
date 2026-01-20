using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Patreon_Perks.Features;

namespace Patreon_Perks.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class FakeNtfWave : ICommand
    {
        public string Command { get; } = "fakentfwave";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "Plays a fake NTF spawn wave (effects + Cassie only).";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Permissions.CheckPermission(sender, "patreonperks.fakentfwave"))
            {
                response = "You don't have permission to use this command.";
                return true;
            }

            if (FakeWaveFeature.TryRunFakeNtfWave(out string resp))
            {
                response = resp;
                return true;
            }

            response = resp;
            return true;
        }
    }
}
