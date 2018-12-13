using System;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;


public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
            // create an instance of TcpClient 

            TcpClient tcpclient = new TcpClient();

            // HOST NAME POP SERVER and gmail uses port number 995 for POP 

            tcpclient.Connect("pop.gmail.com", 995);

            // This is Secure Stream // opened the connection between client and POP Server

            System.Net.Security.SslStream sslstream = new SslStream(tcpclient.GetStream());

            // authenticate as client  

            sslstream.AuthenticateAsClient("pop.gmail.com");

            //bool flag = sslstream.IsAuthenticated;   // check flag

            // Asssigned the writer to stream 

            System.IO.StreamWriter sw = new StreamWriter(sslstream);

            // Assigned reader to stream

            System.IO.StreamReader reader = new StreamReader(sslstream);

            // refer POP rfc command, there very few around 6-9 command

            sw.WriteLine("USER rijwan.ahmad0306@gmail.com");

            // sent to server

            sw.Flush(); sw.WriteLine("PASS 8121030306");
            sw.Flush();

            // RETR 1 will retrive your first email. it will read content of your first email

            sw.WriteLine("RETR 1");

            sw.Flush();
            // close the connection

            sw.WriteLine("Quit ");
            sw.Flush(); string str = string.Empty;
            string strTemp = string.Empty;
            while ((strTemp = reader.ReadLine()) != null)
            {
                // find the . character in line
                if (strTemp == ".")
                {
                    break;
                }
                if (strTemp.IndexOf("-ERR") != -1)
                {
                    break;
                }
                str += strTemp;
            }
            
              //  int pos = fileName.LastIndexOf(".");
                //string mainName = fileName.Substring(0, pos);
              //  string htmlName = mainName + ".htm";

               // string tempFolder = mainName;
                //if (!File.Exists(htmlName))
                //{
                //    // We haven't generate the html for this email, generate it now.
                // //   _GenerateHtmlForEmail(htmlName, fileName, tempFolder);
                //}

              Console.WriteLine("Please open {0} to browse your email");  
                 //   htmlName);
        
                    
    }

    
}