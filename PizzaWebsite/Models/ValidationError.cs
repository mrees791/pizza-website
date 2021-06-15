namespace PizzaWebsite.Models
{
    public class ValidationError
    {
        public ValidationError(string key, string errorMessage)
        {
            Key = key;
            ErrorMessage = errorMessage;
        }

        public string Key { get; set; }
        public string ErrorMessage { get; set; }
    }
}