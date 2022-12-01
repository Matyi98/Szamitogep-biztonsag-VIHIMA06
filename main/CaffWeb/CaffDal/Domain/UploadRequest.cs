namespace CaffDal.Domain
{
    public class UploadRequest
    {
        public int OwnerId { get; set; }
        public string CaffName { get; set; }
        public byte[] RawCaff { get; set; }

    }
}
