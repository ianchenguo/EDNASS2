﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ENETCare.Business;

namespace ENETCare.GUI.Mockup
{
	public partial class ReceivingMockup : System.Web.UI.Page
	{
		private MedicationPackageBLL medicationPackageBLL;

		protected void Page_Load(object sender, EventArgs e)
		{
			medicationPackageBLL = new MedicationPackageBLL("LoginUserName");
		}

		protected void ReceiveButton_Click(object sender, EventArgs e)
		{
			string barcode = BarcodeTextBox.Text;
			try
			{
				medicationPackageBLL.ReceivePackage(barcode);
				Response.Redirect("IndexMockup.aspx");
			}
			catch (ENETCareException ex)
			{
				Response.Write(string.Format("<p>Error: {0}</p>\n", ex.Message));
			}
		}
	}
}