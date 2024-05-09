using Instalments.Models;
using Instalments.Service;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuHoTaiQuay.Authencation;

namespace Instalments.Controllers
{
    public class InstalmentsCardsBulkReportController : Controller
    {
		InstalmentsCreateBulkService service = new InstalmentsCreateBulkService();

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		// GET: InstalmentsCardsBulkReport
		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult Index()
        {
			ViewBag.Menu = CShared.GenerateMenu(Request, (String)RouteData.Values["id"]);
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

			//List<FileMasterInstalmentsCreateBulk> lsResult = new List<FileMasterInstalmentsCreateBulk>();
			//GetData

			//lsResult = service.getListFileInstCreateBulk(cbBranch, cbStatus, sFromDate, sToDate,sUserId);



			List<FileMasterInstalmentsCreateBulk> lsResult = new List<FileMasterInstalmentsCreateBulk>()
			{
				new FileMasterInstalmentsCreateBulk
				{
					UUID = "1",
					CHANNEL_TYPE = "Online",
					FILE_NAME = "instalments_20240506",
					SERVICE_ID = "12345",
					SERVICE_TYPE = "Installment",
					PRODUCT = "Loan",
					PROCESS_TYPE = "Bulk Creation",
					TOTAL_AMOUNT = "10000",
					TOTAL_ROW = 100,
					TOTAL_ROW_SUCCESS = 80,
					TOTAL_ROW_FAIL = 20,
					BATCH_ID = "BATCH001",
					DIRECTION = "Inbound",
					RECEIVE_DATE = "2024-05-06",
					PROCESS_START_DATE = "2024-05-06 08:00:00",
					PROCESS_END_DATE = "2024-05-06 10:00:00",
					RESULT_CODE = "SUCCESS",
					RESULT_MESSAGE = "All records processed successfully",
					IS_PASS_LOOP = "Yes",
					CREATE_USER = "Admin",
					CREATE_BRANCH = "Head Office",
					CREATE_DATE = "2024-05-06 08:00:00",
					APPROVE_USER = "Supervisor",
					APPROVE_BRANCH = "Head Office",
					APPROVE_DATE = "2024-05-06 09:00:00",
					UPDATE_TIME = "2024-05-06 10:00:00",
					STATUS = 1,
					REMARK = "Bulk creation completed",
					ATTACHMENT = "instalments_bulk_file.zip",
					LIST_ATTACHMENT_NAME = new List<string> { "sn.pdf", "sn.pdf" },
					ATTACHMENT_PATH = "/Upload/Instalments/InstalmentsCardsBulk/ChungTu/052024/07052024/",
					REJECT_REASON = ""
				},
				new FileMasterInstalmentsCreateBulk
				{
					UUID = "2",
					CHANNEL_TYPE = "Offline",
					FILE_NAME = "instalments_20240505",
					SERVICE_ID = "54321",
					SERVICE_TYPE = "Installment",
					PRODUCT = "Loan",
					PROCESS_TYPE = "Bulk Creation",
					TOTAL_AMOUNT = "15000",
					TOTAL_ROW = 120,
					TOTAL_ROW_SUCCESS = 100,
					TOTAL_ROW_FAIL = 20,
					BATCH_ID = "BATCH002",
					DIRECTION = "Inbound",
					RECEIVE_DATE = "2024-05-05",
					PROCESS_START_DATE = "2024-05-05 07:00:00",
					PROCESS_END_DATE = "2024-05-05 09:30:00",
					RESULT_CODE = "SUCCESS",
					RESULT_MESSAGE = "All records processed successfully",
					IS_PASS_LOOP = "No",
					CREATE_USER = "Admin",
					CREATE_BRANCH = "Branch Office",
					CREATE_DATE = "2024-05-05 07:00:00",
					APPROVE_USER = "Supervisor",
					APPROVE_BRANCH = "Branch Office",
					APPROVE_DATE = "2024-05-05 09:00:00",
					UPDATE_TIME = "2024-05-05 09:30:00",
					STATUS = 1,
					REMARK = "Bulk creation completed",
					ATTACHMENT = "instalments_bulk_file.zip",
					LIST_ATTACHMENT_NAME = new List<string> { "sn.pdf", "sn.pdf" },
					ATTACHMENT_PATH = "/Upload/Instalments/InstalmentsCardsBulk/ChungTu/052024/07052024/",
					REJECT_REASON = ""
				}
			};

			return PartialView("SearchData", lsResult);
		}

		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult ViewDetail(string uuid)
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
			// Get Data
			/*lsResult = service.getDetailsFileInstCreate(uuid);

			// Get file
			var file = service.getFileInstCreateMaster(uuid);
			if (file != null)
			{
				ViewBag.userNhap = file.CREATE_USER;
				ViewBag.ngayNhap = file.CREATE_DATE;
				ViewBag.userDuyet = file.APPROVE_USER;
				ViewBag.ngayDuyet = file.APPROVE_DATE;
				ViewBag.remark = file.REMARK;
				ViewBag.rejectReason = file.REJECT_REASON;
			}
			*/

			lsResult.Add(new InstalmentsCreateBulk("68202009527", "0964995622", "NGO HOANG PHUC", "9995885858", "5321371111293086", "04/05/2024", "500000", "9999", "9999", "Phuc", "3", "R3MTHS", "INSTRTLS"));
			lsResult.Add(new InstalmentsCreateBulk("68202009528", "0964995623", "NGO HOANG PHUC", "9995885859", "5321371111293087", "04/05/2025", "500000", "9999", "9999", "Phuc", "6", "R6MTHS", "INSTRTLS"));
			lsResult.Add(new InstalmentsCreateBulk("68202009529", "0964995624", "NGO HOANG PHUC", "9995885859", "5321371111293086", "04/05/2026", "500000", "9999", "9999", "Phuc", "9", "R9MTHS", "INSTRTLS"));

			foreach (var item in lsResult)
			{
				item.RESULT_CODE = "A";
				item.RESULT_MESSAGE = "Approve";
			}
			ViewBag.userNhap = "Test";
			ViewBag.ngayNhap = "06/05/2024";
			ViewBag.userDuyet = "Phuc";
			ViewBag.ngayDuyet = "06/05/2024";
			ViewBag.remark = "DUyet lo the như quan que";
			ViewBag.rejectReason = "";
			ViewBag.fileUUID = "test51456465";
			return PartialView("ViewDetail", lsResult);
		}

		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult exportEcxelReport(string reportType, string fileUUID)
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
				log.Error("[CancelCardsBulkApprove][exportEcxelReport] Exception: ", e);
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

	}
}