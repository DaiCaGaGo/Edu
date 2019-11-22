namespace Common
{
    public class StringTool
    {
        private static readonly string[] VietnameseSigns = new string[]
         {
          "aAeEoOuUiIdDyY",
          "áàạảãâấầậẩẫăắằặẳẵ",
          "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
          "éèẹẻẽêếềệểễ",
          "ÉÈẸẺẼÊẾỀỆỂỄ",
          "óòọỏõôốồộổỗơớờợởỡ",
          "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
          "úùụủũưứừựửữ",
          "ÚÙỤỦŨƯỨỪỰỬỮ",
          "íìịỉĩ",
          "ÍÌỊỈĨ",
          "đ",
          "Đ",
          "ýỳỵỷỹ",
          "ÝỲỴỶỸ"
         };

        public static string RemoveSign4VietnameseString(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return RemoveSign4VietnameseUnicodeComposite(str);
        }

        /// <summary>
        /// Removes the sign4 vietnamese unicode composite.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        private static string RemoveSign4VietnameseUnicodeComposite(string str)
        {
            str = str
                .Replace(((char)769).ToString(), "") //dấu sắc
                .Replace(((char)768).ToString(), "") //dấu huyền
                .Replace(((char)777).ToString(), "") //dấu hỏi
                .Replace(((char)771).ToString(), "") //dấu ngã
                .Replace(((char)803).ToString(), ""); //dấu nặng

            return str;
        }
    }
}