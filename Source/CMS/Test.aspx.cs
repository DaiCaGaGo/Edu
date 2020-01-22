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
using System.Net;
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
            //string FilePath = Server.MapPath("~/PdfLinkFile/Attachment.pdf");
            //WebClient User = new WebClient();
            //Byte[] FileBuffer = User.DownloadData(FilePath);
            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //}

            HocSinhBO hocSinhBO = new HocSinhBO();
            List<HocSinhEntity> lstHocSinh = hocSinhBO.getHocSinhByPhoneDangKyZalo("84919882638", 2019, "");
            if (lstHocSinh.Count > 0)
            {
                for (int i = 0; i < lstHocSinh.Count; i++)
                {
                    if (i == 0)
                    {
                        lbl1.Text = lstHocSinh[i].HO_TEN;
                        lbl1.Visible = true;
                        lbl2.Visible = false;
                    }
                    else if (i == 1)
                    {
                        lbl2.Text = lstHocSinh[i].HO_TEN;
                        lbl2.Visible = true;
                    }
                }
            }
        }
        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[!@#$%^&*]", "", RegexOptions.Compiled);
        }
        
    }
}