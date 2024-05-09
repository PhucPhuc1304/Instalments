using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instalments.Models
{
	public class ImportFileModel
	{
		public string FileName { get; set; }
		public string UserThucHien { get; set; }
		public string Token { get; set; }
		public string fileUser { get; set; }
		public string para1 { get; set; }
		public string para2 { get; set; }
		public FileContentResult fileData { get; set; }
	}
}