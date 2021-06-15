namespace DataLibrary.Models.Sql
{
    public class WhereClauseItem
    {
        public WhereClauseItem(string columnName, string placeholderName, string value, ComparisonType comparisonType)
        {
            ColumnName = columnName;
            PlaceholderName = placeholderName;
            Value = value;
            ComparisonType = comparisonType;
        }

        public string ColumnName { get; set; }
        public string PlaceholderName { get; set; }
        public string Value { get; set; }
        public ComparisonType ComparisonType { get; set; }
    }
}