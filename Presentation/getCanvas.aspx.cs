/*!
* DisplayMonkey source file
* http://displaymonkey.org
*
* Copyright (c) 2015 Fuel9 LLC and contributors
*
* Released under the MIT license:
* http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Diagnostics;

namespace DisplayMonkey
{
	public partial class getCanvas : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// get host from URL first
				int displayId = Request.IntOrZero("display");

				try
				{
					Canvas canvas = Canvas.InitFromDisplay(displayId);

					HttpContext.Current.Response.Cookies["DisplayMonkey"]["DisplayID"] = displayId.ToString();
					HttpContext.Current.Response.Cookies["DisplayMonkey"].Expires = new DateTime(2038, 1, 1);

					// assemble page
					this.lTitle.Text = canvas.Name;
					this.lHead.Text = canvas.Head;
					this.lContent.Text = canvas.Body;
				}

				catch (Exception ex)
				{
					// back to display selection
					this.lContent.Text = Server.HtmlEncode(ex.ToString()).Replace("\n", "<br />");
				}
			}
		}
	}
}