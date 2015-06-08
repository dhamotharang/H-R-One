// JScript File
/*
function checkAll(ctrl,cbName, status) {
  var i=0;
  var curItem=null;
  var ctrlName = ctrl + '_ctl0' + i + '_' + cbName;
  
  curItem=document.getElementById(ctrlName);
  while(curItem!=null) {
    curItem.checked=status;
    i++;
    if(i>=10) {
        ctrlName = ctrl + '_ctl' + i + '_' + cbName;
    } else {
        ctrlName = ctrl + '_ctl0' + i + '_' + cbName;
    }
    curItem=document.getElementById(ctrlName);  
  }
}
*/
function getInternetExplorerVersion()
// Returns the version of Internet Explorer or a -1
// (indicating the use of another browser).
{
  var rv = -1; // Return value assumes failure.
  if (navigator.appName == 'Microsoft Internet Explorer')
  {
    var ua = navigator.userAgent;
    var re  = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
    if (re.exec(ua) != null)
      rv = parseFloat( RegExp.$1 );
  }
  return rv;
}
function checkAll(ctrl, cbName, status)
{
    var list = document.getElementsByTagName('Input');
    for (i=0;i<list.length;i++)
        if ( list[i].id.indexOf(ctrl+'_ctl',0)==0||(list[i].id.indexOf('ctl',0)==0 && ctrl==''))
            if (list[i].id.indexOf('_' + cbName,0)>=0)
                list[i].checked=status;
}

function openAlert()
{
//alert('Help is not available yet');
//  Help Message will be display inside Help Window;
    openHelp();
}

function openHelp()
{
    var helpSearch=window.location.pathname;
    var url='./ShowHelpPDF.aspx?help='+helpSearch;
    window.open(url,'help','menubar=no,toolbar=no,location=no');
}

function toggleLayer( whichLayer )
{
  var elem, vis;
  if( document.getElementById ) // this is the way the standards work
    elem = document.getElementById( whichLayer );
  else if( document.all ) // this is the way old msie versions work
      elem = document.all[whichLayer];
  else if( document.layers ) // this is the way nn4 works
    elem = document.layers[whichLayer];
  vis = elem.style;
  // if the style.display value is blank we try to figure it out here
  if(vis.display==''&&elem.offsetWidth!=undefined&&elem.offsetHeight!=undefined)
    vis.display = (elem.offsetWidth!=0&&elem.offsetHeight!=0)?'block':'none';
  vis.display = (vis.display==''||vis.display=='block')?'none':'block';
}

function toggleLayer( whichLayer, IsShowLayer)
{
  var elem, vis;
  if( document.getElementById ) // this is the way the standards work
    elem = document.getElementById( whichLayer );
  else if( document.all ) // this is the way old msie versions work
      elem = document.all[whichLayer];
  else if( document.layers ) // this is the way nn4 works
    elem = document.layers[whichLayer];
  vis = elem.style;
  // if the style.display value is blank we try to figure it out here
    vis.display = IsShowLayer?'block':'none';
}

  function addZero(vNumber){ 
    return ((vNumber < 10) ? "0" : "") + vNumber 
  } 
        
  function formatDate(vDate, vFormat){ 
    var vDay                      = addZero(vDate.getDate()); 
    var vMonth            = addZero(vDate.getMonth()+1); 
    var vYearLong         = addZero(vDate.getFullYear()); 
    var vYearShort        = addZero(vDate.getFullYear().toString().substring(3,4)); 
    var vYear             = (vFormat.indexOf("yyyy")>-1?vYearLong:vYearShort) 
    var vHour             = addZero(vDate.getHours()); 
    var vMinute           = addZero(vDate.getMinutes()); 
    var vSecond           = addZero(vDate.getSeconds()); 
    var vDateString       = vFormat.replace(/dd/g, vDay).replace(/MM/g, vMonth).replace(/y{1,4}/g, vYear) 
    vDateString           = vDateString.replace(/hh/g, vHour).replace(/mm/g, vMinute).replace(/ss/g, vSecond) 
    return vDateString 
  } 
function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g,"");
}
function IsValidDateFormat(dateTextBox, dateFormat)
{
	var	monthName =	new	Array("January","February","March","April","May","June","July","August","September","October","November","December");

    var dateString=trim(dateTextBox.value);
    var year, month, day;
    if (dateString.length==0)
        return true;
    else if (dateString.length==8)
    {
        year=dateString.substring(0,4);
        month=dateString.substring(4,6);
        day=dateString.substring(6);
    }
    else
    {
				formatChar = " "
				aFormat	= dateFormat.split(formatChar)
				if (aFormat.length<3)
				{
					formatChar = "/"
					aFormat	= dateFormat.split(formatChar)
					if (aFormat.length<3)
					{
						formatChar = "."
						aFormat	= dateFormat.split(formatChar)
						if (aFormat.length<3)
						{
							formatChar = "-"
							aFormat	= dateFormat.split(formatChar)
							if (aFormat.length<3)
							{
								// invalid date	format
								formatChar=""
							}
						}
					}
				}
				

				tokensChanged =	0
				if ( formatChar	!= "" )
				{
					// use user's date
					aData =	dateTextBox.value.split(formatChar)

					for	(i=0;i<3;i++)
					{
						if ((aFormat[i]=="d") || (aFormat[i]=="dd"))
						{
							day = parseInt(aData[i], 10)
							tokensChanged ++
						}
						else if	((aFormat[i]=="m") || (aFormat[i]=="mm")||(aFormat[i]=="M") || (aFormat[i]=="MM"))
						{
							month =	parseInt(aData[i], 10)
							tokensChanged ++
						}
						else if	(aFormat[i]=="yyyy")
						{
							year = parseInt(aData[i], 10)
							tokensChanged ++
						}
						else if	(aFormat[i]=="mmm" || (aFormat[i]=="MMM"))
						{
							for	(j=0; j<12;	j++)
							{
								if (aData[i]==monthName[j])
								{
									month=j+1
									tokensChanged ++
								}
							}
						}
					}
				}

				if ((tokensChanged!=3)||isNaN(day)||isNaN(month)||isNaN(year))
				{
				    return false;
				}
    }
    
    try
    {
        d= new Date(year,month-1,day);
    }
    catch(e)
    {
        try
        {
            d= new Date(s.replace('-','/'));
        }
        catch(ex)
        {
            return false;
        }
    }
    dateString=formatDate(d,dateFormat);
    if (d.getFullYear()>1800)
    {
        dateTextBox.value=dateString;
        return true;
    }
    return false;
}

//function GetAJAXRequestResultText(requestURL)
//{
//    return "1";
//    if (window.XMLHttpRequest)
//      {// code for IE7+, Firefox, Chrome, Opera, Safari
//      xmlhttp=new XMLHttpRequest();
//      }
//    else
//      {// code for IE6, IE5
//      xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
//      }
//      
//    xmlhttp.open("GET",requestURL,true);
//    xmlhttp.send();
//    return xmlhttp.responseText;
//}

//void raiseOnChangeEvent(ctrl)
//{
//    var e = document.createEvent('HTMLEvents');
//    e.initEvent('change', false, false);
//    ctrl.dispatchEvent(e);
//}