public class VelocityCalculator
{
    const double G = 6.67430e-11;    // Gravitational constant (m^3 kg^-1 s^-2)
    const double MSun = 1.989e30;    // Mass of the Sun (kg)
    const double AU = 1.496e11;      // Astronomical unit in meters
    const double mu = G * MSun;      // Standard gravitational parameter of the Sun (m^3 s^-2)

    // Function to solve Kepler's equation for Eccentric Anomaly
    public static double SolveKepler(double M, double e)
    {
        double E = M;  // Initial guess for E
        double delta = 1e-6;
        while (true)
        {
            double newE = M + e * Math.Sin(E);
            if (Math.Abs(newE - E) < delta) break;
            E = newE;
        }
        return E;
    }

    // Function to calculate true anomaly
    public static double TrueAnomaly(double E, double e)
    {
        return 2 * Math.Atan2(Math.Sqrt(1 + e) * Math.Sin(E / 2), Math.Sqrt(1 - e) * Math.Cos(E / 2));
    }

    // Function to calculate 3D velocity in ecliptic coordinates
    public static double[] CalculateVelocity(double a, double e, double i, double omega, double w, double M)
    {
        // Solve Kepler's equation for Eccentric Anomaly
        double E = SolveKepler(M, e);

        // Calculate true anomaly
        double nu = TrueAnomaly(E, e);

        // Calculate radial distance r
        double r = a * (1 - e * e) / (1 + e * Math.Cos(nu));

        // Calculate orbital velocity using vis-viva equation
        double v = Math.Sqrt(mu * (2 / (r * AU) - 1 / (a * AU)));

        // Radial and tangential velocity components in the orbital plane
        double vr = Math.Sqrt(mu / (a * AU)) * (e * Math.Sin(nu)) / Math.Sqrt(1 - e * e);
        double vt = Math.Sqrt(mu / (a * AU)) * (1 + e * Math.Cos(nu)) / Math.Sqrt(1 - e * e);

        // Convert to 3D ecliptic coordinates using inclination, omega, and w
        double x = r * Math.Cos(nu);
        double y = r * Math.Sin(nu);

        // Rotation matrix to convert to 3D coordinates
        double cosOmega = Math.Cos(omega);
        double sinOmega = Math.Sin(omega);
        double cosI = Math.Cos(i);
        double sinI = Math.Sin(i);
        double cosW = Math.Cos(w);
        double sinW = Math.Sin(w);

        // Velocity components in the ecliptic plane
        double[] velocity =
        [
            v * (cosW * cosOmega - sinW * sinOmega * cosI),  // x component
            v * (cosW * sinOmega + sinW * cosOmega * cosI),  // y component
            v * (sinW * sinI),  // z component
        ];
        return velocity;
    }

    public static double CalculateMass(double diameterInMeters, double densityInKgPerM3)
    {
        // Calculate the volume of the NEO using the formula for the volume of a sphere
        double radius = diameterInMeters / 2;
        double volume = (4.0 / 3.0) * Math.PI * Math.Pow(radius, 3); // Volume in cubic meters

        // Calculate mass
        double mass = densityInKgPerM3 * volume; // Mass in kilograms
        return mass;
    }

    // Main function to test velocity calculation
    public static NEOVelocityRecord NEORecordVelocity(NEORecord neoRecord)
    {
        // Calculate velocity
        double a = double.Parse(neoRecord.a ?? "0");
        double e = double.Parse(neoRecord.e ?? "0");
        double i = Math.PI / 180 * double.Parse(neoRecord.i ?? "0");   // Convert degrees to radians
        double omega = Math.PI / 180 * double.Parse(neoRecord.om ?? "0"); // Convert degrees to radians
        double w = Math.PI / 180 * double.Parse(neoRecord.w ?? "0");    // Convert degrees to radians
        double M = Math.PI / 180 * double.Parse(neoRecord.ma ?? "0");    // Convert degrees to radians

        double[] velocity = CalculateVelocity(a, e, i, omega, w, M);
        double mass = CalculateMass(double.Parse(neoRecord.diameter == "" ? "0" : neoRecord.diameter ?? "0"), 2500);

        //! uncomment for debugging later on
        // Console.WriteLine($"Velocity (X): {velocity[0]} m/s");
        // Console.WriteLine($"Velocity (Y): {velocity[1]} m/s");
        // Console.WriteLine($"Velocity (Z): {velocity[2]} m/s");

        return new NEOVelocityRecord(name: neoRecord.name ?? "unknown", velX: velocity[0], velY: velocity[1], velZ: velocity[2], dangerous: neoRecord.pha == "Y", mass: mass);
    }
}
