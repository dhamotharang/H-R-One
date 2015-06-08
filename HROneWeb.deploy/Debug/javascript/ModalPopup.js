// JScript File

function OpenModelPopup(popUpLayer)
{
     

    modalPopupBackgroundLayer=document.getElementById('ModalPopupBackgroundDiv');
    if (modalPopupBackgroundLayer==null)
    {
        modalPopupBackgroundLayer=document.createElement('div');
        modalPopupBackgroundLayer.id="ModalPopupBackgroundDiv";
        modalPopupBackgroundLayer.className='ModalPopupBackgroundDiv';
        document.body.appendChild(modalPopupBackgroundLayer);

    }
    if (document.attachEvent)
    {
     window.attachEvent('onresize', function(){ popUpResize(popUpLayer)});
     window.attachEvent('onscroll', function(){ popUpResize(popUpLayer)});
    }
    else
    {
     window.addEventListener('resize',function(){ popUpResize(popUpLayer)}, false);
     window.addEventListener('scroll',function(){ popUpResize(popUpLayer)}, false);
    }

    popUpLayer.style.visibility='visible';
    popUpLayer.style.display='';
    
    setAlpha(modalPopupBackgroundLayer, 75);


    modalPopupBackgroundLayer.style.display='';
    modalPopupBackgroundLayer.style.visibility='visible';
    
    popUpResize(popUpLayer);
}

function popUpResize(popUpLayer)
{
    _pl_clientWidth = (document.documentElement.clientWidth == 0) ? document.body.clientWidth : document.documentElement.clientWidth;
    _pl_clientHeight = (document.documentElement.clientHeight == 0) ? document.body.clientHeight : document.documentElement.clientHeight; 

    _pl_scrollLeft = (document.documentElement.scrollLeft == 0) ? document.body.scrollLeft : document.documentElement.scrollLeft;
    _pl_scrollTop = (document.documentElement.scrollTop == 0) ? document.body.scrollTop : document.documentElement.scrollTop; 

    popUpLayer.style.top= (_pl_scrollTop+(_pl_clientHeight-popUpLayer.clientHeight)/2) + 'px';
    popUpLayer.style.left=(_pl_scrollLeft+(_pl_clientWidth-popUpLayer.clientWidth)/2) + 'px';

    document.getElementById ('ModalPopupBackgroundDiv').style.top=_pl_scrollTop + 'px';
    document.getElementById ('ModalPopupBackgroundDiv').style.left=_pl_scrollLeft + 'px';
    document.getElementById ('ModalPopupBackgroundDiv').style.width=  _pl_clientWidth + 'px';
    document.getElementById ('ModalPopupBackgroundDiv').style.height= _pl_clientHeight + 'px';

}
function CloseModelPopup(popUpLayer)
{
    
    if (document.attachEvent)
    {
     window.detachEvent('onresize',function(){ popUpResize(popUpLayer)});
     window.detachEvent('onscroll',function(){ popUpResize(popUpLayer)});
    }
    else
    {
     window.removeEventListener('resize',function(){ popUpResize(popUpLayer)}, false);
     window.removeEventListener('scroll',function(){ popUpResize(popUpLayer)}, false);
    }

    document.getElementById ('ModalPopupBackgroundDiv').style.display='none';
    popUpLayer.style.display='none';
}

function setAlpha(obj,alpha) {
	if(typeof obj != 'object') obj = document.getElementById(obj);
	if(!obj) return;
	var alphaPresent=alpha/100;
	obj.style.filter = "alpha(opacity:"+alpha+")";
	obj.style.KHTMLOpacity = alphaPresent;
	obj.style.MozOpacity = alphaPresent;
	obj.style.opacity = alphaPresent;
}  