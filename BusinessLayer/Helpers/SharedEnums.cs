using System.ComponentModel;

namespace BusinessLayer.Helpers
{
    public class SharedEnums
    {

       public static string[] ColorNames = {
            "#669999","#6699FF","Red", "Blue","#66CC99", "Green", "Yellow", "Orange",
            "Purple", "Pink", "Brown","#66CCFF", "Black", "White",
            "Gray", "Cyan", "Magenta", "Lime", "Teal",
            "Maroon", "Navy", "Olive", "Silver", "Indigo", 
            "#660066","#666600","#666699","#669900","#669966",
            "#3366CC","#33CCFF","#33CC99","#66FF00","#6666CC",
            "#0000CC","#66FFCC","#CCFF33","#339966","#003366",
            "#00CCFF","#3333CC","#000000","#CCFF00","#FF3300",
            "#999999","#0066CC","#330000","#FF9900","#CC0066",  
            "#CC99CC","#FF3399","#66CCCC","#66FF66","#3399FF",         
            "#333399" ,"#006600","#33FFFF","#99CC99","#993333",
            "#FF0099","#3300CC","#CCFF66","#CC3366","#0066CC",
            "#FFFF00","#CCCC99","#FF9999","#CC0099","#993399"
        };
        public enum CRUD_OperationType
        {
            Create,
            Read,
            Update,
            Delete
        }
        public enum TaxAuthorityTypes
        {
            [Description("EGS")]
            EGS,
            [Description("GS1")]
            GS1,         
        }
        public enum ElectronicInvoiceTypes
        {   
            select,
            Egypt,           
            KSA,
        }

        public enum ItemTypes
        {
            WithOut,
            Serial,
            ExpirationDate,
            Composite
        }

        public enum DocumentTypes
        {
            OpenBalance=1,//الرصيد الافتتاحي
            AddToStore,//اضافة الي مخزن
            StoreRecieve,//استلام مخزني
            StoreOut,//صرف مخزني
            StoreDeprecate,//اهلاك مخزني
            StoreCount,//جرد مخزني
            PurchaseInvoice,//فاتورة مشتريات
            PurchaseReturn,//مرتجع مشتريات
            SalesInvoice,//فاتورةمبيعات
            SalesReturn,//مرتجع مبيعات
            StoreSettlement,//تسوية جردية



            ManualDailyEntry,//القيود اليدوية
            SupplierCreation,//انشاء مورد
            CustomerCreation,//انشاء عميل
            CashRecieveTransaction,//استلام نقدية
            CashExchangeTransaction,//صرف نقدية
            DebitSettlementTransaction,//تسوية مدينة
            CreditSettlementTransaction,//تسوية داينة
            WriteCheckOutTransaction,//كتابة شيك صادر
            CheckOutExchangeTransaction,//صرف شيك صادر
            RecieveCheckInTransaction,//استلام شيك وارد
            CheckInExchangeTransaction,//صرف شيك وارد
            CheckReturnTransaction,//ارتجاع شيك
            CashDepositInBankTransaction,//ايداع نقدي في بنك 
            CashWithdrawalFromBankTransaction//سحب نقدي من بنك 





        }

        public enum TransactionTypes
        {
            CurruntTransaction,
            PostedTransaction,          
        }
        public enum ClassType
        {
            select,
            B,
            P,
            F            
        }
        public enum PermissionActions
        {
            Create = 1,
            Edit,
            Delete,
            List,
            Report
        }
        public enum SystemRolesNames
        {
           Super,
            Admin
        }

        public enum CustomerClassType
        {
            [Description("select")]
            select =' ',
            [Description("C")]
            EndCustomer ='C',
            [Description("B")]
            Company ='B',
            [Description("G")]
            GovernmentOrganization = 'G',
            [Description("P")]
            EgyptionPerson ='P',
            [Description("F")]
            ForeignPerson = 'F',
        }
        public enum SupplerClassType
        {
            [Description("select")]
            select = ' ',
            [Description("C")]
            EndSuppler = 'C',
            [Description("B")]
            Company = 'B',
            [Description("G")]
            GovernmentOrganization = 'G',
            [Description("P")]
            EgyptionPerson = 'P',
            [Description("F")]
            ForeignPerson = 'F',
        }
        public enum CompanyClassType
        {
            [Description("select")]
            select = ' ',
            [Description("B")]
            Company = 'B',
            [Description("P")]
            EgyptionPerson = 'P',
            [Description("F")]
            ForgenPerson = 'F',
        }

