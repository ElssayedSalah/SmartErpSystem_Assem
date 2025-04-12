using BusinessLayer.Services;
using DataAccessLayer.Entities.Financial;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Seeds
{
   public static class BasicData
    {
        public static async Task SeedBasicDataAsync(IBaseService<FinancialPeriod> _FinancialPeriodService, IBaseService<Currency> _CurrencyService, IBaseService<Unit> _UnitService, IBaseService<Document> _DocumentService, IBaseService<Taxes> _TaxesService)
        {
            //seed FinancialPeriods ==================================================================
            var FinancialPeriods = _FinancialPeriodService.GetAll().Any();
            if (!FinancialPeriods)
            {
                var FinancialPeriod = new FinancialPeriod() { Year = DateTime.Now.Year, DateFrom = new DateTime(DateTime.Now.Year, 1, 1), DateTo = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)) };
                _FinancialPeriodService.Add(FinancialPeriod);
            }

            //seed Taxes ==================================================================
            var Taxes = _TaxesService.GetAll().Any();
            if (!Taxes)
            {
                _TaxesService.Add(new Taxes() {NameAr="ضريبة القيمة المضافة" , NameEn="VAT" , Rate=15, TaxId=1} );
            }

            //seed Currencys =======================================================================
            var Currencys = _CurrencyService.GetAll().Any();
                if (!Currencys)
                {
                    var CurrencyList = new List<Currency>() {
                       new Currency(){ Code=1, NameAr="جنية مصري" , NameEn="EGP", TaxAuthorityCode="EG", DefaultCurrency=true , CurrencyChangrRate=1, ActivationState=true },

                       new Currency(){ Code=2, NameAr="ريال سعودي" , NameEn="Rial Ksa", TaxAuthorityCode="", DefaultCurrency=false , CurrencyChangrRate=10, ActivationState=true },

                        new Currency(){ Code=3, NameAr="دولار أمريكي" , NameEn="Dolar", TaxAuthorityCode="", DefaultCurrency=false , CurrencyChangrRate=60, ActivationState=true },

                    };
                    await _CurrencyService.AddRange(CurrencyList);
                }

                //seed Units ===============================================================================
                var Units = _UnitService.GetAll().Any();
                if (!Units)
                {
                    var UnitList = new List<Unit>() {
                       new Unit(){ Code=1, NameAr="كيلو جرام" , NameEn="KG", TaxAuthorityCode="KGM", ActivationState=true },

                       new Unit(){ Code=2, NameAr="متر" , NameEn="Meter", TaxAuthorityCode="MTR", ActivationState=true },

                        new Unit(){ Code=3, NameAr="وحدة" , NameEn="Unit", TaxAuthorityCode="C62", ActivationState=true },

                    };
                    await _UnitService.AddRange(UnitList);
                }

                //seed Documents ===============================================================================
                var Documents = _DocumentService.GetAll().Any();
                if (!Documents)
                {
                    var DocumentList = new List<Document>() {
                       new Document(){ NameAr="الرصيد الإفتتاحي" , NameEn="OpenBalance", DocSign=1, DocType="I", DocTypeId=1},
                       new Document(){ NameAr="إضافة إلي مخزن" , NameEn="AddToStore", DocSign=1, DocType="I", DocTypeId=2},
                       new Document(){ NameAr="إستلام مخزني" , NameEn="StoreRecieve", DocSign=1, DocType="I", DocTypeId=3},
                       new Document(){ NameAr="صرف مخزني" , NameEn="StoreOut", DocSign=-1, DocType="I", DocTypeId=4},
                       new Document(){ NameAr="إهلاك مخزني" , NameEn="StoreDeprecate", DocSign=-1, DocType="I", DocTypeId=5},
                       new Document(){ NameAr="جرد مخزني" , NameEn="StoreDestructible", DocSign=0, DocType="I", DocTypeId=6},
                       new Document(){ NameAr="فاتورة مشتريات" , NameEn="PurchaseInvoice", DocSign=1, DocType="I", DocTypeId=7},
                       new Document(){ NameAr="مرتجع مشتريات" , NameEn="ReturenPurchaseInvoice", DocSign=-1, DocType="I", DocTypeId=8},
                       new Document(){ NameAr="فاتورة مبيعات" , NameEn="SalesInvoice", DocSign=-1, DocType="I", DocTypeId=9},
                       new Document(){ NameAr="مرتجع مبيعات" , NameEn="SalesInvoice", DocSign=1, DocType="I", DocTypeId=10},
                       new Document(){ NameAr="تسوية جردية" , NameEn="StoreSettlement", DocSign=0, DocType="I", DocTypeId=11},
                       new Document(){ NameAr="القيود اليومية" , NameEn="ManualDailyEntry", DocSign=0, DocType="F", DocTypeId=12},
                       new Document(){ NameAr="تعريف مورد" , NameEn="Supplier", DocSign=0, DocType="I", DocTypeId=13},
                       new Document(){ NameAr="تعريف عميل" , NameEn="Customer", DocSign=0, DocType="I", DocTypeId=14},
                       new Document(){ NameAr="إستلام نقدي" , NameEn="Cash Recieve", DocSign=0, DocType="F", DocTypeId=15},
                       new Document(){ NameAr="صرف نقدية" , NameEn="Cash Exchange", DocSign=0, DocType="F", DocTypeId=16},
                       new Document(){ NameAr="تسوية مدينة" , NameEn="Debit Settlement", DocSign=0, DocType="F", DocTypeId=17},
                       new Document(){ NameAr="تسوية دائنة" , NameEn="Credit Settlement", DocSign=0, DocType="F", DocTypeId=18},
                       new Document(){ NameAr="كتابة شيك صادر" , NameEn="Write Check Out", DocSign=0, DocType="F", DocTypeId=19},
                       new Document(){ NameAr="صرف شيك صادر" , NameEn="Check Out Exchange", DocSign=0, DocType="F", DocTypeId=20},
                       new Document(){ NameAr="إستلام شيك وارد" , NameEn="Recieve Check In", DocSign=0, DocType="F", DocTypeId=21},
                       new Document(){ NameAr="صرف شيك وارد" , NameEn="Check In Exchange", DocSign=0, DocType="F", DocTypeId=22},
                       new Document(){ NameAr="إرتجاع شيك" , NameEn="Check Return", DocSign=0, DocType="F", DocTypeId=23},
                       new Document(){ NameAr="إيداع نقدي في بنك" , NameEn="Cash Deposit In Bank", DocSign=0, DocType="F", DocTypeId=24},
                       new Document(){ NameAr="سحب نقدي من بنك" , NameEn="Cash With drawal From Bank", DocSign=0, DocType="F", DocTypeId=25},


                    };
                    await _DocumentService.AddRange(DocumentList);
                }


        }
    }
}
