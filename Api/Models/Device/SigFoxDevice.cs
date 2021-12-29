namespace RtgsmsApi.Models.Device
{
    public class SigFoxDevice : BaseModel
    {
        public string Device { get; set; }
        public string Time { get; set; }
        public string deviceTypeId { get; set; }
        public string Data { get; set; }
        public int Temp { get; set; }
        public int Hum { get; set; }
        public int Lat { get; set; }
        public int Long { get; set; }
        public int Geophone_analog_value { get; set; }
        public int Ldr { get; set; }
        public int x_acc { get; set; }
        public int Y_acc { get; set; }
        public int Z_acc { get; set; }
        public int Flags { get; set; }
    }
}
