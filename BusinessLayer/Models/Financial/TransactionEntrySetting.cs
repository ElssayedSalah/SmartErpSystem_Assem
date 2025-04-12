namespace BusinessLayer.Models.Financial
{
    public class TransactionEntrySetting
    {      
       
        public int DailyTypeId { get; set; }
       
        public bool TransferState { get; set; }
       
        public int FirstSideAccountId { get; set; }
       
        public int SecondSideAccountId { get; set; }

        public int VAT_TaxAccountId { get; set; }

        public int FirstSideNaturalId { get; set; }
        public int SecondSideNaturalId { get; set; }

        public bool IsFirstSideTaxble { get; set; }
        public bool IsSecondSideTaxble { get; set; }
        public bool EnableEntryCreation { get; set; }

    }
}
