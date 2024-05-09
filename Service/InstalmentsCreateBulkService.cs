using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DBManager;
using Instalments.Models;
using log4net;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;


namespace Instalments.Service
{
	public class InstalmentsCreateBulkService
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly string schemaWAY4 = CShared.schemaWay4;
		private readonly string package = "OPT_HDB_CM_CARD_BULK_SERVICE";

		public ResultMessage InsertFileMasterInstalmentsCreateBulk(FileMasterInstalmentsCreateBulk data)
		{
			ResultMessage result = new ResultMessage();

			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "INSERT_HDB_CM_BULK_FILE_MASTER");

			log.InfoFormat("[InstalmentsCreateBulkService][InsertFileMasterInstalmentsCreateBulk] storeName={0}", storeName);
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_UUID", OracleDbType = OracleDbType.Varchar2, Value = data.UUID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CHANNEL_TYPE", OracleDbType = OracleDbType.Varchar2, Value = "PORTAL_CM", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_FILE_NAME", OracleDbType = OracleDbType.Varchar2, Value = data.FILE_NAME, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_SERVICE_ID", OracleDbType = OracleDbType.Varchar2, Value = "INST_CREATE_BULK", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_SERVICE_TYPE", OracleDbType = OracleDbType.Varchar2, Value = "INST_CREATE_BULK", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_PRODUCT", OracleDbType = OracleDbType.Varchar2, Value = "INSTALMENT", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_PROCESS_TYPE", OracleDbType = OracleDbType.Varchar2, Value = "BATCH", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_TOTAL_AMOUNT", OracleDbType = OracleDbType.Int32, Value = data.TOTAL_AMOUNT, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_TOTAL_ROW", OracleDbType = OracleDbType.Int32, Value = data.TOTAL_ROW, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_TOTAL_ROW_SUCCESS", OracleDbType = OracleDbType.Int32, Value = 0, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_BATCH_ID", OracleDbType = OracleDbType.Varchar2, Value = data.BATCH_ID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_CODE", OracleDbType = OracleDbType.Varchar2, Value = "N", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Value = "New record", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CREATE_USER", OracleDbType = OracleDbType.Varchar2, Value = data.CREATE_USER, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CREATE_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = data.CREATE_BRANCH, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_USER", OracleDbType = OracleDbType.Varchar2, Value = data.APPROVE_USER, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = data.APPROVE_BRANCH, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_DATE", OracleDbType = OracleDbType.Varchar2, Value = data.APPROVE_DATE, Direction = ParameterDirection.Input });

				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_CODE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });
				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });


				connect.ExecuteProc(storeName, listParam.ToArray());

				log.Info("[InstalmentsCreateBulkService][InsertFileMasterInstalmentsCreateBulk] Insert file result : " + listParam[listParam.Count - 2].Value.ToString());
				log.Info("[InstalmentsCreateBulkService][InsertFileMasterInstalmentsCreateBulk] Insert file result : " + listParam[listParam.Count - 1].Value.ToString());
				result.code = listParam[listParam.Count - 2].Value.ToString();
				result.message = listParam[listParam.Count - 1].Value.ToString();
			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][InsertFileMasterInstalmentsCreateBulk] Exception: " + ex.Message, ex);
				return result;
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public int InsertBatchDetails(List<InstalmentsCreateBulk> lstData)
		{
			var size = lstData.Count;
			ResultMessage result = new ResultMessage();

			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "INSERT_INST_CREATE_BULK_DETAIL");

			log.InfoFormat("[InstalmentsCreateBulkService][InsertBatchDetails] storeName={0}", storeName);
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_UUID", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.UUID).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_BATCH_ID", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.BATCH_ID).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_FILE_NAME", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.FILE_NAME).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CREATE_USER", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.CREATE_USER).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CREATE_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.CREATE_BRANCH).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_CODE", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.RESULT_CODE).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.RESULT_MESSAGE).ToArray(), Direction = ParameterDirection.Input });

				listParam.Add(new OracleParameter() { ParameterName = "P_CCCD", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.CCCD).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_PHONE", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.PHONE).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CLIENT_NAME", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.CLIENT_NAME).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CONTRACT_NUMBER", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.CONTRACT_NUMBER).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_CARD_NUMBER", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.CARD_NUMBER).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_TRANS_DATE", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.TRANS_DATE).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_TRANS_AMOUNT", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.TRANS_AMOUNT).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_AUTH_CODE", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.AUTH_CODE).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RECORD_ID", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.RECORD_ID).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_MERCHANT", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.MERCHANT).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_TENOR", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.TENOR).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_OPTION_CODE", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.OPTION_CODE).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_NOTE", OracleDbType = OracleDbType.Varchar2, Value = lstData.Select(x => x.NOTE).ToArray(), Direction = ParameterDirection.Input });

				connect.InsertImportBatches(storeName, size, listParam.ToArray());
				log.Info("[InstalmentsCreateBulkService][InsertBatchDetails] Import success: " + size);
			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][InsertBatchDetails] Exception: " + ex.Message, ex);
				return 0;
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return size;
		}

		public List<InstalmentsCreateBulk> ValidateFileInstalmentsCreateBulk(string fileUUID, string batchId)
		{

			List<InstalmentsCreateBulk> listData = new List<InstalmentsCreateBulk>();


			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "VALIDATE_FILE_INST_CREATE_BULK");

			log.InfoFormat("[InstalmentsCreateBulkService][ValidateFileInstalmentsCreateBulkService] storeName={0}", storeName);
			log.InfoFormat("[InstalmentsCreateBulkService][ValidateFileInstalmentsCreateBulkService] fileUUID={0}", fileUUID);
			log.InfoFormat("[InstalmentsCreateBulkService][ValidateFileInstalmentsCreateBulkService] batchId={0}", batchId);

			DataSet ds = new DataSet();
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_FILE_UUID", OracleDbType = OracleDbType.Varchar2, Value = fileUUID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_BATCHID", OracleDbType = OracleDbType.Varchar2, Value = batchId, Direction = ParameterDirection.Input });

				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_DATA", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });


				connect.FillProc(storeName, listParam.ToArray(), ref ds);

				if (ds != null && ds.Tables.Count != 0)
				{
					listData = ds.Tables[0].ToList<InstalmentsCreateBulk>();
				}
				log.Info("[InstalmentsCreateBulkService][ValidateFileInstalmentsCreateBulkService] Result list count: " + listData.Count);
			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][ValidateFileInstalmentsCreateBulkService] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return listData;
		}

		public bool doUpdateResultFileMaster(FileMasterInstalmentsCreateBulk file)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "UPDATE_RESULT_FILE_BULK");
			bool result = true;
			log.InfoFormat("[InstalmentsCreateBulkService][doUpdateResultFileMaster] storeName={0}", storeName);
			log.InfoFormat("[InstalmentsCreateBulkService][doUpdateResultFileMaster] fileUUID={0}", file.UUID);
			log.InfoFormat("[InstalmentsCreateBulkService][doUpdateResultFileMaster] batchId={0}", file.BATCH_ID);
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_UUID", OracleDbType = OracleDbType.Varchar2, Value = file.UUID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_BATCH_ID", OracleDbType = OracleDbType.Varchar2, Value = file.BATCH_ID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_STATUS", OracleDbType = OracleDbType.Varchar2, Value = file.STATUS.ToString(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_USER", OracleDbType = OracleDbType.Varchar2, Value = file.APPROVE_USER, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = file.APPROVE_BRANCH, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_DATE", OracleDbType = OracleDbType.Varchar2, Value = file.APPROVE_DATE, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_CODE", OracleDbType = OracleDbType.Varchar2, Value = file.RESULT_CODE, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Value = file.RESULT_MESSAGE, Direction = ParameterDirection.Input });

				connect.ExecuteProc(storeName, listParam.ToArray());

				log.Info("[InstalmentsCreateBulkService][doUpdateResultFileMaster] Update success file: " + file.UUID);

			}
			catch (Exception ex)
			{
				result = false;
				log.Error("[InstalmentsCreateBulkService][doUpdateResultFileMaster]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public bool doUpdateAttachment(FileMasterInstalmentsCreateBulk file)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "UPDATE_ATTACHMENT_FILE_BULK");
			bool result = true;
			log.InfoFormat("[InstalmentsCreateBulkService][doUpdateAttachment] storeName={0}", storeName);
			log.InfoFormat("[InstalmentsCreateBulkService][doUpdateAttachment] fileUUID={0}", file.UUID);
			log.InfoFormat("[InstalmentsCreateBulkService][doUpdateAttachment] batchId={0}", file.BATCH_ID);
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_UUID", OracleDbType = OracleDbType.Varchar2, Value = file.UUID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_BATCHID", OracleDbType = OracleDbType.Varchar2, Value = file.BATCH_ID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_REMARK", OracleDbType = OracleDbType.Varchar2, Value = file.REMARK, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_ATTACHMENT", OracleDbType = OracleDbType.Varchar2, Value = file.ATTACHMENT, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_ATTACHMENT_PATH", OracleDbType = OracleDbType.Varchar2, Value = file.ATTACHMENT_PATH, Direction = ParameterDirection.Input });

				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_CODE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });
				connect.ExecuteProc(storeName, listParam.ToArray());

				log.Info("[InstalmentsCreateBulkService][doUpdateAttachment] Result code: " + listParam[5]);
				log.Info("[InstalmentsCreateBulkService][doUpdateAttachment] Result message: " + listParam[6]);

			}
			catch (Exception ex)
			{
				result = false;
				log.Error("[InstalmentsCreateBulkService][doUpdateAttachment]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public List<FileMasterInstalmentsCreateBulk> getListFileInstCreateBulk(string branch, string status, string fromDate, string toDate, string userId)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "GET_LIST_FILE_MASTER_CM_BULK");
			log.InfoFormat("[InstalmentsCreateBulkService][getListFileInstCreateBulk] storeName={0}", storeName);
			List<FileMasterInstalmentsCreateBulk> result = new List<FileMasterInstalmentsCreateBulk>();

			DataSet ds = new DataSet();
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = branch, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_STATUS", OracleDbType = OracleDbType.Varchar2, Value = status, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_FROM_DATE", OracleDbType = OracleDbType.Varchar2, Value = fromDate, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_TO_DATE", OracleDbType = OracleDbType.Varchar2, Value = toDate, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_SERVICE_ID", OracleDbType = OracleDbType.Varchar2, Value = "INST_CREATE_BULK", Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_USER_ID", OracleDbType = OracleDbType.Varchar2, Value = userId, Direction = ParameterDirection.Input });


				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });
				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_DATA", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

				connect.FillProc(storeName, listParam.ToArray(), ref ds);

				log.Info("[InstalmentsCreateBulkService][getListFileInstCreateBulk] Result message: " + listParam[listParam.Count - 2].Value.ToString());
				if (ds != null && ds.Tables.Count != 0)
				{
					result = ds.Tables[0].ToList<FileMasterInstalmentsCreateBulk>();
					if (result.Count > 0)
					{
						result.ForEach(x => x.LIST_ATTACHMENT_NAME = x.ATTACHMENT.Split(',').ToList());
					}
				}
				log.Info("[InstalmentsCreateBulkService][getListFileInstCreateBulk] Result list count: " + result.Count);

			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][getListFileInstCreateBulk] Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public List<InstalmentsCreateBulk> getDetailsFileInstCreate(string fileUUID)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "GET_DETAIL_FILE_INST_CREATE_BULK");
			List<InstalmentsCreateBulk> result = new List<InstalmentsCreateBulk>();

			log.InfoFormat("[InstalmentsCreateBulkService][getDetailsFileCancelBulk] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_UUID", OracleDbType = OracleDbType.Varchar2, Value = fileUUID, Direction = ParameterDirection.Input });


				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });
				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_DATA", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

				connect.FillProc(storeName, listParam.ToArray(), ref ds);

				log.Info("[InstalmentsCreateBulkService][getDetailsFileCancelBulk] Result message: " + listParam[listParam.Count - 2].Value.ToString());
				if (ds != null && ds.Tables.Count != 0)
				{
					result = ds.Tables[0].ToList<InstalmentsCreateBulk>();
				}
				log.Info("[InstalmentsCreateBulkService][getDetailsFileCancelBulk] Result list count: " + result.Count);

			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][getDetailsFileCancelBulk]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public FileMasterInstalmentsCreateBulk getFileInstCreateMaster(string fileUUID)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "GET_FILE_MASTER_CM_BULK");
			log.InfoFormat("[InstalmentsCreateBulkService][getFileInstCreateMaster] storeName={0}", storeName);
			List<FileMasterInstalmentsCreateBulk> result = new List<FileMasterInstalmentsCreateBulk>();

			DataSet ds = new DataSet();
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_FILE_UUID", OracleDbType = OracleDbType.Varchar2, Value = fileUUID, Direction = ParameterDirection.Input });


				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });
				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_DATA", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

				connect.FillProc(storeName, listParam.ToArray(), ref ds);

				log.Info("[InstalmentsCreateBulkService][getFileInstCreateMaster] Result message: " + listParam[listParam.Count - 2].Value.ToString());
				if (ds != null && ds.Tables.Count != 0)
				{
					result = ds.Tables[0].ToList<FileMasterInstalmentsCreateBulk>();
					if (result.Count > 0)
					{
						result.ForEach(x => x.LIST_ATTACHMENT_NAME = x.ATTACHMENT.Split(',').ToList());
					}
				}

			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][getFileInstCreateMaster]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result.Count > 0 ? result[0] : null;
		}


		public string updateResultApproveFileMaster(FileMasterInstalmentsCreateBulk file)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "UPDATE_RESULT_APPROVE_FILE_MASTER");
			string result = "";

			log.InfoFormat("[InstalmentsCreateBulkService][updateResultApproveFileMaster] storeName={0}", storeName);

			DataSet ds = new DataSet();
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_UUID", OracleDbType = OracleDbType.Varchar2, Value = file.UUID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_BATCH_ID", OracleDbType = OracleDbType.Varchar2, Value = file.BATCH_ID, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_STATUS", OracleDbType = OracleDbType.Varchar2, Value = file.STATUS.ToString(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_USER", OracleDbType = OracleDbType.Varchar2, Value = file.APPROVE_USER, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = file.APPROVE_BRANCH, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_DATE", OracleDbType = OracleDbType.Varchar2, Value = file.APPROVE_DATE, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_ROW_SUCCESS", OracleDbType = OracleDbType.Varchar2, Value = file.TOTAL_ROW_SUCCESS, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_ROW_FAIL", OracleDbType = OracleDbType.Varchar2, Value = file.TOTAL_ROW_FAIL, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_REJECT_REASON", OracleDbType = OracleDbType.Varchar2, Value = file.REJECT_REASON, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_CODE", OracleDbType = OracleDbType.Varchar2, Value = file.RESULT_CODE, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Value = file.RESULT_MESSAGE, Direction = ParameterDirection.Input });

				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });

				connect.FillProc(storeName, listParam.ToArray(), ref ds);

				OracleString p2val = (OracleString)listParam[listParam.Count - 1].Value;
				if (!p2val.IsNull)
				{
					result = p2val.Value;
				}
				log.InfoFormat("[InstalmentsCreateBulkService][updateResultApproveFileMaster] result={0}", result);
			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][updateResultApproveFileMaster]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public bool doUpdateResultBatch(List<InstalmentsCreateBulk> listData, string fileUUID, string batchId)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "UPDATE_RESULT_APPROVE_INST_CREATE_DETAIL");
			bool result = true;
			log.InfoFormat("[InstalmentsCreateBulkService][doUpdateResultBatch] storeName={0}", storeName);
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_UUID", OracleDbType = OracleDbType.Varchar2, Value = listData.Select(x => x.UUID).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_BATCH_ID", OracleDbType = OracleDbType.Varchar2, Value = listData.Select(x => x.BATCH_ID).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_USER", OracleDbType = OracleDbType.Varchar2, Value = listData.Select(x => x.APPROVE_USER).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = listData.Select(x => x.APPROVE_BRANCH).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_APPROVE_BRANCH", OracleDbType = OracleDbType.Varchar2, Value = listData.Select(x => x.APPROVE_DATE).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_CODE", OracleDbType = OracleDbType.Varchar2, Value = listData.Select(x => x.RESULT_CODE).ToArray(), Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_RESULT_MESSAGE", OracleDbType = OracleDbType.Varchar2, Value = listData.Select(x => x.RESULT_MESSAGE).ToArray(), Direction = ParameterDirection.Input });


				connect.InsertImportBatches(storeName, listData.Count, listParam.ToArray());
			}
			catch (Exception ex)
			{
				result = false;
				log.Error("[InstalmentsCreateBulkService][doUpdateResultBatch]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public string checkFileExist(string fileName)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "GET_FILE_ID");
			string result = "";

			log.InfoFormat("[InstalmentsCreateBulkService][checkFileExist] storeName={0}", storeName);
			log.InfoFormat("[InstalmentsCreateBulkService][checkFileExist] fileName={0}", fileName);

			DataSet ds = new DataSet();
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_FILE_NAME", OracleDbType = OracleDbType.Varchar2, Value = fileName, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_FILE_UUID", OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Output, Size = 250 });
				connect.FillProc(storeName, listParam.ToArray(), ref ds);

				OracleString p2val = (OracleString)listParam[1].Value;
				if (!p2val.IsNull)
				{
					result = p2val.Value;
				}
				log.InfoFormat("[InstalmentsCreateBulkService][checkFileExist] result={0}", result);
			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][checkFileExist]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		/// <summary>
		/// Kiểm tra GD reversal
		/// </summary>
		/// <param name="docId"></param>
		/// <returns>Trả về 0 khi GD không có xử lý reversal, và 1 khi GD có xử lý reversal</returns>
		public int checkReversalTrans(string docId)
		{
			SqlUtil connect = new SqlUtil("WAY4");
			string storeName = string.Format("{0}.{1}.{2}", schemaWAY4, package, "GET_FILE_ID");
			int result = 0;

			log.InfoFormat("[InstalmentsCreateBulkService][checkReversalTrans] storeName={0}", storeName);
			log.InfoFormat("[InstalmentsCreateBulkService][checkReversalTrans] docId={0}", docId);

			DataSet ds = new DataSet();
			try
			{
				List<OracleParameter> listParam = new List<OracleParameter>();

				listParam.Add(new OracleParameter() { ParameterName = "P_RECORD_ID", OracleDbType = OracleDbType.Varchar2, Value = docId, Direction = ParameterDirection.Input });
				listParam.Add(new OracleParameter() { ParameterName = "P_OUT_DATA", OracleDbType = OracleDbType.Int32, Direction = ParameterDirection.Output, Size = 250 });
				connect.FillProc(storeName, listParam.ToArray(), ref ds);


				result = Int32.Parse(listParam[1].Value.ToString());
				log.InfoFormat("[InstalmentsCreateBulkService][checkReversalTrans] result={0}", result);
			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCreateBulkService][checkReversalTrans]  Exception: " + ex.Message, ex);
			}
			finally
			{
				try
				{
					connect.Close();
				}
				catch { }
			}

			return result;
		}

		public string SentDataCreateInstalment(string pContractNumber, string pCardNumber, string pDocID, string pInstalmentServiceCode,
		string pTenor, string pOptionCode, string pReason)
		{
			string result = "";

			ReqMessage request = new ReqMessage
			{
				Channel = CShared.Channel,
				PartnerId = CShared.PartnerId,
				RequestId = Guid.NewGuid().ToString(),
				ServiceCode = "83",
				RequestTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
			};
			request.Extras = new Dictionary<string, object>();
			request.Extras.Add("reason", String.IsNullOrEmpty(pReason) ? "Create instalment plan" : pReason);
			request.Extras.Add("contractNumber", pContractNumber);
			request.Extras.Add("cardNumber", pCardNumber);
			request.Extras.Add("docId", pDocID);
			request.Extras.Add("instalmentServiceCode", pInstalmentServiceCode);
			request.Extras.Add("tenor", pTenor);
			request.Extras.Add("optionCode", pOptionCode);

			log.InfoFormat("[InstalmentsModel][SentDataCreateInstalment] ===>>>>>> request = {0}", JsonConvert.SerializeObject(request));
			GWSocket_Way4 socket = new GWSocket_Way4();
			ResMessage response = socket.SendAndReceive(request);
			log.InfoFormat("[InstalmentsModel][SentDataCreateInstalment] ===<<<<<< response = {0}", JsonConvert.SerializeObject(response));

			try
			{
				result = JsonConvert.SerializeObject(response);
			}
			catch (Exception e)
			{
				log.InfoFormat("[InstalmentsModel][SentDataCreateInstalment] ===<<<<<< Exception: {0}", e.Message);
			}

			return result;
		}
	}
}