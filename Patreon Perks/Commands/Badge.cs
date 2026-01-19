using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Patreon_Perks.Commands
{
	// Token: 0x02000006 RID: 6
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Badge : ICommand
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000023BA File Offset: 0x000005BA
		public string Command { get; } = "badge";

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000023C2 File Offset: 0x000005C2
		public string[] Aliases { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000023CA File Offset: 0x000005CA
		public string Description { get; }

		// Token: 0x06000021 RID: 33 RVA: 0x000023D4 File Offset: 0x000005D4
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			string text = string.Join(" ", arguments.Skip(0));
			if (!Permissions.CheckPermission(sender, "patreonperks.badge"))
			{
				response = "You don't have permissions to use this command.";
				return true;
			}
			if (arguments.Count == 0)
			{
				response = "You didn't specified a new badge name.";
				return true;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				response = "Your badge name can't be empty.";
				return true;
			}
			string[] array = player.RankName.Split(new char[]
			{
				' '
			});
			if (array.Length >= 3 && array[0].Equals("LVL:", StringComparison.OrdinalIgnoreCase))
			{
				player.RankName = string.Concat(new string[]
				{
					array[0],
					" ",
					array[1],
					" ",
					string.Join(" ", arguments.Skip(0))
				});
				response = "Badge name changed to " + text + ".";
				return true;
			}
			response = "Error: Unable to find 'LVL:' followed by a number in the badge name.";
			return true;
		}
	}
}
