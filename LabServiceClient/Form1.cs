using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using CloudElite.ServiceModel.Dispatcher;

namespace LabServiceClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnInvoke_Click(object sender, EventArgs e)
        {
            LabServiceClient.LabServices.LabServiceSoapClient clnt = new LabServices.LabServiceSoapClient();
            if (this.cbTokenHeader.Checked)
            {
                clnt.Endpoint.Behaviors.Add(new MessageInspectBehavior());
            }
            try
            {
                this.tbResult.Text = clnt.HelloWorld();
            }
            catch (Exception ex)
            {
                this.tbResult.Text = ex.Message;
            }
        }

        private void btnInvokeNatural_Click(object sender, EventArgs e)
        {
            LabService svc = new LabService();
            svc.HelloWorldCompleted += (o, ea) => {
                this.tbResult.Text = ea.Result;
            };
            try
            {
                svc.HelloWorldAsync();
            }
            catch (Exception ex)
            {
                this.tbResult.Text = ex.Message;
            }
        }

        private void btnDummyCall_Click(object sender, EventArgs e)
        {
            LabService svc = new LabService();
            try
            {
                svc.DummyCall();
            }
            catch (Exception ex)
            {
                this.tbResult.Text = ex.Message;
            }
        }
    }
}
