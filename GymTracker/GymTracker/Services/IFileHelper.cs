using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Services
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
