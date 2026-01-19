using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;

namespace Patreon_Perks.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Rgb : ICommand
    {
        public string Command { get; } = "rgb";
        public string[] Aliases { get; }
        public string Description { get; }

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
            Main.Singleton.RgbCoroutine = Timing.RunCoroutine(UpdateLight());

            Timing.CallDelayed(5f, () =>
            {
                Timing.KillCoroutines(Main.Singleton.RgbCoroutine);

                foreach (Room room in Room.List)
                    room.ResetColor();
            });

            response = "You made a disco.";
            return true;
        }

        public IEnumerator<float> UpdateLight()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.5f);

                foreach (Room room in Room.List)
                {
                    UnityEngine.Color32 c = RainbowColors[UnityEngine.Random.Range(0, RainbowColors.Count)];
                    room.Color = new UnityEngine.Color(c.r / 255f, c.g / 255f, c.b / 255f);
                }
            }
        }

        public List<UnityEngine.Color32> RainbowColors { get; set; } = new List<UnityEngine.Color32>
        {
            new UnityEngine.Color32(228, 3, 3, 255),
            new UnityEngine.Color32(255, 140, 0, 255),
            new UnityEngine.Color32(255, 237, 0, 255),
            new UnityEngine.Color32(0, 128, 38, 255),
            new UnityEngine.Color32(9, 77, 255, 255),
            new UnityEngine.Color32(117, 7, 135, 255),
        };
    }
}
