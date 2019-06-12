using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Visio = Microsoft.Office.Interop.Visio;
using System.Drawing;

namespace VisioDrawer
{
    public class DocServerToADAPT
    {
        private static Visio.Application application;
        private static Visio.Document doc;
        private static Visio.Page adaptPage;
        private static Visio.Master visioCubeMaster;
        private static List<Visio.Shape> cubeList;
        private static double posX = 1.25;
        private static double posY = 1.75;
        
        /// <summary>
        /// Draws a TM1 Server in Visio
        /// </summary>
        /// <param name="server">The DocumentationServer instance to draw</param>
        /// <param name="withSystemCubes">true if you want system cubes to be drawn</param>
        /// <param name="withSystemDims">true if you want system dimensions to be drawn</param>
        /// <param name="templatePath">Path to the ADAPT template</param>
        /// <param name="savePath">Save Path</param>
        public static void Draw(DocumentationServer server, bool withSystemCubes, bool withSystemDims, string savePath)
        {
            try
            {
                cubeList = new List<Visio.Shape>();

                // start Visio
                application = new Visio.Application();
                application.Visible = false;

                // load and set up the ADAPT template
                string filename = @"\Adapt.vst";
                doc = application.Documents.Add(Environment.CurrentDirectory + filename);
                doc.Creator = @"gmc²";
                doc.Title = @"Server: " + server.name;
                doc.SaveAs(savePath + server.name + ".vsdx");
                //doc.PaperSize = Visio.VisPaperSizes.visPaperSizeB4;

                // setting up the pages
                adaptPage = doc.Pages[1];
                adaptPage.Name = @"gmc² IBM Cognos Documentation Tool";
                adaptPage.AutoSize = true;

                // get the necessary shapes...
                visioCubeMaster = doc.Masters.get_ItemU(@"Cube");

                // ...and draw the cubes
                foreach (DocCube docCube in server.cubes)
                {
                    if (withSystemCubes || !docCube.name.StartsWith("}", StringComparison.OrdinalIgnoreCase))
                    {
                        DrawCube(docCube, withSystemDims);
                    }
                }

                posX = 1.25;
                posY = 1.75;

                foreach (DocCube docCube in server.cubes)
                {
                    if (docCube.nof_look > 0 && (withSystemCubes || !docCube.name.StartsWith("}", StringComparison.OrdinalIgnoreCase)))
                    {
                        DrawLooksConnections(docCube);
                    }

                    if (docCube.nof_fed > 0 && (withSystemCubes || !docCube.name.StartsWith("}", StringComparison.OrdinalIgnoreCase)))
                    {
                        DrawFeedsConnections(docCube);
                    }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                kill();
            }
        }

        private static void DrawLooksConnections(DocCube docCube)
        {
            Visio.Shape cubeTo = cubeList.Find(x => x.Name.Equals(docCube.name));

            foreach (DocCube look_cube in docCube.look_up_cubes)
            {
                Visio.Shape cubeFrom = cubeList.Find(x => x.Name.Equals(look_cube.name));
                ConnectWithDynamicGlueAndConnector("Used By", cubeFrom, cubeTo, false);
            }
        }

        private static void DrawFeedsConnections(DocCube docCube)
        {
            Visio.Shape cubeFrom = cubeList.Find(x => x.Name.Equals(docCube.name));

            foreach (DocCube fed_cube in docCube.fed_cubes)
            {
                Visio.Shape cubeTo = cubeList.Find(x => x.Name.Equals(fed_cube.name));
                ConnectWithDynamicGlueAndConnector("Used By", cubeFrom, cubeTo, true);
            }
        }

        private static void DrawCube(DocCube docCube, bool withSysDims)
        {
            Visio.Shape cubeShape = adaptPage.Drop(visioCubeMaster, posX > 6 ? posX = 1.25 : posX += 1.5, posX == 1.25 ? posY += 1.5 : posY);

            // set font size for CubeShape
            cubeShape.Shapes[2].CellsSRC[(short)Visio.VisSectionIndices.visSectionCharacter,
                (short)Visio.VisRowIndices.visRowCharacter,
                (short)Visio.VisCellIndices.visCharacterSize].Formula = "=7 pt.";

            cubeShape.Text = docCube.name;
            cubeShape.Name = docCube.name;

            // get DimShape
            Visio.Shape dimShape = cubeShape.Shapes[1];
            StringBuilder dims = new StringBuilder();
            
            foreach (DocDimension docDim in docCube.dims)
            {
                if (withSysDims || !docDim.name.StartsWith("}", StringComparison.OrdinalIgnoreCase))
                {
                    dims.AppendLine(docDim.name);
                }
            }

            // set font size for DimShape
            dimShape.CellsSRC[(short)Visio.VisSectionIndices.visSectionCharacter,
                (short)Visio.VisRowIndices.visRowCharacter,
                (short)Visio.VisCellIndices.visCharacterSize].Formula = "=5 pt.";

            // write dims to DimShape
            dimShape.Text = dims.ToString();

            cubeList.Add(cubeShape);
        }

        private static void ConnectWithDynamicGlueAndConnector(string connectorName, Visio.Shape shapeFrom, Visio.Shape shapeTo, bool isFeeder)
        {
            if (!cubeList.Contains(shapeFrom) || !cubeList.Contains(shapeTo))
                return;

            Microsoft.Office.Interop.Visio.Cell beginXCell;
            Microsoft.Office.Interop.Visio.Cell endXCell;
            Microsoft.Office.Interop.Visio.Shape connector;

            connectionNames(shapeFrom);

            // Add a Dynamic connector to the page.
            connector = DropMasterOnPage(connectorName, 0.0, 0.0);
            if (isFeeder)
            {
                connector.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowLine,
                    (short)Visio.VisCellIndices.visLineColor).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            }

            // Connect the begin point.
            beginXCell = connector.get_CellsSRC(
                (short)Visio.VisSectionIndices.visSectionObject,
                (short)Visio.VisRowIndices.visRowXForm1D,
                (short)Visio.VisCellIndices.vis1DBeginX);

            beginXCell.GlueTo(shapeFrom.get_CellsSRC(
                (short)Visio.VisSectionIndices.visSectionConnectionPts, 
                isFeeder ? (short)8 : (short)10, 
                (short)Visio.VisCellIndices.visCnnctX));

            // Connect the end point.
            endXCell = connector.get_CellsSRC(
                (short)Visio.VisSectionIndices.visSectionObject,
                (short)Visio.VisRowIndices.visRowXForm1D,
                (short)Visio.VisCellIndices.vis1DEndX);

            endXCell.GlueTo(shapeTo.get_CellsSRC(
                (short)Visio.VisSectionIndices.visSectionConnectionPts, 
                isFeeder ? (short)9 : (short)11, 
                (short)Visio.VisCellIndices.visCnnctX));
        }

