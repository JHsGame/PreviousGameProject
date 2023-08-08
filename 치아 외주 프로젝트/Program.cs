using FellowOakDicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicom_VS_Test
{
    class Program
    {
        public static void GetDicomTagSample(string dcmFilePath)
        {
            string modalityCode;

            try
            {
                DicomFile df = DicomFile.Open(dcmFilePath, FileReadOption.SkipLargeTags);
                DicomDataset ds = df.Dataset;
                try
                {
                    modalityCode = ds.GetString(DicomTag.Modality);
                    System.Console.WriteLine(modalityCode);
                }
                catch (DicomDataException ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        static void Main(string[] args)
        {
            string path = "C:\\Users\\solu0\\Downloads\\gouda_CT\\gouda_CT\\DCT0000.dcm";
            GetDicomTagSample(path);
        }
    }
}
