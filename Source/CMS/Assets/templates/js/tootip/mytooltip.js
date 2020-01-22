//tooltip plugin Jquery
//Documentation
/**
 *  Yêu cầu : jquery framework
 *  khởi chạy : $(element).myTooltip(option)
 *  Các option bao gồm :
 *  gim : Classname muốn đặt cho tooltip DIV, tiện cho việc CSS, mặc định classname là myTooltip
 *  placement: vị trí hiển thị : 2 loại là top or bottom - mặc định là top
 *  speed : Tốc độ (cái này chỉ xuất hiện với tooltip dạng trượt elastic) - đơn vị là milisecond - mặc định là 200
 *  content : nội dung của DIV, là giá trị thuộc tính của element - mặc định là title
 *  template :  hiệu ứng của tooltip - Giá trị : elastic - kiểu trượt lên trượt xuống, none - không có hiệu ứng
 *  styles : điều chỉnh CSS
 **/

$.fn.myTooltip = function (option){
    var _option = {
        gim:'myTooltip',
        placement:'top',
        speed:200,
        content:'title',
        template:'none',
        destroy:false,
        styles:{
            background:'rgba(0,0,0,0.6)',
        },
        afterShow:function(){
            
        },
    };
    $.extend(_option,option);
    return this.each(function() {  
        var obj = $(this);
        var content = (obj.attr(_option.content)) ? obj.attr(_option.content) : 'Thẻ không có title';
        var placement = _option.placement;
        var gim = _option.gim;
        var template = (_option.template == 'elastic') ? 'elastic' : 'none';
        var speed = _option.speed;
        var styles = _option.styles;
        var tooltipBody;
        //Tạo tooltip body + css inline
        if(template == 'elastic'){
            tooltipBody = '<div class="'+gim+'" style="font-weight:normal;  color:#fff; font-size:11px; z-index:1000;   border-radius:4px;  display:none;  position:absolute;  padding:2px 4px;    background:'+_option.styles.background+'" >'+content+'</div>';
        }
        else{
            switch(placement){
                case 'top' :
                    tooltipArrow = '<div style="position:absolute;  display:block;  bottom:-5px;    left:15px;    margin-left:-5px;   border-top: 5px solid '+_option.styles.background+';  border-right: 5px solid transparent;    border-left: 5px solid transparent;" ></div>';
                    tooltipInner = '<div style="max-width:600px;      text-align:center;  text-decoration:none;" >'+content+'</div>';
                    tooltipBody = '<div class="'+gim+'" style=" height:16px;    line-height:16px;    font-weight:normal;  color:#fff; font-size:11px; z-index:1000;   border-radius:3px;  display:block;  position:absolute;  padding:2px 6px;    background:'+_option.styles.background+'" >'+tooltipArrow+tooltipInner+'</div>';    
                    break;
                case 'bottom' :
                    tooltipArrow = '<div style="position:absolute;  display:block;  top:-5px;    left:15px;    margin-left:-5px;   border-bottom: 5px solid '+_option.styles.background+';  border-right: 5px solid transparent;    border-left: 5px solid transparent;" ></div>';
                    tooltipInner = '<div style="max-width:600px;      text-align:center;  text-decoration:none;" >'+content+'</div>';
                    tooltipBody = '<div class="'+gim+'" style=" height:16px;    line-height:16px;   bottom:-25px;    font-weight:normal;  color:#fff; font-size:11px; z-index:1000;   border-radius:3px;  display:block;  position:absolute;  padding:2px 6px;    background:'+_option.styles.background+'" >'+tooltipArrow+tooltipInner+'</div>';
                    break;
                case 'left' :
                    tooltipArrow = '<div style="position:absolute;  display:block;  top:5px;    right:-5px;   border-left: 5px solid '+_option.styles.background+';  border-top: 5px solid transparent;    border-bottom: 5px solid transparent;" ></div>';
                    tooltipInner = '<div style="text-align:center;  text-decoration:none;" >'+content+'</div>';
                    tooltipBody = '<div class="'+gim+'" style=" height:16px;    line-height:16px;    font-weight:normal;  color:#fff; font-size:11px; z-index:1000;   border-radius:3px;  display:block;  position:absolute;  padding:2px 6px;    background:'+_option.styles.background+'" >'+tooltipArrow+tooltipInner+'</div>';
                    break;
                case 'right' : 
                    tooltipArrow = '<div style="position:absolute;  display:block;  top:5px;    left:-5px;   border-right: 5px solid '+_option.styles.background+';  border-top: 5px solid transparent;    border-bottom: 5px solid transparent;" ></div>';
                    tooltipInner = '<div style="text-align:center;  text-decoration:none;" >'+content+'</div>';
                    tooltipBody = '<div class="'+gim+'" style=" height:16px;    line-height:16px;    font-weight:normal;  color:#fff; font-size:11px; z-index:1000;   border-radius:3px;  display:block;  position:absolute;  padding:2px 6px;    background:'+_option.styles.background+'" >'+tooltipArrow+tooltipInner+'</div>';
                    break;
                default:
                    tooltipArrow = '<div style="position:absolute;  display:block;  bottom:-5px;    left:15px;    margin-left:-5px;   border-top: 5px solid '+_option.styles.background+';  border-right: 5px solid transparent;    border-left: 5px solid transparent;" ></div>';
                    tooltipInner = '<div style="max-width:600px;      text-align:center;  text-decoration:none;" >'+content+'</div>';
                    tooltipBody = '<div class="'+gim+'" style=" height:16px;    line-height:16px;    font-weight:normal;  color:#fff; font-size:11px; z-index:1000;   border-radius:3px;  display:block;  position:absolute;  padding:2px 6px;    background:'+_option.styles.background+'" >'+tooltipArrow+tooltipInner+'</div>';    
                    break;
            }
        }
   
        //Main function
        obj.hover(function(){
            //Hiệu ứng trượt
            if(_option.destroy) return false;
            
            var objTop = obj.offset().top;
            var objLeft = obj.offset().left;
            var objW    = obj.width();
            var objH    = obj.height();
            $('body').append(tooltipBody);
            obj.removeAttr('title');
            $('.'+gim).css(styles);
            var gimW    = $('.'+gim).width();
            var gimH    = $('.'+gim).height();
            if(template == 'elastic'){
                switch(placement){
                    case 'top':
                        $('.'+gim).css({
                            'display':'block',
                            'opacity':0,
                            'top':(objTop - 30)+'px',
                            'left':((objW - gimW) / 2 + objLeft) + 'px'
                        }).animate({
                            opacity:1,
                            top:(objTop - 15)+'px'
                        },speed);
                        $('.'+gim).css(styles);
                        break;
                    case 'bottom':
                        $('.'+gim).css({
                            'display':'block',
                            'opacity':0,
                            'top':(objTop + objH +15)+'px',
                            'left':((objW - gimW) / 2 + objLeft) + 'px'
                        }).animate({
                            opacity:1,
                            top:(objTop + objH )+'px'
                        },speed);
                        //CSS option
                        $('.'+gim).css(styles);
                        break;
                }
                
            }
            else{
                switch(placement){
                    case 'top' :
                        $('.'+gim).css({
                            'top':objTop-25,
                            'left':objLeft
                        });
                        break;   
                    case 'bottom' :
                        $('.'+gim).css({
                            'top':(objTop + gimH + 5),
                            'left':objLeft
                        });
                        break; 
                    case 'left' :
                        $('.'+gim).css({
                            'top':(objTop + (objH - gimH)/2),
                            'left':(objLeft - gimW - 20)
                        });
                        break;
                    case 'right' :
                        $('.'+gim).css({
                            'top':(objTop + (objH- gimH)/2),
                            'left':(objLeft + objW + 10)
                        });
                        break;        
                }
            }
            //After show
            _option.afterShow();
        },function(){
            $('.'+gim).remove();
        })
    });
}