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
        public TableP(string cSSClass, List<string[]> rows, string[] headerRow, bool sortable)
        {
            /*float cellPadding = 50;
            float paddingOfHeaderCell = 3;
            float[] columnsWidth = new float[headerRow.Length];
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            graphics.PageUnit = GraphicsUnit.Pixel;
            Font font = new Font("Times New Roman", 8);

            for (int i = 0; i < columnsWidth.Length; i++)
                columnsWidth[i] = 0;*/

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

                /*float columnWidth = graphics.MeasureString(row[1], font).Width + cellPadding;

                if (columnWidth > columnsWidth[0])
                    columnsWidth[0] = columnWidth;*/
            }

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 2; j < rows[i].Length; j++)
                {
                    TableCell tableCell = new TableCell();
                    tableCell.CssClass = "tableCell";

                    tableCell.Controls.Add(new LabelP("label", rows.ElementAt(i)[0], rows.ElementAt(i)[j], String.Empty));
                    this.Rows[i].Cells.Add(tableCell);

                    /*float columnWidth = graphics.MeasureString(rows.ElementAt(i)[j], font).Width + cellPadding;

                    if (columnWidth > columnsWidth[j - 1])
                        columnsWidth[j - 1] = columnWidth;*/
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
                    tableHeaderCell.Controls.Add(new LinkButtonP("sortLink", "column" + i.ToString(), headerRow[i]));
                else
                    tableHeaderCell.Controls.Add(new LiteralControl(headerRow[i]));

                /*float columnWidth = graphics.MeasureString(headerRow[i], font).Width + paddingOfHeaderCell;

                if (columnWidth > columnsWidth[i])
                    columnsWidth[i] = columnWidth;

                tableHeaderCell.Width = new Unit(columnsWidth[i]);*/

                tableHeaderRow.Cells.Add(tableHeaderCell);
            }

            this.Rows.AddAt(0, tableHeaderRow);
        }
    }
}