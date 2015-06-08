var btnCount = 10;
var staCount = 3;
var btnImages = new Array();
function btnMouseOut(img)
{
	document.images[img].src = btnImages[img.substring(img.indexOf('mbtn')+4,img.length)][0].src;
};
function btnMouseOver(img)
{
	document.images[img].src = btnImages[img.substring(img.indexOf('mbtn')+4,img.length)][1].src;
};
function btnMouseDown(img)
{
	document.images[img].src = btnImages[img.substring(img.indexOf('mbtn')+4,img.length)][2].src;
};
var timerID = null;
var timerOn = false;
var timeCount = 1000;
var mnuCount = 9;
function showLayer(btntd,btnmnu,bdir,mdir,mpos,xoffset,yoffset)
{
	var td,layer;
	if (btnmnu==0) return;
	td = document.getElementById(btntd);
	layer = document.getElementById(btnmnu);
	if (bdir==0)
	{
		switch(mpos)
		{
			case 0:
			layer.style.left = xoffset + "px";
			break;
			case 1:
			layer.style.left = td.offsetWidth/2 + xoffset + "px";
			break;
			case 2:
			layer.style.left = td.offsetWidth+xoffset + "px";
			break;
		}
		if (mdir==0)
			layer.style.top = td.offsetHeight+yoffset + "px";
		else
			layer.style.top = -layer.offsetHeight-yoffset + "px";
	}
	else
	{
		switch(mpos)
		{
			case 0:
			layer.style.top = yoffset + "px";
			break;
			case 1:
			layer.style.top = td.offsetHeight/2 + yoffset + "px";
			break;
			case 2:
			layer.style.top = td.offsetHeight+yoffset + "px";
			break;
		}
		if (mdir==0)
			layer.style.left = td.offsetWidth+xoffset + "px";
		else
			layer.style.left = -layer.offsetWidth-xoffset + "px";
	}
	layer.style.visibility = "visible";
    svn=document.getElementsByTagName("SELECT");
    for (a=0;a<svn.length;a++)
            svn[a].style.visibility="hidden";
};
function hideLayer(btnmnu)
{
	var layer;
	if (btnmnu==0) return;
	layer = document.getElementById(btnmnu);
	layer.style.visibility = "hidden";
    svn=document.getElementsByTagName("SELECT");
    for (a=0;a<svn.length;a++)
            svn[a].style.visibility="visible";
};
function hideAllLayers()
{
	for (i=0; i<mnuCount; i++)
		hideLayer("mbtn_mnu"+i);
};
function startTime()
{
	if (timerOn == false)
	{
 		timerID=setTimeout( "hideAllLayers()" , timeCount);
 		timerOn = true;
 	}
};
function stopTime()
{
	if (timerOn)
	{
 		clearTimeout(timerID);
 		timerID = null;
 		timerOn = false;
 	}
};
function get_menu_tb_css(iMIndex,bTbAutosize,iTbWidth,iTbHeight,bTbBgColor,TbBgColor,bTbBgImage,strTbBgImage,bTbBrSetParts,iTbBrSize,TbBrColor,strTbBrStyle,bTbBrLeft,iTbBrSizeLeft,TbBrColorLeft,strTbBrStyleLeft,bTbBrRight,iTbBrSizeRight,TbBrColorRight,strTbBrStyleRight,bTbBrTop,iTbBrSizeTop,TbBrColorTop,strTbBrStyleTop,bTbBrBottom,iTbBrSizeBottom,TbBrColorBottom,strTbBrStyleBottom,imgFolder)
{
str=".mnu"+iMIndex+"_tb {";
if(!bTbAutosize)
	str+="width:"+iTbWidth+"px;height:"+iTbHeight+"px;";
if(bTbBgColor)
	str+="background-color:#"+TbBgColor+";";
if(bTbBgImage)
	str+="background-image:url("+imgFolder+"/"+strTbBgImage+");"
if(!bTbBrSetParts)
	str+="border:"+iTbBrSize+"px #"+TbBrColor+" "+strTbBrStyle+";";
else{
	if(bTbBrLeft) str+="border-left:"+iTbBrSizeLeft+"px #"+TbBrColorLeft+" "+strTbBrStyleLeft+";";
	if(bTbBrRight) str+="border-right:"+iTbBrSizeRight+"px #"+TbBrColorRight+" "+strTbBrStyleRight+";";
	if(bTbBrTop) str+="border-top:"+iTbBrSizeTop+"px #"+TbBrColorTop+" "+strTbBrStyleTop+";";
	if(bTbBrBottom) str+="border-bottom:"+iTbBrSizeBottom+"px #"+TbBrColorBottom+" "+strTbBrStyleBottom+";";
};
str+="} ";
return str;
};
function get_menu_line_css(iMIndex,SeperateColor)
{
str=".mnu"+iMIndex+"_line {height:0px;background-color:#"+SeperateColor+"} ";
return str;
};
function get_menu_td_css(bNormal,iMIndex,iTdIndex,strTdHorizontal,strTdVertical,iTdPadding,fcolor,ffamily,fsize,fitalic,fbold,funderline,iTdBrSize,TdBrColor,strTdBrStyle,bTdBgColor,TdBgColor,bTdBgImage,strTdBgImage,imgFolder)
{
var sLink;
sLink="{display:block;";
sLink+="text-align:"+strTdHorizontal+";vertical-align:"+strTdVertical+";padding:"+iTdPadding+"px;";
sLink+="color:#"+fcolor+";font-family:"+ffamily+";font-size:"+fsize+"px;";
if(fitalic) sLink+="font-style:italic;"; else sLink+="font-style:normal;";
if(fbold) sLink+="font-weight:bold;"; else sLink+="font-weight:normal;";
if(funderline) sLink+="text-decoration:underline;"; else sLink+="text-decoration:none;";
sLink+="border:"+iTdBrSize+"px #"+TdBrColor+" "+strTdBrStyle+";";
if(bTdBgColor) sLink+="background-color:#"+TdBgColor+";";
if(bTdBgImage) sLink+="background-image:url("+imgFolder+"/"+strTdBgImage+");";
sLink+="} ";
if(bNormal) str=".mnu"+iMIndex+"_td"+iTdIndex+" a:link "+sLink+".mnu"+iMIndex+"_td"+iTdIndex+" a:visited "+sLink;
else str=".mnu"+iMIndex+"_td"+iTdIndex+" a:hover "+sLink+".mnu"+iMIndex+"_td"+iTdIndex+" a:active "+sLink;
return str;
};
function get_menu_tb_td(iMIndex,iTdIndex,bSetAsDivision,strTdLink,strTdTarget,strTdIcon,strTdText,iTdIconAlign,imgFolder)
{
if(bSetAsDivision) {
	str="<tr><td class=\"mnu"+iMIndex+"_line\"></td></tr>";
	return str;
};
str="<tr><td nowrap class=\"mnu"+iMIndex+"_td"+iTdIndex+"\">";
str+="<a href=\""+strTdLink+"\"";
if(strTdTarget.length>0) str+=" target=\""+strTdTarget+"\"";
str+=">";
if(strTdIcon.length>0){
if(iTdIconAlign==0) str+="<img src=\""+imgFolder+"/"+strTdIcon+"\" border=\"0\">"+strTdText;
else if(iTdIconAlign==1) str+=strTdText+"<img src=\""+imgFolder+"/"+strTdIcon+"\" border=\"0\">";
else if(iTdIconAlign==2) str+="<img src=\""+imgFolder+"/"+strTdIcon+"\" border=\"0\"><br>"+strTdText;
else if(iTdIconAlign==3) str+=strTdText+"<br><img src=\""+imgFolder+"/"+strTdIcon+"\" border=\"0\">";
}
else str+=strTdText;
str+="</a></td></tr>";
return str;
};
function get_menu(iMIndex,imgFolder)
{
str="<div style=\"position:relative;\">";
str+="<div id=\"mbtn_mnu"+iMIndex+"\" style=\"position:absolute;visibility:hidden;z-index:100;\">";
if(iMIndex==0) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(0,true,"0","0",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(0,"C0C0C0");
str+=get_menu_td_css(true,0,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,0,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(0,0,false,"SetupRequirements.aspx","_self","","- Setup requirements",0,imgFolder);
str+="</table>";
}
else if(iMIndex==1) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(1,false,"205","1",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(1,"FFA64D");
str+=get_menu_td_css(true,1,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,1,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,1,1,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,1,1,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,1,2,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,1,2,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,1,3,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,1,3,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,1,4,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,1,4,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,1,5,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,1,5,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(1,0,false,"Company_List.aspx","_self","","- Company Setup&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(1,1,false,"HierarchyLevel.aspx","_self","","- Hierarchy Level Setup&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(1,2,false,"HierarchyElement_List.aspx","_self","","- Business Hierarchy Setup",0,imgFolder);
str+=get_menu_tb_td(1,3,false,"PaymentCode_List.aspx","_self","","- Payment Code Setup&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(1,4,false,"EmploymentType.aspx","_self","","- Miscellaneous Code Setup",0,imgFolder);
str+=get_menu_tb_td(1,5,false,"StatutoryHoliday.aspx","_self","","- Statutory Holiday&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+="</table>";
}
else if(iMIndex==2) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(2,false,"210","1",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(2,"C0C0C0");
str+=get_menu_td_css(true,2,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,2,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(2,0,false,"Emp_List.aspx","_self","","- Employee Personal Information",0,imgFolder);
str+="</table>";
}
else if(iMIndex==3) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(3,false,"150","1",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(3,"C0C0C0");
str+=get_menu_td_css(true,3,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,3,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,3,1,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,3,1,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,3,2,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,3,2,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(3,0,false,"LeaveType_List.aspx","_self","","- Leave Type Setup",0,imgFolder);
str+=get_menu_tb_td(3,1,false,"LeaveCode_List.aspx","_self","","- Leave Code Setup",0,imgFolder);
str+=get_menu_tb_td(3,2,false,"LeavePlan_List.aspx","_self","","- Leave Plan Setup",0,imgFolder);
str+="</table>";
}
else if(iMIndex==4) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(4,true,"0","0",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(4,"C0C0C0");
str+=get_menu_td_css(true,4,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,1,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,1,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,2,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,2,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,3,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,3,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,4,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,4,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,5,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,5,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,6,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,6,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,7,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,7,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,8,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,8,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,9,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,9,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,10,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,10,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,4,11,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,4,11,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(4,0,false,"Payroll_Formula_List.aspx","_self","","- Payroll Formula&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,1,false,"Payroll_Group_List.aspx","_self","","- Payroll Group&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,2,false,"Payroll_CND_List.aspx","_self","","- Claims and Deductions&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,3,false,"Payroll_TrialRun.aspx","_self","","- Payroll Trial Run&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,4,false,"Payroll_TrialRunAdjust_List.aspx","_self","","- Payroll Trial Run Adjustment&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,5,false,"Payroll_UndoTrialRun.aspx","_self","","- Rollback Trial Run&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,6,false,"Payroll_Confirm.aspx","_self","","- Payroll Confirmation&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,7,false,"Payroll_GenerateBankFile_View.aspx","_self","","- Generate Bank File&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,8,false,"Payroll_GenerateMPFFile_List.aspx","_self","","- Generate MPF File&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,9,false,"Payroll_ProcessEnd.aspx","_self","","- Payroll Process End&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(4,10,false,"Payroll_HistoryAdjust_List.aspx","_self","","- Payroll History Enquiry / Adjustment",0,imgFolder);
str+=get_menu_tb_td(4,11,false,"Payroll_RollbackPayrollPeriod.aspx","_self","","- Rollback Payroll Process End",0,imgFolder);
str+="</table>";
}
else if(iMIndex==5) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(5,false,"130","1",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(5,"C0C0C0");
str+=get_menu_td_css(true,5,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,5,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,5,1,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,5,1,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,5,2,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,5,2,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";
str+=get_menu_tb_td(5,0,false,"MPFPlan_List.aspx","_self","","- MPF Plan Setup",0,imgFolder);
str+=get_menu_tb_td(5,1,false,"AVCPlan_List.aspx","_self","","- AVC Plan Setup",0,imgFolder);
str+=get_menu_tb_td(5,2,false,"ORSOPlan_List.aspx","_self","","- ORSO Plan Setup",0,imgFolder);
str+="</table>";
}
else if(iMIndex==6) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(6,true,"0","0",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(6,"C0C0C0");
str+=get_menu_td_css(true,6,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,6,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,6,1,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,6,1,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,6,2,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,6,2,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,6,3,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,6,3,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,6,4,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,6,4,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(6,0,false,"Taxation_Company_List.aspx","_self","","- Taxation Company&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(6,1,false,"Taxation_PaymentMapping_View.aspx","_self","","- Taxation Category Mapping&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(6,2,false,"Taxation_Generation_List.aspx","_self","","- Taxation Generation&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(6,3,false,"Taxation_Adjustment_List.aspx","_self","","- Taxation Adjustment&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(6,4,false,"Taxation_GenerateDisk.aspx","_self","","- Taxation Diskette Generation",0,imgFolder);
str+="</table>";
}
else if(iMIndex==7) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(7,false,"150","1",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(7,"C0C0C0");
str+=get_menu_td_css(true,7,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,7,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,7,1,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,7,1,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,7,2,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,7,2,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(7,0,false,"Reports_Emp.aspx","_self","","- Employee Reports&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(7,1,false,"Reports_Payroll_MPF.aspx","_self","","- Payroll & MPF Reports",0,imgFolder);
str+=get_menu_tb_td(7,2,false,"Report_Taxation_Report_List.aspx","_blank","","- Taxation Reports&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+="</table>";
}
else if(iMIndex==8) {
str+="<style type=\"text/css\">";str+=get_menu_tb_css(8,false,"150","1",true,"FFFFFF",false,"",false,"1","C0C0C0","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",false,"0","FFFFFF","solid",imgFolder);
str+=get_menu_line_css(8,"C0C0C0");
str+=get_menu_td_css(true,8,0,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,8,0,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,8,1,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,8,1,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,8,2,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,8,2,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+=get_menu_td_css(true,8,3,"Left","Middle","5","808080","Arial","12",false,false,false,"0","000000","solid",false,"FFFFFF",false,"",imgFolder);
str+=get_menu_td_css(false,8,3,"Left","Middle","5","000000","Arial","12",false,false,false,"0","000000","solid",true,"FCB350",false,"",imgFolder);
str+="</style>";
str+="<table class=\"mnu"+iMIndex+"_tb\" cellspacing=\"2\" cellpadding=\"0\" onMouseOver=\"stopTime();\" onMouseOut=\"startTime();\">";str+=get_menu_tb_td(8,0,false,"USerGroup_List.aspx","_self","","- User Group&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(8,1,false,"User_List.aspx","_self","","- User Maintenance",0,imgFolder);
str+=get_menu_tb_td(8,2,false,"ChangePassword.aspx","_self","","- Change Password&nbsp;&nbsp;",0,imgFolder);
str+=get_menu_tb_td(8,2,false,"SystemParameter.aspx","_self","","- System Parameter&nbsp;&nbsp;",0,imgFolder);
str+="</table>";
};
str+="</div></div>";
return str;
};
function get_menutd(iIndex,iMIndex,bOver,bdir,mdir,mpos,xoffset,yoffset,iWidth,iHeight,sHint,sLink,sTarget,imgFolder) {
str="<td style=\"padding-right:0px\" id=\"mbtn_td"+iIndex+"\">";
str+=get_menu(iMIndex,imgFolder);
str+="<a ";
if (bOver)
	str+="onMouseOver=\"btnMouseOver('mbtn"+iIndex+"');hideAllLayers();"
else
	str+="onMouseOver=\"btnMouseOver('mbtn"+iIndex+"');hideAllLayers();\" onclick=\"";
str+="showLayer('mbtn_td"+iIndex+"','mbtn_mnu"+iMIndex+"',"+bdir+","+mdir+","+mpos+","+xoffset+","+yoffset+");stopTime();\"";
str+=" onMouseOut=\"btnMouseOut('mbtn"+iIndex+"');startTime();\" onMouseDown=\"btnMouseDown('mbtn"+iIndex+"');\" href=\""+sLink+"\"";
if (sTarget.length >0)
	str+=" target=\""+sTarget+"\"";
str+=">";
str+="<img src=\""+imgFolder+"/mbtn"+iIndex+"_0.gif\" name=mbtn"+iIndex+" border=0 width=\""+iWidth+"\" height=\""+iHeight+"\" alt=\""+sHint+"\">";
str+="</a></td>";
return str;
};
function gen_td(iIndex,bOver,sLink,sTarget,iWidth,iHeight,sHint,imgFolder) {
str="<td style=\"padding-right:0px\" id=\"mbtn_td"+iIndex+"\">";
str+="<a ";
str+="onMouseOver=\"btnMouseOver('mbtn"+iIndex+"');"
str+="hideAllLayers();stopTime();\" ";
str+="onMouseOut=\"btnMouseOut('mbtn"+iIndex+"');startTime();\" onMouseDown=\"btnMouseDown('mbtn"+iIndex+"');\" href=\""+sLink+"\"";
if (sTarget.length >0)
	str+=" target=\""+sTarget+"\"";
str+=">";
str+="<img src=\""+imgFolder+"/mbtn"+iIndex+"_0.gif\" name=mbtn"+iIndex+" border=0 width=\""+iWidth+"\" height=\""+iHeight+"\" alt=\""+sHint+"\">";
str+="</a></td>";
return str;
};
function get_table(bHor,imgFolder) {
str="<table id=\"mbtn_tb\" cellpadding=0 cellspacing=0 border=0>";
str+="<tr>";
str+=get_menutd(0,0,true,0,0,0,3,3,"89","35","Introduction and overview","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(1,1,true,0,0,0,3,3,"89","35","Code Setup","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(2,2,true,0,0,0,3,3,"89","35","Employee","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(3,3,true,0,0,0,3,3,"89","35","Leave","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(4,4,true,0,0,0,3,3,"89","35","Payroll","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(5,5,true,0,0,0,3,3,"89","35","MPF Setup","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(6,6,true,0,0,0,3,3,"89","35","Taxation","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(7,7,true,0,0,0,3,3,"89","35","Report","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=get_menutd(8,8,true,0,0,0,3,3,"89","35","Security","#","",imgFolder);
if (bHor==false) str+="</tr><tr>";
str+=gen_td(9,true,"Logout.aspx","_self","96","35","Logout",imgFolder);
str+="</tr>";
str+="</table>";
return str;
};
function draw_buttons(imgFolder,divID,iPressed)
{
for (i= 0; i< btnCount; i++)
{
	btnImages[i] = new Array();
	for (j= 0; j< staCount; j++)
	{
		btnImages[i][j] = new Image();
		btnImages[i][j].src = imgFolder + '/mbtn' + i + '_' + j + '.gif';
	}
}
result=get_table(true,imgFolder);
document.write(result);
}