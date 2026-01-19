using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;

// Aliases to avoid ambiguous references
using ExPlayer = Exiled.API.Features.Player;
using ExLightToy = Exiled.API.Features.Toys.Light;

namespace Patreon_Perks
{
    public static class GlowFeature
    {
        // One light per player (keyed by UserId)
        private static readonly Dictionary<string, ExLightToy> LightsByUserId = new Dictionary<string, ExLightToy>();
        private static readonly HashSet<string> EnabledUsers = new HashSet<string>();

        private static CoroutineHandle _followCoroutine;
        private static bool _enabled;

        // Tweakables (move to Config if you want)
        public static Color GlowColor = new Color(0.25f, 0.85f, 1f, 1f);
        public static float GlowIntensity = 3.0f;
        public static float GlowRange = 2.2f;
        public static Vector3 GlowOffset = new Vector3(0f, 0.5f, 0f);   // head height-ish
        public static float FollowTickSeconds = 0.03f;                  // ~33 times/sec


        public static void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            // IMPORTANT: subscribe via fully-qualified event handlers to avoid Player ambiguity
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;

           // _followCoroutine = Timing.RunCoroutine(FollowLoop());
        }

        public static void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;

            if (_followCoroutine.IsRunning)
                Timing.KillCoroutines(_followCoroutine);

            // Cleanup all lights
            foreach (var kvp in LightsByUserId)
            {
                try { kvp.Value?.Destroy(); }
                catch { }
            }

            LightsByUserId.Clear();
            EnabledUsers.Clear();
        }

        public static bool IsEnabledFor(ExPlayer player)
            => player != null && EnabledUsers.Contains(player.UserId);

        public static void SetEnabled(ExPlayer player, bool enabled)
        {
            if (player == null || string.IsNullOrEmpty(player.UserId))
                return;

            if (enabled)
            {
                EnabledUsers.Add(player.UserId);
                EnsureLight(player);
            }
            else
            {
                EnabledUsers.Remove(player.UserId);
                RemoveLight(player.UserId);
            }
        }

        public static bool Toggle(ExPlayer player)
        {
            bool newState = !IsEnabledFor(player);
            SetEnabled(player, newState);
            return newState;
        }

        private static void EnsureLight(ExPlayer player)
        {
            if (player == null || !player.IsAlive)
                return;

            if (LightsByUserId.TryGetValue(player.UserId, out var existing) && existing != null)
            {
                ApplySettings(existing);
                existing.Position = player.Position + GlowOffset;
                return;
            }

            // Create light toy
            var light = ExLightToy.Create(position: player.Position + GlowOffset, rotation: Vector3.zero, scale: Vector3.one, spawn: true);
            light.Transform.SetParent(player.Transform, true);
            ApplySettings(light);

            LightsByUserId[player.UserId] = light;
        }

        private static void ApplySettings(ExLightToy light)
        {
            if (light == null) return;

            light.Color = GlowColor;
            light.Intensity = GlowIntensity;
            light.Range = GlowRange;
            light.IsStatic = false;

            // Disable shadows on the underlying Unity Light component
            try
            {
                // In Exiled toy wrappers, GameObject is usually exposed.
                var go = light.GameObject;
                if (go != null)
                {
                    var unityLight = go.GetComponent<UnityEngine.Light>();
                    if (unityLight != null)
                    {
                        //unityLight.shadows = LightShadows.None;
                    }
                }
            }
            catch
            {
                // If wrapper changes, we just ignore and keep working.
            }
        }


        private static void RemoveLight(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return;

            if (LightsByUserId.TryGetValue(userId, out var light) && light != null)
            {
                try { light.Destroy(); }
                catch { }
            }

            LightsByUserId.Remove(userId);
        }

        private static void OnLeft(Exiled.Events.EventArgs.Player.LeftEventArgs ev)
        {
            if (ev?.Player == null) return;

            EnabledUsers.Remove(ev.Player.UserId);
            RemoveLight(ev.Player.UserId);
        }

        private static void OnDied(Exiled.Events.EventArgs.Player.DiedEventArgs ev)
        {
            if (ev?.Player == null) return;

            // Remove toy on death (keep EnabledUsers so it can come back on respawn/role)
            RemoveLight(ev.Player.UserId);
        }

        private static void OnChangingRole(Exiled.Events.EventArgs.Player.ChangingRoleEventArgs ev)
        {
            if (ev?.Player == null) return;

            RemoveLight(ev.Player.UserId);

            if (EnabledUsers.Contains(ev.Player.UserId))
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    if (ev.Player != null && ev.Player.IsAlive)
                        EnsureLight(ev.Player);
                });
            }
        }

        private static IEnumerator<float> FollowLoop()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(FollowTickSeconds);

                if (!_enabled)
                    yield break;

                // Copy enabled users to avoid modification during iteration
                var enabledNow = new List<string>(EnabledUsers);

                foreach (var userId in enabledNow)
                {
                    var p = ExPlayer.Get(userId);

                    if (p == null || !p.IsAlive)
                    {
                        // If dead/offline, remove toy but keep enabled state
                        RemoveLight(userId);
                        continue;
                    }

                    if (!LightsByUserId.TryGetValue(userId, out var light) || light == null)
                    {
                        EnsureLight(p);
                        continue;
                    }

                    light.Position = p.Position + GlowOffset;
                }

                // Remove orphan lights (no longer enabled)
                var orphanKeys = new List<string>();
                foreach (var kvp in LightsByUserId)
                {
                    if (!EnabledUsers.Contains(kvp.Key))
                        orphanKeys.Add(kvp.Key);
                }

                foreach (var id in orphanKeys)
                    RemoveLight(id);
            }
        }
    }
}
