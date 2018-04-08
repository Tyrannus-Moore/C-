namespace UltiPokerSimulator
{
    partial class Truck
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Truck));
            this.btBegin = new System.Windows.Forms.Button();
            this.pbBottom1 = new System.Windows.Forms.PictureBox();
            this.btGo = new System.Windows.Forms.Button();
            this.lbProperty = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbTimer = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPORT = new System.Windows.Forms.TextBox();
            this.btWaitOp = new System.Windows.Forms.Button();
            this.lbWait = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbBottom1)).BeginInit();
            this.SuspendLayout();
            // 
            // btBegin
            // 
            this.btBegin.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btBegin.Location = new System.Drawing.Point(350, 393);
            this.btBegin.Name = "btBegin";
            this.btBegin.Size = new System.Drawing.Size(105, 65);
            this.btBegin.TabIndex = 0;
            this.btBegin.Text = "开始游戏";
            this.btBegin.UseVisualStyleBackColor = true;
            this.btBegin.Click += new System.EventHandler(this.btBegin_Click);
            // 
            // pbBottom1
            // 
            this.pbBottom1.Image = global::UltiPokerSimulator.Properties.Resources._0;
            this.pbBottom1.InitialImage = null;
            this.pbBottom1.Location = new System.Drawing.Point(210, 12);
            this.pbBottom1.Name = "pbBottom1";
            this.pbBottom1.Size = new System.Drawing.Size(72, 95);
            this.pbBottom1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBottom1.TabIndex = 1;
            this.pbBottom1.TabStop = false;
            // 
            // btGo
            // 
            this.btGo.Font = new System.Drawing.Font("幼圆", 15F, System.Drawing.FontStyle.Bold);
            this.btGo.Location = new System.Drawing.Point(279, 434);
            this.btGo.Name = "btGo";
            this.btGo.Size = new System.Drawing.Size(65, 65);
            this.btGo.TabIndex = 3;
            this.btGo.Text = "不服";
            this.btGo.UseVisualStyleBackColor = true;
            this.btGo.Visible = false;
            this.btGo.Click += new System.EventHandler(this.btGo_Click);
            // 
            // lbProperty
            // 
            this.lbProperty.AutoSize = true;
            this.lbProperty.Font = new System.Drawing.Font("幼圆", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProperty.ForeColor = System.Drawing.Color.Red;
            this.lbProperty.Location = new System.Drawing.Point(204, 425);
            this.lbProperty.Name = "lbProperty";
            this.lbProperty.Size = new System.Drawing.Size(0, 33);
            this.lbProperty.TabIndex = 5;
            this.lbProperty.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbTimer
            // 
            this.lbTimer.AutoSize = true;
            this.lbTimer.Font = new System.Drawing.Font("Goudy Stout", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimer.Location = new System.Drawing.Point(530, 189);
            this.lbTimer.Name = "lbTimer";
            this.lbTimer.Size = new System.Drawing.Size(307, 77);
            this.lbTimer.TabIndex = 6;
            this.lbTimer.Text = ".NET7";
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(12, 66);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(192, 90);
            this.txtMsg.TabIndex = 7;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(12, 12);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(126, 21);
            this.txtIP.TabIndex = 8;
            this.txtIP.Text = "192.168.43.250";
            // 
            // txtPORT
            // 
            this.txtPORT.Location = new System.Drawing.Point(12, 39);
            this.txtPORT.Name = "txtPORT";
            this.txtPORT.Size = new System.Drawing.Size(100, 21);
            this.txtPORT.TabIndex = 9;
            this.txtPORT.Text = "1";
            // 
            // btWaitOp
            // 
            this.btWaitOp.Enabled = false;
            this.btWaitOp.Font = new System.Drawing.Font("幼圆", 15F, System.Drawing.FontStyle.Bold);
            this.btWaitOp.Location = new System.Drawing.Point(461, 434);
            this.btWaitOp.Name = "btWaitOp";
            this.btWaitOp.Size = new System.Drawing.Size(65, 65);
            this.btWaitOp.TabIndex = 10;
            this.btWaitOp.Text = "来战";
            this.btWaitOp.UseVisualStyleBackColor = true;
            this.btWaitOp.Visible = false;
            this.btWaitOp.Click += new System.EventHandler(this.btWaitOp_Click);
            // 
            // lbWait
            // 
            this.lbWait.AutoSize = true;
            this.lbWait.Font = new System.Drawing.Font("幼圆", 26.25F);
            this.lbWait.Location = new System.Drawing.Point(537, 154);
            this.lbWait.Name = "lbWait";
            this.lbWait.Size = new System.Drawing.Size(285, 35);
            this.lbWait.TabIndex = 11;
            this.lbWait.Text = "等待对手出牌...";
            this.lbWait.Visible = false;
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Truck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 511);
            this.Controls.Add(this.lbWait);
            this.Controls.Add(this.btWaitOp);
            this.Controls.Add(this.txtPORT);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.lbTimer);
            this.Controls.Add(this.lbProperty);
            this.Controls.Add(this.btGo);
            this.Controls.Add(this.pbBottom1);
            this.Controls.Add(this.btBegin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Truck";
            this.Text = "拖板车";
            ((System.ComponentModel.ISupportInitialize)(this.pbBottom1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btBegin;
        private System.Windows.Forms.PictureBox pbBottom1;
        private System.Windows.Forms.Button btGo;
        private System.Windows.Forms.Label lbProperty;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbTimer;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPORT;
        private System.Windows.Forms.Button btWaitOp;
        private System.Windows.Forms.Label lbWait;
        private System.Windows.Forms.Timer timer2;
    }
}

