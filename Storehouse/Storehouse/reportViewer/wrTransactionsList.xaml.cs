using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Storehouse.reportViewer
{  
    public partial class wrTransactionsList
    {
      
        public List<Model.TransactionsListItem> Lr = null;
        public int CurrentTransactionType;
        public wrTransactionsList()
        {   
            try
            {
                InitializeComponent();
                _reportViewer.Load += ReportViewer_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0} \n {1} ", ex.Message, ex.StackTrace));
            }
        }

       

        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                var reportDS = new Microsoft.Reporting.WinForms.ReportDataSource
                {
                    Name = "Ds",
                    Value = Lr
                };




                _reportViewer.LocalReport.DataSources.Add(reportDS);


            _reportViewer.LocalReport.ReportEmbeddedResource = "Storehouse.reportViewer.templates.rTransactionsList.rdlc";




            ReportParameter[] parameters = new ReportParameter[1];

            parameters[0] = new ReportParameter("Type", CurrentTransactionType.ToString());



            _reportViewer.LocalReport.SetParameters(parameters);

                _reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }
    }
}
