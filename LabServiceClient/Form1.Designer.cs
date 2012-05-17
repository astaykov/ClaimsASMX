namespace LabServiceClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInvoke = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.cbTokenHeader = new System.Windows.Forms.CheckBox();
            this.btnInvokeNatural = new System.Windows.Forms.Button();
            this.btnDummyCall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInvoke
            // 
            this.btnInvoke.Location = new System.Drawing.Point(12, 46);
            this.btnInvoke.Name = "btnInvoke";
            this.btnInvoke.Size = new System.Drawing.Size(75, 23);
            this.btnInvoke.TabIndex = 0;
            this.btnInvoke.Text = "invoke";
            this.btnInvoke.UseVisualStyleBackColor = true;
            this.btnInvoke.Click += new System.EventHandler(this.btnInvoke_Click);
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(12, 75);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(517, 244);
            this.tbResult.TabIndex = 1;
            // 
            // cbTokenHeader
            // 
            this.cbTokenHeader.AutoSize = true;
            this.cbTokenHeader.Location = new System.Drawing.Point(12, 12);
            this.cbTokenHeader.Name = "cbTokenHeader";
            this.cbTokenHeader.Size = new System.Drawing.Size(117, 17);
            this.cbTokenHeader.TabIndex = 2;
            this.cbTokenHeader.Text = "Add Token Header";
            this.cbTokenHeader.UseVisualStyleBackColor = true;
            // 
            // btnInvokeNatural
            // 
            this.btnInvokeNatural.Location = new System.Drawing.Point(112, 46);
            this.btnInvokeNatural.Name = "btnInvokeNatural";
            this.btnInvokeNatural.Size = new System.Drawing.Size(180, 23);
            this.btnInvokeNatural.TabIndex = 0;
            this.btnInvokeNatural.Text = "invoke with SoapHttpProtocol";
            this.btnInvokeNatural.UseVisualStyleBackColor = true;
            this.btnInvokeNatural.Click += new System.EventHandler(this.btnInvokeNatural_Click);
            // 
            // btnDummyCall
            // 
            this.btnDummyCall.Location = new System.Drawing.Point(298, 46);
            this.btnDummyCall.Name = "btnDummyCall";
            this.btnDummyCall.Size = new System.Drawing.Size(180, 23);
            this.btnDummyCall.TabIndex = 0;
            this.btnDummyCall.Text = "dummyCall+";
            this.btnDummyCall.UseVisualStyleBackColor = true;
            this.btnDummyCall.Click += new System.EventHandler(this.btnDummyCall_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 331);
            this.Controls.Add(this.cbTokenHeader);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.btnDummyCall);
            this.Controls.Add(this.btnInvokeNatural);
            this.Controls.Add(this.btnInvoke);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInvoke;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.CheckBox cbTokenHeader;
        private System.Windows.Forms.Button btnInvokeNatural;
        private System.Windows.Forms.Button btnDummyCall;
    }
}

