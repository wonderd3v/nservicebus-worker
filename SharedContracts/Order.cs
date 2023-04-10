namespace SharedContracts
{
    public class Order
    {
        public string Id { get; set; }
        public long Amount { get; set; }
        public string ArticleNumber { get; set; }
        public PaymentGateway PaymentGateway { get; set; } 
        public Customer Customer { get; set; }
    }
}