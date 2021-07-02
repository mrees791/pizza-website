// Namespace for pizza builder view scripts
const pizzaSitePizzaBuilderNs = {
    updatePizzaBuilderCanvas: () => {
        // Update selected crust
        var $crustSelect = $('#crust-select');
        var $selectedCrustOption = $crustSelect.find(':selected');
        var selectedCrustImgSrc = $selectedCrustOption.attr('pb-img-src')

        var canvas = document.getElementById('pizza-builder-canvas');
        if (canvas.getContext) {
            console.log('Canvas element is supported by this browser.');
            var context = canvas.getContext('2d');

            // Initialize layers
            var crustImg = new Image();
            crustImg.src = selectedCrustImgSrc;

            // Draw layers
            context.drawImage(crustImg, 0, 0);
        } else {
            console.log('Canvas element is not supported by this browser.');
        }
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
    }
};