//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;
//using GH_IO.Serialization;
//using Grasshopper.Getters;
//using Grasshopper.GUI.HTML;
//using Grasshopper.Kernel.Types;
//using Grasshopper.My.Resources;
//using Rhino;
//using Rhino.DocObjects;
//using Rhino.Geometry;
//using Grasshopper;
//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Types;

//namespace star
//{
//	/// <exclude />
//	// Token: 0x02000180 RID: 384
//	public class Param_EdgeFilter : GH_PersistentParam<GH_EdgeFilter>//, IGH_BakeAwareObject, IGH_PreviewObject
//	{
//		// Token: 0x060018FD RID: 6397 RVA: 0x0007BDDB File Offset: 0x00079FDB
//		public Param_EdgeFilter() : base(new GH_InstanceDescription("Curve", "Crv", "Contains a collection of generic curves", "Params", "Geometry"))
//		{
//			this.m_reparameterize = false;
//			this.m_hidden = false;
//		}

//		// Token: 0x060018FE RID: 6398 RVA: 0x0007BE0F File Offset: 0x0007A00F
//		protected override GH_EdgeFilter InstantiateT()
//		{
//			return new GH_EdgeFilter();
//		}

//		// Token: 0x060018FF RID: 6399 RVA: 0x0007BE18 File Offset: 0x0007A018
//		protected override GH_EdgeFilter PreferredCast(object data)
//		{
//			GH_EdgeFilter result;
//			if (data is Curve)
//			{
//				result = new GH_EdgeFilter((Curve)data);
//			}
//			else
//			{
//				result = null;
//			}
//			return result;
//		}

//		// Token: 0x06001900 RID: 6400 RVA: 0x0007BE40 File Offset: 0x0007A040
//		protected override GH_GetterResult Prompt_Singular(ref GH_EdgeFilter value)
//		{
//			value = GH_EdgeFilter.GetEdgeFilter();
//			GH_GetterResult result;
//			if (value != null)
//			{
//				result = GH_GetterResult.success;
//			}
//			else
//			{
//				result = GH_GetterResult.cancel;
//			}
//			return result;
//		}

//		// Token: 0x06001901 RID: 6401 RVA: 0x0007BE60 File Offset: 0x0007A060
//		protected override GH_GetterResult Prompt_Plural(ref List<GH_EdgeFilter> value)
//		{
//			value = GH_EdgeFilter.GetEdgeFilters();
//			GH_GetterResult result;
//			if (value != null)
//			{
//				result = GH_GetterResult.success;
//			}
//			else
//			{
//				result = GH_GetterResult.cancel;
//			}
//			return result;
//		}

//		// Token: 0x17000937 RID: 2359
//		// (get) Token: 0x06001902 RID: 6402 RVA: 0x0007BE7F File Offset: 0x0007A07F
//		public override string TypeName
//		{
//			get
//			{
//				return "Curve";
//			}
//		}

//		// Token: 0x17000938 RID: 2360
//		// (get) Token: 0x06001903 RID: 6403 RVA: 0x00009338 File Offset: 0x00007538
//		public override GH_Exposure Exposure
//		{
//			get
//			{
//				return GH_Exposure.secondary;
//			}
//		}

//		// Token: 0x17000939 RID: 2361
//		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0007BE86 File Offset: 0x0007A086
//		protected override Bitmap Icon
//		{
//			get
//			{
//				return null;
//			}
//		}

//		// Token: 0x06001905 RID: 6405 RVA: 0x0007BE8D File Offset: 0x0007A08D
//		public override void RemoveEffects()
//		{
//			base.RemoveEffects();
//			this.m_reparameterize = false;
//		}

//		// Token: 0x1700093A RID: 2362
//		// (get) Token: 0x06001906 RID: 6406 RVA: 0x0007BE9C File Offset: 0x0007A09C
//		public override GH_StateTagList StateTags
//		{
//			get
//			{
//				GH_StateTagList stateTags = base.StateTags;
//				if (this.m_reparameterize)
//				{
//					//stateTags.Add(new GH_StateTag_Reparameterize());
//				}
//				return stateTags;
//			}
//		}


