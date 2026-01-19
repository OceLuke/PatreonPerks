using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Patreon_Perks.Utils;

namespace Patreon_Perks.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Scream : ICommand
    {
        public string Command { get; } = "scream";
        public string[] Aliases { get; }
        public string Description { get; }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Permissions.CheckPermission(sender, "patreonperks.scream"))
            {
                response = "You don't have permissions to use this command.";
                return true;
            }

            if (Main.Singleton.ScreamUses.Contains(player.UserId))
            {
                response = "You already made C.A.S.S.I.E. scream.";
                return true;
            }

            CassieCompat.Announce(Main.Singleton.Config.CassieScream);

            Main.Singleton.ScreamUses.Add(player.UserId);
            response = "You made C.A.S.S.I.E. scream.";
            return true;
        }
    }
}
