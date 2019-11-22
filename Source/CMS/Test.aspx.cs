using CMS.XuLy;
using OneEduDataAccess;
using OneEduDataAccess.BO;
using OneEduDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Tesseract;

namespace CMS
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //HocSinhBO hsBO = new HocSinhBO();
            //List<DiemChiTietTheoHocSinhEntity> lst = hsBO.getDiemChiTietByHocSinh(236, "THCS", 2018, 1101, 2, 45886);
            //string sms = RemoveSpecialCharacters("T/B: Nha truong phoi hop voi Trung tam TDTT to chuc lop boi loi va phong tranh duoi nuoc. PH  dang ky cho con tham gia lop boi day du và dung han, có xe đưa đón tại trường. Han cuoi la 31/5/2019. Lien he thay Le Minh (0912048386) Tran trong!");

            //MapPhuHuynhHocSinhBO mapPhuHuynhHocSinhBO = new MapPhuHuynhHocSinhBO();
            //string fromuid = "336483556759542047";
            //List<ZaloTraCuuEntity> lstLop = mapPhuHuynhHocSinhBO.traCuuTinTucTheoUser(fromuid);
            //int count = lstLop.Count;

            //var ENGLISH_LANGUAGE = @"eng";

            //var blogPostImage = @"E:\test_ocr\img1.jpg";
            //string dataPath = @"D:\Edu\Source\CMS\tessdata";

            //using (var ocrEngine = new TesseractEngine(Server.MapPath(@"~/tessdata"), ENGLISH_LANGUAGE, EngineMode.Default))
            //{
            //    using (var imageWithText = Pix.LoadFromFile(blogPostImage))
            //    {
            //        using (var page = ocrEngine.Process(imageWithText))
            //        {
            //            var text = page.GetText();
            //            Console.WriteLine(text);
            //            Console.ReadLine();
            //        }
            //    }
            //}

            //Bitmap img = new Bitmap("E:/test_ocr/test_1.jpg");
            //TesseractEngine engine = new TesseractEngine(Server.MapPath(@"~/tessdata"), "vie", EngineMode.Default);
            //Page page = engine.Process(img, PageSegMode.Auto);
            //string result = page.GetText();
            //lblText.Text = result;
            //Console.WriteLine(result);

            LocalAPI localAPI = new LocalAPI();
            string phone = "84794990188";
            string sd = localAPI.getLoaiNhaMang(phone);

            DiemChiTietBO diemChiTietBO = new DiemChiTietBO();
            List<DiemChiTietEntity> lstDiem = diemChiTietBO.getDiemGuiTinHangNgay(2019, 197, 6, 5605, 1, DateTime.Now, 211391);
            

            //MaNhanXetBO maNXBO = new MaNhanXetBO();
            //List<DataJsonEntity> lst = new List<DataJsonEntity>();
            //lst = maNXBO.getMaNhanXetToDataJson(237);
            //var json = new JavaScriptSerializer().Serialize(lst);
            //lblText.Text = json.ToString();
        }
        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[!@#$%^&*]", "", RegexOptions.Compiled);
        }
    }
}