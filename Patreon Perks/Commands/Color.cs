using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Patreon_Perks.Commands
{
	// Token: 0x02000004 RID: 4
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Color : ICommand
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000021AA File Offset: 0x000003AA
		public string Command { get; } = "color";

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000021B2 File Offset: 0x000003B2
		public string[] Aliases { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000021BA File Offset: 0x000003BA
		public string Description { get; }

		// Token: 0x06000017 RID: 23 RVA: 0x000021C4 File Offset: 0x000003C4
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			if (!Permissions.CheckPermission(sender, "patreonperks.color"))
			{
				response = "You don't have permissions to use this command.";
				return true;
			}
			string text = CollectionExtensions.At<string>(arguments, 0);
			if (text != null)
			{
				int length = text.Length;
				if (length <= 4)
				{
					if (length == 0)
					{
						goto IL_FB;
					}
					if (length != 4)
					{
						goto IL_ED;
					}
					char c = text[0];
					if (c != 'b')
					{
						if (c != 'g')
						{
							if (c != 't')
							{
								goto IL_ED;
							}
							if (!(text == "teal"))
							{
								goto IL_ED;
							}
						}
						else if (!(text == "gold"))
						{
							goto IL_ED;
						}
					}
					else if (!(text == "blue"))
					{
						goto IL_ED;
					}
				}
				else if (length != 6)
				{
					if (length != 9)
					{
						if (length != 11)
						{
							goto IL_ED;
						}
						char c = text[0];
						if (c != 'p')
						{
							if (c != 's')
							{
								goto IL_ED;
							}
							if (!(text == "silver_blue"))
							{
								goto IL_ED;
							}
						}
						else if (!(text == "police_blue"))
						{
							goto IL_ED;
						}
					}
					else if (!(text == "light_red"))
					{
						goto IL_ED;
					}
				}
				else if (!(text == "purple"))
				{
					goto IL_ED;
				}
				response = "You can't use that color, try with, army_green, brown, carmine, crimson, cyan, deep_pink, emerald, green, lime, magenta, mint, nickel, orange, pink, red, silver, tomato or yellow.";
				return true;
			}
			IL_ED:
			if (text != null)
			{
				player.RankColor = CollectionExtensions.At<string>(arguments, 0);
				response = "Color changed successfully.";
				return true;
			}
			IL_FB:
			response = "You didn't specified a color, try with, army_green, brown, carmine, crimson, cyan, deep_pink, emerald, green, lime, magenta, mint, nickel, orange, pink, red, silver, tomato or yellow.";
			return true;
		}
	}
}
