
////document.write('<div id="_pl_background" style="display:none; " ><DIV ID="_pl_message" align="center" valign="middle" style="BORDER-RIGHT: #660000 1px solid; BORDER-TOP: #660000 1px solid; FILTER: alpha(opacity=100); OVERFLOW: auto; BORDER-LEFT: #0066FF 2px solid; BORDER-RIGHT: #0066FF 2px solid; BORDER-TOP: #0066FF 2px solid; BORDER-BOTTOM: #0066FF 2px solid;  BACKGROUND-COLOR: #ffffff"></div></div>');
//function initDownloadDialog(message, url)
//{
//    _pl_div = document.getElementById("_download_background");
//    if (_pl_div==null)
//    {
//        _pl_div=document.createElement('div');
//        _pl_div.id="_download_background";
//    }
//    _pl_div_message=document.getElementById("_download_message");
//    if (_pl_div_message==null)
//    {
//        _pl_div_message=document.createElement('div');
//        _pl_div_message.id="_download_message";
//    }
//    
//    _pl_div_message.style.cssText="text-align:center; vertical-align:middle; BORDER-RIGHT: #660000 1px solid; BORDER-TOP: #660000 1px solid; FILTER: alpha(opacity=100); OVERFLOW: auto; BORDER-LEFT: #0066FF 2px solid; BORDER-RIGHT: #0066FF 2px solid; BORDER-TOP: #0066FF 2px solid; BORDER-BOTTOM: #0066FF 2px solid;  BACKGROUND-COLOR: #ffffff";

//    _pl_div.appendChild(_pl_div_message);

//    _pl_div_message.style.width = '250px';
//    _pl_div_message.style.height = '70px';

//    _pl_div.style.zIndex = 20000;
//    _pl_div.style.backgroundColor='#777777';

//    _pl_div_message.style.zIndex = 30000;

//    _pl_div_message.innerHTML = '<br><B><a href="' + url + '" onclick="clearDownloadDialog()" >' + message + '</a></B>';

//    document.body.appendChild(_pl_div);
//    setTimeout("showDownloadDialog()",1000);
//}


//function showDownloadDialog()
//{
//    if (document.attachEvent)
//    {
//     window.attachEvent('onresize',resizeDownloadDialog);
//     window.attachEvent('onscroll',resizeDownloadDialog);
//    }
//    else
//    {
//     window.addEventListener('resize',resizeDownloadDialog, false);
//     window.addEventListener('scroll',resizeDownloadDialog, false);
//    }

//	document.getElementById('_download_background').style.display='block';
//	document.getElementById('_download_message').style.display='block';
//	if (getInternetExplorerVersion()<8)
//	{
//    svn=document.getElementsByTagName("SELECT");
//    for (a=0;a<svn.length;a++)
//            svn[a].style.visibility="hidden";
//    }
//    resizeDownloadDialog();
//}

//function clearDownloadDialog() 
//{
//    _pl_div = document.getElementById("_download_background");
//    _pl_div_message=document.getElementById("_download_message");

//    _pl_div.style.display='none';
//    _pl_div_message.style.display='none';
//    svn=document.getElementsByTagName("SELECT");
//    for (a=0;a<svn.length;a++)
//            svn[a].style.visibility="visible";
//    
//    if (document.attachEvent)
//    {
//     window.detachEvent('onresize',resizeDownloadDialog);
//     window.detachEvent('onscroll',resizeDownloadDialog);
//    }
//    else
//    {
//     window.removeEventListener('resize',resizeDownloadDialog, false);
//     window.removeEventListener('scroll',resizeDownloadDialog, false);
//    }
//    document.removeChild(_pl_div_message);
//    document.removeChild(_pl_div);
//}

//function resizeDownloadDialog()
//{
//    _pl_div = document.getElementById("_download_background");
//    _pl_div_message=document.getElementById("_download_message");

//    _pl_clientWidth = (document.documentElement.clientWidth == 0) ? document.body.clientWidth : document.documentElement.clientWidth;
//    _pl_clientHeight = (document.documentElement.clientHeight == 0) ? document.body.clientHeight : document.documentElement.clientHeight; 

//    _pl_scrollLeft = (document.documentElement.scrollLeft == 0) ? document.body.scrollLeft : document.documentElement.scrollLeft;
//    _pl_scrollTop = (document.documentElement.scrollTop == 0) ? document.body.scrollTop : document.documentElement.scrollTop; 

//    _pl_div.style.position = 'absolute';
//    _pl_div.style.left=_pl_scrollLeft + 'px';
//    _pl_div.style.top=_pl_scrollTop + 'px';

//    _pl_div.style.width = _pl_clientWidth + 'px';
//    _pl_div.style.height = _pl_clientHeight + 'px';

//    _pl_div_message.position = 'absolute';
//    _pl_div_message.style.marginLeft = (_pl_clientWidth-_pl_div_message.clientWidth)/2 + 'px';
//    _pl_div_message.style.marginTop = (_pl_clientHeight-_pl_div_message.clientHeight)/2 + 'px';
//}
