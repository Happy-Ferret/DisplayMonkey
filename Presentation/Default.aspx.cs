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
using System.Data;
using System.Text;

namespace DisplayMonkey
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// get host from URL first
				string theHost = Request.QueryString["host"];

				// otherwise, get display address
				if (string.IsNullOrEmpty(theHost))
				{
					theHost = Request.ServerVariables["REMOTE_HOST"];
					if (string.IsNullOrEmpty(theHost))
					{
						theHost = Request.ServerVariables["REMOTE_ADDR"];
					}
				}

				if (Request.Cookies["DisplayMonkey"] != null)
				{
					int displayID = Int32.MinValue;
					if (Request.Cookies["DisplayMonkey"]["DisplayID"] != null)
					{
						if (Int32.TryParse(Request.Cookies["DisplayMonkey"]["DisplayID"], out displayID))
						{
							Display theDisplay = new Display(displayID)
							{
								Host = theHost
							};
							if (theDisplay.Register())
							{
								RedirectToDisplay(theDisplay);
							}
						}
					}
				}

				try
				{
					// list registered displays
					int i = 0;
					StringBuilder html = new StringBuilder();

					foreach (Display display in Display.List)
					{
						string url = string.Format("getCanvas.aspx?display={0}", display.DisplayId);

#if !DEBUG //TODO: cookies
						if (theHost != "::1" && display.Host == theHost && display.CanvasId > 0)
#else
						if (display.Host == theHost && display.CanvasId > 0)
#endif
						{
							RedirectToDisplay(display);
						}

						html.AppendFormat(
							"<li><a href=\"{0}\">{1}</a></li>",
							url,
							Server.HtmlEncode(display.Name)
							);

						if (i % 4 != 0)
							html.Append("<br>");
					}

					labelDisplays.Text = html.ToString();
				}

				catch (Exception ex)
				{
					Response.Write(ex.Message); // TODO: error label
				}
			}
		}

		private static void RedirectToDisplay(Display display)
		{
			string url = string.Format("getCanvas.aspx?display={0}", display.DisplayId);

			// this host is already registered -> take us to it
			HttpContext.Current.Response.Redirect(string.Format(
				url,
				display.DisplayId
				));
		}
	}
}
