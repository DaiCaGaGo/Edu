$(document).ready(function(){
    $(window).scroll(function(){

    });

    // Button scroll to top
    $(".scrollToTop").click(function() {
        $("html, body").animate({
            scrollTop: 0
        }, 1000);
        return false
    });

    // Button scroll to bottom
    $(".scrollTobottom").click(function() {
        $("html, body").animate({
            scrollTop: $(document).height()
        }, 800);
        return false
    });  

    // Scroll mouse
    /*
    $(window).scroll(function(){
        if($(this).scrollTop()>80){
            $('.main-header').addClass('fixed-head');
        } 
        else{
            $('.main-header').removeClass('fixed-head');
        }
    });
    */

    // Click to close
    $(".close-here").click(function(){
        var dataHere = $(this).attr("data-here");
        var dataSwing = $(this).attr("data-swing");
        $objectHere = $(this).parent().find("#"+dataHere);
        $objectIcon = $(this).parent().find("#icon-here");

        if (dataSwing == "yes") {
            $objectIcon.addClass("icon-here-swing");
            $objectHere.slideUp("slow");
            $(this).attr("data-swing", "no");
            $(this).removeClass("active");
        }
        else if(dataSwing == "no"){
            $objectIcon.removeClass("icon-here-swing");
            $objectHere.slideDown("slow");
            $(this).attr("data-swing", "yes");
            $(this).addClass("active");
        }
        else{
            $objectIcon.addClass("icon-here-swing");
            $objectHere.slideUp("slow");
            $(this).attr("data-swing", "no");
            $(this).removeClass("active");
        }

    });

    $('.btn-down').click(function(){
        var showMenuMobile = $(this).attr("data-show");
        if (showMenuMobile == "yes") {
            $(this).parent().children('ul').hide("slow");
            $(this).parent().children('ul').removeClass("shower");
            $(this).attr("data-show", "no");
            $(this).removeClass('icon-here-swing');
        }
        else {
            //$('.btn-down').parent().children('ul').hide("slow");
            //$('.btn-down').attr("data-show", "no");

            $(this).parent().children('ul').show("slow");
            $(this).parent().children('ul').addClass("shower");
            $(this).attr("data-show", "yes");
            $(this).addClass('icon-here-swing');
        }
    });

    // Menu mobile
    var menushow = true;
    $('.btnMenuMobile').click(function(){
        if(menushow){
            $('#wrap').animate({right:'280px'},'slow');
            $('#wrap').addClass('open');
            $('.bgMenu').addClass('bgMenuShow');
            menushow = false;
        }
        else{
            $('#wrap').animate({right:'0px'},'slow');
            $('#wrap').removeClass('open');
            $('.bgMenu').removeClass('bgMenuShow');
            menushow = true;
        }
        return false;
    });

    $('.bgMenu').click(function(){
        if(menushow){
            $('#wrap').animate({right:'280px'},'slow');
            $('#wrap').addClass('open');
            $('.bgMenu').addClass('bgMenuShow');
            menushow = false;
        }
        else{
            $('#wrap').animate({right:'0px'},'slow');
            $('#wrap').removeClass('open');
            $('.bgMenu').removeClass('bgMenuShow');
            menushow = true;
        }
        return false;
    });
});