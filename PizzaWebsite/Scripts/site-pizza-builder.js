// Namespace for pizza builder view scripts
const pizzaSitePizzaBuilderNs = {
    updateCrustLayer: () => {
        var $crustFieldset = $('#crust-fieldset');
        var $crustImg = $('#crust-layer-img');
        var $selectedInput = $crustFieldset.find(':checked');
        var selectedCrustImgSrc = $selectedInput.attr('pb-img-src');
        $crustImg.attr('src', selectedCrustImgSrc);
    },
    updateSauceImage: () => {
        var $sauceFieldset = $('#sauce-fieldset');
        var $sauceImg = $('#sauce-layer-img');
        var $selectedSauceInput = $sauceFieldset.find(':checked');
        var selectedSauceImgSrc = $selectedSauceInput.attr('pb-img-src');
        $sauceImg.attr('src', selectedSauceImgSrc);
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
    updateCheeseImage: () => {
        var $cheeseFieldset = $('#cheese-fieldset');
        var $cheeseImg = $('#cheese-layer-img');
        var $selectedCheeseInput = $cheeseFieldset.find(':checked');
        var selectedCheeseImgSrc = $selectedCheeseInput.attr('pb-img-src');
        $cheeseImg.attr('src', selectedCheeseImgSrc);
    },
    updateCheeseAmount: () => {
        var $cheeseAmountFieldset = $('#cheese-amount-fieldset');
        var $cheeseImg = $('#cheese-layer-img');
        var $selectedAmountInput = $cheeseAmountFieldset.find(':checked');
        if ($selectedAmountInput.attr('value') === 'none') {
            $cheeseImg.addClass('none-selected');
        } else {
            $cheeseImg.removeClass('none-selected');
        }
    },
    updateCheeseLayer: () => {
        pizzaSitePizzaBuilderNs.updateCheeseImage();
        pizzaSitePizzaBuilderNs.updateCheeseAmount();
    },
    updateSauceLayer: () => {
        pizzaSitePizzaBuilderNs.updateSauceImage();
        pizzaSitePizzaBuilderNs.updateSauceAmount();
    },
    updateAllLayers: () => {
        pizzaSitePizzaBuilderNs.updateCrustLayer();
        pizzaSitePizzaBuilderNs.updateSauceLayer();
        pizzaSitePizzaBuilderNs.updateCheeseLayer();
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
    showToppingMenu: () => {
        var $toppingMenu = $('#pb-topping-menu');
        var $crustSauceCheeseMenu = $('#pb-crust-sauce-cheese-menu');
        $crustSauceCheeseMenu.css('display', 'none');
        $toppingMenu.css('display', 'block');
    },
    showCrustSauceCheeseMenu: () => {
        var $toppingMenu = $('#pb-topping-menu');
        var $crustSauceCheeseMenu = $('#pb-crust-sauce-cheese-menu');
        $toppingMenu.css('display', 'none');
        $crustSauceCheeseMenu.css('display', 'block');
    },
    initializeToppingMenu: () => {
        $('.topping-row').each(function () {
            var $toppingRow = $(this);
            var $toppingControls = $toppingRow.children('.topping-controls');
            var $clickableToppingRow = $toppingRow.children('.clickable-topping-row');
            var $amountFieldset = $toppingControls.children('.amount-fieldset');

            $clickableToppingRow.click(function () {
                if ($toppingRow.hasClass('is-active')) {
                    $toppingRow.removeClass('is-active')
                } else {
                    $toppingRow.addClass('is-active');
                }
                pizzaSitePizzaBuilderNs.updateToppingRowControls($toppingRow);
            });

            $amountFieldset.change(function () {
                pizzaSitePizzaBuilderNs.updateToppingRowSelectedAmount($toppingRow);
            });
        });
    },
    updateToppingRowSelectedAmount: ($toppingRow) => {
        var $toppingControls = $toppingRow.children('.topping-controls');
        var $amountFieldset = $toppingControls.children('.amount-fieldset');
        var inputGroupName = $amountFieldset.attr('input-group-name');
        var checkedValue = $('input[name="' + inputGroupName + '"]:checked').val();

        if (checkedValue === 'none') {
            $toppingRow.removeClass('topping-is-selected');
        } else {
            $toppingRow.addClass('topping-is-selected');
        }
    },
    updateToppingRowControls: ($toppingRow) => {
        var $toppingControls = $toppingRow.children('.topping-controls');
        if ($toppingRow.hasClass('is-active')) {
            $toppingControls.slideDown();
        } else {
            $toppingControls.slideUp();
        }
    },
    updateAllToppingRowsControls: () => {
        $('.topping-row').each(function () {
            pizzaSitePizzaBuilderNs.updateToppingRowControls($(this));
        });
    },
    updateAllToppingRowSelectedAmounts: () => {
        $('.topping-row').each(function () {
            pizzaSitePizzaBuilderNs.updateToppingRowSelectedAmount($(this));
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
        var $cheeseFieldset = $('#cheese-fieldset');
        $cheeseFieldset.change(function () {
            pizzaSitePizzaBuilderNs.updateCheeseImage();
        });
        var $cheeseAmountFieldset = $('#cheese-amount-fieldset');
        $cheeseAmountFieldset.change(function () {
            pizzaSitePizzaBuilderNs.updateCheeseAmount();
        });
        var $crustSauceCheeseButton = $('#tab-button-crust-sauce-cheese');
        $crustSauceCheeseButton.click(function () {
            $toppingButton.removeClass('selected-tab');
            $crustSauceCheeseButton.addClass('selected-tab');
            pizzaSitePizzaBuilderNs.showCrustSauceCheeseMenu();
        });
        var $toppingButton = $('#tab-button-topping');
        $toppingButton.click(function () {
            $crustSauceCheeseButton.removeClass('selected-tab');
            $toppingButton.addClass('selected-tab');
            pizzaSitePizzaBuilderNs.showToppingMenu();
        })
        pizzaSitePizzaBuilderNs.showCrustSauceCheeseMenu();
        pizzaSitePizzaBuilderNs.updateAllLayers();
        pizzaSitePizzaBuilderNs.initializeToppingMenu();

        pizzaSitePizzaBuilderNs.updateAllToppingRowSelectedAmounts();
        pizzaSitePizzaBuilderNs.updateAllToppingRowsControls();
    }
};