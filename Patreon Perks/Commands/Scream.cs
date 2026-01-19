using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Patreon_Perks.Commands
{
	// Token: 0x02000009 RID: 9
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Scream : ICommand
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000026F9 File Offset: 0x000008F9
		public string Command { get; } = "scream";

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002701 File Offset: 0x00000901
		public string[] Aliases { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002709 File Offset: 0x00000909
		public string Description { get; }

		// Token: 0x06000033 RID: 51 RVA: 0x00002714 File Offset: 0x00000914
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
			Cassie.Message(Main.Singleton.Config.CassieScream, false, false, false);
			Main.Singleton.ScreamUses.Add(player.UserId);
			response = "You made C.A.S.S.I.E. scream.";
			return true;
		}
	}
}
