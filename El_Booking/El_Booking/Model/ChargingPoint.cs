using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
	public class ChargingPoint
	{
		private int _chargingPointID;

		public int CharginPointID
		{
			get { return _chargingPointID; }
			set { _chargingPointID = value; }
		}

		public ChargingPoint(int chargingPointID)
		{
			this.CharginPointID = chargingPointID;
		}
	}
}
