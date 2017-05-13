using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VideoLibrary
{
    public abstract class Video : IDisposable
    {
        private readonly VideoClient _client = new VideoClient();

        internal Video()
        {
        }

        public string GetUri()
        {
            return GetUriAsync().GetAwaiter().GetResult();
        }
        public abstract Task<string> GetUriAsync();
        public abstract string Title { get; }
        public abstract WebSites WebSite { get; }
        public virtual VideoFormat Format => VideoFormat.Unknown;
        // public virtual AudioFormat AudioFormat => AudioFormat.Unknown;


        public byte[] GetBytes() =>
            GetBytesAsync().GetAwaiter().GetResult();

        public async Task<byte[]> GetBytesAsync()
        {
            return await _client
                .GetBytesAsync(this)
                .ConfigureAwait(false);
        }

        public Stream Stream() =>
            StreamAsync().GetAwaiter().GetResult();

        public async Task<Stream> StreamAsync()
        {
            return await _client
                .StreamAsync(this)
                .ConfigureAwait(false);
        }

        public virtual string FileExtension
        {
            get
            {
                switch (Format)
                {
                    case VideoFormat.Flash: return ".flv";
                    case VideoFormat.Mobile: return ".3gp";
                    case VideoFormat.Mp4: return ".mp4";
                    case VideoFormat.WebM: return ".webm";
                    case VideoFormat.Unknown: return string.Empty;
                    default:
                        throw new NotImplementedException($"Format {Format} is unrecognized! Please file an issue at libvideo on GitHub.");
                }
            }
        }

        public string FullName
        {
            get
            {
                var builder =
                    new StringBuilder(Title)
                    .Append(FileExtension);

                foreach (char bad in Path.GetInvalidFileNameChars())
                    builder.Replace(bad, '_');

                return builder.ToString();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Video() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
