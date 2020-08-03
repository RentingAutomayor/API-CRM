using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
		public class PreRequestViewModel
		{
				public Nullable<int> id;
				public Nullable<DateTime> registrationDate;
				public PreClientViewModel preClient;
				public VehicleModelViewModel vehicleModel;
				public Nullable<bool> state;
				public StateViewModel stateRequest;
				public UserViewModel user;
				public CanalViewModel firstCanal;
				public CanalViewModel secondCanal;
		}  
}