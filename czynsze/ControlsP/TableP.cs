using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace czynsze.ControlsP
{
    public class TableP : Table
    {
        public TableP(string cSSClass, List<string[]> rows, string[] headerRow, bool sortable, string prefix, List<int> indexesOfNumericColumns)
        {
            float[] widthsOfColumns = new float[headerRow.Length];
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            graphics.PageUnit = GraphicsUnit.Pixel;
            Font font = new Font("Times New Roman", 12);

            for (int i = 0; i < widthsOfColumns.Length; i++)
                widthsOfColumns[i] = 0;

            this.CssClass = cSSClass;

            foreach (string[] row in rows)
            {
                TableRow tableRow = new TableRow();
                tableRow.CssClass = "tableRow";
                tableRow.ID = prefix + row[0] + "_row";
                TableCell tableCell = new TableCell();
                tableCell.CssClass = "tableCell";
                string cellText = row[1];

                if (indexesOfNumericColumns.IndexOf(1) != -1)
                {
                    cellText = HandleNumericCell(cellText);

                    tableCell.CssClass += " numericTableCell";
                }

                tableCell.Controls.Add(new RadioButtonP("radioButton", prefix + row[0], "id"));
                tableCell.Controls.Add(new LabelP("label", row[0], row[1], String.Empty));
                tableRow.Cells.Add(tableCell);
                this.Rows.Add(tableRow);

                float columnWidth = graphics.MeasureString(row[1], font).Width;

                if (columnWidth > widthsOfColumns[0])
                    widthsOfColumns[0] = columnWidth;
            }

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 2; j < rows[i].Length; j++)
                {
                    TableCell tableCell = new TableCell();
                    tableCell.CssClass = "tableCell";
                    string cellText = rows.ElementAt(i)[j];

                    if (indexesOfNumericColumns.IndexOf(j) != -1)
                    {
                        cellText = HandleNumericCell(cellText);

                        tableCell.CssClass += " numericTableCell";
                    }

                    tableCell.Controls.Add(new LabelP("label", rows.ElementAt(i)[0], cellText, String.Empty));
                    this.Rows[i].Cells.Add(tableCell);

                    float columnWidth = graphics.MeasureString(rows.ElementAt(i)[j], font).Width;

                    if (columnWidth > widthsOfColumns[j - 1])
                        widthsOfColumns[j - 1] = columnWidth;
                }
            }

            TableHeaderRow tableHeaderRow = new TableHeaderRow();
            tableHeaderRow.CssClass = "tableHeaderRow";
            tableHeaderRow.TableSection = TableRowSection.TableHeader;

            for (int i = 0; i < headerRow.Length; i++)
            {
                TableHeaderCell tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.CssClass = "tableHeaderCell";

                if (sortable)
                    tableHeaderCell.Controls.Add(new LinkButtonP("sortLink", prefix + "column" + i.ToString(), headerRow[i]));
                else
                    tableHeaderCell.Controls.Add(new LiteralControl(headerRow[i]));

                float columnWidth = graphics.MeasureString(headerRow[i], font).Width;

                if (columnWidth > widthsOfColumns[i])
                    widthsOfColumns[i] = columnWidth;

                tableHeaderCell.Width = new Unit(widthsOfColumns[i]);

                tableHeaderRow.Cells.Add(tableHeaderCell);
            }

            this.Rows.AddAt(0, tableHeaderRow);

            if (this.Rows.Count > 1)
                for (int i = 0; i < headerRow.Length; i++)
                    this.Rows[1].Cells[i].Width = new Unit(widthsOfColumns[i]);
        }

        string HandleNumericCell(string cellText)
        {
            string format;

            if (cellText.IndexOf(',') == -1)
                format = "{0:N0}";
            else
                format = "{0:N2}";

            return String.Format(format, Convert.ToSingle(cellText));
        }
    }
}