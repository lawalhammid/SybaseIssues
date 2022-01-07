using IBankRestWebService.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.ViewModel
{
    public class PostTransactionRequest
    {
        public ApplicationsDbConnection ConnectionStrg { get; set; }
        public PostTransactionParam PostParameters { get; set; }
       
    }

    public class ApplicationsDbConnection
    {
        public string ConnectionString { get; set; }
    }
}

//public string TransRef { get; set; }
//public string DrAccountNo { get; set; }
//public string DrAcctType { get; set; }
//public int DrAcctTC { get; set; }
//public string DrAcctNarration { get; set; }
//public int TranAmount { get; set; }
//public string CrAcctNo { get; set; }
//public string CrAcctType { get; set; }
//public int CrAcctTC { get; set; }
//public string CrAcctNarration { get; set; }
//public string CurrencyISO { get; set; }
//public string PostDate { get; set; }
//public string ValueDate { get; set; }
//public string UserName { get; set; }
//public int? FloatDays { get; set; }
//public string RoutingNo { get; set; }
//public string SupervisorName { get; set; }
//public int? ChannelId { get; set; }
//public int? ForcePostFlag { get; set; }
//public int Reversal { get; set; }
//public int? TranBatchId { get; set; } = null;
//public string ParentTransRef { get; set; }
//public int Direction { get; set; }
//public int? RimNo { get; set; }
//public long? DrAcctChequeNo { get; set; }
//public int? DrAcctChargeCode { get; set; }
//public decimal? DrAcctChargeAmt { get; set; }
//public decimal? DrAcctTaxAmt { get; set; }
//public long? CrAcctChequeNo { get; set; }
//public int? CrAcctChargeCode { get; set; }
//public decimal? CrAcctChargeAmt { get; set; }
//public decimal? CrAcctTaxAmt { get; set; }
//public string TransTracer { get; set; }
//public string DrAcctChgDescr { get; set; }
//public string CrAcctChgDescr { get; set; }
//public decimal? DrAcctCashAmt { get; set; }
//public decimal? CrAcctCashAmt { get; set; }
//public decimal? EquivAmt { get; set; }
//public decimal? OrigExchRate { get; set; }
//public decimal? ExchRate { get; set; }
//public int? DrAcctChgBranch { get; set; }
//public int? CrAcctChgBranch { get; set; }
//public decimal? DrAcctOffshoreAmt { get; set; }
//public decimal? CrAcctOffshoreAmt { get; set; }
//public string OriginChannel { get; set; }
////may be useful
//public string Chargecode { get; set; }
//public string TaxAmount { get; set; }
//public string TaxRate { get; set; }
//public string ChargeAmount { get; set; }



