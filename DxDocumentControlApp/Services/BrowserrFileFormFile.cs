using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DxDocumentControlApp.Services
{
    public class BrowserFileFormFile : IFormFile
    {
        private readonly IBrowserFile _browserFile;

        public BrowserFileFormFile(IBrowserFile browserFile)
        {
            _browserFile = browserFile;
        }

        public string ContentType => _browserFile.ContentType;
        public string ContentDisposition => $"form-data; name=\"{_browserFile.Name}\"; filename=\"{_browserFile.Name}\"";
        public IHeaderDictionary Headers => new HeaderDictionary();
        public long Length => _browserFile.Size;
        public string Name => _browserFile.Name;
        public string FileName => _browserFile.Name;

        public void CopyTo(Stream target)
        {
            using var stream = _browserFile.OpenReadStream();
            stream.CopyTo(target);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            using var stream = _browserFile.OpenReadStream(long.MaxValue);
            await stream.CopyToAsync(target, cancellationToken);
        }

        public Stream OpenReadStream(long maxAllowedSize = long.MaxValue, CancellationToken cancellationToken = default)
        {
            return _browserFile.OpenReadStream(maxAllowedSize);
        }

        public Stream OpenReadStream()
        {
            return _browserFile.OpenReadStream();
        }
    }
}
