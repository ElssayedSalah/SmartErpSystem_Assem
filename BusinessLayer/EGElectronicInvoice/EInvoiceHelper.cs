using BusinessLayer.Models.Inventory;
using DataAccessLayer.Data;
using DataAccessLayer.Entities.Inventory;
using DataAccessLayer.Entities.Purchases;
using DataAccessLayer.Entities.Sales;
using DataAccessLayer.Entities.System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.EGElectronicInvoice
{
  public static class EInvoiceHelper
    {
        public static AccessTokenInfo _AccessToken;       
        public static HttpClient _SystemAPIHttpClient = new HttpClient();
        public static HttpClient _IdentityServiceHttpClient = new HttpClient();
       
        /// <summary>
        /// 
        /// </summary>
        public static async void FetchToken()
        {
            try
            {
                using (_IdentityServiceHttpClient = new HttpClient())
                {
                    _IdentityServiceHttpClient.BaseAddress = new Uri(EGInvoiceCredentials.IdentityServiceUrl);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    _IdentityServiceHttpClient.DefaultRequestHeaders.Clear();

                    var credentials = new List<KeyValuePair<string, string>>();
                    credentials.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                    credentials.Add(new KeyValuePair<string, string>("client_id", EGInvoiceCredentials.Client_ID));
                    credentials.Add(new KeyValuePair<string, string>("client_secret", EGInvoiceCredentials.Client_Secret));

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/connect/token");
                    var content = new FormUrlEncodedContent(credentials);
                    requestMessage.Content = content;
                    requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    using (var response = await _IdentityServiceHttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
                    {
                        var responseBody =await  response.Content.ReadAsStringAsync();
                        AccessTokenInfo accessToken = JsonConvert.DeserializeObject<AccessTokenInfo>(responseBody.ToString());
                        _AccessToken = accessToken;
                       //return "";
                    }
                }
            }
            catch (Exception ex)
            {
               // return ex.ToString();
            }
        }
        public async static Task<SubmissionDocumentResult> SendInvoiceToApi(List<EInvoiceDocument> documents)
        {
            try
            {
                using (_SystemAPIHttpClient = new HttpClient())
                {
                    Signer signer = new Signer();
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    _SystemAPIHttpClient.BaseAddress = new Uri(EGInvoiceCredentials.SystemAPIUrl);
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Clear();
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _SystemAPIHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _AccessToken.Access_Token != null ? _AccessToken.Access_Token : "");
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/documentsubmissions");
                    //convert document to json
                    var JsonInvoice = JsonConvert.SerializeObject(documents.FirstOrDefault());
                    //توقيع الفاتورة
                    var signedInvoice = signer.SignInvoice(JsonInvoice);
                    SubmissionDocumentResult result = new SubmissionDocumentResult();
                    if (signedInvoice == string.Empty)
                    {
                        result.SubmissionId = "! حدث خطأ أثناء توقيع الفاتورة" + "\nتأكد من توصيل توكن التوقيع بالجهاز\nوكلمة المرور صحيحة \n";
                    }
                    else
                    {
                        var content = new StringContent(signedInvoice, Encoding.UTF8, "application/json");
                        requestMessage.Content = content;
                        requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        var response = await _SystemAPIHttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);// send document                                            
                        if (response.IsSuccessStatusCode)
                        {
                            var responseVal = response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<SubmissionDocumentResult>(responseVal.Result);
                        }
                        else
                        {
                            result.SubmissionId = response.StatusCode.ToString();
                        }

                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                return new SubmissionDocumentResult();
            }
        }

        public async static Task<bool> CancelInvoiceToApi(string documentUUID, string reson)
        {
            try
            {
                using (_SystemAPIHttpClient = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    _SystemAPIHttpClient.BaseAddress = new Uri(EGInvoiceCredentials.SystemAPIUrl);
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Clear();
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _SystemAPIHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _AccessToken.Access_Token != null ? _AccessToken.Access_Token : "");

                    var stringjsonData = @"{'status': 'cancelled', 'reason':'" + reson + "'}";
                    var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/v1.0/documents/state/" + documentUUID + "/state");

                    var content = new System.Net.Http.StringContent(stringjsonData, Encoding.UTF8, "application/json");
                    requestMessage.Content = content;
                    requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await _SystemAPIHttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    return response.IsSuccessStatusCode ? true : false;

                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async static Task<DocumentDetailsResult> GetInvoiceDetailsFromApi(string documentUUID)
        {
            try
            {
                using (_SystemAPIHttpClient = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    _SystemAPIHttpClient.BaseAddress = new Uri(EGInvoiceCredentials.SystemAPIUrl);
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Clear();
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _SystemAPIHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _AccessToken.Access_Token != null ? _AccessToken.Access_Token : "");
                    DocumentDetailsResult result = new DocumentDetailsResult();
                    var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/documents/" + documentUUID + "/details");
                    var response = await _SystemAPIHttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseVal = response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<DocumentDetailsResult>(responseVal.Result);
                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<EInvoiceDocument> CreateEInvoiceDocument(Transaction_InvMaster InvMaster, Company company,Branch branch,List<Item> items, List<Unit> units, Currency currency, Taxes Tax, string documentType , Customer customer = null, Suppler supplier = null)
        {
            List<EInvoiceDocument> EInvoiceDocuments = new List<EInvoiceDocument>();
            EInvoiceDocument EInvoiceDocument;
            Customer InvCustomer = new Customer();  

            if (documentType == "D")
            {
                InvCustomer = new Customer { NationalID = supplier.NationalID, NameAr = supplier.NameAr, CountryCode = supplier.CountryCode, Governate = supplier.Governate, RegionCity = supplier.RegionCity, Street = supplier.Street, BuildingNumber = supplier.BuildingNumber, ClassType = supplier.ClassType };
            }
            else
            {
                InvCustomer = new Customer { NationalID = customer.NationalID, NameAr = customer.NameAr, CountryCode = customer.CountryCode, Governate = customer.Governate, RegionCity = customer.RegionCity, Street = customer.Street, BuildingNumber = customer.BuildingNumber, ClassType = customer.ClassType };
            }

            EInvoiceDocument = new EInvoiceDocument()
            {
                //بيانات البائع //بيانات الشركة
                issuer = new Issuer
                {
                    address = new Address
                    {
                        branchID = branch.TaxAuthorityCode,
                        country = company.CountryCode,
                        governate = company.Governate,
                        regionCity = company.RegionCity,
                        street = company.Street,
                        buildingNumber = company.BuildingNumber,
                        postalCode = "",
                        floor = "",
                        room = "",
                        landmark = "",
                        additionalInformation = ""
                    },
                    type = company.ClassType,
                    id = company.TaxAuthorityRegestrationNumber,
                    name = company.NameAr

                },
                //المشتري-العميل
                receiver = new Receiver
                {
                    address = new Address
                    {
                        branchID = "0",
                        country = InvCustomer.CountryCode,
                        governate = InvCustomer.Governate,
                        regionCity = InvCustomer.RegionCity,
                        street = InvCustomer.Street,
                        buildingNumber = InvCustomer.BuildingNumber,
                        postalCode = "",
                        floor = "",
                        room = "",
                        landmark = "",
                        additionalInformation = ""
                    },
                    type = InvCustomer.ClassType,
                    id = InvCustomer.NationalID,
                    name = InvCustomer.NameAr
                },
                documentType = documentType,
                documentTypeVersion = "1.0",
                dateTimeIssued = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"),
                taxpayerActivityCode = company.TaxPayerActivityCode,
                internalID = InvMaster.Code.ToString(),
                purchaseOrderReference = "",
                purchaseOrderDescription = "",
                salesOrderReference = "",
                salesOrderDescription = "",
                proformaInvoiceNumber = "",

                totalDiscountAmount = 0,// (double)Math.Round(InvMasterModel.TotalDiscounts * InvMasterModel.CurrencyFactor,5),
                totalSalesAmount = 0,// (double)Math.Round(InvMasterModel.InvoiceValue * InvMasterModel.CurrencyFactor,5),
                netAmount =0,// (double)Math.Round(InvMasterModel.InvoiceNet * InvMasterModel.CurrencyFactor,5),
                totalAmount =0,// (double)Math.Round(InvMasterModel.InvoiceTotal * InvMasterModel.CurrencyFactor,5),

                extraDiscountAmount = 0,
                totalItemsDiscountAmount = 0,
                invoiceLines = new List<InvoiceLine>(),
                taxTotals = new List<TaxTotals>(){new TaxTotals{ TaxType = Tax.Type, Amount=0 /*InvMasterModel.TaxValue * currency.CurrencyChangrRate*/}
                }
            };          

           CreateEInvoiceLines(EInvoiceDocument, InvMaster.Transaction_InvDetails,items,units,currency,Tax, documentType);
            EInvoiceDocuments.Add(EInvoiceDocument);
            return EInvoiceDocuments;
        }

        public static void CreateEInvoiceLines(EInvoiceDocument EInvoiceDocument,List<Transaction_InvDetails> invDetails, List<Item> items, List<Unit> units, Currency currency,Taxes Tax,string documentType)
        {
            List<InvoiceLine> InvoiceLines = new List<InvoiceLine>();
            decimal ItemPrice = 0;
            InvoiceLines = invDetails.Select(x =>
            {
            InvoiceLine l = new InvoiceLine();
            var item = items.Where(i => i.Id == x.ItemId).FirstOrDefault();
            var unit = units.Where(i => i.Id == x.UnitId).FirstOrDefault();

            if (item != null)
            {
                    if (documentType == "I")
                    {
                        ItemPrice = x.PriceAfterDiscount.Value;
                    }
                    else if(documentType == "C")
                    {
                        ItemPrice = x.SalesPrice.Value;

                    }
                    else
                    {
                        ItemPrice = x.PurchasePrice.Value;

                    }

                    var taxableItems = new List<TaxableItems>();
                taxableItems.Add(new TaxableItems() { taxType = Tax.Type, amount = Math.Round((x.Quntity * ItemPrice * currency.CurrencyChangrRate * Tax.Rate) / 100, 5), subType = "V009", rate = Tax.Rate });

                l.description = item.NameAr;
                l.itemType = item.TaxAuthorityType;
                l.itemCode = item.TaxAuthorityCode;
                l.unitType = unit.TaxAuthorityCode;
                l.quantity = (double)x.Quntity;
                l.internalCode = item.Code.ToString();
                l.salesTotal = (double)Math.Round(x.Quntity * ItemPrice * currency.CurrencyChangrRate, 5);
                l.total = (double)Math.Round((x.Quntity * ItemPrice) + (x.Quntity * ItemPrice * Tax.Rate / 100), 5);
                l.valueDifference = 0;
                l.totalTaxableFees = 0;
                l.netTotal = (double)Math.Round(x.Quntity * ItemPrice * currency.CurrencyChangrRate, 5);
                l.itemsDiscount = 0;
                l.unitValue = new UnitValue { currencySold = currency.TaxAuthorityCode, amountEGP = (double)Math.Round(ItemPrice * currency.CurrencyChangrRate, 5), amountSold = currency.TaxAuthorityCode != "EGP" ? (double)ItemPrice : 0, currencyExchangeRate = currency.TaxAuthorityCode != "EGP" ? (double)currency.CurrencyChangrRate : 0 };
                l.discount = new Discount { rate = x.DiscountValue.Value, amount = Math.Round(x.Quntity * (ItemPrice * currency.CurrencyChangrRate) * x.DiscountValue.Value / 100,5) };
                l.taxableItems = taxableItems;


                    var T1_Amounts = Math.Round(x.Quntity * ItemPrice * currency.CurrencyChangrRate * Tax.Rate / 100, 5);
                    EInvoiceDocument.taxTotals.FirstOrDefault().Amount += T1_Amounts;
                    
                    var TotalAmount = (double)Math.Round(((x.Quntity * ItemPrice) + (x.Quntity * ItemPrice * Tax.Rate / 100)) * currency.CurrencyChangrRate, 5);
                    EInvoiceDocument.totalAmount += TotalAmount;

                    var SalesTotal = Math.Round(x.Quntity * ItemPrice * currency.CurrencyChangrRate, 5);
                    EInvoiceDocument.totalSalesAmount += (double)SalesTotal;

                    var NetAmount = Math.Round(x.Quntity * ItemPrice * currency.CurrencyChangrRate, 5);
                    EInvoiceDocument.netAmount += (double)NetAmount;

                }
                return l;
            }).ToList();

            EInvoiceDocument.invoiceLines = InvoiceLines;

        }

        public async static Task<bool> GenerateEGSCode(itemCode Item)
        {
            try
            {
                using (_SystemAPIHttpClient = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    _SystemAPIHttpClient.BaseAddress = new Uri(EGInvoiceCredentials.SystemAPIUrl);
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Clear();
                    _SystemAPIHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _SystemAPIHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _AccessToken.Access_Token != null ? _AccessToken.Access_Token : "");
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/codetypes/requests/codes");

                    var items = JsonConvert.SerializeObject(Item);//convert document to json                   
                    SubmissionDocumentResult result = new SubmissionDocumentResult();
                    var content = new System.Net.Http.StringContent(items, Encoding.UTF8, "application/json");
                    requestMessage.Content = content;
                    requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    var response = await _SystemAPIHttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);//send document   

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static string ValidateCustomerForEInvoice(Customer customer)
        {
            string msg = "";           
            if (customer != null)
            {
                if (customer.NationalID == null || customer.NationalID == "")
                {
                    msg += "يجب ادخال الرقم القومي للعميل \n";
                }
                if (customer.CountryCode == null || customer.CountryCode == "")
                {
                    msg += "يجب ادخال الدولة للعميل \n";
                }
                if (customer.Governate == null || customer.Governate == "")
                {
                    msg += "يجب ادخال المحافظة \n";
                }
                if (customer.RegionCity == null || customer.RegionCity == "")
                {
                    msg += "يجب ادخال المنطقة \n";
                }
                if (customer.Street == null || customer.Street == "")
                {
                    msg += "يجب ادخال الشارع \n";
                }
                if (customer.BuildingNumber == null || customer.BuildingNumber == "")
                {
                    msg += "يجب ادخال رقم المبني";
                }

            }
            else
            {
                msg = "يجب اختيار عميل";
            }

            return msg;


        }
        public static string ValidateSuplierForEInvoice(Suppler suppler)
        {
            string msg = "";
            if (suppler != null)
            {
                if (suppler.NationalID == null || suppler.NationalID == "")
                {
                    msg += "يجب ادخال الرقم القومي للمورد \n";
                }
                if (suppler.CountryCode == null || suppler.CountryCode == "")
                {
                    msg += "يجب ادخال الدولة للمورد \n";
                }
                if (suppler.Governate == null || suppler.Governate == "")
                {
                    msg += "يجب ادخال المحافظة \n";
                }
                if (suppler.RegionCity == null || suppler.RegionCity == "")
                {
                    msg += "يجب ادخال المنطقة \n";
                }
                if (suppler.Street == null || suppler.Street == "")
                {
                    msg += "يجب ادخال الشارع \n";
                }
                if (suppler.BuildingNumber == null || suppler.BuildingNumber == "")
                {
                    msg += "يجب ادخال رقم المبني";
                }

            }
            else
            {
                msg = "يجب اختيار للمورد";
            }

            return msg;


        }
        public static string ValidateCompanyForEInvoice(Company company)
        {
            string msg = "";
            if (company != null)
            {
                if (company.TaxAuthorityRegestrationNumber == null || company.TaxAuthorityRegestrationNumber == "")
                {
                    msg += "يجب ادخال رقم تسجيل الشركة \n";
                }
                if (company.TaxPayerActivityCode == null || company.TaxPayerActivityCode == "")
                {
                    msg += "يجب ادخال رقم نشاط الشركة \n";
                }
                if (company.Governate == null || company.Governate == "")
                {
                    msg += "يجب ادخال المحافظة \n";
                }
                if (company.RegionCity == null || company.RegionCity == "")
                {
                    msg += "يجب ادخال المنطقة \n";
                }
                if (company.Street == null || company.Street == "")
                {
                    msg += "يجب ادخال الشارع \n";
                }
                if (company.BuildingNumber == null || company.BuildingNumber == "")
                {
                    msg += "يجب ادخال رقم المبني";
                }

            }
            else
            {
                msg = "يجب ادخال بيانات الشركة";
            }

            return msg;


        }
        public static string ValidateCuruncyForEInvoice(Currency currency)
        {
            string msg = "";          
            if (currency != null)
            {
                if (currency.TaxAuthorityCode == null || currency.TaxAuthorityCode == "")
                {
                    msg += "يجب ادخال كود العملة العالمي لمصلحة الضرائب \n";
                }
            }
            else
            {
                msg = "يجب اختيار العملة";
            }

            return msg;


        }
        public static string ValidateItemForEInvoice(Item item)
        {
            string msg = "";
            if (item != null)
            {
                if (item.TaxAuthorityCode == null || item.TaxAuthorityCode == "")
                {
                    msg += $"يجب ادخال كود مصلحة الضرائب للصنف {item.NameAr} \n";
                }
            }
            else
            {
                msg = "يجب اختيار صنف";
            }

            return msg;


        }
        public static string ValidateUnitForEInvoice(Unit unit)
        {
            string msg = "";
            if (unit != null)
            {
                if (unit.TaxAuthorityCode == null || unit.TaxAuthorityCode == "")
                {
                    msg += $"يجب ادخال كود مصلحة الضرائب للوحدة {unit.NameAr} \n";
                }
            }
            else
            {
                msg = "يجب اختيار وحدة";
            }

            return msg;


        }
    }
}
