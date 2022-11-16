using Microsoft.Extensions.Options;

namespace CaffDal.ParserWrapper
{
    public class CaffParserConfig {
        public string ParserPath { get; set; }
        public string OutputWorkdir { get; set; }
    }

    public class CaffParser
    {
        private readonly CaffDbContext dbContext;
        private readonly CaffParserConfig config;

        public CaffParser(CaffDbContext dbContext, IOptions<CaffParserConfig> config) {
            this.dbContext = dbContext;
            this.config = config.Value;
        }

        //Upload Caff
        public void UploadCaff(byte[] caff) {
            //parse caff with native parser
            //write caff to db

            // communicate errors with exceptions
        }

        //Preview Caff
        public byte[] GetPreview(int caffId) {
            // https://stackoverflow.com/questions/9149430/displaying-image-from-db-in-razor-mvc3
            // get caff image from db
            return null!;
        }

        //Download Caff
        public byte[] GetRawCaff(int caffId) {
            // get caff file from db
            return null!;
        }

    }
}
