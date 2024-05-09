using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instalments.Models;
using Instalments.Service;
using log4net;
using Newtonsoft.Json;
using ThuHoTaiQuay.Authencation;


namespace Instalments.Controllers
{
    public class InstalmentsCardsBulkApproveController : Controller
    {
		InstalmentsCreateBulkService service = new InstalmentsCreateBulkService();

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		// GET: InstalmentsCardsBulkApprove
		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult Index()
        {
			ViewBag.Menu = CShared.GenerateMenu(Request, (String)RouteData.Values["id"]);
			ViewData["sFromDate"] = Request.Params["sFromDate"] ?? "";
			ViewData["sToDate"] = Request.Params["sToDate"] ?? "";
			ViewData["sBranch"] = Request.Params["sBranch"] ?? "";
			ViewData["sStatus"] = Request.Params["sStatus"] ?? "";
			ViewData["sUserId"] = Request.Params["txtUserId"] ?? "";
			return View();

        }
		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult SearchData()
		{
			DateTime dtFDate = new DateTime();
			DateTime dtTDate = new DateTime();
			string cbBranch = Request.Params["cbBranch"] ?? "";
			string cbStatus = Request.Params["cbStatus"] ?? "";
			string sFromDate = Request.Params["txtFromDate"] ?? "";
			string sToDate = Request.Params["txtToDate"] ?? "";
			string sUserId = Request.Params["txtUserId"] ?? "";
			//Validating data inputed
			if (string.IsNullOrEmpty(sFromDate))
			{
				return Json(new ResultMessage
				{
					code = "01",
					message = "Vui lòng nhập từ ngày!"
				}, JsonRequestBehavior.AllowGet);
			}

			if (string.IsNullOrEmpty(sToDate))
			{
				return Json(new ResultMessage
				{
					code = "01",
					message = "Vui lòng nhập đến ngày!"
				}, JsonRequestBehavior.AllowGet);
			}

			bool bValidFromDate = DateTime.TryParseExact(sFromDate, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtFDate);
			bool bValidToDate = DateTime.TryParseExact(sToDate, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtTDate);
			if (!bValidFromDate)
			{
				return Json(new ResultMessage
				{
					code = "01",
					message = "Vui lòng nhập từ ngày theo định dạng dd/mm/yyyy!"
				}, JsonRequestBehavior.AllowGet);
			}

			if (!bValidToDate)
			{
				return Json(new ResultMessage
				{
					code = "01",
					message = "Vui lòng nhập đến ngày theo định dạng dd/mm/yyyy!"
				}, JsonRequestBehavior.AllowGet);
			}

			if (dtFDate > dtTDate)
			{
				return Json(new ResultMessage
				{
					code = "01",
					message = "Vui lòng nhập từ ngày nhỏ hơn đến ngày!"
				}, JsonRequestBehavior.AllowGet);
			}

			if ((dtTDate - dtFDate).TotalDays > 7)
			{
				return Json(new ResultMessage
				{
					code = "01",
					message = "Vui lòng nhập trong khoảng thời gian 7 ngày!"
				}, JsonRequestBehavior.AllowGet);
			}

			List<FileMasterInstalmentsCreateBulk> lsResult = new List<FileMasterInstalmentsCreateBulk>();
			//GetData

			lsResult = service.getListFileInstCreateBulk(cbBranch, cbStatus, sFromDate, sToDate, sUserId);


			return PartialView("SearchData", lsResult);
		}

		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult ViewAndApprove(string uuid)
		{
			if (string.IsNullOrEmpty(uuid))
			{
				return Json(new ResultMessage
				{
					code = "01",
					message = "Lỗi thông tin uuid của file, vui lòng tải lại trang!"
				}, JsonRequestBehavior.AllowGet);
			}

			List<InstalmentsCreateBulk> lsResult = new List<InstalmentsCreateBulk>();
			//GetData
			lsResult = service.getDetailsFileInstCreate(uuid);
			var file = service.getFileInstCreateMaster(uuid);
			if (file != null)
			{
				ViewBag.userNhap = file.CREATE_USER;
				ViewBag.ngayNhap = file.CREATE_DATE;
				ViewBag.userDuyet = file.APPROVE_USER;
				ViewBag.ngayDuyet = file.APPROVE_DATE;
				ViewBag.remark = file.REMARK;
				ViewBag.rejectReason = file.REJECT_REASON;
				ViewBag.fileUUID = file.UUID;
			}
			

			return PartialView("ViewAndApprove", lsResult);
		}
		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult doApprove(string fileUUID, string batchId, string statusApprove, int statusFile, string rejectReason)
		{
			try
			{
				SESSION_PARA oPra = CShared.getSession();
				int countSuccess = 0;
				var resCode = "";
				var resMessage = "";
				var status = 3; 
				if (statusFile != 1)
				{
					return Json(new ResultMessage
					{
						code = "01",
						message = "File đã được xử lý trước đó !"
					}, JsonRequestBehavior.AllowGet);
				}

				FileMasterInstalmentsCreateBulk file = service.getFileInstCreateMaster(fileUUID);
				if (file == null)
				{
					return Json(new ResultMessage
					{
						code = "01",
						message = "Không tìm thấy thông tin file !"
					}, JsonRequestBehavior.AllowGet);
				}
				file.APPROVE_BRANCH = oPra.oAccount.Branch;
				file.APPROVE_USER = oPra.oAccount.UserName;
				file.APPROVE_DATE = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
				file.REJECT_REASON = rejectReason;

				List<InstalmentsCreateBulk> listDetails = new List<InstalmentsCreateBulk>();
				listDetails = service.getDetailsFileInstCreate(fileUUID);
				if (listDetails.Count == 0)
				{
					return Json(new ResultMessage
					{
						code = "01",
						message = "Không tìm thấy thông tin file !"
					}, JsonRequestBehavior.AllowGet);
				}

				if (statusApprove.Equals("Approve"))
				{
					DoInstalmentsListCard(listDetails);
					countSuccess = listDetails.FindAll(x => x.RESULT_CODE.Equals("S")).Count;
					resCode = "S";
					resMessage = listDetails.Count == countSuccess ? "Đã duyệt thành công. " : "Có dòng lỗi trong file hủy thẻ. ";
					status = 2;
				}
				else if (statusApprove.Equals("Cancel"))
				{
					resCode = "C";
					resMessage = "Đã hủy file.";
					status = 3;
				}

				// Update result file
				file.RESULT_CODE = resCode;
				file.RESULT_MESSAGE = resMessage;
				file.STATUS = status;
				file.TOTAL_ROW_SUCCESS = countSuccess;
				file.TOTAL_ROW_FAIL = listDetails.Count - countSuccess;

				var updateFile = service.updateResultApproveFileMaster(file);
				if (!updateFile.Equals("Success"))
				{
					return Json(new ResultMessage
					{
						code = "01",
						message = "Lỗi khi phê duyệt file, vui lòng thử lại ! "
					}, JsonRequestBehavior.AllowGet);
				}

				listDetails.ForEach(x =>
				{
					if (resCode == "C")
					{
						x.RESULT_CODE = "C";
						x.RESULT_MESSAGE = "Đã hủy dữ liệu. ";
					}
					
					x.APPROVE_BRANCH = file.APPROVE_BRANCH;
					x.APPROVE_USER = file.APPROVE_USER;
					x.APPROVE_DATE = file.APPROVE_DATE;
					x.BATCH_ID = batchId;
				});
				if (!service.doUpdateResultBatch(listDetails, fileUUID, batchId))
				{
					return Json(new ResultMessage
					{
						code = "01",
						message = "Lỗi cập nhật kết quả trả góp thẻ thẻ, vui lòng liên hệ CNTT !"
					}, JsonRequestBehavior.AllowGet);
				}

				return Json(new ResultMessage
				{
					code = "00",
					message = statusApprove.Equals("Approve") ? "Bạn đã duyệt trả góp thẻ thành công " + countSuccess + " thẻ, duyệt trả góp thẻ không thành công "
					+ file.TOTAL_ROW_FAIL + " thẻ. " : "Bạn đã từ chối hủy " + listDetails.Count + " thẻ."
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				log.Error("[InstalmentsCardsBulkApprove][doApprove] Exception: ", e);
				return Json(new ResultMessage
				{
					code = "01",
					message = "Có lỗi trong quá trình phê duyệt !"
				}, JsonRequestBehavior.AllowGet);
			}
		}

		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult exportEcxelReport(string reportType,string fileUUID)
		{
			try
			{
				SESSION_PARA session = CShared.getSession();
				List<InstalmentsCreateBulk> exportData = new List<InstalmentsCreateBulk>();
				var data = service.getDetailsFileInstCreate(fileUUID);
				string fileName = session.oAccount.UserName + "_export_tra_gop_the_theo_lo" + DateTime.Now.ToString("hhmmssddMMyyyy") + ".xlsx";
				string path = Server.MapPath("~/Upload/Instalments/InstalmentsCardsBulk/Template-InstalmentsCardBulkApprove-Export.xlsx");
				excelXLSXTools _excelTools = new excelXLSXTools();
				DataTable table = new DataTable();
				MemoryStream ms = null;

				switch (reportType)
				{
					case "1":
						fileName = session.oAccount.UserName + "_export_tra_gop_the_theo_lo_hop_le" + DateTime.Now.ToString("hhmmssddMMyyyy") + ".xlsx";
						exportData.AddRange(data.FindAll(x => x.RESULT_CODE.Equals("N") || x.RESULT_CODE.Equals("A")));
						break;
					case "2":
						fileName = session.oAccount.UserName + "_export_tra_gop_the_theo_lo_loi" + DateTime.Now.ToString("hhmmssddMMyyyy") + ".xlsx";
						exportData.AddRange(data.FindAll(x => !x.RESULT_CODE.Equals("N") && !x.RESULT_CODE.Equals("A")));
						break;
					default:
						exportData.AddRange(data);
						break;
				}
				exportData.ForEach(x => x.CARD_NUMBER = x.CARD_NUMBER.Length >= 16 ? CShared.replaceToXXX(x.CARD_NUMBER) : x.CARD_NUMBER);

				table = _excelTools.ConvertToDataTable<InstalmentsCreateBulk>(exportData, new string[] {
				"CCCD", "PHONE", "CLIENT_NAME", "CONTRACT_NUMBER", "CARD_NUMBER", "TRANS_DATE", "TRANS_AMOUNT", "AUTH_CODE", "RECORD_ID", "MERCHANT", "TENOR", "OPTION_CODE", "NOTE", "STATUS", "CREATE_USER", "CREATE_DATE", "CREATE_BRANCH", "APPROVE_USER", "APPROVE_DATE", "APPROVE_BRANCH", "RESULT_CODE", "RESULT_MESSAGE", "REJECT_REASON"
				});
				ms = _excelTools.ExportFileExcel(table, path, 2, false);
				string handle = Guid.NewGuid().ToString();
				TempData[handle] = ms.ToArray();
				return Json(new { code = "00", message = fileName, data = handle }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				log.Error("[InstalmentsCardsBulkApprove][exportEcxelReport] Exception: ", e);
				return Json(new ResultMessage
				{
					code = "01",
					message = "Xảy ra lỗi khi xuất excel! Vui lòng thực hiện lại."
				}, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult Download(string fileGuid, string fileName)
		{
			if (TempData[fileGuid] != null)
			{
				byte[] data = TempData[fileGuid] as byte[];
				return File(data, "application/vnd.ms-excel", fileName);
			}
			else
			{
				return Json(new { code = "01", message = "Lỗi khi xuất dữ liệu file !" }, JsonRequestBehavior.AllowGet);
			}
		}



		private void DoInstalmentsListCard(List<InstalmentsCreateBulk> listData)
		{
			try
			{
				foreach (var info in listData)
				{
					if (service.checkReversalTrans(info.RECORD_ID) != 1)
					{

						string result = service.SentDataCreateInstalment(info.CONTRACT_NUMBER, info.CARD_NUMBER, info.RECORD_ID, "INSTRTLS", info.TENOR, info.OPTION_CODE, info.REJECT_REASON);
						ResMessage response = JsonConvert.DeserializeObject<ResMessage>(result);
						log.InfoFormat("[InstalmentsCardsBulkApprove][SaveInstalmentsApproveCard] ===<<<<<< response = {0}", Newtonsoft.Json.JsonConvert.SerializeObject(response));
						if (response != null)
						{
							info.RESULT_CODE = response.ResponseCode == "0" ? "S" : "E" + response.ResponseCode;
							info.RESULT_MESSAGE = !String.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Lỗi khi gọi way4";
						}
						else
						{
							info.RESULT_CODE = "E99";
							info.RESULT_MESSAGE = "Không nhận được phản hồi từ Way4";
						}
					}
					else
					{
						info.RESULT_CODE = "E101";
						info.RESULT_MESSAGE = "GD đã reversal";
					}


				}
			}
			catch (Exception ex)
			{
				log.InfoFormat("[InstalmentsCardsBulkApprove][DoInstalmentsListCard] ===<<<<<< Exception: {0}", ex.Message);
			}
		}





	}
}