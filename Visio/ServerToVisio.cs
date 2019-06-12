using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Visio = Microsoft.Office.Interop.Visio;

namespace VisioConvert
{
    public class ServerToVisio
    {
        public ServerToVisio(DocumentationServer server)
        {
            Visio.Document doc;
            Visio.Page adaptPage;
            Visio.Selection select;
            Visio.Shape group;

            // start Visio
            Visio.Application application = new Visio.Application();
            application.Visible = true;

            // setting up paths
            string templatePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\Benutzerdefinierte Office-Vorlagen\Visio Templates\Adapt.vst";
            string savePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\VS Saves\Adapt.vst";

            // load and set up the ADAPT template (with 2 pages)
            doc = application.Documents.Add(templatePath);
            doc.Creator = @"gmc²";
            doc.Title = @"Hier steht dann der Titel";
            doc.SaveAs(savePath);

            //doc.Pages.Add();

            // setting up the pages
            adaptPage = doc.Pages[1];
            adaptPage.Name = @"ADAPT";
            Visio.Page legende = doc.Pages[2];
            legende.Name = @"Legende";

            // get the necessary shapes...

            //!!! ALT in Dictionary !!!
            Dictionary<string, Visio.Master> masters = new Dictionary<string, Visio.Master>();
            masters.Add(@"Cube", doc.Masters.get_ItemU(@"Cube"));

            Visio.Master visioCubeMaster = doc.Masters.get_ItemU(@"Cube");
            Visio.Master visioDimensionMaster = doc.Masters.get_ItemU(@"Dimension");
            Visio.Master visioHierarchytMaster = doc.Masters.get_ItemU(@"Hierarchy");
            Visio.Master visioAttributeMaster = doc.Masters.get_ItemU(@"Attribute");
            Visio.Master visioUsedByMaster = doc.Masters.get_ItemU(@"Used By");

            // ...and draw them
            List<Visio.Shape> cubeShapes = new List<Visio.Shape>();

            foreach (DocCube cube in server.cubes)
            {
                cubeShapes.Add(adaptPage.Drop(visioCubeMaster, 6, 5));
            }

            Visio.Shape visioCubeShape1 = adaptPage.Drop(visioCubeMaster, 4.25, 1.5);
            visioCubeShape1.Text = @"My 1st Cube";
            visioCubeShape1.Resize(Visio.VisResizeDirection.visResizeDirN, 2, Visio.VisUnitCodes.visCentimeters);
            visioCubeShape1.Resize(Visio.VisResizeDirection.visResizeDirE, 2, Visio.VisUnitCodes.visCentimeters);

            Visio.Shape visioDimensionShape1 = adaptPage.DropConnected(visioDimensionMaster, visioCubeShape1, Visio.VisAutoConnectDir.visAutoConnectDirNone, visioUsedByMaster);
            visioDimensionShape1.Text = @"My 1st Dimension";

            Visio.Shape visioDimensionShape2 = adaptPage.Drop(visioDimensionMaster, 6.25, 2.5);
            visioDimensionShape2.Text = @"My 2nd Dimension";

            //Visio.Shape visioUsedByShape1 = visioUsedByMaster.Shapes.;

            // Sample code for grouping...
            select = doc.Application.ActiveWindow.Selection;
            select.SelectAll();
            group = select.Group();

            visioDimensionShape2.AutoConnect(visioDimensionShape1, Visio.VisAutoConnectDir.visAutoConnectDirNone);

            //Visio.Connect visioUsedBy1 = adaptPage.Connects(visioUsedByMaster, 6.25, 2.5);
            //visioDimensionShape2.Text = @"My 2nd Dimension";



            //adaptPage.Drop(visioUsedByShape1, 5, 5);
            //Visio.Shape visioCubeShape2 = visioCubeMaster.;

        }
    }
}
