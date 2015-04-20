
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;


// https://code.google.com/p/tar-cs/
// http://www.csharpest.net/?p=110
namespace GitTest
{


    class TarTest
    {

        // Sucks.
        // http://www.cupofdev.com/compress-files-7zip-csharp/


        // TarTest.CreateTar(@"D:\myfile.tar", folder);
        // TarTest.CreateTarGz(@"D:\myfile.tar.gz", folder);
        // TarTest.CreateMemoryTar(@"D:\myfile.tar.gz", folder);


        /// <summary>
        /// Creates a GZipped Tar file from a source directory
        /// </summary>
        /// <param name="outputTarFilename">Output .tar.gz file</param>
        /// <param name="sourceDirectory">Input directory containing files to be added to GZipped tar archive</param>
        public static void CreateMemoryTar(string outputTarFilename, string sourceDirectory)
        {

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                // using (System.IO.Stream gzipStream = new GZipOutputStream(fs))
                // {
                using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(ms))
                {
                    AddDirectoryFilesToTar(tarArchive, sourceDirectory, true);
                }
                // }

            }

        }



        /// <summary>
        /// Creates a GZipped Tar file from a source directory
        /// </summary>
        /// <param name="outputTarFilename">Output .tar.gz file</param>
        /// <param name="sourceDirectory">Input directory containing files to be added to GZipped tar archive</param>
        public static void CreateTar(string outputTarFilename, string sourceDirectory)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(outputTarFilename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                // using (System.IO.Stream gzipStream = new GZipOutputStream(fs))
                // {
                using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(fs))
                {
                    AddDirectoryFilesToTar(tarArchive, sourceDirectory, true);
                }
                // }
            }

        }



        /// <summary>
        /// Creates a GZipped Tar file from a source directory
        /// </summary>
        /// <param name="outputTarFilename">Output .tar.gz file</param>
        /// <param name="sourceDirectory">Input directory containing files to be added to GZipped tar archive</param>
        public static void CreateTarGz(string outputTarFilename, string sourceDirectory)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(outputTarFilename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                using (System.IO.Stream gzipStream = new GZipOutputStream(fs))
                {
                    using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzipStream))
                    {
                        AddDirectoryFilesToTar(tarArchive, sourceDirectory, true);
                    }
                }
            }

        }

        /// <summary>
        /// Recursively adds folders and files to archive
        /// </summary>
        /// <param name="tarArchive"></param>
        /// <param name="sourceDirectory"></param>
        /// <param name="recurse"></param>
        private static void AddDirectoryFilesToTar(TarArchive tarArchive, string sourceDirectory, bool recurse)
        {
            // Recursively add sub-folders
            if (recurse)
            {
                string[] directories = System.IO.Directory.GetDirectories(sourceDirectory);
                foreach (string directory in directories)
                    AddDirectoryFilesToTar(tarArchive, directory, recurse);
            }

            // Add files
            string[] filenames = System.IO.Directory.GetFiles(sourceDirectory);
            foreach (string filename in filenames)
            {
                TarEntry tarEntry = TarEntry.CreateEntryFromFile(filename);
                tarArchive.WriteEntry(tarEntry, true);
            }
        }


    }


}
