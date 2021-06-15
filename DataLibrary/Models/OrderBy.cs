namespace DataLibrary.Models
{
    // todo: Remove. SqlUtility will create order by keywords.
    internal class OrderBy
    {
        public OrderBy()
        {
            SortOrder = SortOrder.Ascending;
        }

        public string OrderByColumn { get; set; }
        public SortOrder SortOrder { get; set; }

        public string GetConditions()
        {
            string conditions = $"{OrderByColumn} ";

            switch (SortOrder)
            {
                case SortOrder.Ascending:
                    conditions += "asc";
                    break;
                case SortOrder.Descending:
                    conditions += "desc";
                    break;
            }

            return conditions;
        }
    }
}