using System;
using Rhino.DocObjects;

namespace Grasshopper.Kernel.Types
{
	/// <summary>
	/// Allows for improved tracking of referenced geometry. Note that this is
	/// an internal interface and not part of the public SDK
	/// </summary>
	// Token: 0x020000FD RID: 253
	internal interface IGH_GeometricGooWithObjRef : IGH_GeometricGoo
	{
		/// <summary>
		/// Get an ObjRef for this IGH_GeometryicGoo
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600140F RID: 5135
		ObjRef GetObjRef();
	}
}