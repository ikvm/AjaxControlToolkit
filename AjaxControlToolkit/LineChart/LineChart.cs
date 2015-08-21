﻿using AjaxControlToolkit.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace AjaxControlToolkit {

    [ClientCssResource(Constants.LineChartName)]
    [ClientScriptResource("Sys.Extended.UI.LineChart", Constants.LineChartName)]
    [ToolboxBitmap(typeof(ToolboxIcons.Accessor), Constants.LineChartName + Constants.IconPostfix)]
    public class LineChart : ChartBase {
        List<LineChartSeries> _series = new List<LineChartSeries>();

        /// <summary>
        /// Provides a set of values for the category axis to create a line chart.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        [ClientPropertyName("categoriesAxis")]
        public string CategoriesAxis { get; set; }

        /// <summary>
        /// Provide list of series to client side.
        /// </summary>
        /// <remarks>
        /// Need help from Series property for designer experience support, cause 
        /// Editor always blocks the property ability to provide values to 
        /// client side as ExtenderControlProperty on run time.
        /// </remarks>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ExtenderControlProperty(true, true)]
        public List<LineChartSeries> ClientSeries {
            get { return _series; }
        }

        /// <summary>
        /// Provide list of series.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(null)]
        [NotifyParentProperty(true)]
        [Editor(typeof(ChartBaseSeriesEditor<LineChartSeries>), typeof(UITypeEditor))]
        public List<LineChartSeries> Series {
            get { return _series; }
        }

        /// <summary>
        /// Enables you to render two types of line charts 1. Basic 2. Stacked.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue(LineChartType.Basic)]
        [ClientPropertyName("chartType")]
        public LineChartType ChartType { get; set; }

        /// <summary>
        /// Enables you to set the interval size for the value axis line.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue(9)]
        [ClientPropertyName("valueAxisLines")]
        public int ValueAxisLines { get; set; }

        /// <summary>
        /// Enables you to set the the color of the value axis lines.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        [ClientPropertyName("valueAxisLineColor")]
        public string ValueAxisLineColor { get; set; }

        /// <summary>
        /// Enables you to set the color of the category axis lines.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        [ClientPropertyName("categoryAxisLineColor")]
        public string CategoryAxisLineColor { get; set; }

        /// <summary>
        /// Enables you to set the color of the base lines of the chart.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        [ClientPropertyName("baseLineColor")]
        public string BaseLineColor { get; set; }

        /// <summary>
        /// Enables you to set the background color of the tooltip box.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("#FFC652")]
        [ClientPropertyName("tooltipBackgroundColor")]
        public string TooltipBackgroundColor { get; set; }

        /// <summary>
        /// Enables you to set the font color of the tooltip box.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("#0E426C")]
        [ClientPropertyName("tooltipFontColor")]
        public string TooltipFontColor { get; set; }

        /// <summary>
        /// Enables you to set the border color of the tooltip box.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("#B85B3E")]
        [ClientPropertyName("tooltipBorderColor")]
        public string TooltipBorderColor { get; set; }

        /// <summary>
        /// Enables you to set the text/label that will be shown in the tooltip and describe about area data value.
        /// </summary>
        [ExtenderControlProperty]
        [DefaultValue("")]
        [ClientPropertyName("areaDataLabel")]
        public string AreaDataLabel { get; set; }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            if(IsDesignMode)
                return;

            foreach(LineChartSeries lineChartSeries in Series) {
                if(String.IsNullOrWhiteSpace(lineChartSeries.Name))
                    throw new Exception("Name is missing in the LineChartSeries. Please provide a name in the LineChartSeries.");
            }
        }

        protected override void CreateChildControls() {
            var parent = new HtmlGenericControl("div");
            parent.ID = "_ParentDiv";
            parent.Attributes.Add("style", "border-style:solid; border-width:1px;");
            var sbScript = new StringBuilder();
            sbScript.Append("<script>");

            sbScript.Append("function init(evt) { ");
            sbScript.Append("    if ( window.svgDocument == null ) { ");
            sbScript.Append("        gDocument = evt.target.ownerDocument;");
            sbScript.Append("    } ");
            sbScript.Append("} ");

            sbScript.Append("function ShowTooltip(me, evt, data, areaDataLabel) { ");
            sbScript.Append(String.Format("    var tooltipDiv = document.getElementById('{0}_tooltipDiv');", ClientID));
            sbScript.Append("    tooltipDiv.innerHTML = String.format('{0}{1}', data, areaDataLabel) ;");
            sbScript.Append("    tooltipDiv.style.top = evt.pageY - 25 + 'px';");
            sbScript.Append("    tooltipDiv.style.left = evt.pageX + 20 + 'px';");
            sbScript.Append("    tooltipDiv.style.visibility = 'visible';");
            sbScript.Append("    me.style.strokeWidth = '5';");
            sbScript.Append("} ");

            sbScript.Append("function HideTooltip(me, evt) { ");
            sbScript.Append(String.Format("    var tooltipDiv = document.getElementById('{0}_tooltipDiv');", ClientID));
            sbScript.Append("    tooltipDiv.innerHTML = '';");
            sbScript.Append("    tooltipDiv.style.visibility = 'hidden';");
            sbScript.Append("    me.style.strokeWidth = '2';");
            sbScript.Append("} ");

            sbScript.Append("</script>");
            parent.InnerHtml = sbScript.ToString();
            Controls.Add(parent);
        }
    }

}