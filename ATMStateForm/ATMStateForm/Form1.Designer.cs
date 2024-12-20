namespace ATMStateForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblState = new Label();
            lblTotalCash = new Label();
            txtPin = new TextBox();
            txtAmount = new TextBox();
            lblPIN = new Label();
            btnEnterPin = new Button();
            btnWithdraw = new Button();
            btnDeposit = new Button();
            btnFinish = new Button();
            lblAmount = new Label();
            lblNetworkStatus = new Label();
            SuspendLayout();
            // 
            // lblState
            // 
            lblState.AutoSize = true;
            lblState.Location = new Point(58, 69);
            lblState.Name = "lblState";
            lblState.Size = new Size(143, 20);
            lblState.TabIndex = 0;
            lblState.Text = "Текущее состояние";
            // 
            // lblTotalCash
            // 
            lblTotalCash.AutoSize = true;
            lblTotalCash.Location = new Point(459, 85);
            lblTotalCash.Name = "lblTotalCash";
            lblTotalCash.Size = new Size(58, 20);
            lblTotalCash.TabIndex = 1;
            lblTotalCash.Text = "Баланс";
            // 
            // txtPin
            // 
            txtPin.Location = new Point(83, 158);
            txtPin.Name = "txtPin";
            txtPin.PasswordChar = '*';
            txtPin.Size = new Size(125, 27);
            txtPin.TabIndex = 2;
            // 
            // txtAmount
            // 
            txtAmount.Location = new Point(465, 169);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(125, 27);
            txtAmount.TabIndex = 3;
            // 
            // lblPIN
            // 
            lblPIN.AutoSize = true;
            lblPIN.Location = new Point(85, 123);
            lblPIN.Name = "lblPIN";
            lblPIN.Size = new Size(35, 20);
            lblPIN.TabIndex = 4;
            lblPIN.Text = "PIN:";
            // 
            // btnEnterPin
            // 
            btnEnterPin.Location = new Point(78, 198);
            btnEnterPin.Name = "btnEnterPin";
            btnEnterPin.Size = new Size(94, 29);
            btnEnterPin.TabIndex = 5;
            btnEnterPin.Text = "Проверить";
            btnEnterPin.UseVisualStyleBackColor = true;
            btnEnterPin.Click += btnEnterPin_Click;
            // 
            // btnWithdraw
            // 
            btnWithdraw.Location = new Point(465, 215);
            btnWithdraw.Name = "btnWithdraw";
            btnWithdraw.Size = new Size(111, 29);
            btnWithdraw.TabIndex = 6;
            btnWithdraw.Text = "Снять";
            btnWithdraw.UseVisualStyleBackColor = true;
            btnWithdraw.Click += btnWithdraw_Click;
            // 
            // btnDeposit
            // 
            btnDeposit.Location = new Point(468, 252);
            btnDeposit.Name = "btnDeposit";
            btnDeposit.Size = new Size(108, 29);
            btnDeposit.TabIndex = 7;
            btnDeposit.Text = "Пополнить";
            btnDeposit.UseVisualStyleBackColor = true;
            btnDeposit.Click += btnDeposit_Click;
            // 
            // btnFinish
            // 
            btnFinish.Location = new Point(697, 395);
            btnFinish.Name = "btnFinish";
            btnFinish.Size = new Size(94, 29);
            btnFinish.TabIndex = 8;
            btnFinish.Text = "Выход";
            btnFinish.UseVisualStyleBackColor = true;
            btnFinish.Click += btnFinish_Click;
            // 
            // lblAmount
            // 
            lblAmount.AutoSize = true;
            lblAmount.Location = new Point(466, 140);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(112, 20);
            lblAmount.TabIndex = 9;
            lblAmount.Text = "Введите сумму";
            // 
            // lblNetworkStatus
            // 
            lblNetworkStatus.AutoSize = true;
            lblNetworkStatus.Location = new Point(58, 34);
            lblNetworkStatus.Name = "lblNetworkStatus";
            lblNetworkStatus.Size = new Size(182, 20);
            lblNetworkStatus.TabIndex = 10;
            lblNetworkStatus.Text = "Подключение: Проверка";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblNetworkStatus);
            Controls.Add(lblAmount);
            Controls.Add(btnFinish);
            Controls.Add(btnDeposit);
            Controls.Add(btnWithdraw);
            Controls.Add(btnEnterPin);
            Controls.Add(lblPIN);
            Controls.Add(txtAmount);
            Controls.Add(txtPin);
            Controls.Add(lblTotalCash);
            Controls.Add(lblState);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblState;
        private Label lblTotalCash;
        private TextBox txtPin;
        private TextBox txtAmount;
        private Label lblPIN;
        private Button btnEnterPin;
        private Button btnWithdraw;
        private Button btnDeposit;
        private Button btnFinish;
        private Label lblAmount;
        private Label lblNetworkStatus;
    }
}
