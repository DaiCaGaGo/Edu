using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZaloPageSDK.com.vng.zalosdk.entity;
using ZaloPageSDK.com.vng.zalosdk.service;

namespace Form_Zalo_Send_Sms
{
    public partial class Main : Form
    {
        ZaloServiceFactory factory = new ZaloServiceConfigure(1563756711382616263, "ETO1fTpkVCt3W69Fo6q4").getZaloServiceFactory();

        public Main()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ZaloMessageService messageService = factory.getZaloMessageService();

            Dictionary<string, string> openWith = new Dictionary<string, string>();
            openWith.Add("content1", "14h ngày 27/7/2016, ");
            openWith.Add("content2", "mời các đồng chí nhận được tin nhắn đến trường để phòng chống cơn bão số 1. Trân trọng thông báo.");

            ZaloPageResult k = messageService.sendTemplateTextMessageByPhoneNum(84906357886, "0e85856eb92b5075093a", openWith, "", true);

            MessageBox.Show("Id: " + k.getId() + ", Error: " + k.getError());
        }
    }
}
