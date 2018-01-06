//(function ($) {
//    'use strict';

//    $(document).ready(function ()
//    { 
//        var counter = -1;
//        var $slides = $('[data-slides]');
//        var images = $slides.data('slides');
//        var count = images.length;
//        if (count > 0) {
//            var slideshow = function () {
//                if (counter < count - 1)
//                    ++counter;
//                else
//                    counter = 0;

//                $('slideshow')
//                    .attr("src", images[counter])
//                    .fadeIn()
//                    .show(0, function () {
//                        setTimeout(change, 10000);
//                    });
//               // $slides
//                   // .css('background-image', 'url("' + images[counter] + '")')
//                   // .fadeIn()
//                   // .show(0, function () {
//                    //    setTimeout(change, 10000);
//                   // });
//            };

//            var change = function () {
//                $('.askanio-background-slideshow').fadeOut(1000, slideshow);
//            };

//            slideshow();
//        }
//    });

//}(jQuery));