using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Patreon_Perks.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Glow : ICommand
    {
        public string Command { get; } = "glow";
        public string[] Aliases { get; }
        public string Description { get; } = "Toggle a glow on yourself.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Permissions.CheckPermission(sender, "patreonperks.glow"))
            {
                response = "You don't have permission to use this command.";
                return true;
            }

            bool enabled = GlowFeature.Toggle(player);
            response = enabled ? "Glow enabled." : "Glow disabled.";
            return true;
        }
    }
}
