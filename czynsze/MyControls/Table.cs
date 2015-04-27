using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace czynsze.MyControls
{
    public class Table : System.Web.UI.WebControls.Table
    {
        public Table(string cSSClass, IList<string[]> rows, string[] headerRow, bool sortable, string prefix, List<int> indexesOfNumericColumns, List<int> indexesOfColumnsWithSummary)
        {
            float[] widthsOfColumns = new float[headerRow.Length];
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            graphics.PageUnit = GraphicsUnit.Pixel;
            Font font = new Font("Times New Roman", 12);
            decimal[] summary = new decimal[headerRow.Length];
            float columnWidth;
            string cellText;

            for (int i = 0; i < widthsOfColumns.Length; i++)
            {
                widthsOfColumns[i] = 0;
                summary[i] = 0;
            }

            CssClass = cSSClass;

            foreach (string[] row in rows)
            {
                TableRow tableRow = new TableRow();
                tableRow.CssClass = "tableRow";
                tableRow.ID = prefix + row[0] + "_row";
                TableCell tableCell = new TableCell();
                tableCell.CssClass = "tableCell";
                cellText = row[1];

                if (indexesOfNumericColumns.Contains(1))
                {
                    cellText = HandleNumericCell(cellText);
                    tableCell.CssClass += " numericTableCell";
                }

                if (indexesOfColumnsWithSummary.Contains(1) && !String.IsNullOrEmpty(cellText))
                    summary[0] += Decimal.Parse(cellText);

                tableCell.Controls.Add(new RadioButton("radioButton", prefix + row[0], "id"));
                tableCell.Controls.Add(new Label("label", row[0], row[1], String.Empty));
                tableRow.Cells.Add(tableCell);
                Rows.Add(tableRow);

                columnWidth = graphics.MeasureString(row[1], font).Width;

                if (columnWidth > widthsOfColumns[0])
                    widthsOfColumns[0] = columnWidth;
            }

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 2; j < rows[i].Length; j++)
                {
                    TableCell tableCell = new TableCell();
                    tableCell.CssClass = "tableCell";
                    cellText = rows.ElementAt(i)[j];

                    if (indexesOfNumericColumns.Contains(j))
                    {
                        cellText = HandleNumericCell(cellText);
                        tableCell.CssClass += " numericTableCell";
                    }

                    if (indexesOfColumnsWithSummary.Contains(j) && !String.IsNullOrEmpty(cellText))
                        summary[j - 1] += Decimal.Parse(cellText);

                    tableCell.Controls.Add(new Label("label", rows.ElementAt(i)[0], cellText, String.Empty));
                    Rows[i].Cells.Add(tableCell);

                    columnWidth = graphics.MeasureString(rows.ElementAt(i)[j], font).Width;

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
                    tableHeaderCell.Controls.Add(new LinkButton("sortLink", prefix + "column" + i.ToString(), headerRow[i]));
                else
                    tableHeaderCell.Controls.Add(new LiteralControl(headerRow[i]));

                columnWidth = graphics.MeasureString(headerRow[i], font).Width;

                if (columnWidth > widthsOfColumns[i])
                    widthsOfColumns[i] = columnWidth;

                tableHeaderRow.Cells.Add(tableHeaderCell);
            }

            Rows.AddAt(0, tableHeaderRow);

            TableFooterRow tableFooterRow = new TableFooterRow();
            tableFooterRow.CssClass = "tableFooterRow";
            tableFooterRow.TableSection = TableRowSection.TableFooter;

            for (int i = 0; i < headerRow.Length; i++)
            {
                TableCell tableFooterCell = new TableCell();
                tableFooterCell.CssClass = "tableFooterCell numericTableCell";

                if (!indexesOfColumnsWithSummary.Contains(i + 1))
                    tableFooterCell.Text = String.Empty;
                else
                    tableFooterCell.Text = HandleNumericCell(summary[i].ToString());

                columnWidth = graphics.MeasureString(tableFooterCell.Text, font).Width;

                if (columnWidth > widthsOfColumns[i])
                    widthsOfColumns[i] = columnWidth;

                tableFooterRow.Cells.Add(tableFooterCell);
            }

            Rows.Add(tableFooterRow);

            if (Rows.Count > 1)
                for (int i = 0; i < headerRow.Length; i++)
                    Rows[0].Cells[i].Width = Rows[1].Cells[i].Width = Rows[Rows.Count - 1].Cells[i].Width = new Unit(widthsOfColumns[i]);
        }

        static string HandleNumericCell(string cellText)
        {
            if (String.IsNullOrEmpty(cellText))
                return cellText;
            else
            {
                string format;

                if (!cellText.Contains(','))
                    format = "{0:N0}";
                else
                    format = "{0:N2}";

                return String.Format(format, Single.Parse(cellText));
            }
        }
    }
}