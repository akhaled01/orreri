public class NEOVelocityRecord(string name, double velX, double velY, double velZ, bool dangerous, double mass)
{
    public string Name { get; set; } = name;
    public double VelX { get; set; } = velX;
    public double VelY { get; set; } = velY;
    public double VelZ { get; set; } = velZ;
    public bool dangerous { get; set; } = dangerous;
    public double mass { get; set; } = mass;
}
