namespace KooliProjekt.Application.Infrastructure.Files
{
    public static class FileStore
    {
        public const string Products = "products";

        // Iga toote pildid on eraldi kaustas: products/{productId}
        public static string ProductPhotos(int productId)
        {
            return Products + "/" + productId;
        }
    }
}
