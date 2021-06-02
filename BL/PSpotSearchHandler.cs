using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
	public class PSpotSearchHandler
	{
		public int Code;
		public bool? SizeOpt;
		public double? PreferableWidth;
		public double? PreferableLength;
		public bool? RoofOpt;
		public double? MinPrice;
		public double? MaxPrice;
		public string Place_Id;
		public string Address;
		public Dictionary<int, List<Hours>> Hours;

	}
}
