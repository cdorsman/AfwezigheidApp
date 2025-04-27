namespace AfwezigheidsApp
{
    partial class AfwezigheidRegistratieFormulier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.Label lblVerlofType;
        private System.Windows.Forms.ComboBox cmbVerlofType;
        private System.Windows.Forms.Label lblStartDatum;
        private System.Windows.Forms.DateTimePicker dtpStartDatum;
        private System.Windows.Forms.Label lblEindDatum;
        private System.Windows.Forms.DateTimePicker dtpEindDatum;
        private System.Windows.Forms.Button btnSubmit;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblVerlofType = new System.Windows.Forms.Label();
            this.cmbVerlofType = new System.Windows.Forms.ComboBox();
            this.lblStartDatum = new System.Windows.Forms.Label();
            this.dtpStartDatum = new System.Windows.Forms.DateTimePicker();
            this.lblEindDatum = new System.Windows.Forms.Label();
            this.dtpEindDatum = new System.Windows.Forms.DateTimePicker();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblVerlofType
            // 
            this.lblVerlofType.AutoSize = true;
            this.lblVerlofType.Location = new System.Drawing.Point(30, 30);
            this.lblVerlofType.Name = "lblVerlofType";
            this.lblVerlofType.Size = new System.Drawing.Size(80, 17);
            this.lblVerlofType.TabIndex = 0;
            this.lblVerlofType.Text = "Verlof Type:";
            // 
            // cmbVerlofType
            // 
            this.cmbVerlofType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVerlofType.FormattingEnabled = true;
            this.cmbVerlofType.Items.AddRange(new object[] {"Ziek", "Betaald", "Onbetaald", "Vakantie"});
            this.cmbVerlofType.Location = new System.Drawing.Point(130, 27);
            this.cmbVerlofType.Name = "cmbVerlofType";
            this.cmbVerlofType.Size = new System.Drawing.Size(180, 24);
            this.cmbVerlofType.TabIndex = 1;
            // 
            // lblStartDatum
            // 
            this.lblStartDatum.AutoSize = true;
            this.lblStartDatum.Location = new System.Drawing.Point(30, 70);
            this.lblStartDatum.Name = "lblStartDatum";
            this.lblStartDatum.Size = new System.Drawing.Size(76, 17);
            this.lblStartDatum.TabIndex = 2;
            this.lblStartDatum.Text = "Start datum:";
            // 
            // dtpStartDatum
            // 
            this.dtpStartDatum.Location = new System.Drawing.Point(130, 67);
            this.dtpStartDatum.Name = "dtpStartDatum";
            this.dtpStartDatum.Size = new System.Drawing.Size(180, 22);
            this.dtpStartDatum.TabIndex = 3;
            // 
            // lblEindDatum
            // 
            this.lblEindDatum.AutoSize = true;
            this.lblEindDatum.Location = new System.Drawing.Point(30, 110);
            this.lblEindDatum.Name = "lblEindDatum";
            this.lblEindDatum.Size = new System.Drawing.Size(71, 17);
            this.lblEindDatum.TabIndex = 4;
            this.lblEindDatum.Text = "Eind datum:";
            // 
            // dtpEindDatum
            // 
            this.dtpEindDatum.Location = new System.Drawing.Point(130, 107);
            this.dtpEindDatum.Name = "dtpEindDatum";
            this.dtpEindDatum.Size = new System.Drawing.Size(180, 22);
            this.dtpEindDatum.TabIndex = 5;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(130, 150);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 30);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // AfwezigheidRegistratieFormulier
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 210);
            this.Controls.Add(this.lblVerlofType);
            this.Controls.Add(this.cmbVerlofType);
            this.Controls.Add(this.lblStartDatum);
            this.Controls.Add(this.dtpStartDatum);
            this.Controls.Add(this.lblEindDatum);
            this.Controls.Add(this.dtpEindDatum);
            this.Controls.Add(this.btnSubmit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AfwezigheidRegistratieFormulier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registreer afwezigheid";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}