using System;
using System.Data;

namespace Common
{
    public class TemplateSms
    {
        public string[] CheckTemplate(string conte, DataRow[] slrows, bool istruetemp)
        {
            string selectedTemplateId = "";
            string sms = conte + ".";
            string[] arrsms = null; //mảng tham số truyền sang telco;
            bool isTrueTemplate = false;
            foreach (DataRow item in slrows)
            {
                string template = item[1].ToString();//nội dung template sau khi thay thế chuỗi tham số bằng ký tự phân cách.
                char c = '|'; //ký tự phân cách
                if (!template.Contains("|"))
                {
                    for (int i = 1; i < 11; i++)
                        template = template.Replace("{P" + i.ToString() + "}", "|");
                    c = '|';
                }
                else if (!item[1].ToString().Contains("`"))
                {
                    for (int i = 1; i < 11; i++)
                        template = template.Replace("{P" + i.ToString() + "}", "`");
                    c = '`';
                }
                int countparameter = 0;
                foreach (char u in template)
                    if (u == c) countparameter++; //đếm số para trong mẫu tin
                string templateid = item[2].ToString();//templateid dang ky voi nha mang.
                string[] arrFixText = template.Split(c); // mảng chuỗi cố định.
                int countfixtext = 0; //đếm số chuỗi cố định có trong tin nhắn.
                int vitritruoc = 0;
                foreach (string ft in arrFixText)
                {
                    int vitrimoi = sms.IndexOf(ft, vitritruoc, StringComparison.Ordinal);
                    if (vitrimoi >= vitritruoc)
                    {
                        vitritruoc = vitrimoi;
                        countfixtext++;
                    }
                }

                if (countfixtext == arrFixText.Length)
                {
                    if (istruetemp) return new[] { "True" };
                    isTrueTemplate = true; selectedTemplateId = templateid;
                    for (int i = 0; i < arrFixText.Length; i++)
                        if (arrFixText[i] != "" && arrFixText[i].Length > 1)
                        {
                            int vitridau = sms.IndexOf(arrFixText[i], StringComparison.Ordinal);
                            int vitricuoi = vitridau + arrFixText[i].Length;

                            if ((vitridau >= 0) && (vitricuoi > 0))
                            {
                                string chuoidau = sms.Remove(vitridau, sms.Length - vitridau);
                                string chuoicuoi = sms.Remove(0, vitricuoi);

                                sms = String.Format("{0}{1}{2}", chuoidau, c, chuoicuoi);
                            }
                        }
                        else if (arrFixText[i].Length == 1 && arrFixText.GetUpperBound(0) == i)
                        {
                            if (sms.Length - 1 >= 0)
                                sms = sms.Remove(sms.Length - 1, 1) + c;
                        }

                    int countseparent = 0; arrsms = new string[countparameter]; string[] prearrsms = sms.Split(c);

                    foreach (char u in sms)
                        if (u == c) countseparent++;
                    if (countseparent == sms.Length)
                        for (int i = 0; i < countparameter; i++) arrsms[i] = "";
                    else
                    {
                        if (countparameter < prearrsms.GetUpperBound(0))
                            for (int i = 0; i < countparameter; i++)
                                arrsms[i] = prearrsms[i + 1];
                        else
                            for (int i = 0; i < countparameter; i++)
                                arrsms[i] = prearrsms[i];
                    }
                    break;//break foreach template
                }
            }//end foreach template

            if (isTrueTemplate)
            {
                string[] temp = new string[arrsms.Length + 1];
                for (int i = 0; i < arrsms.Length; i++)
                    temp[i + 1] = arrsms[i];
                temp[0] = selectedTemplateId;
                return temp;
            }
            return null;
        }
    }
}