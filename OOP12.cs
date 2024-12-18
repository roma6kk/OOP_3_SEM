using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;

public class ARVLog
{
    private readonly string logFilePath;

    public ARVLog(string filePath)
    {
        try
        {
            logFilePath = Path.GetFullPath(filePath);

            if (!File.Exists(logFilePath))
            {
                using (File.Create(logFilePath)) { } 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing log file: {ex.Message}");
        }
    }

    public void WriteLog(string action, string details)
    {
        try
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Action: {action}, Details: {details}";
            using (StreamWriter writer = new StreamWriter(logFilePath, true, Encoding.UTF8))
            {
                writer.WriteLine(logEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    public string ReadLog()
    {
        try
        {
            if (File.Exists(logFilePath))
            {
                using (StreamReader reader = new StreamReader(logFilePath, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            return "Log file not found.";
        }
        catch (Exception ex)
        {
            return $"Error reading log file: {ex.Message}";
        }
    }

    public List<string> SearchLog(string keyword)
    {
        var results = new List<string>();

        try
        {
            if (File.Exists(logFilePath))
            {
                using (StreamReader reader = new StreamReader(logFilePath, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(keyword))
                        {
                            results.Add(line);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching log file: {ex.Message}");
        }

        return results;
    }

    public List<string> FilterLogByDateRange(DateTime startDate, DateTime endDate)
    {
        var results = new List<string>();

        try
        {
            if (File.Exists(logFilePath))
            {
                using (StreamReader reader = new StreamReader(logFilePath, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (DateTime.TryParse(line.Substring(1, 19), out DateTime logDate))
                        {
                            if (logDate >= startDate && logDate <= endDate)
                            {
                                results.Add(line);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering log file: {ex.Message}");
        }

        return results;
    }

    public int CountLogEntries()
    {
        try
        {
            if (File.Exists(logFilePath))
            {
                using (StreamReader reader = new StreamReader(logFilePath, Encoding.UTF8))
                {
                    int count = 0;
                    while (reader.ReadLine() != null)
                    {
                        count++;
                    }
                    return count;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error counting log entries: {ex.Message}");
        }

        return 0;
    }

    public void KeepCurrentHourLogs()
    {
        try
        {
            if (File.Exists(logFilePath))
            {
                var currentHour = DateTime.Now.Hour;
                var filteredLines = new List<string>();

                using (StreamReader reader = new StreamReader(logFilePath, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (DateTime.TryParse(line.Substring(1, 19), out DateTime logDate))
                        {
                            if (logDate.Date == DateTime.Now.Date && logDate.Hour == currentHour)
                            {
                                filteredLines.Add(line);
                            }
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter(logFilePath, false, Encoding.UTF8))
                {
                    foreach (var log in filteredLines)
                    {
                        writer.WriteLine(log);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error keeping current hour logs: {ex.Message}");
        }
    }
}

public class ARVDiskInfo
{
    private static void DisplayDiskInfo(DriveInfo drive)
    {
        try
        {
            Console.WriteLine($"  Disk {drive.Name}:");
            Console.WriteLine($"  Volume Label: {drive.VolumeLabel}");
            Console.WriteLine($"  File System: {drive.DriveFormat}");
            Console.WriteLine($"  Disk Type: {drive.DriveType}");
            Console.WriteLine($"  Total Space: {drive.TotalSize / (1024 * 1024 * 1024)} ГБ");
            Console.WriteLine($"  Available Space: {drive.AvailableFreeSpace / (1024 * 1024 * 1024)} ГБ");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Disk processing error {drive.Name}: {ex.Message}");
        }
    }

    public static void DisplayFreeSpace()
    {
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            try
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Disk {drive.Name}: Available Space: {drive.AvailableFreeSpace / (1024 * 1024 * 1024)} ГБ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting free disk space {drive.Name}: {ex.Message}");
            }
        }
    }

    public static void DisplayFileSystemInfo()
    {
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            try
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Disk {drive.Name}: File System: {drive.DriveFormat}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting the file system for the disk {drive.Name}: {ex.Message}");
            }
        }
    }

    public static void DisplayAllDiskInfo()
    {
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
            {
                DisplayDiskInfo(drive);
            }
            else
            {
                Console.WriteLine($"Disk {drive.Name} isn't ready.");
            }
        }
    }
}

public class ARVFileInfo
{
    private readonly string filePath;

    public ARVFileInfo(string filePath)
    {
        this.filePath = filePath;
    }

    public void DisplayFullPath()
    {
        try
        {
            if (File.Exists(filePath))
            {
                Console.WriteLine($"Full Path: {Path.GetFullPath(filePath)}");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting the full path: {ex.Message}");
        }
    }

    public void DisplayBasicInfo()
    {
        try
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Console.WriteLine($"File Name: {fileInfo.Name}");
                Console.WriteLine($"Size: {fileInfo.Length} byte");
                Console.WriteLine($"Extension: {fileInfo.Extension}");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting basic information: {ex.Message}");
        }
    }

    public void DisplayDates()
    {
        try
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Console.WriteLine($"Creation date: {fileInfo.CreationTime}");
                Console.WriteLine($"Last-modified date: {fileInfo.LastWriteTime}");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving file dates: {ex.Message}");
        }
    }
}

public class ARVDirInfo
{
    private readonly string dirPath;

    public ARVDirInfo(string dirPath)
    {
        this.dirPath = dirPath;
    }

    public void DisplayFileCount()
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                int fileCount = Directory.GetFiles(dirPath).Length;
                Console.WriteLine($"Number of files: {fileCount}");
            }
            else
            {
                Console.WriteLine("The directory was not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when counting files: {ex.Message}");
        }
    }

    public void DisplayCreationTime()
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                DateTime creationTime = Directory.GetCreationTime(dirPath);
                Console.WriteLine($"Creation time: {creationTime}");
            }
            else
            {
                Console.WriteLine("The directory was not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in getting the creation time: {ex.Message}");
        }
    }

    public void DisplaySubdirectoryCount()
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                int subdirectoryCount = Directory.GetDirectories(dirPath).Length;
                Console.WriteLine($"Number of subdirectories: {subdirectoryCount}");
            }
            else
            {
                Console.WriteLine("Directory not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error counting subdirectories: {ex.Message}");
        }
    }

    public void DisplayParentDirectories()
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                Console.WriteLine("List of parent directories:");

                while (dirInfo.Parent != null)
                {
                    Console.WriteLine(dirInfo.Parent.FullName);
                    dirInfo = dirInfo.Parent;
                }
            }
            else
            {
                Console.WriteLine("Directory not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving parent directories: {ex.Message}");
        }
    }
}

public class ARVFileManager
{
    public static void InspectDirectory(string drivePath)
    {
        try
        {
            if (Directory.Exists(drivePath))
            {
                string inspectDir = Path.Combine(drivePath, "ARVInspect");
                Directory.CreateDirectory(inspectDir);

                string filePath = Path.Combine(inspectDir, "ARVdirinfo.txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("List of directories:");
                    foreach (var dir in Directory.GetDirectories(drivePath))
                    {
                        writer.WriteLine(dir);
                    }

                    writer.WriteLine("\nList of files:");
                    foreach (var file in Directory.GetFiles(drivePath))
                    {
                        writer.WriteLine(file);
                    }
                }

                string copyPath = Path.Combine(inspectDir, "ARVdirinfo_copy.txt");
                File.Copy(filePath, copyPath);
                File.Delete(filePath);
            }
            else
            {
                Console.WriteLine("Drive or directory not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in InspectDirectory: {ex.Message}");
        }
    }

    public static void ManageFiles(string sourceDir, string fileExtension)
    {
        try
        {
            if (Directory.Exists(sourceDir))
            {
                string filesDir = Path.Combine(sourceDir, "ARVFiles");
                Directory.CreateDirectory(filesDir);

                foreach (var file in Directory.GetFiles(sourceDir, "*" + fileExtension))
                {
                    string destFile = Path.Combine(filesDir, Path.GetFileName(file));
                    File.Copy(file, destFile);
                }

                string inspectDir = Path.Combine(sourceDir, "ARVInspect");
                Directory.CreateDirectory(inspectDir);
                string newFilesDir = Path.Combine(inspectDir, "ARVFiles");
                Directory.Move(filesDir, newFilesDir);
            }
            else
            {
                Console.WriteLine("Source directory not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ManageFiles: {ex.Message}");
        }
    }

    public static void ArchiveAndExtract(string sourceDir, string destinationDir)
    {
        try
        {
            if (Directory.Exists(sourceDir))
            {
                string zipPath = Path.Combine(destinationDir, "ARVFiles.zip");

                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                ZipFile.CreateFromDirectory(sourceDir, zipPath);

                string extractDir = Path.Combine(destinationDir, "ExtractedFiles");
                Directory.CreateDirectory(extractDir);

                ZipFile.ExtractToDirectory(zipPath, extractDir);

                Console.WriteLine("Archiving and extraction completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ArchiveAndExtract: {ex.Message}");
        }
    }
}


public class OOP12
{
    public static void Main()
    {
        string logFile = Path.Combine(Directory.GetCurrentDirectory(), "arvlogfile.txt");

        ARVLog logger = new ARVLog(logFile);

        logger.WriteLog("File Opened", "FileName: file1.txt, Path: D:\\Poman\\prog\\C#\\OOP12\\OOP12\\file1.txt");
        logger.WriteLog("File Saved", "FileName: file2.docx, Path: D:\\Poman\\prog\\C#\\OOP12\\OOP12\\file2.docx");

        Console.WriteLine("Содержимое Log-файла:");
        Console.WriteLine(logger.ReadLog());

        Console.WriteLine("\nРезультаты поиска 'docx':");
        var searchResults = logger.SearchLog("docx");
        foreach (var result in searchResults)
        {
            Console.WriteLine(result);
        }

        Console.WriteLine("\nЛоги за последний день:");
        var filteredLogs = logger.FilterLogByDateRange(DateTime.Now.AddDays(-1), DateTime.Now);
        foreach (var log in filteredLogs)
        {
            Console.WriteLine(log);
        }

        int logCount = logger.CountLogEntries();
        Console.WriteLine($"\nВсего входов в Log: {logCount}");

        logger.KeepCurrentHourLogs();
        Console.WriteLine("\nВходы в журнал после сохранения записей о текущем часе:");
        Console.WriteLine(logger.ReadLog());

        //=============2=========

        Console.WriteLine("Информация о дисках в системе:");

        Console.WriteLine("\n1. Свободное место на дисках:");
        ARVDiskInfo.DisplayFreeSpace();

        Console.WriteLine("\n2. Файловая система дисков:");
        ARVDiskInfo.DisplayFileSystemInfo();

        Console.WriteLine("\n3. Полная информация о дисках:");
        ARVDiskInfo.DisplayAllDiskInfo();

        //=============3===========

        Console.Write("Введите путь к файлу (например, D:\\Poman\\prog\\C#\\OOP12\\OOP12\\SomeDir\\SomeFile.txt ): ");
        string filePath = Console.ReadLine();

        ARVFileInfo fileInfo = new ARVFileInfo(filePath);

        Console.WriteLine("\n--- Полный путь ---");
        fileInfo.DisplayFullPath();

        Console.WriteLine("\n--- Базовая информация ---");
        fileInfo.DisplayBasicInfo();

        Console.WriteLine("\n--- Даты файла ---");
        fileInfo.DisplayDates();

        //==============4================

        Console.Write("Введите путь к директории (Например, D:\\Poman\\prog\\C#\\OOP12\\OOP12\\SomeDir ): ");
        string path = Console.ReadLine();

        ARVDirInfo dirInfo = new ARVDirInfo(path);

        Console.WriteLine();
        dirInfo.DisplayFileCount();
        Console.WriteLine();
        dirInfo.DisplayCreationTime();
        Console.WriteLine();
        dirInfo.DisplaySubdirectoryCount();
        Console.WriteLine();
        dirInfo.DisplayParentDirectories();

        //=================5===============

        string drivePath = "D:\\Poman\\prog\\C#\\OOP12\\OOP12";
        string sourceDir = "D:\\Poman\\prog\\C#\\OOP12\\OOP12";
        string destinationDir = "D:\\";
        string fileExtension = ".txt";

        ARVFileManager.InspectDirectory(drivePath);
        ARVFileManager.ManageFiles(sourceDir, fileExtension);
        ARVFileManager.ArchiveAndExtract(sourceDir, destinationDir);

        Console.WriteLine("Все операции выполнены.");
    }
}

