using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.GUI.HTML;
using Grasshopper.Kernel.Types;
using Grasshopper.My.Resources;
using Rhino.Input;
using Rhino.Input.Custom;

namespace Grasshopper.Kernel.Parameters
{
	/// <exclude />
	// Token: 0x02000151 RID: 337
	public class Param_Guid : GH_PersistentParam<GH_Guid>
	{
		// Token: 0x060017A5 RID: 6053 RVA: 0x000796D8 File Offset: 0x000778D8
		public Param_Guid() : base(new GH_InstanceDescription("GH_EdgeFilter", "EdgeFilter", "Contains a collection of Globally Unique Identifiers", "Params", "Primitive"))
		{
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000796FE File Offset: 0x000778FE
		protected override GH_Guid InstantiateT()
		{
			return new GH_Guid();
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00079708 File Offset: 0x00077908
		protected override GH_GetterResult Prompt_Singular(ref GH_Guid value)
		{
			GetObject getObject = new GetObject();
			getObject.SetCommandPrompt("请选择边缘...");
			getObject.GeometryFilter = Rhino.DocObjects.ObjectType.EdgeFilter;
			getObject.EnablePreSelect(true, true);
			getObject.AcceptNothing(false);
			GetResult getResult = getObject.Get();
			GH_GetterResult result;
			if (getResult== GetResult.Object)
			{
				if (value == null)
				{
					value = new GH_Guid(getObject.Object(0).ObjectId);
				}
				else
				{
					value.Value = getObject.Object(0).ObjectId;
				}
				result = GH_GetterResult.success;
			}
			else
			{
				result = GH_GetterResult.cancel;
			}
			return result;
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00079774 File Offset: 0x00077974
		protected override GH_GetterResult Prompt_Plural(ref List<GH_Guid> value)
		{
			GetObject getObject = new GetObject();
			getObject.SetCommandPrompt("请选择边缘...");
			getObject.GeometryFilter = Rhino.DocObjects.ObjectType.EdgeFilter;
			getObject.EnablePreSelect(true, true);
			getObject.AcceptNothing(false);
			GetResult multiple = getObject.GetMultiple(1, 0);
			GH_GetterResult result;
			if (multiple == GetResult.Object)
			{
				if (value == null)
				{
					value = new List<GH_Guid>();
				}
				int num = getObject.ObjectCount - 1;
				for (int i = 0; i <= num; i++)
				{
					value.Add(new GH_Guid(getObject.Object(i).ObjectId));
				}
				result = GH_GetterResult.success;
			}
			else
			{
				result = GH_GetterResult.cancel;
			}
			return result;
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x000797F3 File Offset: 0x000779F3
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary | GH_Exposure.dropdown;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x000797FA File Offset: 0x000779FA
		protected override Bitmap Icon
		{
			get
			{
				return null;// Res_ObjectIcons.Param_Guid_24x24;
			}
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00079801 File Offset: 0x00077A01
		//protected internal override string HtmlHelp_Source()
		//{
		//	GH_HtmlFormatter gh_HtmlFormatter = new GH_HtmlFormatter(this);
		//	gh_HtmlFormatter.Title = "Guid parameter";
		//	gh_HtmlFormatter.Description = "Represents a collection of Guids. Guid parameters are capable of storing persistent data. You can set the persistent records through the parameter menu.";
		//	gh_HtmlFormatter.ContactURI = "https://discourse.mcneel.com/";
		//	gh_HtmlFormatter.AddRemark("Guid parameters can instantiate themselves from properly formatted Strings, referenced objects and -if all else fails- object names. If you supply a String which is not a valid ID format, Grasshopper will iterate over the current document and try to find the first object with a name that equals the given String.", GH_HtmlFormatterPalette.Black, GH_HtmlFormatterPalette.White);
		//	return gh_HtmlFormatter.HtmlFormat();
		//}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x0007983C File Offset: 0x00077A3C
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("{D1869364-0088-4A08-93D0-9E69D76FBA47}");
			}
		}
	}
}
