// JScript File

function popUpListHandler(layer, closeButton, openButton,isAutoHide)
{
    var popUpListListener=
    {
        lastElement:null,
        noHide:function(sender)
        {
            if (sender!=null)
            {
                var srcEl = sender.srcElement? sender.srcElement : sender.target;
                popUpListListener.lastElement=srcEl;
            }
            setTimeout(function()
                {                
                    popUpListListener.lastElement=null;
                },100);

        },
        checkDocumentClick:function()
        {
                    popUpListListener.Hide();
        },
        Hide:function()
        {
            if (popUpListListener.lastElement==null)
            {
                popUpListListener.HideLayer();
            }
        },
        
        HideLayer:function()
        {
            layer.style.visibility='hidden';
            layer.style.display='none';
            closeButton.style.visibility='hidden';
            closeButton.style.display='none';
            openButton.style.visibility='visible';
            openButton.style.display='';
            if (document.addEventListener)
            {
                 closeButton.removeEventListener('click',popUpListListener.HideLayer,false);
                 if (isAutoHide==true)
                 {
                     layer.removeEventListener('click',popUpListListener.noHide,false );
                     document.removeEventListener('click', popUpListListener.checkDocumentClick,false );
                     /* use Mozilla broswer can detect keydown event only */
                     document.removeEventListener('keydown',popUpListListener.keyPress,false);
                 }
            }
            else
            {
                 closeButton.detachEvent('onclick',popUpListListener.HideLayer);
                 if (isAutoHide==true)
                 {
                     layer.detachEvent('onclick',popUpListListener.noHide);
                     document.detachEvent('onclick',popUpListListener.checkDocumentClick);
                     document.detachEvent('onkeypress',popUpListListener.keyPress);
                 }
            }
            popUpListListener=null;
        },
        keyPress:function(e)
        {
            var keynum;
            var keychar;
            var numcheck;

            if(window.event) // IE
                keynum = e.keyCode;
            else if(e.which) // Netscape/Firefox/Opera
                keynum = e.which;
                
            if (keynum==27)
                popUpListListener.HideLayer();
        }    
    }
    
//    popUpListListener.lastElement=layer;

    if (            layer.style.visibility!='visible')
    {
        setTimeout(function()
            {
                layer.style.visibility='visible';
                layer.style.display='';
                closeButton.style.visibility='visible';
                closeButton.style.display='';
                openButton.style.visibility='hidden';
                openButton.style.display='none';
                if (document.addEventListener)
                {
                    closeButton.addEventListener('click',popUpListListener.HideLayer,false);
                    if (isAutoHide)
                    {
                     layer.addEventListener('click',popUpListListener.noHide,false );
                     document.addEventListener('click',popUpListListener.checkDocumentClick,false );
                     /* use Mozilla broswer can detect keydown event only */
                     document.addEventListener('keydown',popUpListListener.keyPress,false);
                    }
                }
                else
                {
                 closeButton.attachEvent('onclick',popUpListListener.HideLayer);
                 if (isAutoHide)
                 {
                     layer.attachEvent('onclick',popUpListListener.noHide);
                     document.attachEvent('onclick',popUpListListener.checkDocumentClick);
                     document.attachEvent('onkeypress',popUpListListener.keyPress);
                 }
                }
            },100);
    }
}

//var popUpListClickEventObject=
//{
//    container:window.document,
//    element:null,
//    callback:null,
//    condition:null,
//    suspended:false,
//    stack:[],
//    INSIDE:0,
//    OUTSIDE:1,
//    activate:function()
//    {
//        this.container.onclick=function(e)
//        {
//            if(!this.suspended)
//                this._fireEvent(e||window.event);
//        };
//    },
//    resume:function()
//    {
//        setTimeout(function()
//        {
//            this.suspended=false;
//        }.bind(this),100);
//    },
//    suspend:function()
//    {
//        this.suspended=true;
//    },
//    push:function()
//    {
//        if(this.element)
//        {
//            this.stack.push(
//            {
//                element:this.element,
//                callback:this.callback,
//                condition:this.condition
//            }
//            );
//            this.element=null;
//            this.callback=null;
//        }
//    },
//    pop:function()
//    {
//        if(this.stack.length>0)
//        {
//            setTimeout(function()
//            {
//                var obj=this.stack.pop();
//                this.element=obj.element;
//                this.callback=obj.callback;
//                this.condition=obj.condition;
//            }.bind(this),100);
//        }
//    },
//    addListener:function(el,f,c)
//    {
//        this.clearCurrent();
//        setTimeout(function()
//        {
//            this.element=el;
//            this.callback=f;
//            this.condition=c||this.OUTSIDE;
//        }.bind(this),100);
//    },
//    clearCurrent:function()
//    {
//        if(this.element)
//        {
//            this._fireEvent(null,true);
//        }
//    },
//    removeListener:function(el)
//    {
//        if(this.element==el)
//        {
//            this.element=null;
//            this.callback=null;
//        }
//    },
//    _fireEvent:function(e,forced)
//    {
//        if(!this.element||this.element.offsetWidth==0)
//        {
//            this.removeListener(this.element);
//        }
//        else if(forced||this.condition==this.INSIDE&&DOM.happensInElement(e,this.element)||this.condition==this.OUTSIDE&&!DOM.happensInElement(e,this.element))
//        {
//            this.callback.call(this.element);
//        }
//    },
//    deactivate:function()
//    {
//        this.element=null;
//        this.callback=null;
//        for(var i=0;i<this.stack.length;i++)
//        {
//            this.stack[i].element=null;
//            this.stack[i].callback=null;
//        }
//        this.container.onclick=null;
//    }
//}