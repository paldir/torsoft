using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace czynsze.ControlsP
{
    public class TableP : Table
    {
        public TableP(string cSSClass, List<string[]> rows, string[] headerRow)
        {
            this.CssClass = cSSClass;

            foreach (string[] row in rows)
            {
                TableRow tableRow = new TableRow();
                tableRow.CssClass = "tableRow";
                tableRow.ID = row[0] + "_row";

                TableCell tableCell = new TableCell();
                tableCell.CssClass = "tableCell";

                tableCell.Controls.Add(new RadioButtonP("radioButton", row[0], "id"));
                tableCell.Controls.Add(new LabelP("label", row[0], row[1], String.Empty));
                tableRow.Cells.Add(tableCell);
                this.Rows.Add(tableRow);
            }

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 2; j < rows[i].Length; j++)
                {
                    TableCell tableCell = new TableCell();
                    tableCell.CssClass = "tableCell";

                    tableCell.Controls.Add(new LabelP("label", rows.ElementAt(i)[0], rows.ElementAt(i)[j], String.Empty));
                    this.Rows[i].Cells.Add(tableCell);
                }
            }

            TableHeaderRow tableHeaderRow = new TableHeaderRow();
            tableHeaderRow.CssClass = "tableHeaderRow";

            foreach (string cell in headerRow)
            {
                TableHeaderCell tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.CssClass = "tableHeaderCell";

                tableHeaderCell.Controls.Add(new LiteralControl(cell));
                tableHeaderRow.Cells.Add(tableHeaderCell);
            }

            this.Rows.AddAt(0, tableHeaderRow);
        }
    }
}