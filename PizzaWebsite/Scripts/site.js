const pizzaSiteNs = {
    createPagination: (paginationSelector, currentPage, rowsPerPage, totalPages, totalNumberOfItems) => {
        var maxPages = 5;
        var isOnLastPage = currentPage === totalPages;
        var isOnFirstPage = currentPage === 1;

        var finalFormat = '';

        if (totalPages > 1) {
            finalFormat = '[< nc';

            for (var iPage = 3; iPage <= totalPages && iPage <= maxPages; iPage++) {
                finalFormat += 'n';
            }

            finalFormat += '! >]';
        }

        //format: '[< ncnnn! >]', // define how the navigation should look like and in which order onFormat() get's called

        $(paginationSelector).paging(totalNumberOfItems,
            { // Total number of items (elements)
                // Set up onclick handler
                onClick: function (ev) {
                },
                format: finalFormat, // define how the navigation should look like and in which order onFormat() get's called
                perpage: rowsPerPage, // show 10 elements per page
                lapping: 0, // don't overlap pages for the moment
                page: currentPage, // start at page, can also be "null" or negative
                pages: totalPages,
                onSelect: function (page) {
                    // add code which gets executed when user selects a page, how about $.ajax() or $(...).slice()?
                    //console.log(this);
                },
                onFormat: function (type) { // Gets called for each character of "format" and returns a HTML representation
                    var page = this.value;
                    const params = new URLSearchParams(window.location.search);
                    params.set('Page', page);
                    var hrefValue = `${location.pathname}?${params.toString()}`;

                    switch (type) {
                        case 'block': // n and c
                            {
                                if (page === currentPage) {
                                    return `<li class="page-item active" aria-current="page"><a class="page-link" href="#">${page}</a></li>`;
                                } else {
                                    return `<li class="page-item"><a class="page-link" href="${hrefValue}">${page}</a></li>`;
                                }
                            }
                        case 'next': // >
                            {
                                if (isOnLastPage) {
                                    return `<li class="page-item disabled"><a class="page-link" href="#" aria-disabled="true"><span aria-hidden="true">&raquo;</span></a></li>`;
                                } else {
                                    return `<li class="page-item"><a class="page-link" href="${hrefValue}" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>`;
                                }
                            }
                        case 'prev': // <<
                            {
                                if (isOnFirstPage) {
                                    return `<li class="page-item disabled"><a class="page-link" href="#" aria-disabled="true"><span aria-hidden="true">&laquo;</span></a></li>`;
                                } else {
                                    return `<li class="page-item"><a class="page-link" href="${hrefValue}" aria-label="Next"><span aria-hidden="true">&laquo;</span></a></li>`;
                                }
                            }
                        case 'first': // [
                            {
                                if (isOnFirstPage) {
                                    return `<li class="page-item disabled"><a class="page-link" href="#" aria-disabled="true">First</a></li>`;
                                } else {
                                    return `<li class="page-item"><a class="page-link" href="${hrefValue}">First</a></li>`;
                                }
                            }
                        case 'last': // ]
                            {
                                if (isOnLastPage) {
                                    return `<li class="page-item disabled"><a class="page-link" href="#" aria-disabled="true">Last</a></li>`;
                                } else {
                                    return `<li class="page-item"><a class="page-link" href="${hrefValue}">Last</a></li>`;
                                }
                            }

                    }
                }
            });
    }
};