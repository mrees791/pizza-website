namespace PizzaWebsite.Models
{
    public class ErrorMessageViewModel
    {
        public string Header { get; set; }
        public string ErrorMessage { get; set; }
        public string ReturnUrlAction { get; set; }
        public bool ShowReturnLink { get; set; }
    }
}