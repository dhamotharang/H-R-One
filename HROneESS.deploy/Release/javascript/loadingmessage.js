preLoadWaitImage = new Image();
preLoadWaitImage.src = "./images/wait.gif";
var current_alpha=75;


//document.write('<div id="_pl_background" style="display:none; " ><DIV ID="_pl_message" align="center" valign="middle" style="BORDER-RIGHT: #660000 1px solid; BORDER-TOP: #660000 1px solid; FILTER: alpha(opacity=100); OVERFLOW: auto; BORDER-LEFT: #0066FF 2px solid; BORDER-RIGHT: #0066FF 2px solid; BORDER-TOP: #0066FF 2px solid; BORDER-BOTTOM: #0066FF 2px solid;  BACKGROUND-COLOR: #ffffff"></div></div>');
function initLoadMessage()
{
    _pl_div=document.createElement('div');
    _pl_div.id="_pl_background";

    _pl_div_message=document.createElement('div');
    _pl_div_message.id="_pl_message";
    _pl_div_message.style.cssText="text-align:center; vertical-align:middle; BORDER-RIGHT: #660000 1px solid; BORDER-TOP: #660000 1px solid; FILTER: alpha(opacity=100); OVERFLOW: auto; BORDER-LEFT: #0066FF 2px solid; BORDER-RIGHT: #0066FF 2px solid; BORDER-TOP: #0066FF 2px solid; BORDER-BOTTOM: #0066FF 2px solid;  BACKGROUND-COLOR: #ffffff";

    _pl_div.appendChild(_pl_div_message);

    _pl_div_message.style.width = '250px';
    _pl_div_message.style.height = '70px';

    _pl_div.style.zIndex = 20000;
    _pl_div.style.backgroundColor='#777777';

    _pl_div_message.style.zIndex = 30000;

    _pl_div_message.innerHTML = '<br><B>.:: Now Loading ::. </B><br><img src="./images/wait.gif" width="150" height="13"><br><a href="javascript: void(0);" onclick="_pl_onload()" >Cancel</a>';

    _pl_alpha('_pl_background', current_alpha);
    _pl_alpha('_pl_message', 100);

    document.body.appendChild(_pl_div);
    _pl_resize();
}




var TRIGGER_BY_AJAX=false;
var _is_removing_loading_screen=false;

function _pl_onload() 
{
    TRIGGER_BY_AJAX=false;
    _is_removing_loading_screen=true;
    _remove_alpha();

    if (document.attachEvent)
    {
     window.detachEvent('onload',_pl_onload);
     window.detachEvent('onresize',_pl_resize);
     window.detachEvent('onscroll',_pl_resize);
    }
    else
    {
     window.removeEventListener('load',_pl_onload,false);
     window.removeEventListener('resize',_pl_resize, false);
     window.removeEventListener('scroll',_pl_resize, false);
    }
}
// Experimental script
function _pl_show()
{

    if (document.attachEvent)
    {
     window.attachEvent('onload',_pl_onload);
     window.attachEvent('onresize',_pl_resize);
     window.attachEvent('onscroll',_pl_resize);
    }
    else
    {
     window.addEventListener('load',_pl_onload,false);
     window.addEventListener('resize',_pl_resize, false);
     window.addEventListener('scroll',_pl_resize, false);
    }
    _is_removing_loading_screen=false;
    _pl_resize();
	document.getElementById('_pl_background').style.display='block';
	document.getElementById('_pl_message').style.display='block';
    svn=document.getElementsByTagName("SELECT");
    for (a=0;a<svn.length;a++)
            svn[a].style.visibility="hidden";
	setTimeout("_pl_checkreadystatus()",1000);
	setTimeout("_update_alpha()",500);

//	if (document.attachEvent)
//    {
//     window.attachEvent('onunload ',_pl_onload);
//    }
//    else
//    {
//     window.addEventListener('unload ',_pl_onload,false);
//    }

}

function _update_alpha()
{
    if (document.getElementById('_pl_background').style.display=='block'&& !_is_removing_loading_screen)
    {
        current_alpha+=15;
        _pl_alpha('_pl_background', current_alpha);
        if (current_alpha<75 )
        	setTimeout("_update_alpha()",100);
    }
}

function _remove_alpha()
{
    if (document.getElementById('_pl_background').style.display!='none')
    {
        current_alpha-=25;
        if (current_alpha<0)
            current_alpha=0;
        _pl_alpha('_pl_background', current_alpha);
        if (current_alpha>0)
        	setTimeout("_remove_alpha()",100);
        else
        {
	        document.getElementById('_pl_background').style.display='none';
	        document.getElementById('_pl_message').style.display='none';
            svn=document.getElementsByTagName("SELECT");
            for (a=0;a<svn.length;a++)
                    svn[a].style.visibility="visible";
        }
    }
}

function _pl_submit()
{
    try
    {
        window.focus();
    }
    catch(ex)
    {
        return false;
    }  
    _pl_show();
}

function _pl_resize()
{
    _pl_div = document.getElementById("_pl_background");
    _pl_div_message=document.getElementById("_pl_message");

    _pl_clientWidth = (document.documentElement.clientWidth == 0) ? document.body.clientWidth : document.documentElement.clientWidth;
    _pl_clientHeight = (document.documentElement.clientHeight == 0) ? document.body.clientHeight : document.documentElement.clientHeight; 

    _pl_scrollLeft = (document.documentElement.scrollLeft == 0) ? document.body.scrollLeft : document.documentElement.scrollLeft;
    _pl_scrollTop = (document.documentElement.scrollTop == 0) ? document.body.scrollTop : document.documentElement.scrollTop; 

    _pl_div.style.position = 'absolute';
    _pl_div.style.left=_pl_scrollLeft + 'px';
    _pl_div.style.top=_pl_scrollTop + 'px';

    _pl_div.style.width = _pl_clientWidth + 'px';
    _pl_div.style.height = _pl_clientHeight + 'px';

    _pl_div_message.position = 'absolute';
    _pl_div_message.style.marginLeft = (_pl_clientWidth-_pl_div_message.clientWidth)/2 + 'px';
    _pl_div_message.style.marginTop = (_pl_clientHeight-_pl_div_message.clientHeight)/2 + 'px';
}

function _pl_checkreadystatus()
{
    if (!TRIGGER_BY_AJAX)
    {
      if (document.readyState=="complete" || document.readyState=="interactive") 
      {
          _pl_onload();
      }
      else if (document.readyState==undefined)
      {
        setTimeout("_pl_onload()",10000);
      }
      else
    	setTimeout("_pl_checkreadystatus()",1000);
    }
}

function _pl_alpha(obj,al) {
	if(typeof obj != 'object') obj = document.getElementById(obj);
	if(!obj) return;
	var alpha=al/100;
	obj.style.filter = "alpha(opacity:"+al+")";
	obj.style.KHTMLOpacity = alpha;
	obj.style.MozOpacity = alpha;
	obj.style.opacity = alpha;
}  