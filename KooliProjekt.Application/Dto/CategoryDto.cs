using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CategoryProductDto> Products { get; set; }

        public CategoryDto()
        {
            Products = new List<CategoryProductDto>();
        }
    }
}
