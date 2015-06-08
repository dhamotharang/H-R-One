using System;
using System.IO;
using System.Data;
using System.Configuration;
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Core;
using Ionic.Zip;
/// <summary>
/// Summary description for Unzip
/// </summary>
public class zip
{
    //private static void writeLog(string message)
    //{
    //    Console.Write(message);
    //    logWriter.Write(message);
    //}
    //private static void writeLineLog(string message)
    //{
    //    Console.WriteLine(message);
    //    logWriter.WriteLine(message);
    //}
    public static bool Compress(string sourceFolder, string sourceFilenameWithWildCard, string ZipFileName)
    {
        return Compress(sourceFolder, sourceFilenameWithWildCard, ZipFileName, string.Empty);
    }
    public static bool Compress(string sourceFolder, string sourceFilenameWithWildCard, string ZipFileName, string Password)
    {
        string[] allFiles = Directory.GetFiles(sourceFolder, sourceFilenameWithWildCard, SearchOption.AllDirectories);
        if (System.IO.File.Exists(ZipFileName)) 	//  Small piece of code
            System.IO.File.Delete(ZipFileName);
        System.IO.FileStream fos = new FileStream(ZipFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite); //  J# output
        // stream (Zip File)
        ZipOutputStream zos = new ZipOutputStream(fos);           //  J# output zip
        //zos.SetLevel(9);    //  Set the level of compression.
        zos.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
        if (!string.IsNullOrEmpty(Password))
            zos.Password = Password;
        // It may be a value between 0 and 9

        /*
            Add each file from folder to zip, to zip file.
            This way, the tree of the folder to zip will be
            reflected on the zip file
        */

        for (int i = 0; i < allFiles.Length; i++)
        {
            string sourceFile = allFiles[i];

            FileStream fis = File.OpenRead(sourceFile);  //  J# input
            //stream to fill zip file
            /*
                Add the entry to the zip file (The Replace will remove the full path
                Ex.: file C:\FolderToZip\Files\Tmp\myFile.xml,
      will be written as Files\Tmp\myFile.xml on the zip file
                If this code was not written, it would generate the
      whole tree since the beginning of the FolderToZip
                This way the zip file begins directly at the contents
      of C:\FolderToZip
            */

            //ZipEntry ze = new ZipEntry(sourceFile.Replace(((string)sourceFolder + @"\").Replace(@"\\", @"\"), ""));
            //zos.PutNextEntry(ze);
            ZipEntry ze = zos.PutNextEntry(sourceFile.Replace(((string)sourceFolder + @"\").Replace(@"\\", @"\"), ""));

            byte[] buffer = new byte[1024];
            int len;

            while ((len = fis.Read(buffer, 0, buffer.GetLength(0))) > 0)
            {
                zos.Write(buffer, 0, len);  //  Write buffer to Zip File
            }

            fis.Close();    //  Close input Stream
        }

        //  Close outputs
        //zos.CloseEntry();
        zos.Close();
        fos.Close();
        zos.Dispose();
        fos.Dispose();
        return true;

    }

    public static int ExtractAll(string source, string destination, string Password)
    {

        ZipFile zf = null;
        //ZipInputStream zinstream = null; // used to read from the zip file
        int numFileUnzipped = 0; // number of files extracted from the zip file

        try
        {
            //FileStream fs = File.OpenRead(source);
            //zf = new ZipFile(fs);
            zf = new ZipFile(source);
            if (!string.IsNullOrEmpty(Password))
                zf.Password = Password;		// AES encrypted entries are handled automatically
            // we need to extract to a folder so we must create it if needed
            if (Directory.Exists(destination) == false)
                Directory.CreateDirectory(destination);
            zf.ExtractAll(destination, ExtractExistingFileAction.OverwriteSilently);
            //foreach (ZipEntry theEntry in zf)
            //{
            //    if (theEntry.IsDirectory)
            //    {
            //        string dirname = Path.GetDirectoryName(theEntry.FileName); // the file path

            //        // if a path name exists we should create the directory in the destination folder
            //        string target = System.IO.Path.Combine(destination, dirname);
            //        if (dirname.Length > 0 && !Directory.Exists(target))
            //        {
            //            DirectoryInfo dirInfo = Directory.CreateDirectory(target);
            //            dirInfo.CreationTime = theEntry.CreationTime;   //.DateTime;
            //            dirInfo.LastWriteTime = theEntry.LastModified;   //.DateTime;
            //        }
            //    }
            //    if (!theEntry.IsDirectory && !theEntry.IsText)
            //    {
            //        // now we know the proper path exists in the destination so copy the file there
            //        string fname = Path.GetFileName(theEntry.FileName);      // the file name
            //        if (fname != String.Empty)
            //        {
            //            DecompressAndWriteFile(Path.Combine(destination, theEntry.FileName), zf, theEntry);
            //            FileInfo f = new FileInfo(System.IO.Path.Combine(destination, theEntry.FileName));
            //            f.CreationTime = theEntry.CreationTime;   //.DateTime;
            //            f.LastWriteTime = theEntry.LastModified;   //.DateTime;
            //            numFileUnzipped++;
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (zf != null)
            {
                //zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                //zf.Close(); // Ensure we release resources
                zf.Dispose();
            }
        }

        return numFileUnzipped;
    }
    //private static void DecompressAndWriteFile(string destination, ZipFile zf, ZipEntry zipEntry)
    //{
    //    String entryFileName = zipEntry.Name;
    //    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
    //    // Optionally match entrynames against a selection list here to skip as desired.
    //    // The unpacked length is available in the zipEntry.Size property.

    //    byte[] buffer = new byte[4096];		// 4K is optimum
    //    Stream zipStream = zf.GetInputStream(zipEntry);


    //    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
    //    // of the file, but does not waste memory.
    //    // The "using" will close the stream even if an exception occurs.
    //    using (FileStream streamWriter = File.Create(destination))
    //    {
    //        StreamUtils.Copy(zipStream, streamWriter, buffer);
    //    }
    //}

}
