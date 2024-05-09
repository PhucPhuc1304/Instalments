using System;
using System.Collections.Generic;

namespace Instalments.Models
{
	public class InstalmentsCreateBulk : IEquatable<InstalmentsCreateBulk>
	{
		public InstalmentsCreateBulk()
		{
		}

		public InstalmentsCreateBulk(string cCCD, string pHONE, string cLIENT_NAME, string cONTRACT_NUMBER, string cARD_NUMBER, string tRANS_DATE, string tRANS_AMOUNT, string aUTH_CODE, string rECORD_ID, string mERCHANT, string tENOR, string oPTION_CODE, string nOTE)
		{
			CCCD = cCCD;
			PHONE = pHONE;
			CLIENT_NAME = cLIENT_NAME;
			CONTRACT_NUMBER = cONTRACT_NUMBER;
			CARD_NUMBER = cARD_NUMBER;
			TRANS_DATE = tRANS_DATE;
			TRANS_AMOUNT = tRANS_AMOUNT;
			AUTH_CODE = aUTH_CODE;
			RECORD_ID = rECORD_ID;
			MERCHANT = mERCHANT;
			TENOR = tENOR;
			OPTION_CODE = oPTION_CODE;
			NOTE = nOTE;
		}

		public List<string> listNotNullProperties = new List<string>
		{ "CLIENT_NAME", "CONTRACT_NUMBER", "CARD_NUMBER", "TRANS_DATE","AUTH_CODE","RECORD_ID","MERCHANT","TENOR","OPTION_CODE"};


		public Dictionary<string, string> mappingTenor = new Dictionary<string, string>()
		{
			{ "3", "R3MTHS" },
			{ "6", "R6MTHS" },
			{ "9", "R9MTHS" },
			{ "12", "R12MTHS" },
			{ "24", "R24MTHS" }
		};

		public string MapOptionCode(string tenor)
		{
			if (this.mappingTenor.ContainsKey(tenor))
			{
				return this.mappingTenor[tenor];
			}
			else
			{
				return tenor;
			}
		}


		public string CCCD { get; set; }
		public string PHONE { get; set; }
		public string CLIENT_NAME { get; set; }
		public string CONTRACT_NUMBER { get; set; }
		public string CARD_NUMBER { get; set; }
		public string TRANS_DATE { get; set; }
		public string TRANS_AMOUNT { get; set; }
		public string AUTH_CODE { get; set; }
		public string RECORD_ID { get; set; }
		public string MERCHANT { get; set; }
		public string TENOR { get; set; }
		public string OPTION_CODE { get; set; }
		public string NOTE { get; set; }

		public string UUID { get; set; }
		public string BATCH_ID { get; set; }
		public string RESULT_CODE { get; set; }
		public string RESULT_MESSAGE { get; set; }
		public string CREATE_USER { get; set; }
		public string CREATE_BRANCH { get; set; }
		public string CREATE_DATE { get; set; }
		public string APPROVE_USER { get; set; }
		public string APPROVE_BRANCH { get; set; }
		public string APPROVE_DATE { get; set; }
		public string FILE_NAME { get; set; }
		public string STATUS { get; set; }
		public string REJECT_REASON { get; set; }
		public string FULL_CARD_NUMBER { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as InstalmentsCreateBulk);
		}

		public bool Equals(InstalmentsCreateBulk other)
		{
			return other != null &&
				   CCCD == other.CCCD &&
				   PHONE == other.PHONE &&
				   CLIENT_NAME == other.CLIENT_NAME &&
				   CONTRACT_NUMBER == other.CONTRACT_NUMBER &&
				   CARD_NUMBER == other.CARD_NUMBER &&
				   TRANS_DATE == other.TRANS_DATE &&
				   AUTH_CODE == other.AUTH_CODE &&
				   RECORD_ID == other.RECORD_ID &&
				   MERCHANT == other.MERCHANT &&
				   TENOR == other.TENOR &&
				   OPTION_CODE == other.OPTION_CODE;
		}

		public override int GetHashCode()
		{
			int hashCode = 1149816874;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CCCD);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PHONE);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CLIENT_NAME);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CONTRACT_NUMBER);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CARD_NUMBER);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TRANS_DATE);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AUTH_CODE);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RECORD_ID);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MERCHANT);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TENOR);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OPTION_CODE);
			return hashCode;
		}

		public static bool operator ==(InstalmentsCreateBulk left, InstalmentsCreateBulk right)
		{
			return EqualityComparer<InstalmentsCreateBulk>.Default.Equals(left, right);
		}

		public static bool operator !=(InstalmentsCreateBulk left, InstalmentsCreateBulk right)
		{
			return !(left == right);
		}
	}

	public class FileMasterInstalmentsCreateBulk
	{
		public string UUID { get; set; }
		public string CHANNEL_TYPE { get; set; }
		public string FILE_NAME { get; set; }
		public string SERVICE_ID { get; set; }
		public string SERVICE_TYPE { get; set; }
		public string PRODUCT { get; set; }
		public string PROCESS_TYPE { get; set; }
		public string TOTAL_AMOUNT { get; set; }
		public int TOTAL_ROW { get; set; }
		public int TOTAL_ROW_SUCCESS { get; set; }
		public int TOTAL_ROW_FAIL { get; set; }
		public string BATCH_ID { get; set; }
		public string DIRECTION { get; set; }
		public string RECEIVE_DATE { get; set; }
		public string PROCESS_START_DATE { get; set; }
		public string PROCESS_END_DATE { get; set; }
		public string RESULT_CODE { get; set; }
		public string RESULT_MESSAGE { get; set; }
		public string IS_PASS_LOOP { get; set; }
		public string CREATE_USER { get; set; }
		public string CREATE_BRANCH { get; set; }
		public string CREATE_DATE { get; set; }
		public string APPROVE_USER { get; set; }
		public string APPROVE_BRANCH { get; set; }
		public string APPROVE_DATE { get; set; }
		public string UPDATE_TIME { get; set; }
		public int STATUS { get; set; }
		public string REMARK { get; set; }
		public string ATTACHMENT { get; set; }
		public List<string> LIST_ATTACHMENT_NAME { get; set; }
		public string ATTACHMENT_PATH { get; set; }
		public string REJECT_REASON { get; set; }

	}
}