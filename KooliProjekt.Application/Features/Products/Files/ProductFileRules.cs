using System;
using System.IO;
using System.Linq;

namespace KooliProjekt.Application.Features.Products
{
    // Tootepiltidega seotud ühised reeglid, mida kasutavad validaatorid ja handlerid
    public static class ProductFileRules
    {
        public const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public static bool HasAllowedExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName ?? "");
            return AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        // Faili nimi ei tohi sisaldada kaustu ega "..", muidu saaks lugeda teiste kaustade faile
        public static bool IsPlainFileName(string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName)
                && fileName == Path.GetFileName(fileName)
                && fileName != "."
                && fileName != ".."
                && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}
