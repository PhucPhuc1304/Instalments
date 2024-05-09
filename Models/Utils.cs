using DBManager;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ISSUING_APP.Models;
using Oracle.DataAccess.Client;
using System.Configuration;


namespace Instalments.Models
{
	public class Utils
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static string packagePortalV2 = "HDB_ISSUING_TWO";

		public static CBoxData LoadProduct(params string[] argument)
		{
			return new CBoxData();
		}

		#region Way4
		/// <summary>
		/// Load danh sách sản phẩm debit
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadProduct_Debit(params string[] argument)
		{
			#region BK 13/05: Dùng bảng cấu hình khác
			//List<CBoxData> lsResult = new List<CBoxData>();
			//SqlUtil connect = new SqlUtil("PORTAL");
			//string sqlFilter = string.Format(@"SELECT DISTINCT CONTRACTPRODUCTCODE CODE, CONTRACTPRODUCTNAME NAME
			//            FROM ECRM.HDB_ISSUING_CLASSIFIER_CONFIG
			//            WHERE APPL_PR_GROUPCODE='ISSDEB'
			//            ORDER BY CONTRACTPRODUCTCODE");
			//log.InfoFormat("[Utils][LoadProduct_Credit] sql={0}", sqlFilter);

			//DataSet ds = new DataSet();
			//try
			//{
			//    connect.FillSQL(sqlFilter, ref ds);
			//    lsResult = ds.Tables[0].ToList<CBoxData>();
			//}
			//catch (Exception ex)
			//{
			//    log.Error("[Utils][LoadProduct_Credit] Exception: " + ex.Message, ex);
			//}
			//finally
			//{
			//    try
			//    {
			//        connect.Close();
			//    }
			//    catch { }
			//}

			//return lsResult;
			#endregion

			//Added 13/05: Dùng bảng cấu hình mới
			Issuing_Config oConfig = new Issuing_Config();
			List<Issuing_Config> lsTmp = oConfig.GetProductsByGroupCode(CShared.DEBIT);
			List<CBoxData> lsResult = new List<CBoxData>();
			foreach (var item in lsTmp)
			{
				lsResult.Add(new CBoxData
				{
					CODE = item.CONTRACTPRODUCTCODE,
					NAME = item.CONTRACTPRODUCTNAME
				});
			}

			return lsResult;
		}

		/// <summary>
		/// Load danh sách sản phẩm credit
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadProduct_Credit(params string[] argument)
		{
			#region BK
			//List<CBoxData> lsResult = new List<CBoxData>();

			//SqlUtil connect = new SqlUtil("PORTAL");
			//string sqlFilter = string.Format(@"SELECT DISTINCT CONTRACTPRODUCTCODE CODE, CONTRACTPRODUCTNAME NAME  
			//            FROM ECRM.HDB_ISSUING_CLASSIFIER_CONFIG
			//            WHERE APPL_PR_GROUPCODE='ISSCRE'
			//            ORDER BY CONTRACTPRODUCTCODE");
			//log.InfoFormat("[Utils][LoadProduct_Credit] sql={0}", sqlFilter);

			//DataSet ds = new DataSet();
			//try
			//{
			//    connect.FillSQL(sqlFilter, ref ds);
			//    lsResult = ds.Tables[0].ToList<CBoxData>();
			//}
			//catch (Exception ex)
			//{
			//    log.Error("[Utils][LoadProduct_Credit] Exception: " + ex.Message, ex);
			//}
			//finally
			//{
			//    try
			//    {
			//        connect.Close();
			//    }
			//    catch { }
			//}

			//return lsResult;
			#endregion

			//Added 13/05: Dùng bảng cấu hình mới
			Issuing_Config oConfig = new Issuing_Config();
			List<Issuing_Config> lsTmp = oConfig.GetProductsByGroupCode(CShared.CREDIT);
			List<CBoxData> lsResult = new List<CBoxData>();
			foreach (var item in lsTmp)
			{
				lsResult.Add(new CBoxData
				{
					CODE = item.CONTRACTPRODUCTCODE,
					NAME = item.CONTRACTPRODUCTNAME
				});
			}

			return lsResult;
		}

		public static List<CBoxData> LoadProduct_Prepaid(params string[] argument)
		{
			Issuing_Config oConfig = new Issuing_Config();
			List<Issuing_Config> lsTmp = oConfig.GetProductsByGroupCode(CShared.PREPAID);
			List<CBoxData> lsResult = new List<CBoxData>();
			foreach (var item in lsTmp)
			{
				lsResult.Add(new CBoxData
				{
					CODE = item.CONTRACTPRODUCTCODE,
					NAME = item.CONTRACTPRODUCTNAME
				});
			}

			return lsResult;
		}

