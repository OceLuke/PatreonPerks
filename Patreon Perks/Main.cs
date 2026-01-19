using System;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace Patreon_Perks
{
    internal class Main : Plugin<Config>
    {
        public override string Author => "ClaudioPanConQueso";
        public override string Name => "Patreon Perks";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 0, 0);

        public static Main Singleton;

        // ===== Existing Perk Tracking =====
        public List<string> ScreamUses = new List<string>();
        public List<string> RgbUses = new List<string>();
        public CoroutineHandle RgbCoroutine;

        // ===== NEW: Glow Tracking =====
        public List<string> GlowUses = new List<string>();

        public override void OnEnabled()
        {
            Singleton = this;
            SubServerEvents();

            GlowFeature.Enable();   // ADD THIS

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            GlowFeature.Disable();  // ADD THIS

            UnSubServerEvents();
            Singleton = null;
            base.OnDisabled();
        }

        // ===== Server Events =====
        public void SubServerEvents()
        {
            Exiled.Events.Handlers.Server.RestartingRound += OnRestart;
        }

        public void UnSubServerEvents()
        {
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestart;
        }

        // ===== Round Reset =====
        public void OnRestart()
        {
            try { RgbUses.Clear(); } catch { }
            try { ScreamUses.Clear(); } catch { }

            // NEW: reset glow usage each round
            try { GlowUses.Clear(); } catch { }
        }
    }
}
