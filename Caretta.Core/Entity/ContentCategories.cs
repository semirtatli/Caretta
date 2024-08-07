namespace Caretta.Core.Entity
{
    public class ContentCategories
    {
        public Guid ContentId { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Content Content { get; set; }
    }
}
