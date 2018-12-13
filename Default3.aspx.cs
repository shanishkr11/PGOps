using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Data;

 

public partial class Default3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = new DataTable("Inbox");
            dt.Columns.Add("Subject", typeof(string));
            dt.Columns.Add("summary", typeof(string));
            dt.Columns.Add("modified", typeof(string));

            System.Net.WebClient objClient = new System.Net.WebClient();
            string response;
            string title;
            string summary;
            string nr = "";
            string modified = "";
            //Creating a new xml document
            XmlDocument doc = new XmlDocument();

            //Logging in Gmail server to get data
            objClient.Credentials = new System.Net.NetworkCredential("rijwan.ahmad0306@gmail.com", "8121030306");
            //reading data and converting to string
            response = Encoding.UTF8.GetString(
                       objClient.DownloadData(@"https://mail.google.com/mail/feed/atom"));

            response = response.Replace(@"<feed version=""0.3"" xmlns=""http://purl.org/atom/ns#"">", @"<feed>");

            //loading into an XML so we can get information easily
            doc.LoadXml(response);

            //nr of emails
            nr = doc.SelectSingleNode(@"/feed/fullcount").InnerText;

            //Reading the title and the summary for every email
            foreach (XmlNode node in doc.SelectNodes(@"/feed/entry"))
            {
                title = node.SelectSingleNode("title").InnerText;
                summary = node.SelectSingleNode("author/name").InnerText;
               // modified = node.SelectSingleNode("modified").InnerText;
               // modified = node.SelectSingleNode("modified").InnerText;
                modified = node.SelectSingleNode("modified").InnerText;
                dt.Rows.Add(new object[] { title, summary, modified.Substring(0,10) });

            }
            gvEmail.DataSource = dt;
            gvEmail.DataBind();
        }
        catch (Exception exe)
        {
            //MessageBox.Show("Check your network connection");
        }
    }
}
