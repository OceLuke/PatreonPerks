using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace Patreon_Perks.Commands
{
	// Token: 0x0200000B RID: 11
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Wide : ICommand
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002884 File Offset: 0x00000A84
		public string Command { get; } = "wide";

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000288C File Offset: 0x00000A8C
		public string[] Aliases { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002894 File Offset: 0x00000A94
		public string Description { get; }

		// Token: 0x0600003D RID: 61 RVA: 0x0000289C File Offset: 0x00000A9C
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			if (!Permissions.CheckPermission(sender, "patreonperks.wide"))
			{
				response = "You don't have permissions to use this command.";
				return true;
			}
			player.Scale = new Vector3(Main.Singleton.Config.WideSize, 1f, Main.Singleton.Config.WideSize);
			response = "You're now wide.";
			return true;
		}
	}
}
