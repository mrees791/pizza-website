// Namespace for pizza builder view scripts
const pizzaSitePizzaBuilderNs = {
    initializePizzaBuilder: () => {
        var $mainNavbar = $('#main-navbar');
        var $header = $('#pb-header');
        var $body = $('body');
        var $mainContainer = $('#main-container');
        var $pizzaBuilder = $('#pizza-builder');
        var $menuSection = $('#pizza-builder-menu-section');
        var $imageSection = $('#pizza-builder-image-section');
        var pbOffset = $pizzaBuilder.offset().top;
        $(window).scroll(function () {
            var currentTop = $(document).scrollTop();
            if (currentTop >= pbOffset) {
                $imageSection.css('margin-top', currentTop - pbOffset);
            } else {
                $imageSection.css('margin-top', 0);
            }
        });
    }
};