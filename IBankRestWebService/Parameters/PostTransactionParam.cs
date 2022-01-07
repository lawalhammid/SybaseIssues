using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBankRestWebService.Parameters
{
    public class PostTransactionParam
    {
        public string TransReference { get; set; }
        public string DrAcctNo { get; set; }
        public string DrAcctType { get; set; }
        public int? DrAcctCbsTC { get; set; }
        public string DrAcctNarration { get; set; }
        public decimal? Amount { get; set; }
        public string CrAcctNo { get; set; }
        public string CrAcctType { get; set; }
        public int? CrAcctCbsTC { get; set; }
        public string CrAcctNarration { get; set; }
        public string CcyCode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string CbsUserId { get; set; }
        public int? FloatDays { get; set; }
        public string RoutingNo { get; set; }
        public string CbsUSupervisorId { get; set; }
        public int? CbsChannelId { get; set; }
        public int? ForceFloatFlag { get; set; }
        public int? Reversal { get; set; }
        public int? BatchId { get; set; }
        public string ParentTransactionId { get; set; }

        public int? Direction { get; set; }
        public int? DrAcctCustomerID { get; set; }
        public int? DrAcctChequeNo { get; set; }
        public int? DrAcctChgCode { get; set; }
        public decimal? DrAcctChgAmt { get; set; }
        public decimal? DrAcctTaxAmt { get; set; }
        public int? CrAcctChequeNo { get; set; }
        public int? CrAcctChgCode { get; set; }
        public decimal? CrAcctChgAmt { get; set; }
        public decimal? CrAcctTaxAmt { get; set; }
        public string TransTracer { get; set; }
        public string DrAcctChgNarr { get; set; }
        public string CrAcctChgNarr { get; set; }
        public decimal? DrAcctCashAmt { get; set; }
        public Nullable<decimal> CrAcctCashAmt { get; set; }
        public decimal? LcyEquivAmt { get; set; }
        public decimal? OrigExchRate { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? DrAcctChgBr { get; set; }
        public int? CrAcctChgBr { get; set; }
        public decimal? DrAcctOffshoreAmt { get; set; }
        public decimal? CrAcctOffshoreAmt { get; set; }
        public string OriginChannel { get; set; }
        public string UserName { get; set; }
        public string RimNo { get; set; }



    }
}

