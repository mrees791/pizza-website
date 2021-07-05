// Namespace for pizza builder view scripts
const pizzaSitePizzaBuilderNs = {
    updateCrustLayer: () => {
        var $crustFieldset = $('#crust-fieldset');
        var $crustImg = $('#crust-layer-img');
        var $selectedInput = $crustFieldset.find(':checked');
        var selectedCrustImgSrc = $selectedInput.attr('pb-img-src');
        $crustImg.attr('src', selectedCrustImgSrc);
    },
    updateSauceAmount: () => {
        var $sauceAmountFieldset = $('#sauce-amount-fieldset');
        var $sauceImg = $('#sauce-layer-img');
        var $selectedAmountInput = $sauceAmountFieldset.find(':checked');
        if ($selectedAmountInput.attr('value') === 'none') {
            $sauceImg.addClass('none-selected');
        } else {
            $sauceImg.removeClass('none-selected');
        }
    },
    updateSauceImage: () => {
        var $sauceFieldset = $('#sauce-fieldset');
        var $sauceImg = $('#sauce-layer-img');
        var $selectedSauceInput = $sauceFieldset.find(':checked');
        var selectedSauceImgSrc = $selectedSauceInput.attr('pb-img-src');
        $sauceImg.attr('src', selectedSauceImgSrc);
    },
    updateSauceLayer: () => {
        pizzaSitePizzaBuilderNs.updateSauceImage();
        pizzaSitePizzaBuilderNs.updateSauceAmount();
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
            pizzaSitePizzaBuilderNs.updateSauceImage();
        });
        var $sauceAmountFieldset = $('#sauce-amount-fieldset');
        $sauceAmountFieldset.change(function () {
            pizzaSitePizzaBuilderNs.updateSauceAmount();
        });

        pizzaSitePizzaBuilderNs.updateAllLayers();
    }
};