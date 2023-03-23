namespace DigitalDrawingStore.Listener.Service
{
    partial class DDSWServiceInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.XperiCAD_serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.XperiCADServiceInsatler = new System.ServiceProcess.ServiceInstaller();
            // 
            // XperiCAD_serviceProcessInstaller
            // 
            this.XperiCAD_serviceProcessInstaller.Password = null;
            this.XperiCAD_serviceProcessInstaller.Username = null;
            // 
            // XperiCADServiceInsatler
            // 
            this.XperiCADServiceInsatler.Description = "XperiCAD Digital Drawing and manufacturing manage CAD file format";
            this.XperiCADServiceInsatler.DisplayName = "XperiCAD - DDSW";
            this.XperiCADServiceInsatler.ServiceName = "DDSW Listener Service";
            this.XperiCADServiceInsatler.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // DDSWServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.XperiCAD_serviceProcessInstaller,
            this.XperiCADServiceInsatler});

        }

        #endregion
        private System.ServiceProcess.ServiceProcessInstaller XperiCAD_serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller XperiCADServiceInsatler;
    }
}