using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Patreon_Perks.Commands
{
	// Token: 0x02000005 RID: 5
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Nick : ICommand
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000022FD File Offset: 0x000004FD
		public string Command { get; } = "nick";

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002305 File Offset: 0x00000505
		public string[] Aliases { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000230D File Offset: 0x0000050D
		public string Description { get; }

		// Token: 0x0600001C RID: 28 RVA: 0x00002318 File Offset: 0x00000518
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			string text = string.Join(" ", arguments.Skip(0));
			if (!Permissions.CheckPermission(sender, "patreonperks.nick"))
			{
				response = "You don't have permissions to use this command.";
				return true;
			}
			if (arguments.Count == 0)
			{
				response = "You didn't specified a new nickname.";
				return true;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				response = "Your nickname can't be empty.";
				return true;
			}
			if (player.DoNotTrack)
			{
				response = "You have to disable 'Do Not Track' to use this command.";
				return false;
			}
			player.CustomName = text;
			response = "Nick changed to " + text + ".";
			return true;
		}
	}
}
