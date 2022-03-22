namespace Servidor_SMS
{
    partial class frmMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRunServer = new System.Windows.Forms.Button();
            this.lestatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRunServer
            // 
            this.btnRunServer.Location = new System.Drawing.Point(210, 106);
            this.btnRunServer.Name = "btnRunServer";
            this.btnRunServer.Size = new System.Drawing.Size(397, 143);
            this.btnRunServer.TabIndex = 0;
            this.btnRunServer.Text = "Run server";
            this.btnRunServer.UseVisualStyleBackColor = true;
            this.btnRunServer.Click += new System.EventHandler(this.btnRunServer_Click);
            // 
            // lestatus
            // 
            this.lestatus.AutoSize = true;
            this.lestatus.Location = new System.Drawing.Point(408, 358);
            this.lestatus.Name = "lestatus";
            this.lestatus.Size = new System.Drawing.Size(0, 32);
            this.lestatus.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 504);
            this.Controls.Add(this.lestatus);
            this.Controls.Add(this.btnRunServer);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Servido SMS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunServer;
        private System.Windows.Forms.Label lestatus;
    }
}