		/// <summary>
		/// Load card product: xác định thẻ chính or phụ
		/// </summary>
		/// <param name="contractProduct"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadCardProduct_Credit(string contractProduct)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format(@"SELECT CODE, NAME 
                FROM HDB.APPL_PRODUCT 
                WHERE APPL_PRODUCT__OID = ( 
                        SELECT ID from hdb.APPL_PRODUCT where AMND_STATE = 'A' and CON_CAT = 'A' AND CODE = '{0}' 
                    ) AND AMND_STATE = 'A'", contractProduct);
			log.InfoFormat("[Utils][LoadProduct_Credit] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadProduct_Credit] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		/// <summary>
		/// LoadClassifier_Credit
		/// </summary>
		/// <param name="sCode"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadClassifier_Credit(string sContractProductCode)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("PORTAL");
			string sqlFilter = string.Format(@"SELECT DISTINCT CLASSIFIERCODE CODE, CLASSIFIERNAME NAME  
                FROM {0}.HDB_ISSUING_CLASSIFIER_CONFIG
                WHERE APPL_PR_GROUPCODE='ISSCRE' AND CONTRACTPRODUCTCODE='{1}'", CShared.schemaPortal, sContractProductCode);

			log.InfoFormat("[Utils][LoadClassifier_Credit] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadClassifier_Credit] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		/// <summary>
		/// LoadClassifierType: Call Way4
		/// </summary>
		/// <returns></returns>
		public static List<CBoxData> LoadClassifierType()
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = @"SELECT CODE, NAME FROM HDB.CS_STATUS_TYPE WHERE CODE IN ('CHAIN','LC_CR_RGL') AND AMND_STATE = 'A'";

			log.InfoFormat("[Utils][LoadClassifierType] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadClassifierType] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}
		/// <summary>
		/// LoaddClassifierCode: Call Way4
		/// </summary>
		/// <param name="sClassifierType"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadClassifierCode(string sClassifierType)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format(@"SELECT CODE, NAME FROM HDB.CS_STATUS_VALUE 
                                            WHERE AMND_STATE = 'A' AND CS_STATUS_TYPE__OID = (SELECT ID FROM HDB.CS_STATUS_TYPE WHERE AMND_STATE = 'A' AND CODE = '{0}')", sClassifierType);

			log.InfoFormat("[Utils][LoaddClassifierCode] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoaddClassifierCode] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		/// <summary>
		/// Lấy danh sách Classifier theo sản phẩm thẻ
		/// </summary>
		/// <param name="sContractProductCode"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadClassifier_Debit(string sContractProductCode)
		{
			#region BK 13/05/19: Dùng bảng cấu hình mới
			//List<CBoxData> lsResult = new List<CBoxData>();
			//SqlUtil connect = new SqlUtil("PORTAL");
			//string sqlFilter = string.Format(@"SELECT DISTINCT CLASSIFIERCODE CODE, CLASSIFIERNAME NAME  
			//    FROM ECRM.HDB_ISSUING_CLASSIFIER_CONFIG
			//    WHERE APPL_PR_GROUPCODE='ISSDEB' AND CONTRACTPRODUCTCODE='{0}'", sContractProductCode);

			//log.InfoFormat("[Utils][LoadClassifier_Credit] sql={0}", sqlFilter);

			//DataSet ds = new DataSet();
			//try
			//{
			//    connect.FillSQL(sqlFilter, ref ds);
			//    lsResult = ds.Tables[0].ToList<CBoxData>();
			//}
			//catch (Exception ex)
			//{
			//    log.Error("[Utils][LoadClassifier_Credit] Exception: " + ex.Message, ex);
			//}
			//finally
			//{
			//    try
			//    {
			//        connect.Close();
			//    }
			//    catch { }
			//}

			//return lsResult;
			#endregion

			#region Added 13/5
			Issuing_Config oConfig = new Issuing_Config();
			List<Issuing_Config> lsTmp = oConfig.GetClassifierByProductCode(sContractProductCode);
			List<CBoxData> lsResult = new List<CBoxData>();
			foreach (var item in lsTmp)
			{
				lsResult.Add(new CBoxData
				{
					CODE = item.CLASSIFIERCODE,
					NAME = item.CONTRACTPRODUCTNAME,
					PARA1 = item.CLASSIFIERTYPE
				});
			}

			return lsResult;
			#endregion
		}


		///// <summary>
		///// Load danh sách sản phẩm credit
		///// </summary>
		///// <param name="argument"></param>
		///// <returns></returns>
		//public static List<CBoxData> LoadProduct_Credit(params string[] argument)
		//{
		//    List<CBoxData> lsResult = new List<CBoxData>();
		//    SqlUtil connect = new SqlUtil("WAY4");
		//    string sqlFilter = @"select Code, Name from hdb.appl_product p 
		//        where p.appl_pr_group__id = (select id from hdb.appl_pr_group where code = 'ISSCRE' and amnd_state = 'A')
		//        and p.amnd_state = 'A'
		//        and p.liab_category is null
		//        and p.con_cat = 'A'";
		//    log.InfoFormat("[Utils][LoadProduct_Credit] sql={0}", sqlFilter);

		//    DataSet ds = new DataSet();
		//    try
		//    {
		//        connect.FillSQL(sqlFilter, ref ds);
		//        lsResult = ds.Tables[0].ToList<CBoxData>();
		//    }
		//    catch (Exception ex)
		//    {
		//        log.Error("[Utils][LoadProduct_Credit] Exception: " + ex.Message, ex);
		//    }
		//    finally
		//    {
		//        try
		//        {
		//            connect.Close();
		//        }
		//        catch { }
		//    }

		//    return lsResult;
		//}

		/// <summary>
		/// Kiểm tra clientNo tồn tại trên way4
		/// </summary>
		/// <param name="ClientNumber"></param>
		/// <returns></returns>
		public static bool ExistClientOnWay4(string ClientNumber)
		{
			bool bResult = false; ;
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format("SELECT COUNT(*) ICOUNT FROM HDB.CLIENT WHERE CLIENT_NUMBER = '{0}' AND AMND_STATE = 'A'", ClientNumber);
			log.InfoFormat("[Utils][ExistClientOnWay4] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				bResult = int.Parse(ds.Tables[0].Rows[0]["ICOUNT"].ToString()) > 0;
			}
			catch (Exception ex)
			{
				log.Error("[Utils][ExistClientOnWay4] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return bResult;
		}
		//// Edit by Thanh Trieu 27/06/2019
		public static bool ExistLocAcctOnWay4(string locAcct)
		{
			bool bResult = false;
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format("SELECT COUNT(*) ICOUNT FROM HDB.ACNT_CONTRACT WHERE CONTRACT_NUMBER = '{0}' AND AMND_STATE = 'A' AND CON_CAT = 'A'", locAcct);
			log.InfoFormat("[Utils][ExistClientOnWay4] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				bResult = int.Parse(ds.Tables[0].Rows[0]["ICOUNT"].ToString()) > 0;
			}
			catch (Exception ex)
			{
				log.Error("[Utils][ExistLocAcctOnWay4] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return bResult;
		}

		public static bool checkW4ContractStatus(string contractNo)
		{
			bool bResult = false;
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format("SELECT COUNT(1) ICOUNT FROM acnt_contract ctr,"
				+ " contr_status sta WHERE ctr.contr_status = sta.id AND ctr.contract_number = '{0}'"
				+ " AND ctr.amnd_state = 'A' AND ctr.con_cat = 'A'  AND   sta.con_cat = 'A'"
				+ " AND sta.amnd_state = 'A'", contractNo);
			log.InfoFormat("[Utils][getW4ContractStatus] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				bResult = int.Parse(ds.Tables[0].Rows[0]["ICOUNT"].ToString()) > 0;
			}
			catch (Exception ex)
			{
				log.Error("[Utils][getW4ContractStatus] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return bResult;
		}

		public static Result validateVJAProduct(string cardNumber, string fullName)
		{
			Result rs = new Result();
			SqlUtil connect = new SqlUtil("EOC");
			//string schemaEOC = "EOCD2";
			string schemaEOC = CShared.schemaEOC;
			string packageEOC = "HDB_WEB_ISSUING_THANHTT7";
			DateTime dt = DateTime.Today;
			OracleParameter[] parameters = new OracleParameter[5];
			string procedureName = string.Format("{0}.{1}.PROC_VALIDATE2_W4P", schemaEOC, packageEOC);
			try
			{
				parameters[0] = connect.Parameter("P_CARD_NUMBER", OracleDbType.Varchar2, cardNumber); parameters[0].IsNullable = true; parameters[0].Direction = ParameterDirection.Input;
				parameters[1] = connect.Parameter("P_FULL_NAME", OracleDbType.Varchar2, fullName); parameters[1].IsNullable = true; parameters[1].Direction = ParameterDirection.Input;
				(parameters[2] = connect.Parameter("O_PHONE_NO", OracleDbType.Varchar2)).Direction = ParameterDirection.Output;
				parameters[2].Size = 15;
				(parameters[3] = connect.Parameter("ERR_CODE", OracleDbType.Varchar2)).Direction = ParameterDirection.Output;
				parameters[3].Size = 2;
				(parameters[4] = connect.Parameter("ERR_MSG", OracleDbType.Varchar2)).Direction = ParameterDirection.Output;
				parameters[4].Size = 1000;
				// Thực thi
				connect.ExecuteProc(procedureName, parameters);
				log.Info("[PROC_VALIDATE2_W4P][ERR_CODE] " + parameters[3].Value.ToString());
				log.Info("[PROC_VALIDATE2_W4P][ERR_MSG] " + parameters[4].Value.ToString());
				rs.code = parameters[3].Value.ToString();
				rs.message = parameters[4].Value.ToString();
				rs.para1 = parameters[2].Value.ToString();
			}
			catch (Exception ex)
			{
				log.Error("[Exception][PROC_VALIDATE2_W4P] " + ex.Message, ex);
				rs.code = "099";
				rs.message = "Lỗi hệ thống!";
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return rs;
		}

		public static bool checkDuplicateFile(string fileName)
		{
			bool result = false;

			SqlUtil connect = new SqlUtil("EOC");
			OracleCommand cmd = connect.Command();
			cmd.CommandText = "HDB_WEB_ISSUING_THANHTT7.FUNC_CHECK_DUPLICATE_FILE";
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("zReturn_Value", OracleDbType.Int16,
					ParameterDirection.ReturnValue);
			cmd.Parameters.Add("P_FILE_NAME", OracleDbType.Varchar2, 50, fileName, ParameterDirection.Input);
			//cmd.Parameters.Add("O_COUNT", OracleDbType.Int32);
			//cmd.Parameters["O_COUNT"].Direction = ParameterDirection.ReturnValue;

			connect.Open();
			cmd.ExecuteNonQuery();

			string count = Convert.ToString(cmd.Parameters["zReturn_Value"].Value);
			if (count != "0") result = true;

			return result;
		}
		//// end;
		/// <summary>
		/// Danh mục câu hỏi bảo mật
		/// </summary>
		/// <returns></returns>
		public static List<CBoxData> LoadQuestions()
		{
			//
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = @"select CODE, NAME from HDB.SY_HANDBOOK where group_code = 'SEC_Q' AND AMND_STATE = 'A'";
			log.InfoFormat("[Utils][LoadQuestions] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadQuestions] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		/// <summary>
		/// getCardProductCode
		/// CHECK: PARA1 = 1, HƠN 2 DÒNG THÔNG BÁO LỖI
		///        PARA1 = 2, CÓ 2 DÒNG MAIN VÀ SUB, KG CÓ DÒNG NÀO THÌ THÔNG BÁO LỖI
		///        PARA1 = 3, CHỈ CÓ 1 DÒNG, NẾU KHÁC THÔNG BÁO LỖI
		/// </summary>
		/// <param name="ProductID"></param>
		/// <returns></returns>
		public static List<CBoxData> getCardProductCode(string ProductID)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			//PRODUCT_GROUP = 'ISSDEB'
			bool vccs = false;
			if (ProductID.Contains("_E_"))
			{
				ProductID = ProductID.Replace("_E_", "_");
				vccs = true;
			}
			string sqlFilter = string.Format(@"SELECT CODE, NAME, '1' PARA1 from hdb.APPL_PRODUCT where AMND_STATE = 'A' and CON_CAT = 'A' AND CODE = '{0}'
                                    UNION ALL
                                    SELECT CODE, NAME, '2' PARA1 FROM HDB.APPL_PRODUCT WHERE APPL_PRODUCT__OID = ( 
                                        SELECT ID from hdb.APPL_PRODUCT where AMND_STATE = 'A' and CON_CAT = 'A' AND CODE = '{0}' 
                                    )
                                    AND BASE_RELATION is null AND CON_CAT = 'C' and amnd_state = 'A'
                                    UNION ALL
                                    SELECT CODE, NAME, '3' PARA1 FROM HDB.APPL_PRODUCT WHERE APPL_PRODUCT__OID = (
                                         SELECT ID from hdb.APPL_PRODUCT where AMND_STATE = 'A' and CON_CAT = 'A' AND CODE = '{0}' 
                                    ) AND BASE_RELATION = '00' AND CON_CAT = 'C' and amnd_state = 'A'", ProductID);

			log.InfoFormat("[Utils][getCardProductCode] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				List<CBoxData> lsResultTemp = new List<CBoxData>();
				lsResultTemp = ds.Tables[0].ToList<CBoxData>();
				if (lsResultTemp.Count > 0)
				{
					lsResult.AddRange(lsResultTemp.Where(x => x.PARA1 == "1").ToList());
					if (vccs)
					{
						lsResultTemp = lsResultTemp.Where(x => x.CODE.Contains("_E_") && x.PARA1 != "1").ToList();

					}
					else
					{
						lsResultTemp = lsResultTemp.Where(x => !x.CODE.Contains("_E_") && x.PARA1 != "1").ToList();
					}
					lsResult.AddRange(lsResultTemp);
				}
			}
			catch (Exception ex)
			{
				log.Error("[Utils][getCardProductCode] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return lsResult;
		}

		/// <summary>
		/// getRelatedProductCode: dùng cho thẻ Debit
		/// Mục đích: cho phép GDV chọn được nhiều RelatedProductCode ~ TKTT
		/// </summary>
		/// <param name="ProductID"></param>
		/// <returns></returns>
		public static List<CBoxData> getRelatedProductCode(string ProductID)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format(@"SELECT CODE, NAME, '3' PARA1 FROM HDB.APPL_PRODUCT WHERE APPL_PRODUCT__OID = (
                                     SELECT ID from hdb.APPL_PRODUCT where AMND_STATE = 'A' and PRODUCT_GROUP = 'ISSDEB' and CON_CAT = 'A' AND CODE = '{0}' 
                                ) AND CON_CAT = 'C' and amnd_state = 'A' and BASE_RELATION is not null", ProductID);

			log.InfoFormat("[Utils][getRelatedProductCode] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][getRelatedProductCode] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return lsResult;
		}

		/// <summary>
		/// Tìm số Contract Number từ số thẻ
		/// Mục đích: phục vụ cho việc đăng ký thẻ phụ
		/// </summary>
		/// <param name="CardNo"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadContractNorByCardNo(string CardNo)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format(@"SELECT CONTRACT_NUMBER CODE, CONTRACT_NAME NAME FROM HDB.ACNT_CONTRACT 
                    WHERE ID = (SELECT ACNT_CONTRACT__OID FROM HDB.ACNT_CONTRACT WHERE CONTRACT_NUMBER = '{0}')
                    AND AMND_STATE = 'A'", CardNo);

			log.InfoFormat("[Utils][LoadContractNorByCardNo] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadContractNorByCardNo] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return lsResult;
		}

		/// <summary>
		/// Tìm số Card Number từ số Contract
		/// Mục đính: tìm ngược lại số thẻ từ số contract
		/// </summary>
		/// <param name="ContractNo"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadCardNoByContractNo(string ContractNo)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format(@"SELECT CONTRACT_NUMBER CODE, CONTRACT_NAME 
                FROM HDB.ACNT_CONTRACT WHERE ACNT_CONTRACT__OID = (
                    SELECT ID FROM HDB.ACNT_CONTRACT WHERE CONTRACT_NUMBER = '{0}'
                ) AND AMND_STATE = 'A'", ContractNo);

			log.InfoFormat("[Utils][LoadCardNoByContractNo] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadCardNoByContractNo] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return lsResult;
		}
		#endregion

		#region EOC
		/// <summary>
		/// [***] NÊN TẠO STORE TRÊN EOC
		/// Tìm danh sách Saller
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadSalerByName(string Name)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("EOC");
			string sqlFilter = string.Format("SELECT ACCT_EXEC CODE, ACCT_EXEC_NAME NAME FROM {0}.fm_acct_exec WHERE ACCT_EXEC LIKE '%{1}%' OR ACCT_EXEC_NAME LIKE '%{1}%'", CShared.schemaEOC, Name);
			log.InfoFormat("[Utils][GetSaler] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][GetSaler] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		/// [***] NÊN TẠO STORE TRÊN EOC
		public static List<CBoxData> LoadSaler(string Name)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("EOC");
			string sqlFilter = string.Format("SELECT ACCT_EXEC CODE, ACCT_EXEC_NAME NAME FROM {0}.fm_acct_exec WHERE ACCT_EXEC like '%{1}%' OR ACCT_EXEC_NAME like '%{1}%'", CShared.schemaEOC, Name.ToUpper());
			log.InfoFormat("[Utils][GetSaler] sql={0}", sqlFilter);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(sqlFilter, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][GetSaler] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}




		public static List<CBoxData> LoadStaff(string text)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("EOC");
			string sqlFilter = string.Format("{0}.{1}.PROC_SEARCH_STAFF", CShared.schemaEOC, CShared.packageCardEOC);
			log.InfoFormat("[Utils][LoadStaff] sql={0}", sqlFilter);


			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_TEXT", OracleDbType.Varchar2)).Value = text;
				(parameters[1] = connect.Parameter("P_TMPCUR", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
				connect.FillProc(sqlFilter, parameters, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadStaff] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}


		public static List<CBoxData> GetExpire(string text)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string sqlFilter = string.Format("{0}.{1}.GET_EXPIRE", CShared.schemaWay4, CShared.packageWay4);
			log.InfoFormat("[Utils][GET_EXPIRE] sql={0}", sqlFilter);
			log.InfoFormat("[Utils][GET_EXPIRE] P_PRODUCT_CODE={0}", text);
			string product = text + "_M";
			List<CBoxData> listCodeDN = LoadAllProductDN();

			if (listCodeDN.Exists(x => x.CODE == text)) { product = text; }

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_PRODUCT_CODE", OracleDbType.Varchar2)).Value = product;
				(parameters[1] = connect.Parameter("P_DATA", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
				connect.FillProc(sqlFilter, parameters, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][GET_EXPIRE] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		/// <summary>
		/// Load
		/// </summary>
		/// <param name="sCategoryName"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadCategoryFromCore(string sCategoryName)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("EOC");
			string EOC_SCHEMA = System.Configuration.ConfigurationManager.AppSettings["EOC_SCHEMA"];
			//string EOC_PACKAGE = System.Configuration.ConfigurationManager.AppSettings["EOC_PACKAGE"];
			string EOC_PACKAGE = "HDB_WEB_ISSUING_EOC";

			string storeName = string.Format("{0}.{1}.{2}", EOC_SCHEMA, EOC_PACKAGE, "LOADCATEGORY_FROMCORE");
			log.InfoFormat("[Utils][LoadCategoryFromCore] sql={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_CATEGORYNAME", OracleDbType.Varchar2)).Value = sCategoryName;
				(parameters[1] = connect.Parameter("P_TMPCUR", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

				connect.FillProc(storeName, parameters, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadCategoryFromCore] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		/// <summary>
		/// Đọc danh sách Branch từ Way4DB
		/// </summary>
		/// <returns></returns>
		public static List<CBoxData> LoadBranchsWay4()
		{
			SESSION_PARA oPara = CShared.getSession();

			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.OPT_HDB_CARD_MANAGERMANT.GET_BRANCH", CShared.schemaWay4);
			log.InfoFormat("[Utils][LoadBranchsWay4] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[1];
				(parameters[0] = connect.Parameter("P_DATA", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

				connect.FillProc(storeName, parameters, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadBranchsWay4] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		public static List<CBoxData> LoadBranchs()
		{
			SESSION_PARA oPara = CShared.getSession();

			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("EOC");
			string storeName = string.Format("{0}.HDB_WEB_ISSUING_EOC.GET_BRANCH", CShared.schemaEOC);
			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[1];
				(parameters[0] = connect.Parameter("P_TMPCUR", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

				connect.FillProc(storeName, parameters, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadBranchs] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}





		/// <summary>
		/// Lấy danh sách tài khoản
		/// </summary>
		/// <param name="sCONTRACTPRODUCTCODE"></param>
		/// <param name="sCLIENT_NO"></param>
		/// <returns></returns>
		public static List<CRB_ACCT> GetListAccountsByClientNo(string sCONTRACTPRODUCTCODE, string sCLIENT_NO)
		{
			//Lấy thông tin account_type hỗ trợ cho contractproduct
			List<CBoxData> lsAcctType = Utils.LoadAccttype_ByContractProduct(sCONTRACTPRODUCTCODE);
			string sAcctType = "";
			foreach (var actTypeItem in lsAcctType)
			{
				if (string.IsNullOrEmpty(sAcctType)) sAcctType = string.Format("'{0}'", actTypeItem.CODE.Trim());
				else sAcctType += "," + string.Format("'{0}'", actTypeItem.CODE.Trim());
			}

			CRB_ACCT ca = new CRB_ACCT();
			return ca.GetListAcct(sCLIENT_NO, sAcctType);
		}


		#endregion

		#region ECRM
		/// <summary>
		/// Load danh mục dùng chung
		/// </summary>
		/// <param name="sGroupType"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadGlobals_ByGroupType(string sGroupType)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("PORTAL");
			string storeName = String.Format("{0}.HDB_ISSUING.LoadGlobals_ByGroupType", CShared.schemaPortal);
			//log.InfoFormat("[Utils][LoadGlobals_ByGroupType] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_GROUPTYPE", OracleDbType.Varchar2)).Value = sGroupType;
				(parameters[1] = connect.Parameter("P_TMPCUR", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

				connect.FillProc(storeName, parameters, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
				lsResult = lsResult.OrderBy(x => x.CODE).ToList();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadGlobals_ByGroupType] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}

		public static string GenIssuingCode()
		{
			SESSION_PARA oPra = CShared.getSession();
			SqlUtil connect = new SqlUtil("PORTAL");
			string storeName = String.Format("SELECT {0}.HDB_ISSUING_CARDS_SEQ.nextval ID FROM DUAL", CShared.schemaPortal);
			log.InfoFormat("[Utils][GenIssuingCode] storeName={0}", storeName);
			string sResult = "";
			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(storeName, ref ds);
				sResult = ds.Tables[0].Rows[0][0].ToString();
				sResult = "0000000000" + sResult;
				sResult = string.Format("{0}{1}", oPra.oAccount.Branch, sResult.Substring(sResult.Length - 10, 10));
			}
			catch (Exception ex)
			{
				log.Error("[Utils][GenIssuingCode] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return sResult;
		}

		/// <summary>
		/// Sinh số phiếu cho từng nghiệp vụ
		/// </summary>
		/// <param name="sTaskType"></param>
		/// <returns></returns>
		public static string GenCode(string sTaskType)
		{
			SESSION_PARA oPra = CShared.getSession();
			SqlUtil connect = new SqlUtil("PORTAL");
			string storeName = "";
			switch (sTaskType)
			{
				case TaskType.ISSUING_CARD:
					storeName = String.Format("SELECT {0}.HDB_ISSUING_CARDS_SEQ.nextval ID FROM DUAL", CShared.schemaPortal);
					break;
				case TaskType.ISSUING_EDITCLIENT:
					storeName = String.Format("SELECT {0}.HDB_ISSUING_EDITCLIENT_SEQ.nextval ID FROM DUAL", CShared.schemaPortal);
					break;
			}
			if (string.IsNullOrEmpty(storeName))
				return string.Empty;

			log.InfoFormat("[Utils][GenCode] storeName={0}", storeName);
			string sResult = "";
			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(storeName, ref ds);
				sResult = ds.Tables[0].Rows[0][0].ToString();
				sResult = "0000000000" + sResult;
				sResult = string.Format("{0}{1}", oPra.oAccount.Branch, sResult.Substring(sResult.Length - 10, 10));
			}
			catch (Exception ex)
			{
				log.Error("[Utils][GenCode] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return sResult;
		}

		/// <summary>
		/// Load danh sách AcctType theo ContractProduct
		/// </summary>
		/// <param name="sContractProduct"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadAccttype_ByContractProduct(string sContractProduct)
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("PORTAL");
			string storeName = string.Format("SELECT DISTINCT ACCT_TYPE CODE FROM {0}.HDB_ISSUING_ACCTTYPES_CONFIG WHERE CONTRACTPRODUCTCODE = '{1}'", CShared.schemaPortal, sContractProduct);
			log.InfoFormat("[Utils][LoadAccttype_ByContractProduct] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				connect.FillSQL(storeName, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][LoadAccttype_ByContractProduct] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}
		/// Load List Country
		public static List<CBoxData> GetListCountry(string pChar = "2")
		{
			List<CBoxData> lsResult = new List<CBoxData>();
			SqlUtil connect = new SqlUtil("PORTAL");
			string storeName = String.Format("{0}.HDB_ISSUING.SP_GET_LIST_COUNTRY", CShared.schemaPortal);
			log.InfoFormat("[Utils][GetListCountry] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_CHAR", OracleDbType.Varchar2)).Value = pChar;
				(parameters[1] = connect.Parameter("P_CUR", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

				connect.FillProc(storeName, parameters, ref ds);
				lsResult = ds.Tables[0].ToList<CBoxData>();
				lsResult = lsResult.OrderBy(x => x.CODE).ToList();
			}
			catch (Exception ex)
			{
				log.Error("[Utils][GetListCountry] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return lsResult;
		}
		#endregion



		/// <summary>
		/// Lấy danh sách product code cua the prepaid
		/// </summary>
		/// <param name="sGroupType"></param>
		/// <returns></returns>
		public static List<ProductCodePrepaid> GetProductCodePrepaid(string group, string contractProductcode, string cardProductCode)
		{
			List<ProductCodePrepaid> data = new List<ProductCodePrepaid>();
			Result rs = new Result();
			SqlUtil connect = new SqlUtil("PORTAL");
			string procedureName = string.Format("{0}.{1}.GET_PRODUCT_CODE_PREPAID", CShared.schemaPortal, CShared.packageISSPortal);
			List<OracleParameter> listParam = new List<OracleParameter>();
			try
			{
				DataSet ds = new DataSet();
				listParam.Add(new OracleParameter() { ParameterName = "P_GROUP_PRODUCT", OracleDbType = OracleDbType.Varchar2, Value = group, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CONTRACT_PRODUCT", OracleDbType = OracleDbType.Varchar2, Value = contractProductcode, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CARD_PRODUCT", OracleDbType = OracleDbType.Varchar2, Value = cardProductCode, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_DATA", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });
				listParam.Add(new OracleParameter() { ParameterName = "P_CODE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 20 });
				listParam.Add(new OracleParameter() { ParameterName = "P_MESSAGE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 255 });
				int length = listParam.Count;
				connect.FillProc(procedureName, listParam.ToArray(), ref ds);
				rs.code = listParam[length - 2].Value.ToString();
				rs.message = listParam[length - 1].Value.ToString();
				if (rs.code == "00")
				{
					data = ds.Tables[0].ToList<ProductCodePrepaid>();
				}

				return data;
			}
			catch (Exception ex)
			{
				log.Error("[PORTAL][" + CShared.schemaPortal + "][GET_PRODUCT_CODE_PREPAID][Error System]: " + ex);
			}
			finally
			{
				connect.Close();
			}
			return data;
		}



		public static List<Classifier> GetClassfier(string productCode, string classifierType, string classifierCode)
		{
			List<Classifier> data = new List<Classifier>();
			Result rs = new Result();
			SqlUtil connect = new SqlUtil("PORTAL");
			string procedureName = string.Format("{0}.{1}.GET_CLASSIFIER_CONFIG", CShared.schemaPortal, CShared.packageISSPortal);
			List<OracleParameter> listParam = new List<OracleParameter>();
			try
			{

				DataSet ds = new DataSet();
				listParam.Add(new OracleParameter() { ParameterName = "P_PRODUCT_CODE", OracleDbType = OracleDbType.Varchar2, Value = productCode, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CLASSIFIER_TYPE", OracleDbType = OracleDbType.Varchar2, Value = classifierType, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CLASSIFIER_CODE", OracleDbType = OracleDbType.Varchar2, Value = classifierCode, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_DATA", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

				int length = listParam.Count;
				connect.FillProc(procedureName, listParam.ToArray(), ref ds);
				rs.code = listParam[length - 2].Value.ToString();
				rs.message = listParam[length - 1].Value.ToString();
				data = ds.Tables[0].ToList<Classifier>();
			}
			catch (Exception ex)
			{
				log.Error("[" + CShared.schemaPortal + "][" + CShared.packageISSPortal + "][GET_INFO_PREPAID_CARD][Error System]: " + ex);
			}
			finally
			{
				connect.Close();
			}
			return data;
		}

		public bool UpdateStaff(string contractNumber, string cardNumber, string code, string name, string uuid)
		{
			List<CardClassifier> lsResult = new List<CardClassifier>();
			SqlUtil connect = new SqlUtil("PORTAL");
			string procedureName = string.Format(@"{0}.{1}.PROC_SET_STAFF", CShared.schemaPortal, packagePortalV2);
			OracleParameter[] parameters = new OracleParameter[5];
			log.InfoFormat("[UpdateStaff][PROC_SET_STAFF] sql={0}", procedureName);
			try
			{
				parameters[0] = connect.Parameter("P_CONTRACT_NUMBER", OracleDbType.Varchar2, contractNumber);
				parameters[1] = connect.Parameter("P_CARD_NUMBER", OracleDbType.Varchar2, cardNumber);
				parameters[2] = connect.Parameter("P_STAFF_CODE", OracleDbType.Varchar2, code);
				parameters[3] = connect.Parameter("P_STAFF_NAME", OracleDbType.Varchar2, name);
				parameters[4] = connect.Parameter("P_UUID", OracleDbType.Varchar2, uuid);

				parameters[0].Direction = ParameterDirection.Input;
				parameters[1].Direction = ParameterDirection.Input;
				parameters[2].Direction = ParameterDirection.Input;
				parameters[3].Direction = ParameterDirection.Input;
				parameters[4].Direction = ParameterDirection.Input;
				connect.ExecuteProc(procedureName, parameters);
				return true;
			}
			catch (Exception ex)
			{
				log.Error("[UpdateStaff][PROC_SET_STAFF] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return false;
		}

		/*
         Check ngay qua han cua contract
         */
		public int CheckExtDataContract(string contractNumber)
		{
			int days = 0;
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", CShared.schemaWay4, CShared.packageWay4, "GET_EXT_DATA_CONTRACT");
			log.InfoFormat("[CheckExtDataContract] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_CONTRACT_NUMBER", OracleDbType.Varchar2)).Value = contractNumber;
				(parameters[1] = connect.Parameter("P_DATA", OracleDbType.Int32)).Direction = ParameterDirection.Output; parameters[1].Size = 255;

				connect.FillProc(storeName, parameters, ref ds);
				days = Int32.Parse(parameters[1].Value.ToString());
			}
			catch (Exception ex)
			{
				log.Error("[CheckExtDataContract] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return days;
		}

		// Count number of sub card free fee yearly
		public int GetSubCarFreeFee(string contractNumber)
		{
			int days = 0;
			SqlUtil connect = new SqlUtil("PORTAL");
			string storeName = string.Format("{0}.{1}.{2}", CShared.schemaPortal, CShared.packageISSPortal, "COUNT_FREE_FEE_SUBCARD");
			log.InfoFormat("[GetSubCarFreeFee] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_CONTRACT_NUMBER", OracleDbType.Varchar2)).Value = contractNumber;
				(parameters[1] = connect.Parameter("P_DATA", OracleDbType.Int32)).Direction = ParameterDirection.Output; parameters[1].Size = 255;

				connect.FillProc(storeName, parameters, ref ds);
				days = Int32.Parse(parameters[1].Value.ToString());
			}
			catch (Exception ex)
			{
				log.Error("[GetSubCarFreeFee] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return days;
		}

		// Get card number by contract and last 4 digits
		public string GetCardByContrSuffix(string contractNumber, string last4digits)
		{
			string cardNumber = "";
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", CShared.schemaWay4, CShared.packageWay4, "GET_CARD_BY_CONTR_SUFFIX");
			log.InfoFormat("[IssuingCardData][GetCardByContrSuffix] storeName={0}", storeName);
			log.InfoFormat("[IssuingCardData][GetCardByContrSuffix] contractNumber = {0}", contractNumber);
			log.InfoFormat("[IssuingCardData][GetCardByContrSuffix] last4digits = {0}", last4digits);
			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[4];
				(parameters[0] = connect.Parameter("P_CONTRACT_NUMBER", OracleDbType.Varchar2)).Value = contractNumber;
				(parameters[1] = connect.Parameter("P_LAST_SUFFIX_CARD", OracleDbType.Varchar2)).Value = last4digits;
				(parameters[2] = connect.Parameter("P_CARD_NUMBER", OracleDbType.Varchar2)).Direction = ParameterDirection.Output; parameters[2].Size = 255;
				(parameters[3] = connect.Parameter("P_RESULT", OracleDbType.Int32)).Direction = ParameterDirection.Output; parameters[3].Size = 255;

				connect.FillProc(storeName, parameters, ref ds);
				cardNumber = (parameters[2].Value.ToString());
			}
			catch (Exception ex)
			{
				log.Error("[IssuingCardData][GetCardByContrSuffix] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return cardNumber;
		}

		public string GetContStatus(string contractNumber)
		{
			string status = "";
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", CShared.schemaWay4, CShared.packageWay4, "GET_CONTRACT_STATUS");
			log.InfoFormat("[GetContStatus] storeName={0}", storeName);
			log.InfoFormat("[GetContStatus] contractNumber={0}", contractNumber);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[2];
				(parameters[0] = connect.Parameter("P_CONTRACT_NUMBER", OracleDbType.Varchar2)).Value = contractNumber;
				(parameters[1] = connect.Parameter("P_DATA", OracleDbType.Varchar2)).Direction = ParameterDirection.Output; parameters[1].Size = 1000;

				connect.FillProc(storeName, parameters, ref ds);
				status = parameters[1].Value.ToString();
				log.InfoFormat("[GetContStatus] status={0}", status);
			}
			catch (Exception ex)
			{
				log.Error("[GetContStatus] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return status;
		}

		public string CheckExpiredMainCard(string contractNumber)
		{
			string status = "";
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", CShared.schemaWay4, CShared.packageWay4, "CHECK_MAINCARD_EXPIRED");
			log.InfoFormat("[CheckExpiredMainCard] storeName={0}", storeName);
			log.InfoFormat("[CheckExpiredMainCard] contractNumber={0}", contractNumber);

			DataSet ds = new DataSet();
			try
			{
				OracleParameter[] parameters = new OracleParameter[3];
				(parameters[0] = connect.Parameter("P_CONTRACT_NUMBER", OracleDbType.Varchar2)).Value = contractNumber;
				(parameters[1] = connect.Parameter("P_DATA", OracleDbType.Varchar2)).Direction = ParameterDirection.Output; parameters[1].Size = 1000;
				(parameters[2] = connect.Parameter("P_RESULT", OracleDbType.Varchar2)).Direction = ParameterDirection.Output; parameters[2].Size = 1000;

				connect.FillProc(storeName, parameters, ref ds);

				log.InfoFormat("[CheckExpiredMainCard] result={0}", parameters[2].Value.ToString());
				if (parameters[2].Value.ToString() == "Success")
				{
					status = parameters[1].Value.ToString();
					log.InfoFormat("[CheckExpiredMainCard] status={0}", status);
				}
			}
			catch (Exception ex)
			{
				log.Error("[CheckExpiredMainCard] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return status;
		}

		// Get credit policy info
		public Result GetContractCreditPolicy(string contractNumber)
		{
			Result rs = new Result();
			rs.para2 = "";
			SqlUtil connect = new SqlUtil("WAY4");
			string procedureName = string.Format("{0}.{1}.HDB_GET_CONTRACT_DETAIL", CShared.schemaWay4, "OPT_HDB_CARD_MANAGERMANT_TWO");
			OracleParameter[] parameters = new OracleParameter[4];
			try
			{
				log.InfoFormat("[HDB][OPT_HDB_CARD_MANAGERMANT_TWO][HDB_GET_CONTRACT_DETAIL]:  ");
				parameters[0] = connect.Parameter("P_CONTRACT_NUMBER", OracleDbType.Varchar2, contractNumber);
				parameters[1] = connect.Parameter("P_DATA", OracleDbType.RefCursor);
				parameters[1].Direction = ParameterDirection.Output;
				(parameters[2] = connect.Parameter("P_CODE", OracleDbType.Varchar2)).Direction = ParameterDirection.Output;
				parameters[2].Size = 20;
				(parameters[3] = connect.Parameter("P_ERRM", OracleDbType.Varchar2)).Direction = ParameterDirection.Output;
				parameters[3].Size = 200;
				DataSet ds = new DataSet();

				connect.FillProc(procedureName, parameters, ref ds);
				if (parameters[2].Value.ToString().Equals("00"))
				{
					if (ds.Tables[0].Rows.Count != 0)
					{
						List<CONTRACT_DETAIL> result = ds.Tables[0].ToList<CONTRACT_DETAIL>();
						rs.para2 = result[0].CREDIT_POLICY;
						rs.code = "00";
						rs.message = "Query Success";
					}
					else
					{
						rs.code = "01";
						rs.message = "Không tìm thấy thông tin";
					}

				}
				else
				{
					log.InfoFormat("[GetContractCreditPolicy] => Error query {0}, {1}", parameters[2].Value.ToString(), parameters[3].Value.ToString());
					rs.code = "01";
					rs.message = "Lỗi truy vấn thông tin";
				}
			}
			catch (Exception ex)
			{
				log.Error("[Exception][GetContractCreditPolicy] " + ex.Message, ex);
				rs.code = "01";
				rs.message = "Lỗi truy vấn thông tin";
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}
			return rs;
		}

		public static string getInfoPriCustomer360(string clientNumber)
		{
			string priCustomerCode = "";
			try
			{
				#region Build Message API
				ReqMessage request = new ReqMessage
				{
					//Channel = CShared.Channel,
					Channel = "EBANKING",
					PartnerId = CShared.PartnerId,
					RequestId = Guid.NewGuid().ToString(),
					RequestTime = DateTime.Now.ToLongTimeString()
				};
				request.ServiceCode = "170";

				request.Extras = new Dictionary<string, object>();
				request.Extras.Add("cif", clientNumber);
				#endregion

				log.InfoFormat("[IssuingCard][getInfoPriCustomer360] ===>>>>>> request = {0}", Newtonsoft.Json.JsonConvert.SerializeObject(request));
				GWSocket_Way4 socket = new GWSocket_Way4();
				ResMessage response = socket.SendAndReceive(request);
				log.InfoFormat("[IssuingCard][getInfoPriCustomer360] ===<<<<<< response = {0}", Newtonsoft.Json.JsonConvert.SerializeObject(response));

				if (response == null)
				{
					return "";
				}
				else if ("1".Equals(response.Extras["isExisted"].ToString()))
				{
					priCustomerCode = response.Extras["code"].ToString();
				}

				return priCustomerCode;
			}
			catch (Exception e)
			{
				log.Error("[IssuingCard][getInfoPriCustomer360] ===>>>>>> Exception: ", e);
				return "";
			}
		}

		/// <summary>
		/// Load danh sách sản phẩm debit
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		public static List<CBoxData> LoadAllProductDN(string type = "")
		{

			Issuing_Config oConfig = new Issuing_Config();
			List<Issuing_Config> lsTmp = oConfig.GetProductsDN(type);
			List<CBoxData> lsResult = new List<CBoxData>();
			foreach (var item in lsTmp)
			{
				lsResult.Add(new CBoxData
				{
					CODE = item.CONTRACTPRODUCTCODE,
					NAME = item.CONTRACTPRODUCTNAME
				});
			}

			return lsResult;
		}
	}
}