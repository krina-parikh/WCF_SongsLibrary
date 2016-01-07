$(document).ready(function(){
    // Setup feature slideouts
    var slogan=$('#slogan');
    function sloganFade(fadeTo) {
        slogan.stop().fadeTo(400,fadeTo);
    }
    $('.news-short-desc').hover(function(){
        $(this).next().stop().css('visibility','visible').fadeTo(500,1);
        sloganFade(.3);
    },function(){
        $(this).next().stop().fadeTo(500,0,function(){
            $(this).css('visibility','hidden');
        });
        sloganFade(1);
    });
    $('.news-slide-out').hover(function(){
        $(this).stop().css('visibility','visible').fadeTo(500,1);
        sloganFade(.3);
    },function(){
        $(this).stop().fadeTo(500,0,function(){
            $(this).css('visibility','hidden');
        });
        sloganFade(1);
    });
    //Setup download form
    $('#soeId').blur(function(){
        if ($(this).val()!='') {
            $('#downloadButton').removeClass('disabled');
            $(this).parents('.control-group').removeClass('error').find('.help-inline').hide();
        } else {
            $('#downloadButton').addClass('disabled');
        }
    });
    $('#downloadButton').click(function(){
        if (!$(this).hasClass('disabled')) {
            $('#download-form').submit();
        } else {
            $('#soeId').focus().parents('.control-group').addClass('error').find('.help-inline').show();
        }
    });
});
$(document).on('submit', "#download-form", function(e) {
    $.fileDownload($(this).attr('action'), {
        httpMethod:'POST',
        data:$(this).serialize(),
        successCallback:function(){
            setTimeout(function(){
                $('#downloadButton').button('reset');
                $('#downloadModal').modal('hide');
            },1000);
        },
        failCallback:function(html){
            $('#download-form').prepend('<div class="alert alert-error" style="display:none"><strong>Error:</strong>'+html+'</div>')
                               .find('.alert').slideDown('slow',function(){
                var me=this;
                setTimeout(function(){
                    $(me).slideUp('slow',function(){
                        $(this).remove();
                    });
                },2000);
            });
            $('#downloadButton').button('reset');
        }
    });
    $('#downloadButton').button('loading');
    e.preventDefault();
});
$(document).ready(function(){
});