        public enum IDType
        {
            //NationalID = "NAT",
            //TaxIdentificationNumber = "TIN",
            //IqamaNumber = "IQA",
            //PassportID = "PAS",
            //CommercialregistrationNnumber = "CRN",
            //Momralicense = "MOM",
            //MLSDlicense = "MLS",
            //Sagialicense = "SAG",
            //GCC_ID = "GCC",
            //Other_ID = "OTH",
            [Description("select")]
            select = ' ',
            [Description("NAT")]
            NationalID ,
            [Description("TIN")]
            TaxIdentificationNumber ,
            [Description("IQA")]
            IqamaNumber ,
            [Description("PAS")]
            PassportID ,
            [Description("CRN")]
            CommercialregistrationNnumber,
            [Description("MOM")]
            Momralicense ,
            [Description("MLS")]
            MLSDlicense,
            [Description("SAG")]
            Sagialicense,
            [Description("GCC")]
            GCC_ID ,
            [Description("OTH")]
            Other_ID,



        }
        public enum DiscountTypes
        {
            WithOut = 1,
            Value,
            Persentage
        }

        public enum TaxTypes
        {
            T1,
            T4
        }

        public enum EGInvoiceDocumentType
        {  
            All=1,
            Invoice,
            Debit,
            Credit
        }
        public enum EGInvoiceDocumentState
        {
            All=1,
            Valid,
            Invalid,
            Cancelled,
            Rejected,
            NotSended
        }

        //schemeID="NAT",TIN,IQA,PAS,CRN,MOM,MLS,SAG,GCC,OTH
        //Value=NationalID,TaxIdentificationNumber,IqamaNumber,PassportID,CommercialregistrationNnumber,
        //Momralicense,MLSDlicense,Sagialicense,GCC ID,Other ID
        //new IDType{code="NAT",name="رقم قومي"},        
        //          new IDType{code="TIN",name="رقم تسجيل ضريبي"},               
        //          new IDType { code = "IQA", name = "رقم إقامة" },      
        //          new IDType { code = "PAS", name = "رقم جواز سفر" },           
        //          new IDType { code = "CRN", name = "رقم سجل تجاري" },         
        //          new IDType { code = "MOM", name = "Momra license" }, 
        //          new IDType { code = "MLS", name = "MLSD license" }, 
        //          new IDType { code = "SAG", name = "Sagia license" }, 
        //          new IDType { code = "GCC", name = "GCC ID" }, 
        //          new IDType { code = "OTH", name = "Other ID" }, 

        public enum AccountNatures
        {
           Debit ,//مدين
           Credit,//داين
        }
        public enum PostingAccounts
        {
            select=0,
            Budget = 1,//الميزانية
            IncomeStatement = 2,//قائمة الدخل
            Trading = 3,//المتاجرة
            Operating = 4,//التشغيل
            OngoingOperations = 5, // العمليات الجارية
        }
        public enum PostingAccountTypes
        {
            //الميزانية
            Assets = 101,//الاصول
            opponents = 102,//الخصوم
            ObligationsAndEquity = 103,//الالتزامات و حقوق الملكية

            //قائمة الدخل
            TradingRevenue = 201,//إيرادات المتاجرة
            OtherIncome = 202,//إيرادات اخرى
            TradingExpenses = 203,//مصروفات المتاجرة
            OtherExpenses = 204 //مصروفات اخرى
        }
        public enum EntryTransferState
        {
            Transfered, //مرحل
            NotTransfered, //غير مرحل
        }
        public enum EntryBalanceState
        {
            Balanced, //متزن
            NotBalanced, //غير متزن
        }
        
        public enum EntryCreationMethod
        {
            Automatic, //ألي
            Manual, //يدوي
        }

        public enum EntryType
        {
            Open, //افتتاحي
            Transaction, //حركة
        }

        public enum EntrySides
        {
            select,
            Account, 
            Supplier, 
            Customer, 
            Bank, 
            Treasury,
            Employee, 
        }
    }

}
