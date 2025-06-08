namespace JampotCapstone.Models
{
    public class PaymentRequest
    {
        public string Token { get; set; }

        public int Amount { get; set; } //amount in smallest currency denomination (cents for USD)
    }
}
