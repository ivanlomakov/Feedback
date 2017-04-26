/* jQuery Star Rating Plugin
 * 
 * @Author
 * Copyright Nov 02 2010, Irfan Durmus - http://irfandurmus.com/
 *
 * @Version
 * 0.3b
 *
 * @License
 * Dual licensed under the MIT or GPL Version 2 licenses.
 *
 * Visit the plugin page for more information.
 * http://irfandurmus.com/projects/jquery-star-rating-plugin/
 *
 */

; (function ($) {

    var imageCode = 'data:image/png;base64,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';

    $.fn.rating = function(callback){
        
        callback = callback || function(){};

        // each for all item
        this.each(function(i, v){
            
            $(v).data('rating', {callback:callback})
                .bind('init.rating', $.fn.rating.init)
                .bind('set.rating', $.fn.rating.set)
                .bind('hover.rating', $.fn.rating.hover)
                .trigger('init.rating');
        });
    };
    
    $.extend($.fn.rating, {
        init: function(e){
            var el = $(this),
                list = '',
                isChecked = null,
                childs = el.children(),
                i = 0,
                l = childs.length;
            
            for (; i < l; i++) {
                list = list + '<a class="star" title="' + $(childs[i]).val() + '" ><img src="' + imageCode + '"/><a/>';
                if ($(childs[i]).is(':checked')) {
                    isChecked = $(childs[i]).val();
                };
            };
            
            childs.hide();
            
            el
                .append('<div class="stars">' + list + '</div>')
                .trigger('set.rating', isChecked);
            
            $('a', el).bind('click', $.fn.rating.click);            
            el.trigger('hover.rating');
        },
        set: function(e, val) {
            var el = $(this),
                item = $('a', el),
                input = undefined;
            
            if (val) {
                item.removeClass('fullStar');
                
                input = item.filter(function(i){
                    if ($(this).attr('title') == val)
                        return $(this);
                    else
                        return false;
                });
                
                input
                    .addClass('fullStar')
                    .prevAll()
                    .addClass('fullStar');
            }
            
            return;
        },
        hover: function(e){
            var el = $(this),
                stars = $('a', el);
            
            stars.bind('mouseenter', function(e){
                // add tmp class when mouse enter
                $(this)
                    .addClass('tmp_fs')
                    .prevAll()
                    .addClass('tmp_fs');
                
                $(this).nextAll()
                    .addClass('tmp_es');
            });
            
            stars.bind('mouseleave', function(e){
                // remove all tmp class when mouse leave
                $(this)
                    .removeClass('tmp_fs')
                    .prevAll()
                    .removeClass('tmp_fs');
                
                $(this).nextAll()
                    .removeClass('tmp_es');
            });
        },
        click: function (e) {
            e.preventDefault();
            var el = $(e.target).parent(),
                container = el.parent().parent(),
                inputs = container.children('input'),
                rate = el.attr('title');
                
            matchInput = inputs.filter(function(i){
                if ($(this).val() == rate)
                    return true;
                else
                    return false;
            });
            
            matchInput
                .change()
                .prop('checked', true)
				.siblings('input').prop('checked', false);
            
            container
                .trigger('set.rating', matchInput.val())
                .data('rating').callback(rate, e);
        }
    });
    
})(jQuery);
