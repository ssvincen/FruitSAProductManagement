namespace FruitSA.Application.Shared.Product
{
    public class AddProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

    }
}
