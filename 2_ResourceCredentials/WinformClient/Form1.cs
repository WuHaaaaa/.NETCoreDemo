using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdentityModel;
using IdentityModel.Client;

namespace WinformClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            string username = this.textBox1.Text.ToString();
            string password = this.textBox2.Text.ToString();

            HttpClient client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:6000");

            if (disco.IsError)
            {
                MessageBox.Show(disco.Error);
                return;
            }

            var tokenResp = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "WinForm",
                ClientSecret = "winform_secret",
                UserName = username,
                Password = password,
            });
            if (tokenResp.IsError)
            {
                MessageBox.Show(tokenResp.Error);
                return;
            }

            this.textBox3.Text = tokenResp.Json.ToString();
            if (!tokenResp.IsError)
            {
                accessToken = tokenResp.AccessToken;
            }
        }

        private string accessToken = string.Empty;

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                MessageBox.Show("请先获取Token");
                return;
            }

            HttpClient client = new HttpClient();

            client.SetBearerToken(accessToken);
            var contentResp = await client.GetAsync("http://localhost:5000/api/order");

            if (contentResp.IsSuccessStatusCode)
            {
                var content = await contentResp.Content.ReadAsStringAsync();
                this.textBox4.Text = content;
            }
            else
            {
                MessageBox.Show(contentResp.StatusCode.ToString());
                return;
            }
        }
    }
}