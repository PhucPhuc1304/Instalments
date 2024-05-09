using Instalments.Models;
using Instalments.Models.Instalments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Instalments.Service;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Reflection;
using System.Data;
using ThuHoTaiQuay.Authencation;



namespace Instalments.Controllers
{

    public class InstalmentsCardsBulkController : Controller
    {

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		// GET: InstalmentsBulk
		[CustomAuthorize(Roles = "ttt-review;ttt-report")] 
		public ActionResult Index()
        {
			ViewBag.Menu = CShared.GenerateMenu(Request, (String)RouteData.Values["id"]);
			return View();
        }
		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult UploadFiles()
		{
			ResultMessage rp = new ResultMessage();
			rp.code = "00";
			rp.message = "";
			try
				{
					SESSION_PARA oPra = CShared.getSession();
					InstalmentsCreateBulkService service = new InstalmentsCreateBulkService();
					string path = Server.MapPath(string.Format("~/Upload/Instalments/InstalmentsCardsBulk/UploadData/{0}/", DateTime.Now.ToString("MMyyyy")));
					string folderRoot = string.Format("/Upload/Instalments/InstalmentsCardsBulk/UploadData/{0}/", DateTime.Now.ToString("MMyyyy"));
					CreateFolder(folderRoot);
					HttpPostedFileBase file = Request.Files[0];
					var fileName = Path.GetFileName(file.FileName);
					var _ext = Path.GetExtension(file.FileName);
					if (!String.IsNullOrEmpty(service.checkFileExist(fileName.Substring(0, fileName.IndexOf(".xls")))))
					{
						rp.message = "File đã tồn tại trên hệ thống! Vui lòng upload file mới hoặc đổi tên file! ";
						rp.code = "01";
						return Json(rp, JsonRequestBehavior.AllowGet);
					}

					string lFileName = fileName + "_" + DateTime.Now.Ticks;
					path = path + lFileName;
					ViewBag.MsgFileName = lFileName;
					file.SaveAs(path);
					rp.para1 = lFileName;
					rp.para2 = fileName;
				}
				catch (Exception ex)
				{
				log.Error("[Instalments][InstalmentsCardBulk] Exception: " + ex.Message, ex);

				rp.message = ex.Message;
				rp.code = "01";
			}
			return Json(rp, JsonRequestBehavior.AllowGet);
		}
		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		[HttpPost]
		public ActionResult LoadFile(ImportFileModel rq)
		{
			List<InstalmentsCreateBulk> lstCard = new List<InstalmentsCreateBulk>();
			FileMasterInstalmentsCreateBulk fileMaster = new FileMasterInstalmentsCreateBulk();
			ResultMessage rp = new ResultMessage();
			InstalmentsCreateBulkService service = new InstalmentsCreateBulkService();
			var totalRow = 0;
			var totalRowDung = 0;
			var totalRowSai = 0;
			if (string.IsNullOrEmpty(rq.FileName))
				return PartialView("SearchData", lstCard);
			{
				try
				{
					SESSION_PARA oPra = CShared.getSession();
					var account = oPra.oAccount;
					var batchId = Guid.NewGuid().ToString();

					string path = Server.MapPath(string.Format("~/Upload/Instalments/InstalmentsCardsBulk/UploadData/{0}/", DateTime.Now.ToString("MMyyyy")));
					string folderRoot = string.Format("/Upload/Instalments/InstalmentsCardsBulk/UploadData/{0}/", DateTime.Now.ToString("MMyyyy"));
					string fileLocation = path + rq.FileName;
					MemoryStream fileStream = null;
					try
					{
						fileMaster.UUID = Guid.NewGuid().ToString();
						fileMaster.FILE_NAME = rq.FileName.Substring(0, rq.FileName.IndexOf(".xls"));
						fileMaster.BATCH_ID = batchId;
						fileMaster.CREATE_USER = account.UserName;
						fileMaster.CREATE_BRANCH = account.Branch;
						// read file data
						fileStream = ExcelUtil.GetFileStream(fileLocation);
						fileStream.Position = 0;

						IWorkbook workbook = null;
						if (rq.FileName.IndexOf(".xlsx") > 0)
							workbook = new XSSFWorkbook(fileStream);
						else if (rq.FileName.IndexOf(".xls") > 0)
							workbook = new HSSFWorkbook(fileStream);

						ISheet sheet = workbook.GetSheetAt(0);
						InstalmentsCreateBulk manager = new InstalmentsCreateBulk();

						for (int rowNumber = 1; rowNumber <= sheet.LastRowNum; rowNumber++)
						{

							IRow row = sheet.GetRow(rowNumber);
							if (row != null)
							{
								manager = new InstalmentsCreateBulk(
								CShared.replaceSpaceUnicode(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()),
								CShared.replaceSpaceUnicode(row.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()) 
								);
								manager.UUID = Guid.NewGuid().ToString();
								manager.BATCH_ID = batchId;
								manager.FILE_NAME = fileMaster.FILE_NAME;
								manager.CREATE_USER = account.UserName;
								manager.CREATE_BRANCH = account.Branch;
								var strMessage = CheckNullInfo(manager, manager.listNotNullProperties);
								manager.RESULT_CODE = "N";
								manager.RESULT_MESSAGE = "";

								if (!String.IsNullOrEmpty(strMessage))
								{
									manager.RESULT_CODE = "E101";
									manager.RESULT_MESSAGE += strMessage;
								}

								if (lstCard.Contains(manager))
								{
									var dup = lstCard.Find(x => x.Equals(manager));

									manager.RESULT_CODE = "E102";
									manager.RESULT_MESSAGE += "Dữ liệu trùng lặp. ";
									dup.RESULT_CODE = "E102";
									dup.RESULT_MESSAGE += "Dữ liệu trùng lặp. ";
								}
								if (!manager.mappingTenor.ContainsKey(manager.TENOR))
								{
									manager.RESULT_CODE = "E103";
									manager.RESULT_MESSAGE += "Kỳ hạn không hợp lệ ";

								}

								string expectedOptionCode = manager.MapOptionCode(manager.TENOR);
								if (manager.OPTION_CODE != expectedOptionCode)
								{
									manager.RESULT_CODE = "E104";
									manager.RESULT_MESSAGE += " OPTION CODE không hợp lệ";
								}
								manager.RESULT_MESSAGE = manager.RESULT_CODE == "N" ? "New record" : manager.RESULT_MESSAGE;
								lstCard.Add(manager);
							}
							else
							{
								rp.message = "File không có thông tin!";
								rp.code = "01";
								DeleteFolder(folderRoot);
								return Json(rp, JsonRequestBehavior.AllowGet);
							}

						}
						if (lstCard.Count == 0)
						{
							rp.message = "File không có thông tin!";
							rp.code = "01";
							DeleteFolder(folderRoot);
							return Json(rp, JsonRequestBehavior.AllowGet);
						}
						if (lstCard.Count > 200)
						{
							rp.message = "File nhiều hơn 200 dòng dữ liệu! Vui lòng tách file xử lý riêng!";
							rp.code = "01";
							DeleteFolder(folderRoot);
							return Json(rp, JsonRequestBehavior.AllowGet);
						}
					}
					catch (Exception ex)
					{
						log.InfoFormat(ex.ToString());
						rp.message = "Fail. Đã có lỗi xảy ra trong quá trình upload file!";
						rp.code = "01";
						DeleteFolder(folderRoot);
						return Json(rp, JsonRequestBehavior.AllowGet);
					}
					finally
					{
						try
						{
							if (fileStream != null)
							{
								fileStream.Close();
								fileStream.Dispose();
							}

							DeleteFolder(folderRoot);
						}
						catch (Exception ex)
						{
							log.InfoFormat(ex.ToString());
						}
					}
				}
				catch (Exception ex)
				{
					log.InfoFormat(ex.ToString());
				}
			}

			try
			{

				totalRow = lstCard.Count;
				totalRowDung = lstCard.FindAll(x => x.RESULT_CODE == "N").Count;
				totalRowSai = lstCard.FindAll(x => x.RESULT_CODE != "N").Count;
				if (totalRowSai == 0)
				{
					fileMaster.TOTAL_ROW = totalRow;
					fileMaster.TOTAL_ROW_SUCCESS = totalRowDung;
					ResultMessage insertFile = service.InsertFileMasterInstalmentsCreateBulk(fileMaster);
					 if (!insertFile.code.Equals("00"))
					 {
						 rp.message = "Fail. Đã có lỗi xảy ra trong quá trình insert thông tin file master! Vui lòng kiểm tra lại thông tin !";
						 rp.code = "01";
						 log.Error("[InstalmentsCardBulk][LoadFile] Exception: " + insertFile.message);
						 return Json(rp, JsonRequestBehavior.AllowGet);
					 }
					 // insert batch detail
					 var size = service.InsertBatchDetails(lstCard);
					 if (size != totalRow)
					 {
						 rp.message = "Fail. Đã có lỗi xảy ra trong quá trình insert thông tin chi tiết!";
						 rp.code = "01";
						 return Json(rp, JsonRequestBehavior.AllowGet);
					 }

				}
			}
			catch (Exception ex)
			{
				log.InfoFormat(ex.ToString());
				rp.message = "Fail. Đã có lỗi xảy ra trong quá trình upload file!";
				rp.code = "01";
				return Json(rp, JsonRequestBehavior.AllowGet);
			}

			ViewBag.totalRow = totalRow;
			ViewBag.totalRowDung = totalRowDung;
			ViewBag.totalRowSai = totalRowSai;
			ViewBag.fileUUID = fileMaster.UUID;
			ViewBag.batchId = fileMaster.BATCH_ID;
			return (ActionResult)PartialView("SearchData", lstCard);
		}

		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult UploadAttachment()
		{
			log.Info("================ Begin UploadAttachment ===================");
			SESSION_PARA oPra = CShared.getSession();
			log.Info("User:" + oPra.oAccount.UserName);
			log.Info("IP_Client:" + CShared.GetIPAddress());

			try
			{
				if (Request.Files.Count != 0)
				{
					HttpPostedFileBase uploadedFile = Request.Files[0];

					// Tạo file name mới
					var _ext = Path.GetExtension(uploadedFile.FileName);
					var fileName = Path.GetFileName(uploadedFile.FileName);
					if (fileName.Length > 30)
					{
						return Json(new
						{
							code = "01", //DUNG LUONG FILE LON
							message = string.Format("Tên file chứng từ không được quá 30 ký tự. Vui lòng đính kèm lại.")
						}, JsonRequestBehavior.AllowGet);
					}

					//Kiểm tra dung lượng upload file
					int byteCount = uploadedFile.ContentLength;
					if (byteCount > 2000000)
					{
						return Json(new
						{
							code = "01", //DUNG LUONG FILE LON
							message = string.Format("Bạn tải lên tập tin có dung lượng: {0}, lớn hơn 2M. Vui lòng đính kèm lại.", CShared.SizeSuffix(byteCount))
						}, JsonRequestBehavior.AllowGet);
					}

					string folderRoot = string.Format("/Upload/Instalments/InstalmentsCardsBulk/Chungtu/{0}/{1}/", DateTime.Now.ToString("MMyyyy"), DateTime.Now.ToString("ddMMyyyy"));
					CreateFolder(folderRoot);

					string path = Server.MapPath(string.Format("~/Upload/Instalments/InstalmentsCardsBulk/Chungtu/{0}/{1}/", DateTime.Now.ToString("MMyyyy"), DateTime.Now.ToString("ddMMyyyy")));
					string urlImage = string.Format("/Upload/Instalments/InstalmentsCardsBulk/Chungtu/{0}/{1}/{2}", DateTime.Now.ToString("MMyyyy"), DateTime.Now.ToString("ddMMyyyy"), fileName);
					path += fileName;
					uploadedFile.SaveAs(path);

					if (System.IO.File.Exists(path))
					{
						log.Info("oldFileName:" + uploadedFile.FileName);
						log.Info("newFileName:" + fileName);
						return Json(new
						{
							code = "00",
							oldFileName = uploadedFile.FileName,
							newFileName = fileName,
							path = urlImage,
							ext = _ext,
							size = byteCount
						}, JsonRequestBehavior.AllowGet);
					}
				}

				log.Info("================ End UploadAttachment ===================");

				return Json(new
				{
					code = "01",
					message = "Xảy ra lỗi khi upload file chứng từ!"
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				log.Error("[InstalmentsCardBulk][UploadAttachment]  Exception: " + ex.Message, ex);
				return Json(new
				{
					code = "01",
					message = "Xảy ra lỗi khi upload file chứng từ!"
				}, JsonRequestBehavior.AllowGet);
			}
		}


		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult ValidateInstalmentsDataBulk(string fileUUID, string batchId)
		{
			ResultMessage rp = new ResultMessage();
			var totalRow = 0;
			var totalRowDung = 0;
			var totalRowSai = 0;

			try
			{
				List<InstalmentsCreateBulk> lstCard = new List<InstalmentsCreateBulk>();
				InstalmentsCreateBulkService service = new InstalmentsCreateBulkService();

				lstCard = service.ValidateFileInstalmentsCreateBulk(fileUUID, batchId);

				foreach (var item in lstCard)
				{
						item.RESULT_CODE = "A";
						item.RESULT_MESSAGE = "Approve";
				}

				if (lstCard.Count == 0)
				{
					rp.message = "Lỗi load kết quả kiểm tra! Vui lòng liên hệ CNTT hỗ trợ!";
					rp.code = "01";
					return Json(rp, JsonRequestBehavior.AllowGet);
				}
				totalRow = lstCard.Count;
				totalRowDung = lstCard.FindAll(x => x.RESULT_CODE == "A").Count;
				totalRowSai = lstCard.FindAll(x => x.RESULT_CODE != "A").Count;

				if (totalRowSai > 0)
				{
					FileMasterInstalmentsCreateBulk file = new FileMasterInstalmentsCreateBulk();
					file.UUID = fileUUID;
					file.BATCH_ID = batchId;
					file.RESULT_CODE = "E105";
					file.RESULT_MESSAGE = "File has detail error";
					file.STATUS = 0;
					if (!service.doUpdateResultFileMaster(file))
					{
						rp.code = "01";
						rp.message = "Lỗi cập nhật kết quả file !";
						return Json(rp, JsonRequestBehavior.AllowGet);
					}
				}

				ViewBag.totalRow = totalRow;
				ViewBag.totalRowDung = totalRowDung;
				ViewBag.totalRowSai = totalRowSai;
				ViewBag.fileUUID = fileUUID;
				ViewBag.batchId = batchId;

				return (ActionResult)PartialView("SearchData", lstCard);
			}
			catch (Exception ex)
			{
				log.InfoFormat(ex.ToString());
				rp.message = "Fail. Đã có lỗi xảy ra trong quá trình kiểm tra dữ liệu!";
				rp.code = "01";
				return Json(rp, JsonRequestBehavior.AllowGet);
			}
		}
		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		public ActionResult doSendApprove(string fileUUID, string batchId, string remark, string listNameAttachment, string pathAttachment)
		{
			SESSION_PARA oPra = CShared.getSession();

			ResultMessage reValue = new ResultMessage();
			if (oPra.oAccount == null)
			{
				reValue.code = "99";
				reValue.message = "Vui lòng đăng nhập lại";
				return Json(reValue, JsonRequestBehavior.AllowGet);
			}
			reValue.code = "00";
			reValue.message = "Chuyển duyệt thành công.";

			InstalmentsCreateBulkService service = new InstalmentsCreateBulkService();
			FileMasterInstalmentsCreateBulk file = new FileMasterInstalmentsCreateBulk();
			file.UUID = fileUUID;
			file.BATCH_ID = batchId;
			file.RESULT_CODE = "A";
			file.RESULT_MESSAGE = "Send to approve";
			file.STATUS = 1;
			file.REMARK = remark;
			file.ATTACHMENT = listNameAttachment;
			file.ATTACHMENT_PATH = pathAttachment;

			if (!service.doUpdateResultFileMaster(file))
			{
				reValue.code = "01";
				reValue.message = "Lỗi chuyển duyệt file không thành công vui lòng thử lại!";
				return Json(reValue, JsonRequestBehavior.AllowGet);
			}
			if (!service.doUpdateAttachment(file))
			{
				reValue.code = "01";
				reValue.message = "Lỗi lưu thông tin chứng từ không thành công vui lòng thử lại!";
				return Json(reValue, JsonRequestBehavior.AllowGet);
			}

			return Json(reValue, JsonRequestBehavior.AllowGet);
		}


		[CustomAuthorize(Roles = "ttt-review;ttt-report;")]
		[HttpPost]
		public ActionResult exportEcxelReport(string reportType, List<InstalmentsCreateBulk> data)
		{
			// 1 - BC du lieu dung
			// 2 - BC du lieu sai
			try
			{
				SESSION_PARA session = CShared.getSession();
				List<InstalmentsCreateBulk> exportData = new List<InstalmentsCreateBulk>();
				string fileName = session.oAccount.UserName + "_export_tra_gop_the_theo_lo" + DateTime.Now.ToString("hhmmssddMMyyyy") + ".xlsx";
				string path = Server.MapPath("~/Upload/Instalments/InstalmentsCardsBulk/Template-InstalmentsCardBulk-Export.xlsx");
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
						"CCCD", "PHONE", "CLIENT_NAME", "CONTRACT_NUMBER", "CARD_NUMBER","TRANS_DATE","TRANS_AMOUNT","AUTH_CODE","RECORD_ID","MERCHANT","TENOR","OPTION_CODE","NOTE", "RESULT_CODE","RESULT_MESSAGE"
					});

				ms = _excelTools.ExportFileExcel(table, path,2,false);

				string handle = Guid.NewGuid().ToString();
				TempData[handle] = ms.ToArray();

				return Json(new { code = "00", message = fileName, data = handle }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				log.Error("[InstalmentsCardBulk][exportEcxelReport] Exception: ", e);
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

		public string CheckNullInfo(InstalmentsCreateBulk data, List<string> listNotNull)
		{
			try
			{
				string message = "";
				foreach (PropertyInfo pi in data.GetType().GetProperties())
				{
					if (pi.PropertyType == typeof(string))
					{
						string value = (string)pi.GetValue(data);
						if (string.IsNullOrEmpty(value) && listNotNull.Contains(pi.Name))
						{
							message += pi.Name + ", ";
						}
					}
				}

				return message != "" ? message.Trim() + " không được để trống. " : "";
			}
			catch (Exception ex)
			{
				log.InfoFormat(ex.ToString());
				return ex.ToString();
			}
		}
		private void CreateFolder(string Folder)
		{
			if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(Folder)))
			{
				Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(Folder));
			}
		}
		private void DeleteFolder(string Folder)
		{
			if (Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(Folder)))
			{
				Directory.Delete(System.Web.HttpContext.Current.Server.MapPath(Folder), true);
			}
		}
	

	}


}