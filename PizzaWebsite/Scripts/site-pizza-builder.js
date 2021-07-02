// Namespace for pizza builder view scripts
const pizzaSitePizzaBuilderNs = {
    updateCrustLayer: () => {
        var $crustFieldset = $('#crust-fieldset');
        var $crustImg = $('#crust-layer-img');
        var $selectedInput = $crustFieldset.find(':checked');
        var selectedCrustImgSrc = $selectedInput.attr('pb-img-src');
        $crustImg.attr('src', selectedCrustImgSrc);
    },
    updateAllLayers: () => {
        pizzaSitePizzaBuilderNs.updateCrustLayer();
    },
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

        // test crust fieldset
        var $crustFieldset = $('#crust-fieldset');
        $crustFieldset.change(function () {
            pizzaSitePizzaBuilderNs.updateCrustLayer();
        });

        pizzaSitePizzaBuilderNs.updateAllLayers();
    }
};