//		// Token: 0x1700093B RID: 2363
//		// (get) Token: 0x0600190B RID: 6411 RVA: 0x0007C190 File Offset: 0x0007A390
//		// (set) Token: 0x0600190C RID: 6412 RVA: 0x0007C198 File Offset: 0x0007A398
//		public bool Hidden
//		{
//			get
//			{
//				return this.m_hidden;
//			}
//			set
//			{
//				this.m_hidden = value;
//			}
//		}

//		// Token: 0x1700093C RID: 2364
//		// (get) Token: 0x0600190D RID: 6413 RVA: 0x0000904D File Offset: 0x0000724D
//		public bool IsPreviewCapable
//		{
//			get
//			{
//				return true;
//			}
//		}

//		// Token: 0x1700093D RID: 2365
//		// (get) Token: 0x0600190E RID: 6414 RVA: 0x0007C1A1 File Offset: 0x0007A3A1
//		public BoundingBox ClippingBox
//		{
//			get
//			{
//				return base.Preview_ComputeClippingBox();
//			}
//		}

//		// Token: 0x0600190F RID: 6415 RVA: 0x00009347 File Offset: 0x00007547
//		public void DrawViewportMeshes(IGH_PreviewArgs args)
//		{
//		}

//		// Token: 0x06001910 RID: 6416 RVA: 0x0007C1A9 File Offset: 0x0007A3A9
//		public void DrawViewportWires(IGH_PreviewArgs args)
//		{
//			base.Preview_DrawWires(args);
//		}

//		// Token: 0x1700093E RID: 2366
//		// (get) Token: 0x06001911 RID: 6417 RVA: 0x0007C1B2 File Offset: 0x0007A3B2
//		public bool IsBakeCapable
//		{
//			get
//			{
//				return !this.m_data.IsEmpty;
//			}
//		}

//		// Token: 0x06001912 RID: 6418 RVA: 0x0007C1C2 File Offset: 0x0007A3C2
//		public void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
//		{
//			this.BakeGeometry(doc, null, obj_ids);
//		}

//		// Token: 0x06001913 RID: 6419 RVA: 0x0007C1D0 File Offset: 0x0007A3D0
//		public void BakeGeometry(RhinoDoc doc, ObjectAttributes att, List<Guid> obj_ids)
//		{
//			GH_BakeUtility gh_BakeUtility = new GH_BakeUtility(base.OnPingDocument());
//			gh_BakeUtility.BakeObjects(this.m_data, att, doc);
//			obj_ids.AddRange(gh_BakeUtility.BakedIds);
//		}

//		// Token: 0x1700093F RID: 2367
//		// (get) Token: 0x06001914 RID: 6420 RVA: 0x0007C204 File Offset: 0x0007A404
//		public override Guid ComponentGuid
//		{
//			get
//			{
//				return new Guid("{D5967B9F-E8EE-436b-A8AD-29FDC32232D5}");
//			}
//		}

//		// Token: 0x06001915 RID: 6421 RVA: 0x0007C210 File Offset: 0x0007A410
//		public override bool Write(GH_IWriter writer)
//		{
//			bool result = base.Write(writer);
//			if (this.m_reparameterize)
//			{
//				writer.SetBoolean("Reparameterize", this.m_reparameterize);
//			}
//			return result;
//		}

//		// Token: 0x06001916 RID: 6422 RVA: 0x0007C234 File Offset: 0x0007A434
//		public override bool Read(GH_IReader reader)
//		{
//			bool result = base.Read(reader);
//			this.m_reparameterize = false;
//			if (reader.ItemExists("UnitizeStream"))
//			{
//				this.m_reparameterize = reader.GetBoolean("UnitizeStream");
//				return result;
//			}
//			reader.TryGetBoolean("Reparameterize", ref this.m_reparameterize);
//			return result;
//		}

//		// Token: 0x040004AB RID: 1195
//		private bool m_reparameterize;

//		// Token: 0x040004AC RID: 1196
//		private bool m_hidden;
//	}
//}
