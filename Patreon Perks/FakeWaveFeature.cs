using Exiled.API.Enums;
using Exiled.API.Features;
using Respawning;
using Respawning.Waves;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
// Alias to avoid Cassie namespace/type weirdness in your project
using ExCassie = Exiled.API.Features.Cassie;

namespace Patreon_Perks.Features
{
    public static class FakeWaveFeature
    {
        private static bool _enabled;

        private static bool _ciUsed;
        private static bool _ntfUsed;

        public static void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;

            ResetPerRound();
        }

        public static void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;

            ResetPerRound();
        }

        public static bool TryRunFakeCiWave(out string response)
        {
            if (!Round.IsStarted)
            {
                response = "Round has not started.";
                return false;
            }

            if (_ciUsed)
            {
                response = "You already used .fakeciwave this round.";
                return false;
            }

            _ciUsed = true;

            if (TryGetFaction(new[] { "ChaosInsurgency", "Chaos", "CI" }, out var faction))
                PlayWaveEffectOnly(faction);

            ExCassie.MessageTranslated(
                message: "CHAOS INSURGENCY FORCES HAVE ENTERED THE FACILITY THROUGH GATE A",
                translation: "Chaos Insurgency Forces have entered the facility through Gate A.",
                isHeld: false,
                isNoisy: true,
                isSubtitles: true
            );

            response = "Fake CI wave triggered (effects + Cassie only).";
            return true;
        }

        public static bool TryRunFakeNtfWave(out string response)
        {
            if (!Round.IsStarted)
            {
                response = "Round has not started.";
                return false;
            }

            if (_ntfUsed)
            {
                response = "You already used .fakentfwave this round.";
                return false;
            }

            _ntfUsed = true;

            if (TryGetFaction(new[] { "NineTailedFox", "Ntf", "MTF" }, out var faction))
                PlayWaveEffectOnly(faction);

            string callsign = GenerateCallsign();
            string cassie = $"MOBILE TASK FORCE UNIT EPSILON 11 DESIGNATED {callsign} HAS ENTERED THE FACILITY";
            string subtitles = $"Mobile Task Force unit Epsilon-11 designated {callsign.Replace(" ", "-")} has entered the facility.";

            ExCassie.MessageTranslated(
                message: cassie,
                translation: subtitles,
                isHeld: false,
                isNoisy: true,
                isSubtitles: true
            );

            response = "Fake NTF wave triggered (effects + Cassie only).";
            return true;
        }

        private static void PlayWaveEffectOnly(SpawnableFaction faction)
        {
            try
            {
                if (Respawn.TryGetWaveBase(faction, out SpawnableWaveBase wave) && wave != null)
                    Respawn.PlayEffect(wave);
            }
            catch (Exception e)
            {
                Log.Warn($"FakeWaveFeature: failed to play wave effect for {faction}: {e}");
            }
        }

        private static bool TryGetFaction(string[] possibleNames, out SpawnableFaction faction)
        {
            faction = default;

            try
            {
                Type t = typeof(SpawnableFaction);
                foreach (string name in possibleNames)
                {
                    // Try exact field name first
                    FieldInfo fi = t.GetField(name, BindingFlags.Public | BindingFlags.Static);
                    if (fi != null)
                    {
                        object v = fi.GetValue(null);
                        if (v is SpawnableFaction f)
                        {
                            faction = f;
                            return true;
                        }
                    }
                }

                // Last resort: search by contains (case-insensitive)
                var fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (var fi in fields)
                {
                    string fn = fi.Name;
                    if (possibleNames.Any(p => fn.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        object v = fi.GetValue(null);
                        if (v is SpawnableFaction f)
                        {
                            faction = f;
                            return true;
                        }
                    }
                }
            }
            catch { }

            return false;
        }

        private static string GenerateCallsign()
        {
            string[] nato =
            {
                "ALPHA","BRAVO","CHARLIE","DELTA","ECHO","FOXTROT","GOLF","HOTEL",
                "INDIA","JULIET","KILO","LIMA","MIKE","NOVEMBER","OSCAR","PAPA",
                "QUEBEC","ROMEO","SIERRA","TANGO","UNIFORM","VICTOR","WHISKEY","XRAY","YANKEE","ZULU"
            };

            int num = UnityEngine.Random.Range(1, 10);
            string word = nato[UnityEngine.Random.Range(0, nato.Length)];
            return $"{word} {num}";
        }

        private static void OnRoundStarted() => ResetPerRound();
        private static void OnRestartingRound() => ResetPerRound();

        private static void ResetPerRound()
        {
            _ciUsed = false;
            _ntfUsed = false;
        }
    }
}
