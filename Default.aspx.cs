using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Web.Services;
using System.Data;

public partial class Default : System.Web.UI.Page
{
    DataTable dt = null;
    // ContactManager ContactManager1;

    // private AdminManager adminmgr;
    public static int CompanyID;
    public static int WorkAreaID;
    public static string UserID;
    public bool isValidUser;
    public static string popserver;
    public static string portno;
    public static string emailId;
    public static string Password;
    public static int noofemailforfetch;

    public static List<Email> Emails = new List<Email>();  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.Cookies["userid"] != null)

                txtEmailId.Text = Request.Cookies["userid"].Value;

            if (Request.Cookies["pwd"] != null)

                txtPassword.Attributes.Add("value", Request.Cookies["pwd"].Value);
            if (Request.Cookies["userid"] != null && Request.Cookies["pwd"] != null)
                rememberme.Checked = true;
        } 
    }

    private string getsortstring(string str)
    {
        string strimsub = "";
        if (str != "" && str != null)
        {
            if (str.Length > 50)
            {
                strimsub = str;
                strimsub = strimsub.Substring(0, 35) + "...";
            }
            else
            {
                strimsub = str;
            }
        }
        else
        {
            strimsub = str;
        }
        return strimsub;

    }

    public void BindData()
    {

        int noofemailshow = 0;
        if (txtnoofemail.Text.Trim() != "")
            noofemailshow = Convert.ToInt32(txtnoofemail.Text.Trim());
        else
            noofemailshow = 25;

        if (rememberme.Checked == true)
        {
            Response.Cookies["userid"].Value = txtEmailId.Text.Trim();
            Response.Cookies["pwd"].Value = txtPassword.Text.Trim();
            Response.Cookies["userid"].Expires = DateTime.Now.AddDays(15);
            Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(15);
        }

        noofemailforfetch = noofemailshow;
        popserver = txtpopserveraddress.Text.Trim();
        portno = txtport.Text.Trim();
        emailId  =txtEmailId.Text.Trim();
        Password = txtPassword.Text.Trim();

        Pop3Client pop3Client;
        pop3Client = new Pop3Client();
        pop3Client.Connect(popserver,Convert.ToInt32(portno), true);
        pop3Client.Authenticate(txtEmailId.Text.Trim(), txtPassword.Text.Trim(), AuthenticationMethod.UsernameAndPassword);

        dt = new DataTable("Inbox");
        dt.Columns.Add("counteremail", typeof(string));
        dt.Columns.Add("linkforcont", typeof(string));
        dt.Columns.Add("Sender", typeof(string));
        dt.Columns.Add("Emails", typeof(string));
        dt.Columns.Add("Subject", typeof(string));
        dt.Columns.Add("Date", typeof(DateTime));
        dt.Columns.Add("Reply", typeof(string));
        dt.Columns.Add("Replyall", typeof(string));
        dt.Columns.Add("Forward", typeof(string));

        int count = pop3Client.GetMessageCount();
        int counter = 0;
        if (count <= noofemailshow)
            noofemailshow = 0;
        int noofemail = 0;
        int noofemailtoday = 0;

        for (counter = count; counter >= count - noofemailshow; counter--)
        {
            Message message = pop3Client.GetMessage(counter); 
            noofemail++;
            string Reply = "";
            string ReplyAll = "";
            string linforcontact = "";
            string Forward = "";
            string Subject = "";
            string strimsub = "";
            strimsub = getsortstring(message.Headers.Subject);
            Reply = string.Format("<a href='#stack2' class='btn' style='color:white' data-toggle='modal'><input type='button' class='btncontact' value='{0}' /></a>", "Reply", message.Headers.From.MailAddress);
            ReplyAll = string.Format("<a href = 'mailto:{1},{2}' class='btn' style='color:white'>{0}</a>", "Reply to All", message.Headers.From.MailAddress, message.Headers.Cc);
            Forward = string.Format("<a href = 'mailto:{1}' class='btn' style='color:white'>{0}</a>", "Forward", "");
            if (message.Headers.DateSent.ToString("MM/dd/yyyy") != DateTime.Now.ToString("MM/dd/yyyy"))
                Subject = string.Format("<span style='color:#000000;font-weight:bold' class='checkreademail'>{0}</span><span style='float:right;color:#000000;' class='checkreademail''>{1}</span><br><span style='color:#999999'>{2}</span>", message.Headers.From.DisplayName, message.Headers.DateSent.ToShortDateString(), strimsub);
            else
            {
                Subject = string.Format("<span style='color:#000000;font-weight:bold' class='checkreademail''>{0}</span><span style='float:right;color:#000000;' class='checkreademail''>{1}</span><br><span style='color:#999999'>{2}</span>", message.Headers.From.DisplayName, message.Headers.DateSent.ToLongTimeString(), strimsub);
                noofemailtoday++;
            }
            linforcontact = checkcontact(message.Headers.From.Address);
            dt.Rows.Add(new object[] { counter, linforcontact, message.Headers.Sender, message.Headers.From.MailAddress, Subject, message.Headers.DateSent.ToLongDateString() + " " + message.Headers.DateSent.ToLongTimeString(), Reply, ReplyAll, Forward });

        }
        noofmailtoday.InnerText = noofemailtoday.ToString();

        EnumerableRowCollection<DataRow> rows = null;
        //rows = (from row in dt.AsEnumerable()
        //        where row.Field<DateTime>("Date") > DateTime.Now.AddDays(-30)
        //        orderby row["Date"] descending
        //        select row);
        //dt = rows.AsDataView().ToTable();
        gvEmail.DataSource = dt;
        gvEmail.DataBind();
        gvEmail.Columns[0].HeaderText = "Inbox";
        ViewState["Paging"] = dt;
    }

    

    //public void Outbox()
    //{
    //    OutLook._Application _app = new OutLook.Application();
    //    OutLook._NameSpace _ns = _app.GetNamespace("MAPI");
    //    OutLook.MAPIFolder outbox = _ns.GetDefaultFolder(OutLook.OlDefaultFolders.olFolderSentMail);
    //    _ns.SendAndReceive(true);

    //    dt = new DataTable("Inbox");
    //    dt.Columns.Add("counteremail", typeof(string));
    //    dt.Columns.Add("linkforcont", typeof(string));
    //    dt.Columns.Add("Sender", typeof(string));
    //    dt.Columns.Add("Emails", typeof(string));
    //    dt.Columns.Add("Subject", typeof(string));
    //    dt.Columns.Add("Date", typeof(DateTime));
    //    dt.Columns.Add("Reply", typeof(string));
    //    dt.Columns.Add("Replyall", typeof(string));
    //    dt.Columns.Add("Forward", typeof(string));
    //    // dt.Columns.Add("Body", typeof(string));


    //    int counter = 0;

    //    int noofemail = 0;
    //    for (counter = outbox.Items.Count; counter >= 1; counter--)
    //    {
    //        noofemail++;
    //        OutLook.MailItem item = (OutLook.MailItem)outbox.Items[counter];
    //        if (item.SenderEmailAddress.Contains("@twitter.com"))
    //            continue;
    //        string Reply = "";
    //        string ReplyAll = "";
    //        string linforcontact = "";

    //        string Forward = "";
    //        string Subject = "";
    //        string strimsub = "";
    //        if (item.Subject != null)
    //        {
    //            if (item.Subject.Length > 50)
    //            {
    //                strimsub = item.Subject;
    //                strimsub = strimsub.Substring(0, 35) + "...";
    //            }
    //            else
    //            {
    //                strimsub = item.Subject;
    //            }
    //        }
    //        else
    //        {
    //            strimsub = item.Subject;
    //        }
    //        Reply = string.Format("<a href='#stack2' class='btn' style='color:white' data-toggle='modal'><input type='button' class='btncontact' value='{0}' /></a>", "Reply", item.SenderEmailAddress);
    //        ReplyAll = string.Format("<a href = 'mailto:{1},{2}' class='btn' style='color:white'>{0}</a>", "ReplyAll", item.SenderEmailAddress, item.CC);
    //        Forward = string.Format("<a href = 'mailto:{1}' class='btn' style='color:white'>{0}</a>", "Forward", "");
    //        if (item.SentOn.ToString("MM/dd/yyyy") != DateTime.Now.ToString("MM/dd/yyyy"))
    //            Subject = string.Format("<span style='color:#000000'>{0}</span><span style='float:right;color:#000000'>{1}</span><br><span style='color:#999999'>{2}</span>", item.SenderName, item.SentOn.ToShortDateString(), strimsub);
    //        else
    //            Subject = string.Format("<span style='color:#000000'>{0}</span><span style='float:right;color:#000000'>{1}</span><br><span style='color:#999999'>{2}</span>", item.SenderName, item.SentOn.ToLongTimeString(), strimsub);

    //        linforcontact = checkcontact(item.SenderEmailAddress);


    //        dt.Rows.Add(new object[] { counter, linforcontact, item.SenderName, item.SenderEmailAddress, Subject, item.SentOn.ToLongDateString() + " " + item.SentOn.ToLongTimeString(), Reply, ReplyAll, Forward });
    //        //if (noofemail == 1000)
    //        //{
    //        //    break;
    //        //}
    //    }
    //    EnumerableRowCollection<DataRow> rows = null;
    //    rows = (from row in dt.AsEnumerable()
    //            where row.Field<DateTime>("Date") > DateTime.Now.AddDays(-30)
    //            orderby row["Date"] descending
    //            select row);

    //    dt.Rows.Add(rows);

    //    gvEmail.DataSource = dt;
    //    gvEmail.Columns[0].HeaderText = "Outbox";
    //    gvEmail.DataBind();
    //    //gvEmail.Columns[0].HeaderText = "";
    //}

    [WebMethod]
    public static string GetbodyData(int count)
    {
        
        if (HttpContext.Current != null)
        {
            Page page = (Page)HttpContext.Current.Handler;
            //item.UnRead = false;
            Pop3Client pop3Client;
            pop3Client = new Pop3Client();
            pop3Client.Connect("pop.gmail.com", 995, true);
            pop3Client.Authenticate(emailId, Password, AuthenticationMethod.UsernameAndPassword);
              
            Message message = pop3Client.GetMessage(count);
            MessagePart body = message.FindFirstHtmlVersion();

            //-----------------------------------------------------------
            
             
            List<MessagePart> attachments = message.FindAllAttachments();
            Email email = new Email();
            foreach (MessagePart attachment in attachments)
            {
                email.Attachments.Add(new Attachment
                {
                    FileName = attachment.FileName + "_" + count ,
                    ContentType = attachment.ContentType.MediaType,
                    Content = attachment.Body 
                    //messegecount = 
                });
            }
                            
                Emails.Add(email);

            if (body != null)
            {
                return body.GetBodyAsText();
            }
            else
            {
                body = message.FindFirstPlainTextVersion();
                if (body != null)
                {
                    return body.GetBodyAsText();
                }
            }
            
        }
        return "";
    }
    public void loadattachment(int count)
    {
        List<Attachment> attachments = Emails.Where(email => email.MessageNumber == count).FirstOrDefault().Attachments;
        
    }

    protected void Download(object sender, EventArgs e)
    {
         
        LinkButton lnkAttachment = (sender as LinkButton);
        string name = lnkAttachment.Text;
        string[] str = name.Split('_');
        Repeater row = (lnkAttachment.Parent.Parent.NamingContainer as Repeater);
        List<Attachment> attachments = Emails.Where(email => email.MessageNumber == Convert.ToInt32(str[1])).FirstOrDefault().Attachments;
        Attachment attachment = attachments.Where(a => a.FileName == lnkAttachment.Text).FirstOrDefault();
        Response.AddHeader("content-disposition", "attachment;filename=" + attachment.FileName);
        Response.ContentType = attachment.ContentType;
        Response.BinaryWrite(attachment.Content);
        Response.End();
    }

   /*
    [WebMethod]
    public static string btnSave(AddContacts obj)
    {
        string contactid = "";
        try
        {
            AddContacts Contact = new AddContacts();
            Contact.Name = obj.Name;    //this is contact pertion name
            if (obj.ContactName == "")
                Contact.ContactName = obj.Name;
            else
                Contact.ContactName = obj.ContactName; //this is company name

            Contact.EmailId = obj.EmailId;
            Contact.Country = obj.Country;

            Contact.CompanyID = CompanyID;
            Contact.CreatedBy = UserID;
            Contact.CreatedDate = DateTime.Now;
            ContactManager objCntMgr = new ContactManager();

            contactid = objCntMgr.CreateContactV2(Contact, 100);
                
        }
        catch (Exception ex)
        {
            contactid = ex.Message;
        }
        return contactid;
    }
    [WebMethod]
    public static string btnSaveEmail(string count)
    {
        OutLook._Application _app = new OutLook.Application();
        OutLook._NameSpace _ns = _app.GetNamespace("MAPI");
        OutLook.MAPIFolder inbox = _ns.GetDefaultFolder(OutLook.OlDefaultFolders.olFolderInbox);
        _ns.SendAndReceive(false);
        OutLook.MailItem item = (OutLook.MailItem)inbox.Items[Convert.ToInt64(count)];
            
        string contactid = "";
        CRMManager ObjCRMManager = new CRMManager();
        string emailrefid = "";
        try
        {
            EmailClass eml = new EmailClass();
            eml.EmlRefNo = FillEmailRefNo(GetCurrentFinancialYear(item.SentOn));
            eml.EmlDate = item.SentOn;
                
            eml.Subject =item.Subject;
            eml.EmlSmtpName = item.SenderEmailType ;
            // eml.EmlSSL = 1;
            eml.SenderEmail = item.SenderEmailAddress;

            eml.EmlTo = item.To; ;
            eml.EmlCC = item.CC;
            eml.EmlBCC = item.BCC;
            eml.Subject = item.Subject;
            eml.Body = item.Body;
           // eml.SentStatus = Convert.ToInt16(item.Sent);

            eml.CreatedBy = UserID;
            eml.CreatedDate = System.DateTime.Now;
            eml.CompanyID = CompanyID;
            eml.WorkAreaID = WorkAreaID;

            string strFlag = ObjCRMManager.InsertEmail(eml);

        }
        catch (Exception ex)
        {
            contactid = ex.Message;
        }
        return contactid;
    }

    //set email for sending data to for backoffice
    [WebMethod]
    public static void BackofficeEmailDetails(string count)
    {
        OutLook._Application _app = new OutLook.Application();
        OutLook._NameSpace _ns = _app.GetNamespace("MAPI");
        OutLook.MAPIFolder inbox = _ns.GetDefaultFolder(OutLook.OlDefaultFolders.olFolderInbox);
        _ns.SendAndReceive(false);
        OutLook.MailItem item = (OutLook.MailItem)inbox.Items[Convert.ToInt64(count)];

        string contactid = "";
        CRMManager ObjCRMManager = new CRMManager();
            
        try
        {
            EmailClass eml = new EmailClass();
            eml.EmlRefNo = FillEmailRefNo(GetCurrentFinancialYear(item.SentOn));
            eml.EmlDate = item.SentOn;

            eml.Subject = item.Subject;
            eml.EmlSmtpName = item.SenderEmailType;
            // eml.EmlSSL = 1;
            eml.SenderEmail = item.SenderEmailAddress;

            eml.EmlTo = item.To; ;
            eml.EmlCC = item.CC;
            eml.EmlBCC = item.BCC;
            eml.Subject = item.Subject;
            eml.Body = item.Body;
            // eml.SentStatus = Convert.ToInt16(item.Sent);

            eml.CreatedBy = UserID;
            eml.CreatedDate = System.DateTime.Now;
            eml.CompanyID = CompanyID;
            eml.WorkAreaID = WorkAreaID;

            //string strFlag = ObjCRMManager.InsertEmail(eml);
            SetCatcheDetails("EmailDetals", eml);
        }
        catch (Exception ex)
        {
            contactid = ex.Message;
        }
            
    }

        
    */

    [WebMethod]
    public static string checkcontact(string emailid)
    {
        string returnval = "";
        //AddContacts Contact = new AddContacts();
        //Contact.EmailId = emailid;
        //Contact.CompanyID = CompanyID;
        //ContactManager objCntMgr = new ContactManager();

        string contactid = "11";// objCntMgr.CreateContactV2(Contact, 101);
        if (contactid == "11")
            returnval = "#stack2";
        else
            returnval = "BackOffice.aspx";

        return returnval;
    }

    protected void gvEmail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt = (DataTable)ViewState["Paging"];
        gvEmail.DataSource = dt;
        gvEmail.PageIndex = e.NewPageIndex;
        gvEmail.DataBind();
    }
    //save email in tble email table

    /*  private static string FillEmailRefNo(string finatialyear)
      {
          CRMManager ObjCRMManager = new CRMManager();
          AdminManager adminmgr = new AdminManager();
          string emairefid = "";
          try
          {
              string strLtrId = ObjCRMManager.GetNextEmailId(Convert.ToInt32(WorkAreaID));

              string workAreaCode = "";
              DataTable dtWorkArea = adminmgr.GetWorkAreaforSelect(WorkAreaID);
              workAreaCode = dtWorkArea.Rows[0]["WorkAreaCode"].ToString();

              if (Convert.ToInt32(strLtrId) < 10)
              {
                  strLtrId = "0" + strLtrId.ToString();
              }

              emairefid = workAreaCode + "/" + finatialyear + "/EML/" + strLtrId;
          }
          catch (Exception ex)
          {
              ExceptionHandler.WriteException(ex);
                 
              return "";
          }
          return emairefid;
      }   

      private void insertEmail()
      {
          string emailrefid = "";
          try
          {
               
          }
          catch (Exception ex)
          {
              //lblResult.ForeColor = System.Drawing.Color.Red;
              //lblResult.Text = lblResult.Text + ". Error while saving details: " + ex.Message;
          }
      }
       */

    public static string GetCurrentFinancialYear(DateTime dateofemail)
    {

        int CurrentYear = dateofemail.Year;
        int PreviousYear = dateofemail.Year - 1;
        int NextYear = dateofemail.Year + 1;
        string PreYear = PreviousYear.ToString();
        string NexYear = NextYear.ToString();
        string CurYear = CurrentYear.ToString();
        string FinYear = string.Empty;

        if (dateofemail.Month > 3)
            FinYear = CurYear + "-" + NexYear;
        else
            FinYear = PreYear + "-" + CurYear;
        return FinYear.Trim();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
    [Serializable]
    public class Email
    {
        public Email()
        {
            this.Attachments = new List<Attachment>();
        }
        public int MessageNumber { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
    [Serializable]
    public class Attachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public int messegecount { get; set; }
    }
     
}