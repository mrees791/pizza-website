// Namespace for pizza builder view scripts
const pizzaSitePizzaBuilderNs = {
    initializePizzaBuilder: () => {
        var $mainNavbar = $('#main-navbar');
        var $pizzaBuilder = $('#pizza-builder');
        var $imageSection = $('#pizza-builder-image-section');

        $(window).scroll(function () {
            var currentTop = $(document).scrollTop();
            var pbOffset = $pizzaBuilder.offset().top;
            var navbarHeight = $mainNavbar.height();
            var marginOffset = pbOffset - navbarHeight;
            if (currentTop >= marginOffset) {
                $imageSection.css('margin-top', currentTop - marginOffset);
            } else {
                $imageSection.css('margin-top', 0);
            }
        });
    }
};