namespace BusinessLayer.Models
{
    public class HomeDashboardModel
    {
        public int PurchasesTransactionCount { get; set; }
        public int PurchasesReturnTransactionCount { get; set; }
        public int SalesTransactionCount { get; set; }
        public int SalesReturnTransactionCount { get; set; }
        public HomeDashboardModel()
        {
            PurchasesTransactionCount = 0;
            PurchasesReturnTransactionCount = 0;
            SalesTransactionCount = 0;
            SalesReturnTransactionCount = 0;
        }




    }
}
