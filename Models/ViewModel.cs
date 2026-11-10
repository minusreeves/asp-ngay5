namespace qlsanpham.Models
{
    public class ViewModel
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string ?Description { get; set; }
        public float Price { get; set; }
        public int CategoryId   { get; set; }
        public string ?CategoryName { get; set; }
    }
}
