// Namespace for pizza builder view scripts
const pizzaSitePizzaBuilderNs = {
    updateCrustLayer: () => {
        var $crustFieldset = $('#crust-fieldset');
        var $crustImg = $('#crust-layer-img');
        var $selectedInput = $crustFieldset.find(':checked');
        var selectedCrustImgSrc = $selectedInput.attr('pb-img-src');
        $crustImg.attr('src', selectedCrustImgSrc);
    },
    updateSauceLayer: () => {
        var $sauceFieldset = $('#sauce-fieldset');
        var $sauceImg = $('#sauce-layer-img');
        var $selectedInput = $sauceFieldset.find(':checked');
        var selectedSauceImgSrc = $selectedInput.attr('pb-img-src');
        $sauceImg.attr('src', selectedSauceImgSrc);
    },
    updateAllLayers: () => {
        pizzaSitePizzaBuilderNs.updateCrustLayer();
        pizzaSitePizzaBuilderNs.updateSauceLayer();
    },
    initializeFixedScrollBuilder: () => {
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
    },
    initializePizzaBuilder: () => {
        // Disabled fixed scroll image.
        // pizzaSitePizzaBuilderNs.initializeFixedScrollBuilder();

        var $crustFieldset = $('#crust-fieldset');
        $crustFieldset.change(function () {
            pizzaSitePizzaBuilderNs.updateCrustLayer();
        });
        var $sauceFieldset = $('#sauce-fieldset');
        $sauceFieldset.change(function () {
            pizzaSitePizzaBuilderNs.updateSauceLayer();
        });

        pizzaSitePizzaBuilderNs.updateAllLayers();
    }
};