using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket_Simulator
{
    class Drag
    {
        public double wetA;
        public static double Radius = 0.169 / 2;
        public static double Radius2 = 0.173 / 2;
        public static double Aref = Radius * Radius2 * Math.PI;
        public double stage;
        public double drag;
        public double Cd;
        

        public Drag(double area)
        {
            wetA = area;
        }

        public void wetarea(Fin fin1,Fin fin2, int stage)
        {

            //for greater wetted Area accuracy include the area of the leading edge and trailing edge of fins

            if (stage == 1)
            {
                wetA = 8 * fin1.Area + 8 * fin2.Area + 179988.81 * 0.001 * 0.001 + 2 * Math.PI * Rocket.Radius * (1.895 - 0.5 + 4.02 - 2.22) + 2 * Math.PI * Rocket.Radius2 * 0.3 + 4 * fin1.TC * 0.003 + 4 * fin2.TC * 0.003;

            }
            else
                wetA = 179988.81 * 0.001 * 0.001 + 2 * Math.PI * Rocket.Radius * (1.895 - 0.5) + 2 * Math.PI * Rocket.Radius2 * 0.3;

        }

        public double skin(Conditions conditions,Rocket rocket, int stage,Fin fin1, Fin fin2)
        {
            double sdrag=0;
            double viscosity=0.558219*Math.Pow(1.00017,0.999946*(rocket.altitude+conditions.launch_elevation))+0.769948;
            double Re = rocket.V * Rocket.Radius * 2/viscosity;
            double Cf;
            double Rcrit;
            double Cfc;
            double Cd;
            double wetfin;
           


            if (stage == 1)
            {
                wetfin = 8 * fin1.Area + 8 * fin2.Area+ 4 * fin1.TC * 0.003 + 4 * fin2.TC * 0.003;
                Rcrit = 51 * Math.Pow(60*0.000001/4.02,-1.039);
                if (Re < 10000)
                    Cf = 0.0148;
                else if (Re >= 10000 && Re < Rcrit)
                    Cf = 1 / (1.5 * Math.Log(Re) - 5.6) / (1.5 * Math.Log(Re) - 5.6);
                else
                    Cf = 0.032 * Math.Pow(60 * 0.000001 / 4.02, 0.2);

            }
            else
            {
                wetfin =8 * fin2.Area  + 4 * fin2.TC * 0.003;
                Rcrit = 51 * Math.Pow(60 * 0.000001 / 2.22, -1.039);
                if (Re < 10000)
                    Cf = 0.0148;
                else if (Re >= 10000 && Re < Rcrit)
                    Cf = 1 / (1.5 * Math.Log(Re) - 5.6) / (1.5 * Math.Log(Re) - 5.6);
                else
                    Cf = 0.032 * Math.Pow(60 * 0.000001 / 2.22, 0.2);
            }

            if (rocket.Mach(conditions) <= 0.8)
            {
                Cfc = Cf*(1 - 0.1 * rocket.Mach(conditions) * rocket.Mach(conditions));
            }
            else
            {
                Cfc = Cf / Math.Pow((1 + rocket.Mach(conditions) * rocket.Mach(conditions) * 0.15),0.58);
            }

            if (Cf / (1 + 0.18 * rocket.Mach(conditions) * rocket.Mach(conditions)) > Cfc)
                Cfc = Cf / (1 + 0.18 * rocket.Mach(conditions) * rocket.Mach(conditions));

            

            Cd = Cfc * (1+1/2/(rocket.length/2/Rocket.Radius2)*wetA);

            
            sdrag = 0.5 * Cfc * conditions.rho * rocket.V * rocket.V * wetA;

            return sdrag;
        }

        public double pressure(Conditions conditions, Rocket rocket, int stage)
        {
            return 0;
        }
    }
}
