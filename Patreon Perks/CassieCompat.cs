using System;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;

namespace Patreon_Perks.Utils
{
    public static class CassieCompat
    {
        public static void Announce(string text, bool held = false, bool noisy = false, bool subtitles = false)
        {
            // Try to find the Cassie type regardless of whether it is:
            // - Exiled.API.Features.Cassie (class)
            // - Exiled.API.Features.Cassie.Cassie (nested class)
            Type cassieType =
                FindType("Exiled.API.Features.Cassie") ??
                FindType("Exiled.API.Features.Cassie.Cassie");

            if (cassieType == null)
            {
                Log.Warn("CassieCompat: Could not find Cassie type. Check your Exiled/ExMod references.");
                return;
            }

            // 1) MessageTranslated(string, string, bool, bool, bool)
            MethodInfo mt = cassieType.GetMethod(
                "MessageTranslated",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string), typeof(string), typeof(bool), typeof(bool), typeof(bool) },
                null);

            if (mt != null)
            {
                mt.Invoke(null, new object[] { text, text, held, noisy, subtitles });
                return;
            }

            // 2) Message(string, bool, bool, bool)
            MethodInfo m = cassieType.GetMethod(
                "Message",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string), typeof(bool), typeof(bool), typeof(bool) },
                null);

            if (m != null)
            {
                m.Invoke(null, new object[] { text, held, noisy, subtitles });
                return;
            }

            // 3) Send(string)
            MethodInfo s = cassieType.GetMethod(
                "Send",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string) },
                null);

            if (s != null)
            {
                s.Invoke(null, new object[] { text });
                return;
            }

            Log.Warn("CassieCompat: No supported Cassie method found (MessageTranslated/Message/Send).");
        }

        private static Type FindType(string fullName)
        {
            try
            {
                // Search loaded assemblies so we don't need to know the exact assembly name (Exiled vs ExMod)
                return AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Select(a => a.GetType(fullName, throwOnError: false, ignoreCase: false))
                    .FirstOrDefault(t => t != null);
            }
            catch
            {
                return null;
            }
        }
    }
}
