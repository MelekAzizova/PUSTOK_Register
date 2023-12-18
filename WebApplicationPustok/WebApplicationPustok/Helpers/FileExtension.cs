namespace WebApplicationPustok.Helpers
{
    public static class FileExtension
    {
        public static bool IsValidSize(this IFormFile file, float kb = 30) => file.Length <= kb * 1024;
        public static bool IsCorrectType(this IFormFile file, string contentTyple="image")=>file.ContentType.Contains(contentTyple); 
        public static async Task<string> SaveAsync(this IFormFile file,string path)
        {
            string extension=Path.GetExtension(file.FileName);
            string fileName=Path.GetFileName( file.FileName).Length>32 ?
                file.FileName.Substring(0,32) :
               Path.GetFileName(file.FileName);
            fileName=Path.Combine(path,Path.GetRandomFileName() + fileName+extension);
            using (FileStream fs = File.Create(Path.Combine(PathConstants.RootPath, fileName)))
            {
                await file.CopyToAsync(fs);
            }
            return fileName;

        }
    }
}
