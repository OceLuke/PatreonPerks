using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace Patreon_Perks.Commands
{
	// Token: 0x0200000A RID: 10
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Size : ICommand
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000027A5 File Offset: 0x000009A5
		public string Command { get; } = "size";

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000027AD File Offset: 0x000009AD
		public string[] Aliases { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000027B5 File Offset: 0x000009B5
		public string Description { get; }

		// Token: 0x06000038 RID: 56 RVA: 0x000027C0 File Offset: 0x000009C0
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			if (!Permissions.CheckPermission(sender, "patreonperks.size"))
			{
				response = "You don't have permissions to use this command.";
				return true;
			}
			if (CollectionExtensions.At<string>(arguments, 0).ToString() == "limit")
			{
				response = "The limit of the size is between 0.8 and 1.2.";
				return true;
			}
			if (arguments.Count != 1)
			{
				response = "You need to specify a new size number.";
				return true;
			}
			float num;
			if (!float.TryParse(CollectionExtensions.At<string>(arguments, 0), out num))
			{
				response = "You need to specify a new size number.";
				return true;
			}
			if (num >= 0.8f && num <= 1.2f)
			{
				player.Scale = new Vector3(num, num, num);
				response = string.Format("Your size has been changed to {0}.", num);
				return true;
			}
			response = "You can only change your size between 0.8 and 1.2.";
			return true;
		}
	}
}
