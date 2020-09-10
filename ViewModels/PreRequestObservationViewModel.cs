using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
		public class PreRequestObservationViewModel
		{
				public int id;
				public string observation;
				public UserViewModel user;
				public Nullable<DateTime> registrationDate;
		}
}