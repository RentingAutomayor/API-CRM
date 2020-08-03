using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
		public class PreClientViewModel:PersonViewModel
		{
				public Nullable<int> idPreClient;
				public Nullable<DateTime> registrationDate;
				public UserViewModel user;
		}
}