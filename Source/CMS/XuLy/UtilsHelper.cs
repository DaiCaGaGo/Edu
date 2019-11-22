using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CMS.XuLy
{
    public class UtilsHelper
    {
        // <summary>
        /// Splits a <see cref="List{T}"/> into multiple chunks.
        /// </summary>
        public static List<List<T>> SplitIntoChunks<T>(List<T> list, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("chunkSize must be greater than 0.");
            }

            List<List<T>> retVal = new List<List<T>>();
            int index = 0;
            while (index < list.Count)
            {
                int count = list.Count - index > chunkSize ? chunkSize : list.Count - index;
                retVal.Add(list.GetRange(index, count));

                index += chunkSize;
            }

            return retVal;
        }

        public static void DisabledAllcontrolInPage(System.Web.UI.Page page)
        {
            //normal
            DisabledControlOfControllCollection(page.Form.Controls);
            //div control
            foreach (HtmlGenericControl itemControl in page.Form.Controls.OfType<HtmlGenericControl>())
            {
                DisabledControlOfControllCollection(itemControl.Controls);
            }
        }

        private static void DisabledControlOfControllCollection(ControlCollection controlCollection)
        {
            foreach (DropDownList itemControl in controlCollection.OfType<DropDownList>())
            {
                itemControl.Enabled = false;
            }
            foreach (TextBox itemControl in controlCollection.OfType<TextBox>())
            {
                itemControl.Enabled = false;
            }
            foreach (Image itemControl in controlCollection.OfType<Image>())
            {
                itemControl.Enabled = false;
            }
            foreach (CheckBox itemControl in controlCollection.OfType<CheckBox>())
            {
                itemControl.Enabled = false;
            }
            foreach (Button itemControl in controlCollection.OfType<Button>())
            {
                itemControl.Enabled = false;
            }

            //telerik control
            foreach (RadComboBox itemControl in controlCollection.OfType<RadComboBox>())
            {
                itemControl.Enabled = false;
            }
            foreach (RadDatePicker itemControl in controlCollection.OfType<RadDatePicker>())
            {
                itemControl.Enabled = false;
            }
            foreach (RadioButton itemControl in controlCollection.OfType<RadioButton>())
            {
                itemControl.Enabled = false;
            }
        }

        public static int GetSelectedValueFromRadCombobox(RadComboBox radCombo, int defaultValue = 0)
        {
            if (radCombo.SelectedItem != null)
                return ParseInt(radCombo.SelectedItem.Value);
            return defaultValue;
        }

        public static int? GetIntValueFromRadNumberic(RadNumericTextBox radNumeric)
        {
            if (radNumeric.Value.HasValue)
            {
                return ParseInt(radNumeric.Value.ToString());
            }
            return null;
        }
        public static decimal? GetDecimalValueFromRadNumberic(RadNumericTextBox radNumeric)
        {
            if (radNumeric.Value.HasValue)
            {
                return ParseDecimal(radNumeric.Value.ToString());
            }
            return null;
        }

        public static string GetSelectedValueStrFromRadCombobox(RadComboBox radCombo, string defaultValue = "")
        {
            if (radCombo.SelectedItem != null)
                return radCombo.SelectedItem.Value;
            return defaultValue;
        }

        public static decimal? GetSelectedDecimalValueFromRadCombobox(RadComboBox radCombo, decimal minValue = 0)
        {
            if (radCombo.SelectedItem != null)
            {
                var decimalValue = ParseDecimal(radCombo.SelectedItem.Value);
                if (decimalValue > minValue)
                    return decimalValue;

            }
            return null;
        }

        public static decimal GetDecinalValueFromRadCombobox(RadComboBox radCombo, decimal defaultValue = 0)
        {
            if (radCombo.SelectedItem != null)
                return ParseDecimal(radCombo.SelectedItem.Value);
            return defaultValue;
        }

        public static string GetSelectedTextFromRadCombobox(RadComboBox radCombo)
        {
            if (radCombo.SelectedItem != null)
                return radCombo.SelectedItem.Text;
            return string.Empty;
        }

        public static void SetSelectedValueRadCombobox(RadComboBox radCombo, string selectedValue, bool isRebindControl = false)
        {
            if (isRebindControl)
                radCombo.DataBind();
            var item = radCombo.Items.FindItemByValue(selectedValue);
            if (item != null)
                item.Selected = true;
        }

        public static int ParseInt(string value, int defaultValue = 0)
        {
            int valueInt = defaultValue;
            int.TryParse(value, out valueInt);
            return valueInt;
        }

        public static decimal ParseDecimal(string value, decimal defaultValue = 0)
        {
            decimal valueInt = defaultValue;
            decimal.TryParse(value, out valueInt);
            return valueInt;
        }

        public static decimal? ParseDecimalNullable(string value)
        {
            decimal valueInt;
            if (decimal.TryParse(value, out valueInt))
            {
                return valueInt;
            }
            return null;
        }

        public static string ParseDatetimeStr(string datetime, string defaultFormat = "dd/MM/yyyy hh:mm:ss", string defaultValue = "")
        {
            string value = defaultValue;
            DateTime datetimeValue;
            if (DateTime.TryParse(datetime, out datetimeValue))
            {
                value = datetimeValue.ToString(defaultFormat);
            }
            return value;
        }

        public static string ParseDateStr(string datetime, string defaultFormat = "dd/MM/yyyy", string defaultValue = "")
        {
            string value = defaultValue;
            DateTime datetimeValue;
            if (DateTime.TryParse(datetime, out datetimeValue))
            {
                value = datetimeValue.ToString(defaultFormat);
            }
            return value;
        }
    }
}