        private static Visio.Shape DropMasterOnPage(string masterNameU, double pinX, double pinY)
        {
            Visio.Documents applicationDocuments;
            Visio.Master masterToDrop;
            Visio.Shape returnShape = null;

            // Find the stencil in the Documents collection by name.
            applicationDocuments = adaptPage.Application.Documents;

            // Get a master on the stencil by its universal name.
            masterToDrop = doc.Masters.get_ItemU(masterNameU);

            // Drop the master on the page that is passed in. Set the
            // PinX and PinY using the data passed in parameters pinX
            // and pinY, respectively.
            returnShape = adaptPage.Drop(masterToDrop, pinX, pinY);

            return returnShape;
        }

        private static void connectionNames(Visio.Shape shape)
        {
            List<string> listOfNames = new List<string>();
            short iRow = (short)Visio.VisRowIndices.visRowConnectionPts;

            while (shape.get_RowExists((short)Visio.VisSectionIndices.visSectionConnectionPts, iRow, (short)0) != 0)
            {
                // Get a cell from the connection point row.
                Visio.Cell cell = shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionConnectionPts, iRow, (short)Visio.VisCellIndices.visCnnctX);

                // Ask the cell what row it is in.
                listOfNames.Add(cell.RowName);

                // Next row.
                ++iRow;
            }
            return;
        }

        private static void kill()
        {
            doc.Save();
            application.Quit();
        }
    }
}
