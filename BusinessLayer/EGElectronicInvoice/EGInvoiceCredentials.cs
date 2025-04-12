namespace BusinessLayer.EGElectronicInvoice
{
    public static class EGInvoiceCredentials
    {
       /// <summary>
       /// عنوان مصلحة الضرايب لفحص المستخدم وجلب التوكن التي يتم استخدامها في ارسال المستندات
       /// </summary>
        public static string IdentityServiceUrl = "https://id.preprod.eta.gov.eg";//https://id.eta.gov.eg
        /// <summary>
        /// عنوان مصلحة الضرايب الذي يتم عليه ارسال المستندات
        /// </summary>
        public static string SystemAPIUrl = "https://api.preprod.invoicing.eta.gov.eg";//https://api.invoicing.eta.gov.eg
        /// <summary>
        /// رقم تعريف النظام علي مصلحة الضرايب
        /// </summary>
        public static string Client_ID = "b93c6a18-8233-432a-a81e-b9984531799f";//d9658a52-8d63-4e48-b5d1-96b0ad2fbdd5
        /// <summary>
        /// رقم تعريف سري للنظام علي مصلة الضرايب 
        /// </summary>
        public static string Client_Secret = "60877c9f-a821-4662-92c3-5565f8479547";//0531eb8c-a433-49e0-934d-37a2e627e864
        /// <summary>
        /// كلمة مرور توكن التوقيع الالكتروني من اجي تراست او مصر المقاصة
        /// </summary>
        public static string TokenPassword = "66550121";//23893748

        public static int E_InvoiceType { get; set; }


        #region test data
        //IdentityServiceUrl = "https://id.preprod.eta.gov.eg";//https://id.eta.gov.eg
        //SystemAPIUrl = "https://api.preprod.invoicing.eta.gov.eg";//https://api.invoicing.eta.gov.eg
        //Client_ID = "b93c6a18-8233-432a-a81e-b9984531799f";//d9658a52-8d63-4e48-b5d1-96b0ad2fbdd5
        //Client_Secret = "60877c9f-a821-4662-92c3-5565f8479547";//0531eb8c-a433-49e0-934d-37a2e627e864
        //taxRegestrationNumber=404110134
        //taxPayerActivityCode=6920
        //tokenPassword=23893748

        ////////units///////////////////////////
        /*
    KG	كيلوجرام		KGM
        ------------------------
    G	جرام		GRM
        ------------------------
    L	لتر		LTR
        ------------------------
    unit	وحدة		C62
        ------------------------
    m	متر		MTR
        ------------------------
    ozza	الاوزا		NULL
        ------------------------
    unit	وحدة 2		C62

        =================================
        EG	مصر	Egypt
SA	السعودية	Saudi Arabia 
AE	الامارات العربية المتحدة	NULL
SD	السودان	Sudan
BH	البحرين	Bahrain
JO	الأردن	Jordan
KW	الكويت	Kuwait
        ================================



         
       */




        #endregion

    }
}
