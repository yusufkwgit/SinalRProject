using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace SignalRWebUi.Controllers
{
    public class QRCodeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string value)
        {
            if (string.IsNullOrEmpty(value)) return View();

            string url = $"https://10.63.145.6:7126/Default/Index?tableName={value}";
            
            QRCodeGenerator createQRCode = new QRCodeGenerator();
            QRCodeData qrCodeData = createQRCode.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(10);
            
            ViewBag.QrCodeImage = "data:image/png;base64," + Convert.ToBase64String(qrCodeBytes);
            
            return View();
        }
    }
}

