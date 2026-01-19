using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.Features;
using Exiled.Events.Handlers;
using MEC;

namespace Patreon_Perks
{
	// Token: 0x02000003 RID: 3
	internal class Main : Plugin<Config>
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020B9 File Offset: 0x000002B9
		public override string Author
		{
			get
			{
				return "ClaudioPanConQueso";
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020C0 File Offset: 0x000002C0
		public override string Name
		{
			get
			{
				return "Patreon Perks";
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020C7 File Offset: 0x000002C7
		public override Version Version
		{
			get
			{
				return new Version(1, 0, 0);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020D1 File Offset: 0x000002D1
		public override Version RequiredExiledVersion
		{
			get
			{
				return new Version(8, 5, 0);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000020DB File Offset: 0x000002DB
		public override void OnEnabled()
		{
			Main.Singleton = this;
			this.SubServerEvents();
			base.OnEnabled();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000020EF File Offset: 0x000002EF
		public override void OnDisabled()
		{
			this.UnSubServerEvents();
			Main.Singleton = null;
			base.OnDisabled();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002103 File Offset: 0x00000303
		public void SubServerEvents()
		{
			Server.RestartingRound += new CustomEventHandler(this.OnRestart);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002120 File Offset: 0x00000320
		public void UnSubServerEvents()
		{
			Server.RestartingRound -= new CustomEventHandler(this.OnRestart);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002140 File Offset: 0x00000340
		public void OnRestart()
		{
			try
			{
				this.RgbUses.Clear();
			}
			catch (Exception)
			{
			}
			try
			{
				this.ScreamUses.Clear();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000005 RID: 5
		public static Main Singleton;

		// Token: 0x04000006 RID: 6
		public List<string> ScreamUses = new List<string>();

		// Token: 0x04000007 RID: 7
		public CoroutineHandle RgbCoroutine;

		// Token: 0x04000008 RID: 8
		public List<string> RgbUses = new List<string>();
	}
}
