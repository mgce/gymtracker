using System;
using System.IO;
using GymTracker.Droid.Services;
using GymTracker.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace GymTracker.Droid.Services
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}