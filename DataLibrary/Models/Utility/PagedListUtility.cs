﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Utility
{
    /// <summary>
    /// Used when an SQL query needs to be paged.
    /// </summary>
    public static class PagedListUtility
    {
        public static int GetOffset(int pageNumber, int rowsPerPage)
        {
            if (pageNumber <= 0)
            {
                return 0;
            }

            return (pageNumber - 1) * rowsPerPage;
        }

        public static int GetNumberOfPages(int rowsPerPage, int resultCount)
        {
            if (rowsPerPage <= 0 || resultCount <= 0)
            {
                return 0;
            }

            int pages = resultCount / rowsPerPage;
            int remainder = resultCount % rowsPerPage;

            if (remainder != 0)
            {
                pages += 1;
            }

            return pages;
        }
    }
}
