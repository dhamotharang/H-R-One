// JScript File

function advancedCheckBoxListHandler(checkboxlistID, spanID, selectAllCtrl, clearAllCtrl, textForNoneLabel)
{
    var advancedCheckBoxListListener=
    {
        updateSpanDisplay:function()
        {
            var displayText='';
            var selectAll=true;
            var list = document.getElementsByTagName('Label');
            for (i=0;i<list.length;i++)
                if ( list[i].getAttributeNode("for").value.indexOf(checkboxlistID,0)==0)
                {
                    var checkBox = document.getElementById(list[i].getAttributeNode("for").value);
                    if (checkBox.checked)
                        if (displayText=='')
                            displayText+=list[i].innerHTML;
                        else
                            displayText+=", " + list[i].innerHTML;
                    else
                        selectAll=false;
                }
            if (selectAll)
                displayText='(' + selectAllCtrl.value + ')';
            if (displayText=='')
                displayText=textForNoneLabel;
            document.getElementById(spanID).innerHTML=displayText;
        },
        selectAll:function()
        {
            var list = document.getElementsByTagName('Input');
            for (i=0;i<list.length;i++)
            {
                if ( list[i].id.indexOf(checkboxlistID,0)==0)
                    list[i].checked=true;
            }
            advancedCheckBoxListListener.updateSpanDisplay();
        },
        unselectAll:function()
        {
            var list = document.getElementsByTagName('Input');
            for (i=0;i<list.length;i++)
            {
                if ( list[i].id.indexOf(checkboxlistID,0)==0)
                    list[i].checked=false;
            }
            advancedCheckBoxListListener.updateSpanDisplay();
        }

    }
    


        var list = document.getElementsByTagName('Input');
        for (i=0;i<list.length;i++)
            if ( list[i].id.indexOf(checkboxlistID,0)==0)
                if (document.attachEvent)
                {
                    list[i].attachEvent('onclick',advancedCheckBoxListListener.updateSpanDisplay);
                }
                else
                {
                    list[i].addEventListener('click',advancedCheckBoxListListener.updateSpanDisplay, false);
                }
        if (document.attachEvent)
        {
            selectAllCtrl.attachEvent('onclick',advancedCheckBoxListListener.selectAll);
            clearAllCtrl.attachEvent('onclick',advancedCheckBoxListListener.unselectAll);
        }
        else
        {
            selectAllCtrl.addEventListener('click',advancedCheckBoxListListener.selectAll, false);
            clearAllCtrl.addEventListener('click',advancedCheckBoxListListener.unselectAll, false);
        }
        
        advancedCheckBoxListListener.updateSpanDisplay();
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