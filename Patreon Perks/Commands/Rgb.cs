using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using UnityEngine;

namespace Patreon_Perks.Commands
{
	// Token: 0x02000008 RID: 8
	[CommandHandler(typeof(ClientCommandHandler))]
	public class Rgb : ICommand
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002553 File Offset: 0x00000753
		public string Command { get; } = "rgb";

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000255B File Offset: 0x0000075B
		public string[] Aliases { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002563 File Offset: 0x00000763
		public string Description { get; }

		// Token: 0x0600002B RID: 43 RVA: 0x0000256C File Offset: 0x0000076C
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			if (!Permissions.CheckPermission(sender, "patreonperks.rgb"))
			{
				response = "You don't have permissions to use this command.";
				return true;
			}
			if (Main.Singleton.RgbUses.Contains(player.UserId))
			{
				response = "You already used this command.";
				return true;
			}
			Main.Singleton.RgbUses.Add(player.UserId);
			Main.Singleton.RgbCoroutine = Timing.RunCoroutine(this.UpdateLight());
			Timing.CallDelayed(5f, delegate()
			{
				Timing.KillCoroutines(new CoroutineHandle[]
				{
					Main.Singleton.RgbCoroutine
				});
				foreach (Room room in Room.List)
				{
					room.ResetColor();
				}
			});
			response = "You made a disco.";
			return true;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002612 File Offset: 0x00000812
		public IEnumerator<float> UpdateLight()
		{
			for (;;)
			{
				yield return Timing.WaitForSeconds(0.5f);
				using (IEnumerator<Room> enumerator = Room.List.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Room room = enumerator.Current;
						room.Color = new Color(this.RainbowColors[Random.Range(0, this.RainbowColors.Count)].r / 255f, this.RainbowColors[Random.Range(0, this.RainbowColors.Count)].g / 255f, this.RainbowColors[Random.Range(0, this.RainbowColors.Count)].b / 255f);
					}
					continue;
				}
				yield break;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002621 File Offset: 0x00000821
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002629 File Offset: 0x00000829
		public List<Color> RainbowColors { get; set; } = new List<Color>
		{
			new Color(228f, 3f, 3f),
			new Color(255f, 140f, 0f),
			new Color(255f, 237f, 0f),
			new Color(0f, 128f, 38f),
			new Color(9f, 77f, 255f),
			new Color(117f, 7f, 135f)
		};
	}
}
