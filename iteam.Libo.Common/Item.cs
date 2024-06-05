namespace iteam.Libo.Common;
    public class Item
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }

        public virtual ICollection<Loan> Loans { get; set; }
    }
