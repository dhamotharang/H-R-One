using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class ESS_home : HROneWebPage
{

    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;
 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;

        binding = new Binding(dbConn, db);
        binding.add(EmpID);
       // binding.add(EmpNo);

        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);



        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
        }


        if (!Page.IsPostBack)
        {

           // view = loadData(info, AVCPlanDetaildb, Repeater); //add by Ben

           if (CurID > 0)
            {
                loadObject();
              
                // Delete.Visible = true;
            }
            else
            {
                 
            }
              //  Delete.Visible = false;
            
        }

    }
    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

   






    /* protected DBManager db = EEmpPersonalInfo.db;
     protected SearchBinding binding;
     protected ListInfo info;
     protected DataView view;

     public int CurID = -1;
     protected void Page_Load(object sender, EventArgs e)
     {


         if (!WebUtils.CheckAccess(Response, Session))
             return;
         else
         {
             binding = new SearchBinding(db);
           //  binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
         
             //EmpNo2.Text = Request.QueryString["EmpNo1"].ToString();
         }


          info = new ListInfo();
          view = loadData(info, db, repeater1);


     }
     public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
     {
         DBFilter filter = binding.createFilter();

         if (info != null && info.orderby != null && !info.orderby.Equals(""))
             filter.add(info.orderby, info.order);

         string select = "c.*";
         string from = "from [" + db.dbclass.tableName + "] c ";
       

         DataTable table = filter.loadData(dbConn, info, select, from);
        
         view = new DataView(table);
         if (info != null)
         {
            
         }
         if (repeater != null)
         {
             repeater.DataSource = view;
             repeater.DataBind();
         }

         return view;
     }
    
    
              <asp:Repeater id="repeater1" runat="server" >
             <ItemTemplate>
             <tr>
             <td>
             <a href="ESS_EmpView.aspx?EmpNo=<%#binding.getValue(Container.DataItem,"EmpNo")%>">
             <%#binding.getValue(Container.DataItem,"EmpNo")%>
             </a>
             <asp:Label Text="123123213!#" runat="server" />
             </td>
             </tr>
             </ItemTemplate>
             </asp:Repeater>
     */

}
