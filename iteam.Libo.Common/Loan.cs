
namespace iteam.Libo.Common;
public class Loan
    {
        public int Id { get; set; }
        public DateTime? BorrowedDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; } = DateTime.Now;
        public DateTime? ReturnDate { get; set; }

        public int BorrowerId { get; set; }
        public virtual User Borrower { get; set; }

        public int BorrowedItemId { get; set; }
        public virtual Item BorrowedItem { get; set; }

        

        
    }
