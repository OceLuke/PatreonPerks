using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace Patreon_Perks.Commands
{
	// Token: 0x02000007 RID: 7
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Reset : ICommand
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000024DA File Offset: 0x000006DA
		public string Command { get; } = "reset";

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000024E2 File Offset: 0x000006E2
		public string[] Aliases { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000024EA File Offset: 0x000006EA
		public string Description { get; }

		// Token: 0x06000026 RID: 38 RVA: 0x000024F4 File Offset: 0x000006F4
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			if (!Permissions.CheckPermission(sender, "patreonperks.reset"))
			{
				response = "You don't have permissions to use this command.";
				return true;
			}
			player.Scale = new Vector3(1f, 1f, 1f);
			response = "You are now normal size.";
			return true;
		}
	}
}
